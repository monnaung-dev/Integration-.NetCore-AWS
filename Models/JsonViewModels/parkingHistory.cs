using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
   

    public class parkingHistory
    {
        public int index { get; set; }
        public string plateNumber { get; set; }
        public string zone { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string duration { get; set; }
        public string amount { get; set; }
    }

}
