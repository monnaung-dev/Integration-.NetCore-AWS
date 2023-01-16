using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class parkingLotSummary
    {
        public int total { get; set; }
        public List<parkingLot> lots { get; set; }
    }

    public class parkingLot
    {
        public string count { get; set; }
        public string percent { get; set; }
        public string pricePerHour { get; set; }
        public string color { get; set; }
    }
}
