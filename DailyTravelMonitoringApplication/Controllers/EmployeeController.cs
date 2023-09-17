using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyTravelMonitoringApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly TravelingTeam_DB_Context _dbContext = new TravelingTeam_DB_Context();
        
        // GET: Employee
        public ActionResult Index()
        {            
            return View(_dbContext.Employees.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee value)
        {
            List<DailyTravelMonitoring> list = new List<DailyTravelMonitoring>();
            if (ModelState.IsValid)
            {
                DailyTravelMonitoring obj = new DailyTravelMonitoring();
                list.Add(obj);
                value.DailyTravelMonitorings = list;
                _dbContext.Employees.Add(value);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Delete(int? id)
        {

            return View(_dbContext.Employees.Find(id));
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult GetDelete(int? id)
        {
            if (id != null)
            {
                Employee obj = _dbContext.Employees.Find(id);

                _dbContext.Employees.Remove(obj);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(_dbContext.Employees.Find(id));
        }
        
    }
}