using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Admin.Data;
using SmartParking.Admin.Helpers;
using SmartParking.Admin.Models.JsonViewModels;

namespace SmartParking.Admin.Web.Controllers
{
    [Authorize(Roles ="admin,audit")]
    public class DashboardController : Controller
    {
        private readonly IDynamoDBContext context;
        UserInfo userInfo;
        TransInfo transInfo;
        RevenueInfo revenueInfo;
        OccupancyInfo occupancyInfo;
        ParkingInfo parkingInfo;
        CustomerReviewsInfo reviewInfo;

        public DashboardController(
            IDynamoDBContext ctx, 
            UserInfo userInfo,
            TransInfo transInfo,
            RevenueInfo revenueInfo,
            OccupancyInfo occupancyInfo,
            ParkingInfo parkingInfo,
            CustomerReviewsInfo reviewInfo)
        {
            this.context = ctx;

            this.userInfo = userInfo;
            this.transInfo = transInfo;
            this.revenueInfo = revenueInfo;
            this.occupancyInfo = occupancyInfo;
            this.parkingInfo = parkingInfo;
            this.reviewInfo = reviewInfo;
        }

        [HttpGet]
        public async Task<JsonResult> GetUsers(int month)
        {
            var data = await userInfo.GetNewUsers(month);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetTrans(int month)
        {
            var data = await transInfo.GetNewTransaction(month);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetRevenues(int month)
        {
            var data = await revenueInfo.GetNewRevenues(month);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetOccupancy(int month)
        {
            var data = await occupancyInfo.GetNewOccupancy(month);
            return Json(data);
        }


        [HttpGet]
        public async Task<JsonResult> GetParkingLotSummary()
        {
            var data = await revenueInfo.GetParkingLotSummary();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetFinesAndSettlements(int month)
        {
            var data = await parkingInfo.GetFineAndSettlementCount(month);
            return Json(data);
        }


        [HttpGet]
        public async Task<JsonResult> GetDailyDurationTrend(string date)
        {
            var data = await occupancyInfo.GetDailyParkingDurationTrends(DateTimeHelper.GetDateTimeFromDateString(date));
            return Json(data);
        }


        [HttpGet]
        public async Task<JsonResult> GetOccupancyPercentage(string date)
        {
            var data = await occupancyInfo.GetDailtyOccupancyPercentage(DateTimeHelper.GetDateTimeFromDateString(date));
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetOccupancyTrend(string date)
        {
            var data = await occupancyInfo.GetDailyOccupancyPercentageByTimeOfDays(DateTimeHelper.GetDateTimeFromDateString(date));
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomerReviews()
        {
            var data = await reviewInfo.GetReviews();
            return Json(data);
        }       


        
    }
}
