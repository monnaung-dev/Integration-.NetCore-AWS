
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
    public class RevenueInfo
    {
        IDynamoDBContext db;

        public RevenueInfo(IDynamoDBContext db)
        {
            this.db = db;
        }


        public async Task<revenue> GetNewRevenues(int Month)
        {
            revenue revenu = new revenue();
            try
            {
                double previousMonthCount = 0;
                double thisMonthCount = 0;

                thisMonthCount = await GetRevenuesByMonth(Month);
                previousMonthCount = await GetRevenues();

                double percent = PercentageHelper.GetPercentage(thisMonthCount, previousMonthCount);

                revenu.count = String.Format("{0:n0}", thisMonthCount);
                revenu.month = DateTimeHelper.GetMonthName(Month);
                revenu.percent = String.Format("{0:F2}", percent);
            }   
            catch(Exception ex)
            {

            }
            
            return revenu;
        }

        async Task<double> GetRevenuesByMonth(int Month)
        {
            double cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("date", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampFromStartOfMonth(Month), DateTimeHelper.getUnixTimeStampFromEndOfMonth(Month) })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            foreach (var p in data)
            {
                cnt = cnt + p.price;

            }
            return cnt;
        }

        async Task<double> GetRevenues()
        {
            double cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }) };
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            foreach(var p in data)
            {
                cnt = cnt + p.price;
                
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

        public async Task<parkingLotSummary> GetParkingLotSummary()
        {
            parkingLotSummary summary = new parkingLotSummary();
            double redCnt = 0;
            double greenCnt = 0;
            double yellowCnt = 0;

            summary.lots = new List<parkingLot>();
            parkingLot red = new parkingLot();
            red.color = "#f44336";
            red.pricePerHour = "500/hr";

            parkingLot green = new parkingLot();
            green.color = "#4caf50";
            green.pricePerHour = "200/hr";

            parkingLot yellow = new parkingLot();
            yellow.color = "#ffc107";
            yellow.pricePerHour = "300/hr";

            summary.lots.Add(red);
            summary.lots.Add(green);
            summary.lots.Add(yellow);

            try
            {
              

                List<Parking> parkings = await GetZones();

                foreach (var p in parkings)
                {
                    if (p.rate >= 200 && p.rate < 300) //Green
                    {
                        greenCnt = greenCnt + p.slots.Count;
                    }
                    else if (p.rate >= 300 && p.rate < 500) //Yellow
                    {
                        yellowCnt = yellowCnt + p.slots.Count;
                    }
                    else if (p.rate >= 500) //Red
                    {
                        redCnt = redCnt + p.slots.Count;
                    }
                }

                summary.total = Convert.ToInt32(redCnt + greenCnt + yellowCnt);

                double redPer = (redCnt / summary.total) * 100;
                double greenPer = (greenCnt / summary.total) * 100;
                double yellowPer = (yellowCnt / summary.total) * 100;

                red.count = Convert.ToInt32(redCnt).ToString();
                red.percent = String.Format("{0:F2}", redPer);

                green.count = Convert.ToInt32(greenCnt).ToString();
                green.percent = String.Format("{0:F2}", greenPer);

                yellow.count = Convert.ToInt32(yellowCnt).ToString();
                yellow.percent = String.Format("{0:F2}", yellowPer);

            }
            catch(Exception ex)
            {

            }
            
            
            return summary;
        }

    }
}
