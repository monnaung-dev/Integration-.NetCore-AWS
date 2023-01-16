
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Helpers
{
    public static class PercentageHelper
    {

        public static double GetPercentage(double now, double previous)
        {
            double percentage = 0;
            percentage = (now / previous) * 100;
            return percentage;
        }
    }
}
