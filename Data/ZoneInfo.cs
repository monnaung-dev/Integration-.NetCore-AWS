using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using SmartParking.Admin.Models;
using SmartParking.Admin.Models.JsonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Data
{
    public class ZoneInfo
    {
        IDynamoDBContext db;

        public ZoneInfo(IDynamoDBContext db)
        {
            this.db = db;
        }
        public async void Get()
        {
            ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" }) };
            var search = db.ScanAsync<Parking>(sc);
            var res = await search.GetRemainingAsync();
           
            foreach (var v in res)
            {
                Console.WriteLine("User " + v.parkingID + "...." + " count " + v.slots.Count);
            }
        }

        public async Task<List<parkingZoneList>> GetZones()
        {
            List<parkingZoneList> zones = new List<parkingZoneList>();
            try
            {
                ScanCondition[] sc = new ScanCondition[] {
                new ScanCondition("slotNumber", ScanOperator.Equal, new object[] { "info" }) };
                var search = db.ScanAsync<Parking>(sc);
                var res = await search.GetRemainingAsync();

                foreach (var v in res)
                {
                    parkingZoneList p = new parkingZoneList();
                    p.id = v.parkingID;
                    p.name = v.title;
                    zones.Add(p);
                }
            }
            catch(Exception ex)
            {
                

                
            }
            return zones;
        }

    }
}
