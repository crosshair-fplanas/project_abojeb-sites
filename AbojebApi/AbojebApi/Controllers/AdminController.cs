using AbojebApi.Core.Enums;
using AbojebApi.Data;
using AbojebApi.Data.Services;
using AbojebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AbojebApi.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vessel()
        {
            return View();
        }

        [HttpPost]
        public string UpdateVesselData()
        {
            ReturnMessageViewModel msg;
            try
            {
                //TODO: uncomment
                ApiService apiSvc = new ApiService();
                apiSvc.GetAllVesselReports();

                msg = new ReturnMessageViewModel
                {
                    Success = true,
                    Title = "Info",
                    Message = "Records successfully updated."
                };
            }
            catch (Exception ex)
            {
                msg = new ReturnMessageViewModel
                {
                    Success = false,
                    Title = "Error",
                    Message = ex.Message
                };
            }

            return JsonConvert.SerializeObject(msg);
        }

        public async Task<ActionResult> Accounts()
        {
            AccountModel am = new AccountModel();
            var users = await am.GetUsersAsync();

            return View(users.Select(s => s.Convert<ApplicationUser, AccountViewModel>()));
        }
    }
}