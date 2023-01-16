using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class dailyOccupancyTrend
    {
        public string day { get; set; }
        public List<dailyOccupancyTrendParkingLot> parkingLots { get; set; }
    }

    public class dailyOccupancyTrendParkingLot
    {
        public string time { get; set; }
        public string count { get; set; }

    }


}
