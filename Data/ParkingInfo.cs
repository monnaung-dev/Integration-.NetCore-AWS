using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartParking.Admin.Models.JsonViewModels;
using Amazon.DynamoDBv2.DocumentModel;
using SmartParking.Admin.Helpers;
using SmartParking.Admin.Models;

namespace SmartParking.Admin.Data
{
    public class ParkingInfo
    {
        IDynamoDBContext db;

        public ParkingInfo(IDynamoDBContext db)
        {
            this.db = db;
        }

        public async Task<fineAndSettlements> GetFineAndSettlementCount(int Month)
        {
            fineAndSettlements fsCount = new fineAndSettlements();

            try
            {
                int fineCount = 0;
                int settlementCount = 0;

                ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("price", ScanOperator.Equal, new object[] { 0 }),
                new ScanCondition("date", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampFromStartOfMonth(Month), DateTimeHelper.getUnixTimeStampFromEndOfMonth(Month) })};
                var search = db.ScanAsync<ParkingUser>(sc);
                var data = await search.GetRemainingAsync();

                foreach (var p in data)
                {
                    fineCount = fineCount + 1;

                    //if (p.paymentDate > 0)
                    //{
                    settlementCount = settlementCount + 1;
                    //}
                }

                fsCount.fine = String.Format("{0:n0}", fineCount);
                fsCount.settlement = String.Format("{0:n0}", settlementCount);
                fsCount.month = DateTimeHelper.GetMonthName(Month);
            }
            catch(Exception ex)
            {

            }
            
            
            return fsCount;
        }

        public async Task<List<violationHistory>> GetViolationHisotry(
            string plateNumber, 
            string zone, 
            DateTime inTime, 
            DateTime outTime, 
            string violationType,
            string offenselevel,string refCode)
        {
            List<violationHistory> list = new List<violationHistory>();
            try
            {
                double price = 0;
                ScanOperationConfig config = new ScanOperationConfig();
                //Expression exp = new Expression();
                //exp.ExpressionStatement = "attribute_not_exists(Issuedby)";
                //config.FilterExpression = exp;
                config.Filter = new ScanFilter();
                config.Filter.AddCondition("invoiceID", ScanOperator.NotEqual, new DynamoDBEntry[] { "info" });
                config.Filter.AddCondition("Issuedby", ScanOperator.IsNotNull);

                if (!string.IsNullOrEmpty(plateNumber) && plateNumber != "undefined")
                {
                    config.Filter.AddCondition("plateNumber", ScanOperator.Equal, new DynamoDBEntry[] { plateNumber });
                }

                if (!string.IsNullOrEmpty(zone) && zone != "undefined")
                {
                    config.Filter.AddCondition("parkingID", ScanOperator.Equal, new DynamoDBEntry[] { zone });
                }

                if (!string.IsNullOrEmpty(violationType) && violationType!= "undefined")
                {
                    config.Filter.AddCondition("violationType", ScanOperator.Contains, new DynamoDBEntry[] { violationType });
                }
                if (!string.IsNullOrEmpty(offenselevel) && offenselevel != "undefined")
                {
                    config.Filter.AddCondition("Offense level", ScanOperator.Equal, new DynamoDBEntry[] { offenselevel });
                }
                if (!string.IsNullOrEmpty(refCode) && refCode != "undefined")
                {
                    config.Filter.AddCondition("Reference code", ScanOperator.Equal, new DynamoDBEntry[] { refCode });
                }

                if (DateTimeHelper.IsValidDateTime(inTime, outTime))
                {
                    config.Filter.AddCondition("dateFrom", ScanOperator.Between, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(inTime).ToString(), DateTimeHelper.getUnixTimeStamp(outTime).ToString() });
                    //config.Filter.AddCondition("dateFrom", ScanOperator.LessThanOrEqual, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(outTime).ToString() });
                }

                var zones = await GetZones();
                var search = db.FromScanAsync<ParkingViolation>(config);
                var data = await search.GetRemainingAsync();

                int i = 0;
                foreach (var p in data)
                {
                    i = i + 1;
                    violationHistory hist = new violationHistory();
                    hist.index = i;
                    hist.plateNumber = p.plateNumber;
                    hist.zone = GetParkingNameFromID(zones, p.parkingID);
                    hist.date = DateTimeHelper.getDateTimeFormUnixTimeStamp(Convert.ToInt64(p.dateFrom)).ToString("dd/MM/yyyy hh:mm:ss tt");
                    hist.dueDate = DateTimeHelper.getDateTimeFormUnixTimeStamp(Convert.ToInt64(p.DueDate)).ToString("dd/MM/yyyy hh:mm:ss tt");
                    hist.fine = String.Format("{0:F2}", p.Fine);
                    hist.offenseLevel = p.Offenselevel;
                    hist.reference = p.ReferenceCode;
                    hist.violationType = p.Category;
                    string pdateStr = "";
                    long paymentDate = 0;
                    if(long.TryParse(pdateStr, out paymentDate))
                    {
                        hist.paymentDate = DateTimeHelper.getDateTimeFormUnixTimeStamp(paymentDate).ToString();
                        hist.paid = "YES";
                    }
                    else
                    {
                        hist.paymentDate = "";
                        hist.paid = "NO";
                    }
                    
                    list.Add(hist);
                }
            }
            catch(Exception ex)
            {

            }
            

           
            return list;
        }

        public async Task<List<parkingHistory>> GetParkingHisotry(
            string plateNumber,
            string zone,
            DateTime inTime,
            DateTime outTime)
        {
            List<parkingHistory> list = new List<parkingHistory>();
            try
            {
                double price = 0;
                ScanOperationConfig config = new ScanOperationConfig();
                config.Filter = new ScanFilter();
                config.Filter.AddCondition("invoiceID", ScanOperator.NotEqual, new DynamoDBEntry[] { "info" });
                config.Filter.AddCondition("price", ScanOperator.GreaterThan, new DynamoDBEntry[] { price });

                if (!string.IsNullOrEmpty(plateNumber) && plateNumber != "undefined")
                {
                    config.Filter.AddCondition("plateNumber", ScanOperator.Equal, new DynamoDBEntry[] { plateNumber });
                }

                if (!string.IsNullOrEmpty(zone) && zone != "undefined")
                {
                    config.Filter.AddCondition("parkingID", ScanOperator.Equal, new DynamoDBEntry[] { zone });
                }


                if (DateTimeHelper.IsValidDateTime(inTime, outTime))
                {
                    config.Filter.AddCondition("dateFrom", ScanOperator.GreaterThanOrEqual, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(inTime).ToString() });
                    config.Filter.AddCondition("dateBook", ScanOperator.LessThanOrEqual, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(outTime).ToString() });
                }

                var zones = await GetZones();
                var search = db.FromScanAsync<ParkingTrans>(config);
                var data = await search.GetRemainingAsync();

                int i = 0;
                foreach (var p in data)
                {
                    i = i + 1;
                    parkingHistory hist = new parkingHistory();
                    hist.index = i;
                    hist.plateNumber = p.plateNumber;
                    hist.zone = GetParkingNameFromID(zones, p.parkingID);
                    hist.inTime = DateTimeHelper.getDateTimeFormUnixTimeStamp(Convert.ToInt64(p.dateFrom)).ToString("dd/MM/yyyy hh:mm:ss tt");
                    hist.outTime = DateTimeHelper.getDateTimeFormUnixTimeStamp(Convert.ToInt64(p.dateBook)).ToString("dd/MM/yyyy hh:mm:ss tt");
                    hist.amount = String.Format("{0:F2}", p.price);
                    hist.duration = DateTimeHelper.getHoursMinsFromUnixTimeStamp(Convert.ToInt64(p.dateFrom), Convert.ToInt64(p.dateBook));
                    list.Add(hist);
                }
            }
            catch(Exception ex)
            {

            }
            
            return list;
        }

        async Task<List<Parking>> GetZones()
        {
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" })};
            var search = db.ScanAsync<Parking>(sc);
            var data = await search.GetRemainingAsync();

            return data.ToList();
        }

        string GetParkingNameFromID(List<Parking> zones, string parkingID)
        {
            string parkingName = "";
            foreach(Parking p in zones)
            {
                if(p.parkingID == parkingID)
                {
                    parkingName = p.title;
                    break;
                }
            }

            return parkingName;
        }

        public async Task<List<parkingUserInfo>> GetParkingUser(
            string name,
            string email,
             string phone,
            DateTime inTime,
            DateTime outTime)
        {
            List<parkingUserInfo> list = new List<parkingUserInfo>();
            try
            {
                ScanOperationConfig config = new ScanOperationConfig();
                config.Filter = new ScanFilter();
                //config.Filter.AddCondition("invoiceID", ScanOperator.NotEqual, new DynamoDBEntry[] { "info" });

                if (!string.IsNullOrEmpty(name) && name != "undefined")
                {
                    config.Filter.AddCondition("fullName", ScanOperator.Equal, new DynamoDBEntry[] { name });
                }

                if (!string.IsNullOrEmpty(email) && email != "undefined")
                {
                    config.Filter.AddCondition("email", ScanOperator.Equal, new DynamoDBEntry[] { email });
                }

                if (!string.IsNullOrEmpty(phone) && phone != "undefined")
                {
                    config.Filter.AddCondition("phone", ScanOperator.Equal, new DynamoDBEntry[] { phone });
                }

                if (DateTimeHelper.IsValidDateTime(inTime, outTime))
                {
                    config.Filter.AddCondition("date", ScanOperator.Between, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(inTime).ToString(), DateTimeHelper.getUnixTimeStamp(outTime).ToString() });
                }
                
                var search = db.FromScanAsync<ParkingUserInfo>(config);
                var data = await search.GetRemainingAsync();

                int i = 0;
                foreach (var p in data)
                {
                    i = i + 1;
                    parkingUserInfo userInfo = new parkingUserInfo();
                    userInfo.index = i;
                    userInfo.name = p.fullName;
                    userInfo.email = p.email;
                    userInfo.phone = p.phone;
                    userInfo.date = DateTimeHelper.getDateTimeFormUnixTimeStamp(Convert.ToInt64(p.date)).ToString("dd/MM/yyyy hh:mm:ss tt");
                    list.Add(userInfo);
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }
        public async Task<List<registerVehicle>> GetRegisterVehicle(
            string plateNumber,
            string model,
             string userName,
            DateTime inTime,
            DateTime outTime)
        {
            List<registerVehicle> list = new List<registerVehicle>();
            try
            {
                ScanOperationConfig config = new ScanOperationConfig();
                config.Filter = new ScanFilter();
                //config.Filter.AddCondition("invoiceID", ScanOperator.NotEqual, new DynamoDBEntry[] { "info" });

                if (!string.IsNullOrEmpty(plateNumber) && plateNumber != "undefined")
                {
                    config.Filter.AddCondition("plateNumber", ScanOperator.Equal, new DynamoDBEntry[] { plateNumber });
                }

                if (!string.IsNullOrEmpty(model) && model != "undefined")
                {
                    config.Filter.AddCondition("model", ScanOperator.Equal, new DynamoDBEntry[] { model });
                }

                if (!string.IsNullOrEmpty(userName) && userName != "undefined")
                {
                    config.Filter.AddCondition("userName", ScanOperator.Equal, new DynamoDBEntry[] { userName });
                }

                if (DateTimeHelper.IsValidDateTime(inTime, outTime))
                {
                    config.Filter.AddCondition("date", ScanOperator.Between, new DynamoDBEntry[] { DateTimeHelper.getUnixTimeStamp(inTime).ToString(), DateTimeHelper.getUnixTimeStamp(outTime).ToString() });
                }

                var search = db.FromScanAsync<RegisterVehicle>(config);
                var data = await search.GetRemainingAsync();

                int i = 0;
                foreach (var p in data)
                {
                    i = i + 1;
                    registerVehicle vehicle = new registerVehicle();
                    vehicle.index = i;
                    vehicle.plateNumber = p.plateNumber;
                    vehicle.model = p.model;
                    vehicle.userName = p.fullName;
                    list.Add(vehicle);
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }
    }
}
