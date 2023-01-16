using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Models.JsonViewModels
{
   

    public class parkingUserInfo
    {
        public int index { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string date { get; set; }        
    }

}
