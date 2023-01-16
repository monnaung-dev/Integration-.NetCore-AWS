using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using SmartParking.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.SmartParking.Admin.Data
{
    public class ParkingInfoList
    {
        IDynamoDBContext db;
        public ParkingInfoList(IDynamoDBContext db)
        {
            this.db = db;
        }
        public async Task<List<Parking>> GetParkingListByInfo()
        {
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" })};
            var search = db.ScanAsync<Parking>(sc);
            var data = await search.GetRemainingAsync();

            return data.ToList();
        }
    }
}
