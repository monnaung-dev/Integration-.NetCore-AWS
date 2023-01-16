﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models
{
    [DynamoDBTable("ParkingUser")]
    public class ParkingUser
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
        public double price
        {
            get; set;
        }

        [DynamoDBProperty]
        public string dateFrom
        {
            get; set;
        }

        [DynamoDBProperty]
        public string dateTo
        {
            get; set;
        }

        [DynamoDBProperty]
        public string parkingID
        {
            get; set;
        }

        [DynamoDBProperty]
        public string plateNumber
        {
            get; set;
        }
    }
}
