using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.SmartParking.Web.Models;
using SmartParking.Admin.Data;
using SmartParking.Admin.Helpers;

namespace NY.SmartParking.Web.Controllers
{
    [Authorize(Roles ="admin,audit")]
    public class ReportController : Controller
    {
        private readonly IDynamoDBContext context;
        ParkingInfo parkingInfo;
        OccupancyInfo occuInfo;
        ZoneInfo zoneInfo;

        public ReportController(
            IDynamoDBContext ctx,
            ParkingInfo parkingInfo,
            OccupancyInfo occuInfo,
            ZoneInfo zoneInfo)
        {
            this.context = ctx;
            this.parkingInfo = parkingInfo;
            this.occuInfo = occuInfo;
            this.zoneInfo = zoneInfo;
        }

        public IActionResult ParkingInfo()
        {
         
            return View();
        }


        public IActionResult ParkingHistory()
        {
            return View();
        }

        public IActionResult Violation()
        {
            return View();
        }

        public IActionResult ParkingUsers()
        {
            return View();
        }

        public IActionResult RegisteredVehicles()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> RegisterVehicle(string plateNumber,
            string model, string userName, string inTime, string outTime)
        {
            DateTime dtInTime = DateTime.MinValue;
            DateTime dtOutTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(inTime))
            {
                string InTime = inTime + " 00:00:00";
                dtInTime = DateTimeHelper.GetDateWithTimeFromDateString(InTime);
            }

            if (!string.IsNullOrEmpty(outTime))
            {
                string OutTime = outTime + " 23:59:59";
                dtOutTime = DateTimeHelper.GetDateWithTimeFromDateString(OutTime);
            }

            var data = await parkingInfo.GetRegisterVehicle(plateNumber, model, userName, dtInTime, dtOutTime);
            return Json(data);
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetParkingInfo()
        {
            DateTime dtDateTime = DateTime.UtcNow;
            var data = await occuInfo.GetParkingInfoByTimeOfDay(dtDateTime);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetZones()
        {
            var data = await zoneInfo.GetZones();
            return Json(data);
        }


        [HttpGet]
        public async Task<JsonResult> GetParkingHistory(string plateNumber,
            string zone, string inTime, string outTime)
        {
            DateTime dtInTime = DateTime.MinValue;
            DateTime dtOutTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(inTime))
            {
                string InTime = inTime + " 00:00:00";
                dtInTime = DateTimeHelper.GetDateWithTimeFromDateString(InTime);
            }

            if (!string.IsNullOrEmpty(outTime))
            {
                string OutTime = outTime + " 23:59:59";
                dtOutTime = DateTimeHelper.GetDateWithTimeFromDateString(OutTime);
            }

            var data = await parkingInfo.GetParkingHisotry(plateNumber, zone, dtInTime, dtOutTime);
            if(data != null)
            {
                if(data.Count > 0)
                {
                    data = data.OrderBy(o => o.inTime).ToList();
                }
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetViolationHistory(string plateNumber,
            string zone, string inTime, string outTime, string violationType,string offenseLevel,
            string refCode)
        {
            DateTime dtInTime = DateTime.MinValue;
            DateTime dtOutTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(inTime))
            {
                string InTime = inTime + " 00:00:00";
                dtInTime = DateTimeHelper.GetDateWithTimeFromDateString(InTime);
            }

            if (!string.IsNullOrEmpty(outTime))
            {
                string OutTime = outTime + " 23:59:59";
                dtOutTime = DateTimeHelper.GetDateWithTimeFromDateString(OutTime);
            }

            var data = await parkingInfo.GetViolationHisotry(plateNumber, zone, dtInTime, dtOutTime, violationType,offenseLevel,refCode);
            if (data != null)
            {
                if (data.Count > 0)
                {
                    data = data.OrderBy(o => o.date).ToList();
                }
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetParkingUsers(string name,
            string email,string phone, string inTime, string outTime)
        {
            DateTime dtInTime = DateTime.MinValue;
            DateTime dtOutTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(inTime))
            {
                string InTime = inTime + " 00:00:00";
                dtInTime = DateTimeHelper.GetDateWithTimeFromDateString(InTime);
            }

            if (!string.IsNullOrEmpty(outTime))
            {
                string OutTime = outTime + " 23:59:59";
                dtOutTime = DateTimeHelper.GetDateWithTimeFromDateString(OutTime);
            }

            var data = await parkingInfo.GetParkingUser(name,email,phone,dtInTime, dtOutTime);            
            return Json(data);
        }
    }
}
