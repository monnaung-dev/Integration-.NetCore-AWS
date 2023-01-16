using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models
{
    [DynamoDBTable("Parking")]
    public class Parking
    {
        [DynamoDBHashKey] //Partition key
        public string parkingID
        {
            get; set;
        }

        [DynamoDBRangeKey] //Sort key
        public string slotNumber
        {
            get; set;
        }

        
        
        [DynamoDBProperty]
        public int freeSlots
        {
            get; set;
        }


        [DynamoDBProperty]
        public List<slot> slots
        {
            get; set;
        }


        [DynamoDBProperty]
        public string title
        {
            get; set;
        }

        [DynamoDBProperty]
        public double rate
        {
            get; set;
        }

        [DynamoDBProperty]
        public address address
        {
            get; set;
        }
    }


    public class slot
    {
        public int device { get; set; }
        public string parkingID { get; set; }
        public string slotNumber { get; set; }
        public int slotStatus { get; set; }
        

    }

    public class address
    {
        public string city { get; set; }
        public string countryCode { get; set; }
        public string line1 { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }

    }
}
