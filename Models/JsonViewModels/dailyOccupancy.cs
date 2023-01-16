using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
    public class dailyOccupancy
    {
        public string day { get; set; }
        public List<dailyOccupancyParkingLot> parkingLots { get; set; }
    }

    public class dailyOccupancyParkingLot
    {
        public string count { get; set; }
        public string name { get; set; }

    }


}
