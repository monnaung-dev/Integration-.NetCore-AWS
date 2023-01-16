using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class occupancy
    {
        public string count { get; set; }
        public string percent { get; set; }
        public string month { get; set; }
    }
}
