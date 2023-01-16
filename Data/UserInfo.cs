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
    public class UserInfo
    {
        IDynamoDBContext db;

        public UserInfo(IDynamoDBContext db)
        {
            this.db = db;
        }


        public async Task<newusers> GetNewUsers(int Month)
        {
            newusers users = new newusers();
            try
            {
                int previousMonthCount = 0;
                int thisMonthCount = 0;

                thisMonthCount = await GetUserCountByMonth(Month);
                previousMonthCount = await GetUsers();
                double percent = PercentageHelper.GetPercentage(thisMonthCount, previousMonthCount);

                users.count = String.Format("{0:n0}", thisMonthCount);
                users.month = DateTimeHelper.GetMonthName(Month);
                users.percent = String.Format("{0:F2}", percent);
            }
            catch(Exception ex)
            {

            }
            
            return users;
        }

        async Task<int> GetUserCountByMonth(int Month)
        {
            int cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.Equal, new object[] { "info" }),
                new ScanCondition("date", ScanOperator.Between, new object[] { DateTimeHelper.getUnixTimeStampFromStartOfMonth(Month), DateTimeHelper.getUnixTimeStampFromEndOfMonth(Month) })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            cnt = data.Count;
            return cnt;
        }

        async Task<int> GetUsers()
        {
            int cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.Equal, new object[] { "info" }) };
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            cnt = data.Count;
            return cnt;
        }
    }
}
