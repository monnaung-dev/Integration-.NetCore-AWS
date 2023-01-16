
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using SmartParking.Admin.Models.JsonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParking.Admin.Helpers;
using SmartParking.Admin.Models;

namespace SmartParking.Admin.Data
{
    public class OccupancyInfo
    {
        IDynamoDBContext db;
        Dictionary<string, double> hoursArray = new Dictionary<string, double>();
        
        public OccupancyInfo(IDynamoDBContext db)
        {
            this.db = db;

            hoursArray.Add("8:00 AM", 0);
            hoursArray.Add("9:00 AM", 0);
            hoursArray.Add("10:00 AM", 0);
            hoursArray.Add("11:00 AM", 0);
            hoursArray.Add("12:00 PM", 0);
            hoursArray.Add("1:00 PM", 0);
            hoursArray.Add("2:00 PM", 0);
            hoursArray.Add("3:00 PM", 0);
            hoursArray.Add("4:00 PM", 0);
            hoursArray.Add("5:00 PM", 0);
            hoursArray.Add("6:00 PM", 0);
        }

        public async Task<occupancy> GetNewOccupancy(int Month)
        {
            occupancy occu = new occupancy();
            occu.count = "0";
            occu.percent = "0";
            occu.month = "";
            try
            {
                double previousMonthCount = 0;
                double thisMonthCount = 0;

                thisMonthCount = await GetOccupancyByMonth(Month);
                previousMonthCount = await GetOccupancies();
                double slotCount = await GetSlotsCount();

                double percent = PercentageHelper.GetPercentage(thisMonthCount, previousMonthCount);

                //  ((slot count * 10 operating hours) / parked hours) * 100
                double thisMonthPercent = (thisMonthCount / (slotCount * 10)) * 100;

                occu.count = String.Format("{0:F2}", thisMonthPercent);
                occu.month = DateTimeHelper.GetMonthName(Month);
                occu.percent = String.Format("{0:F2}", percent);
            }
            catch(Exception ex)
            {

            }
            
            return occu;
        }


        public async Task<List<parkinginfo>> GetParkingInfoByTimeOfDay(DateTime datetime)
        {
            List<parkinginfo> info = new List<parkinginfo>();
            try
            {
                var zones = await GetZones();
             
                Dictionary<string, int> paidCount = new Dictionary<string, int>();

                var parkings = await GetOccupancyByTime(datetime);
                foreach(var p in parkings)
                {
                    if (!paidCount.ContainsKey(p.parkingID))
                    {
                        paidCount.Add(p.parkingID, 0);
                    }

                    paidCount[p.parkingID] = paidCount[p.parkingID] + 1;
                }

                int idx = 0;
                foreach (var z in zones)
                {
                    idx = idx + 1;
                    parkinginfo i = new parkinginfo();
                    i.index = idx.ToString();
                    i.zone = z.title;
                    i.total = z.slots.Count.ToString();
                    i.free = z.freeSlots.ToString();
                    i.occupied = (z.slots.Count - z.freeSlots).ToString();
                    if (paidCount.ContainsKey(z.parkingID))
                    {
                        i.paid = paidCount[z.parkingID].ToString();
                    }

                    info.Add(i);
                }
            }
            catch (Exception ex)
            {

            }

            return info;
        }

        async Task<double> GetSlotsCount()
        {
            double cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" }) };
            var search = db.ScanAsync<Parking>(sc);
            var data = await search.GetRemainingAsync();
            foreach (var p in data)
            {
                cnt = cnt + p.slots.Count;

            }
            return cnt;
        }

        async Task<List<Parking>> GetZones()
        {
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" })};
            var search = db.ScanAsync<Parking>(sc);
            var data = await search.GetRemainingAsync();
            
            return data.ToList();
        }

        async Task<double> GetOccupancyByMonth(int Month)
        {
            double cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("dateFrom", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampFromStartOfMonth(Month), DateTimeHelper.getUnixTimeStampFromEndOfMonth(Month) })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            foreach (var p in data)
            {
                cnt = cnt + DateTimeHelper.getHoursFromUnixTimeStamp(Convert.ToInt64(p.dateFrom), Convert.ToInt64(p.dateTo));

            }
            return cnt;
        }

