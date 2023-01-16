using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{

    public class parkinginfo
    {
        public string index { get; set; }
        public string zone { get; set; }
        public string total { get; set; }
        public string free { get; set; }
        public string occupied { get; set; }
        public string paid { get; set; }

    }
}
