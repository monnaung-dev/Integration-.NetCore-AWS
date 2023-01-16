using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models
{
    [DynamoDBTable("ParkingUser")]
    public class RegisterVehicle
    {
        [DynamoDBHashKey] //Partition key
        public string userID
        {
            get; set;
        }

        [DynamoDBRangeKey] //Sort key
        public string invoiceID
        {
            get; set;
        }


        [DynamoDBProperty]
        public string plateNumber
        {
            get; set;
        }

        [DynamoDBProperty]
        public string model
        {
            get; set;
        }


        [DynamoDBProperty]
        public string fullName
        {
            get; set;
        }
        
    }
}
