using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class violationHistory
    {
        public int index { get; set; }
        public string plateNumber { get; set; }
        public string zone { get; set; }
        public string date { get; set; }
        public string dueDate { get; set; }
        public string violationType { get; set; }
        public string offenseLevel { get; set; }
        public string reference { get; set; }
        public string fine { get; set; }
        public string paid { get; set; }
        public string paymentDate { get; set; }
    }
    
}
