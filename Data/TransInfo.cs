
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
    public class TransInfo
    {
        IDynamoDBContext db;

        public TransInfo(IDynamoDBContext db)
        {
            this.db = db;
        }


        public async Task<transactions> GetNewTransaction(int Month)
        {
            transactions trans = new transactions();
            try
            {
                int previousMonthCount = 0;
                int thisMonthCount = 0;

                thisMonthCount = await GetTransCountByMonth(Month);
                previousMonthCount = await GetTrans();

                double percent = PercentageHelper.GetPercentage(thisMonthCount, previousMonthCount);

                trans.count = String.Format("{0:n0}", thisMonthCount);
                trans.month = DateTimeHelper.GetMonthName(Month);
                trans.percent = String.Format("{0:F2}", percent);
            }
            catch(Exception ex)
            {

            }
            
            return trans;
        }

        async Task<int> GetTransCountByMonth(int Month)
        {
            int cnt = 0;
            string from = DateTimeHelper.getUnixTimeStampFromStartOfMonth(Month).ToString();
            string to = DateTimeHelper.getUnixTimeStampFromEndOfMonth(Month).ToString();
            ScanCondition[] sc = new ScanCondition[] {

                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }),
                new ScanCondition("dateFrom", ScanOperator.Between, new object[] { from, to })};
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            cnt = data.Count;
            return cnt;
        }

        async Task<int> GetTrans()
        {
            int cnt = 0;

            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("invoiceID", ScanOperator.NotEqual, new object[] { "info" }) };
            var search = db.ScanAsync<ParkingUser>(sc);
            var data = await search.GetRemainingAsync();
            cnt = data.Count;
            return cnt;
        }
    }
}
