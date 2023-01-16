using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.SmartParking.Web.Models;

namespace NY.SmartParking.Web.Controllers
{
    [Authorize(Roles ="admin")]
    public class MonitoringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
