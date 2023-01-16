using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models
{
    [DynamoDBTable("ParkingUser")]
    public class ParkingViolation
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
        public long date
        {
            get; set;
        }

        [DynamoDBProperty]
        public string dateFrom
        {
            get; set;
        }

        [DynamoDBProperty("Due Date")]
        public string DueDate
        {
            get; set;
        }

        [DynamoDBProperty]
        public string Evidence
        {
            get; set;
        }

        [DynamoDBProperty]
        public string Fine
        {
            get; set;
        }

        [DynamoDBProperty("Fine Status")]
        public string FineStatus
        {
            get; set;
        }

        [DynamoDBProperty("Issuedby")]
        public string Issuedby
        {
            get; set;
        }

        [DynamoDBProperty("Offense level")]
        public string Offenselevel
        {
            get; set;
        }

        [DynamoDBProperty("Payment Date")]
        public string PaymentDate
        {
            get; set;
        }

        [DynamoDBProperty]
        public string parkingID
        {
            get; set;
        }


        [DynamoDBProperty]
        public string Category
        {
            get; set;
        }

        [DynamoDBProperty]
        public string plateNumber
        {
            get; set;
        }

        [DynamoDBProperty("Reference code")]
        public string ReferenceCode
        {
            get; set;
        }

        [DynamoDBProperty]
        public string slotNumber
        {
            get; set;
        }
    }
}
