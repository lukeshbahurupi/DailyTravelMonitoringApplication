using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyTravelMonitoringApplication.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly TravelingTeam_DB_Context _dbContext = new TravelingTeam_DB_Context();

        // GET: Employee
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {            
            return View(_dbContext.Employees.ToList());
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Employee value)
        {
            List<DailyTravelMonitoring> list = new List<DailyTravelMonitoring>();
            //List<FileDetail> fileDetails = new List<FileDetail>();

            try
            {
                if (ModelState.IsValid)
                {
                    DailyTravelMonitoring obj = new DailyTravelMonitoring();
                    //FileDetail fileDetail = new FileDetail();
                    list.Add(obj);
                    //fileDetails.Add(fileDetail);
                    value.DailyTravelMonitorings = list;



                    _dbContext.Employees.Add(value);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
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
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            return View(_dbContext.Employees.Find(id));
        }
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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
        public ActionResult Edit(int? id)
        {
            return View(_dbContext.Employees.Find(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Employee value)
        {
            if (ModelState.IsValid)
            {            
                _dbContext.Entry(value).State = EntityState.Modified; 
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            return View(_dbContext.Employees.Find(id));
        }
    }
}