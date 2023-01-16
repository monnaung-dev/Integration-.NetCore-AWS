using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class dailyParkingDurationTrend
    {
        public string day { get; set; }
        public List<dailyParkingDurationTrendZone> Zones { get; set; }
    }

    public class dailyParkingDurationTrendZone
    {
        public string name { get; set; }
        public List<dailyOccupancyParkingLot> hours { get; set; }

    }


}