        async Task<double> GetOccupancies()
        {
            double cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }) };
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            foreach(var p in data)
            {
                cnt = cnt + DateTimeHelper.getHoursFromUnixTimeStamp(Convert.ToInt64(p.dateFrom), Convert.ToInt64(p.dateTo));

            }
            return cnt;
        }

        async Task<List<ParkingUser>> GetOccupancyByDay(DateTime date)
        {
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("date", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampForStartOfDay(date), DateTimeHelper.getUnixTimeStampForEndOfDay(date) })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            
         
            return data;
        }

        async Task<List<ParkingTrans>> GetOccupancyByTime(DateTime datetime)
        {
            double price = 0;
            string dateFrom = DateTimeHelper.getUnixTimeStamp(datetime).ToString();


            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("price", ScanOperator.GreaterThan, new object[] { price }),
                 new ScanCondition("dateBook", ScanOperator.IsNotNull),
                new ScanCondition("dateFrom", ScanOperator.LessThanOrEqual, new object[] { dateFrom }),
                new ScanCondition("dateBook", ScanOperator.GreaterThanOrEqual, new object[] { dateFrom })};
            var search = db.ScanAsync<ParkingTrans>(sc);
            var data = await search.GetRemainingAsync();


            return data;
        }

        public async Task<dailyOccupancy> GetDailtyOccupancyPercentage(DateTime date)
        {
            dailyOccupancy occu = new dailyOccupancy();
            occu.parkingLots = new List<dailyOccupancyParkingLot>();
            try
            {
                Dictionary<string, Dictionary<string, dailyOccupModel>> cityZones = new Dictionary<string, Dictionary<string, dailyOccupModel>>();

                occu.day = date.ToString("dd-MM-yyyy");

                List<Parking> zones = await GetZones();
                List<ParkingUser> parkingInfos = await GetOccupancyByDay(date);

                foreach (Parking p in zones)
                {

                    if (!cityZones.ContainsKey(p.address.city))
                    {
                        cityZones.Add(p.address.city, new Dictionary<string, dailyOccupModel>());
                    }

                    if (!cityZones[p.address.city].ContainsKey(p.parkingID))
                    {
                        cityZones[p.address.city][p.parkingID] = new dailyOccupModel();
                        cityZones[p.address.city][p.parkingID].slots = p.slots.Count;
                    }


                }

                foreach (ParkingUser pInfo in parkingInfos)
                {
                    double hours = DateTimeHelper.getHoursFromUnixTimeStamp(Convert.ToInt64(pInfo.dateFrom), Convert.ToInt64(pInfo.dateTo));

                    foreach (var z in cityZones)
                    {
                        if (z.Value.ContainsKey(pInfo.parkingID))
                        {
                            z.Value[pInfo.parkingID].hours = z.Value[pInfo.parkingID].hours + hours;
                            break;
                        }
                    }
                }


                foreach (var zz in cityZones)
                {
                    dailyOccupancyParkingLot lot = new dailyOccupancyParkingLot();
                    lot.name = zz.Key;
                    double totalHours = 0;
                    int totalSlots = 0;

                    foreach (var pk in zz.Value)
                    {
                        totalHours = totalHours + pk.Value.hours;
                        totalSlots = totalSlots + pk.Value.slots;
                    }

                    double per = (totalHours/ (totalSlots * 10)) * 100;
                    lot.count = String.Format("{0:F2}", per);
                    occu.parkingLots.Add(lot);
                }

            }
            catch (Exception ex)
            {

            }
            
            return occu;
        }

        async Task<List<ParkingUser>> GetParkingByDay(DateTime date)
        {
    
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("date", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampForStartOfDay(date), DateTimeHelper.getUnixTimeStampForEndOfDay(date) })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();


            return data.ToList();
        }

        public async Task<dailyOccupancyTrend> GetDailyOccupancyPercentageByTimeOfDays(DateTime date)
        {
            
            dailyOccupancyTrend occu = new dailyOccupancyTrend();
            occu.parkingLots = new List<dailyOccupancyTrendParkingLot>();
            try
            {
                occu.day = date.ToString("dd-MM-yyyy");
                
                List<ParkingUser> parkings = await GetParkingByDay(date);

                double slotsCount = await GetSlotsCount();

                foreach (var p in parkings)
                {
                    foreach (var h in hoursArray.Keys)
                    {
                        if (DateTimeHelper.IsInHourRange(h, Convert.ToInt64(p.dateFrom), Convert.ToInt64(p.dateTo)))
                        {
                            hoursArray[h] = hoursArray[h] + DateTimeHelper.getHoursFromUnixTimeStamp(Convert.ToInt64(p.dateFrom), Convert.ToInt64(p.dateTo));
                        }
                    }

                }

                foreach (var h in hoursArray.Keys)
                {
                    dailyOccupancyTrendParkingLot lot = new dailyOccupancyTrendParkingLot();
                    lot.time = h;
                    double per = (hoursArray[h]/ (slotsCount * 10)) * 100;
                    lot.count = String.Format("{0:F2}", per);
                    occu.parkingLots.Add(lot);
                }
            }
            catch(Exception ex)
            {

            }
            

            return occu;
        }

        public async Task<dailyParkingDurationTrend> GetDailyParkingDurationTrends(DateTime date)
        {
            Dictionary<string, Dictionary<string, dailyDurationModel>> cityZones = new Dictionary<string, Dictionary<string, dailyDurationModel>>();
            
            dailyParkingDurationTrend occu = new dailyParkingDurationTrend();
            try
            {
                occu.day = date.ToString("dd-MM-yyyy");
                
                occu.Zones = new List<dailyParkingDurationTrendZone>();

                List<Parking> zones = await GetZones();
                List<ParkingUser> parkings = await GetParkingByDay(date);

                foreach (Parking p in zones)
                {

                    if (!cityZones.ContainsKey(p.address.city))
                    {
                        cityZones.Add(p.address.city, new Dictionary<string, dailyDurationModel>());
                    }

                    if (!cityZones[p.address.city].ContainsKey(p.parkingID))
                    {
                        cityZones[p.address.city][p.parkingID] = new dailyDurationModel();
                    }
                }

                foreach (ParkingUser pInfo in parkings)
                {
                    double hours = DateTimeHelper.getHoursFromUnixTimeStamp(Convert.ToInt64(pInfo.dateFrom), Convert.ToInt64(pInfo.dateTo));

                    string key = "";

                    foreach (var z in cityZones)
                    {
                        if (z.Value.ContainsKey(pInfo.parkingID))
                        {
                            key = z.Key;
                            break;
                        }
                    }

                    if (hours <= 1)
                    {
                        cityZones[key][pInfo.parkingID].OneHour = cityZones[key][pInfo.parkingID].OneHour + 1;
                    }
                    else if (hours > 1 && hours <= 2)
                    {
                        cityZones[key][pInfo.parkingID].TwoHour = cityZones[key][pInfo.parkingID].TwoHour + 1;
                    }
                    else if (hours > 2 && hours <= 5)
                    {
                        cityZones[key][pInfo.parkingID].TwoToFiveHour = cityZones[key][pInfo.parkingID].TwoToFiveHour + 1;
                    }
                    else if (hours > 5)
                    {
                        cityZones[key][pInfo.parkingID].FiveHour = cityZones[key][pInfo.parkingID].FiveHour + 1;
                    }

                }

                foreach (var z in cityZones)
                {
                    dailyParkingDurationTrendZone h = new dailyParkingDurationTrendZone();
                    h.name = z.Key;
                    h.hours = new List<dailyOccupancyParkingLot>();


                    double oneCount = 0;
                    double twoCount = 0;
                    double twoToFiveCount = 0;
                    double FiveCount = 0;

                    foreach (var hh in z.Value)
                    {
                        oneCount = oneCount + hh.Value.OneHour;
                        twoCount = twoCount + hh.Value.TwoHour;
                        twoToFiveCount = twoToFiveCount + hh.Value.TwoToFiveHour;
                        FiveCount = FiveCount + hh.Value.FiveHour;
                    }

                    dailyOccupancyParkingLot one = new dailyOccupancyParkingLot();
                    one.name = "1 Hour";
                    one.count = String.Format("{0:n0}", oneCount);

                    dailyOccupancyParkingLot two = new dailyOccupancyParkingLot();
                    two.name = "1-2 Hour";
                    two.count = String.Format("{0:n0}", twoCount);

                    dailyOccupancyParkingLot twoToFive = new dailyOccupancyParkingLot();
                    twoToFive.name = "2-5 Hour";
                    twoToFive.count = String.Format("{0:n0}", twoToFiveCount);

                    dailyOccupancyParkingLot Five = new dailyOccupancyParkingLot();
                    Five.name = ">5 Hour";
                    Five.count = String.Format("{0:n0}", FiveCount);

                    h.hours.Add(one);
                    h.hours.Add(two);
                    h.hours.Add(twoToFive);
                    h.hours.Add(Five);

                    occu.Zones.Add(h);
                }
            }
            catch(Exception ex)
            {

            }
            

            return occu;
        }

    }

    public class dailyOccupModel
    {
        public int slots { get; set; }
        public double hours { get; set; }
    }

    public class dailyDurationModel
    {
        public double OneHour { get; set; }
        public double TwoHour { get; set; }
        public double TwoToFiveHour { get; set; }
        public double FiveHour { get; set; }
    }
}
