using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.SmartParking.Web.Models.ParkingAttendantViewModels
{
    public class ParkingAttendant
    {
        public string userId { get; set; }
        public string parkingID { get; set; }
        public string time { get; set; }
        public string phoneNumber { get; set; }
        public string plateNumber { get; set; }
        public string slotNumber { get; set; }
        public double cost { get; set; }
        public string dateBook { get; set; }
        public string dateFrom { get; set; }
        public string invoiceId { get; set; }
    }
}
