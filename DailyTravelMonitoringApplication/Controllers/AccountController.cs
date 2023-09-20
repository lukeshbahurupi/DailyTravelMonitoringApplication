using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
           
            try
            {
                    if (ModelState.IsValid)
                {
               
                    Employee emp = _dbContext.Employees.FirstOrDefault(el => el.MobileNo == model.MobileNo);
                    List<DailyTravelMonitoring> list = _dbContext.DailyTravelMonitorings.Where(el => el.EmployeeId == emp.EmployeeId).ToList();
                    bool isValidUser = _dbContext.Users.Any(user => user.UniqeId == model.UniqeId && user.MobileNo == model.MobileNo && user.Password == model.Password);
                    if(isValidUser)
                    {
                        FormsAuthentication.SetAuthCookie(model.MobileNo, false);
                        DailyTravelMonitoring obj = new DailyTravelMonitoring();
                        obj.TravelDate = DateTime.Today;
                        if(!list.Any(el => el.TravelDate == DateTime.Today))
                        {
                            DailyTravelMonitoring newObj = new DailyTravelMonitoring() { TravelDate = DateTime.Today, EmployeeId = emp.EmployeeId};
                            _dbContext.DailyTravelMonitorings.Add(newObj);
                            _dbContext.SaveChanges();
                        }

                        return RedirectToAction("Details", "Monitoring",new {obj.ID});
                    }
                }
            }
            catch
            {
                return View("Error");
            }

            
            ModelState.AddModelError("", "Login from other mobile");
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User model) 
        { 
            List<UserRolesMapping> rolesMapping = new List<UserRolesMapping>();
            try
            {
                if (ModelState.IsValid)
                {
                    Employee el = _dbContext.Employees.FirstOrDefault(e => e.MobileNo == model.MobileNo && e.Email == model.Email);
                    if (el.Name.ToLower() == model.Name.ToLower() && el.MobileNo == model.MobileNo && el.Email == model.Email)
                    {
                        UserRolesMapping obj = new UserRolesMapping();
                        obj.RoleID = 2;
                        rolesMapping.Add(obj);
                        model.UserRolesMappings=rolesMapping;
                        _dbContext.Users.Add(model);
                        _dbContext.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    
                }
                ModelState.AddModelError("", "Somthing Went Wrong");
                return View();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return View();           
        }



        /**************************************************************************/


        public ActionResult ADLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ADLogin(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {           
            
                    User obj = _dbContext.Users.FirstOrDefault(el => el.MobileNo == model.MobileNo && el.Password == model.Password);
                    if(model.UniqeId == null && obj.UniqeId.Length==12)
                    {
                    model.UniqeId = obj.UniqeId;
                    }
                    bool isValidUser = _dbContext.Users.Any(user => user.UniqeId == model.UniqeId && user.MobileNo == model.MobileNo && user.Password == model.Password);
                    if (isValidUser)
                    {
                        FormsAuthentication.SetAuthCookie(model.MobileNo, false);
                        return RedirectToAction("Index", "Monitoring");
                    }
                }
            }
            catch
            {
                return View("Error");
            }
            ModelState.AddModelError("", "Not valid User");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User model) 
        {
            try
            {
                if (ModelState.IsValid)
                {

            
                    List<UserRolesMapping> rolesMapping = new List<UserRolesMapping>();
                    if (ModelState.IsValid)
                    {
                        UserRolesMapping obj = new UserRolesMapping();
                        obj.RoleID = 1;
                        rolesMapping.Add(obj);
                       model.UserRolesMappings=rolesMapping;
                        _dbContext.Users.Add(model);
                        _dbContext.SaveChanges();
                        return RedirectToAction("ADLogin");
                    }
                }
            }
            catch
            {
                return View("Error");
            }
            ModelState.AddModelError("", "Somthing Went Wrong");
            return View();            
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            return View(_dbContext.Users.Find(id));
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_dbContext.Users.ToList());
        }
    }
}