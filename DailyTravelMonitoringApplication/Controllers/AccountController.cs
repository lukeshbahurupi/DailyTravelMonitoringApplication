using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace DailyTravelMonitoringApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly TravelingTeam_DB_Context _dbContext = new TravelingTeam_DB_Context();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            bool isValidUser = _dbContext.Users.Any(user => user.UniqeId == model.UniqeId && user.MobileNo == model.MobileNo && user.Password == model.Password);
            if(isValidUser)
            {
                FormsAuthentication.SetAuthCookie(model.MobileNo, false);
                return RedirectToAction("Index", "Monitoring");
            }
            ModelState.AddModelError("", "Not valid User");
            return View();
        }
    }
}