using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.SmartParking.Admin.Data;
using NY.SmartParking.Web.Models.ParkingAttendantViewModels;
using SmartParking.Admin.Models;

namespace NY.SmartParking.Web.Controllers
{
    [Authorize(Roles = "admin,attendant")]
    public class ParkingAttendantController : Controller
    {
        private readonly IDynamoDBContext context;
        ParkingInfoList infoList;

        public ParkingAttendantController(IDynamoDBContext context,ParkingInfoList infoList)
        {
            this.context = context;
            this.infoList = infoList;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Save(ParkingAttendant parkingAttendant)
        {
            var userId = User.Identity.Name;
            Guid invoiceId = Guid.NewGuid();
            parkingAttendant.userId = userId.ToString();
            parkingAttendant.invoiceId = invoiceId.ToString();
            var parking = new ParkingUserRegister()
            {
                userID = parkingAttendant.userId,
                invoiceID = parkingAttendant.invoiceId,
                dateBook = parkingAttendant.dateBook,
                dateFrom = parkingAttendant.dateFrom,
                parkingID = parkingAttendant.parkingID,
                plateNumber = parkingAttendant.plateNumber,
                price = parkingAttendant.cost,
                slotNumber = parkingAttendant.slotNumber,
                phone = parkingAttendant.phoneNumber
            };
            try
            {
                context.SaveAsync(parking);
            }catch(Exception ex)
            {

            }
            
            string message = "Successfully Save";
            return Json(new { message });
        }
        public async Task<JsonResult> GetAllParkingByInfo()
        {
            var data = await infoList.GetParkingListByInfo();
            List<string> rate500Lists = new List<string>();
            List<string> rate300Lists = new List<string>();
            foreach (var i in data)
            {
                string rate500=string.Empty;
                string rate300 = string.Empty;
                if (i.rate == 500)
                {
                    rate500 = i.parkingID;
                    rate500Lists.Add(rate500);
                }
                else if (i.rate == 300)
                {
                    rate300 = i.parkingID;
                    rate300Lists.Add(rate300);
                }
                
            }
            return Json(new { data, rate500Lists, rate300Lists });
        }
    }
}