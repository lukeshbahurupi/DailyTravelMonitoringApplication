using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DailyTravelMonitoringApplication.Controllers
{
    //[Authorize]
    public class MonitoringController : Controller
    {
        private readonly TravelingTeam_DB_Context _dbContext = new TravelingTeam_DB_Context();
        // GET: Monitoring
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_dbContext.DailyTravelMonitorings.ToList());
        }
        [Authorize(Roles = "User")]
        public ActionResult Lists(int? id)
        {            
            List<DailyTravelMonitoring> list = _dbContext.DailyTravelMonitorings.Where(el => el.EmployeeId == id).ToList();
            Employee ele = _dbContext.Employees.FirstOrDefault(el => el.MobileNo == User.Identity.Name);
            if(ele.EmployeeId == id)
            {
                return View(list);
            }
            return RedirectToAction("Details");
        }

        [Authorize(Roles ="Admin,User")]
        public ActionResult Details(int? id)
        {
            try
            {
                 if (id != null && id != 0)
                {
                    DailyTravelMonitoring obj = _dbContext.DailyTravelMonitorings.Find(id);
                    User user = _dbContext.Users.FirstOrDefault(el => el.MobileNo == User.Identity.Name);
                    if (user.Name == obj.Employee.Name)
                    {
                        return View(obj);
                    }
                }
            else
            {
                if (id == null || id == 0)
                {
                    Employee emp = _dbContext.Employees.FirstOrDefault(el => el.MobileNo == User.Identity.Name);
                    //return View(_dbContext.DailyTravelMonitorings.FirstOrDefault(el => el.EmployeeId == emp.EmployeeId));
                    return RedirectToRoute(new { controller = "Monitoring", action = "Lists", id = emp.EmployeeId });
                }

            }
            }
            catch 
            {
                return View("Error");
            }           
            
            ModelState.AddModelError("", "Not valid User");
            return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles = "User")]
        public ActionResult Edit(int? id) 
        {
            try
            {
                
            
                if (id != null)
                {
                     DailyTravelMonitoring value = _dbContext.DailyTravelMonitorings.Find(id);
                    if(value.TravelDate==null)value.TravelDate=DateTime.Today;
                    return View(value);
                }
            }
            catch 
            {
                return View("Error");
            }
            return RedirectToAction("Index");
            
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Edit(DailyTravelMonitoring value)
        {
           
            DailyTravelMonitoring list = _dbContext.DailyTravelMonitorings.Find(value.ID);
            
            value.EmployeeId = list.EmployeeId;
            value.TravelDate = list.TravelDate;
            if(list.C_IDGuid != null || list.IDGuid != null)
            {
                value.C_Extension = list.C_Extension;
                value.C_FileName = list.C_FileName;
                value.C_IDGuid = list.C_IDGuid;

                value.Extension = list.Extension;
                value.FileName = list.FileName;
                value.IDGuid = list.IDGuid;
            }
            if (ModelState.IsValid)
            {
                try
                {               
                
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        //var file2 = Request.Files[1];
                        if(file.ContentLength == 0 && list.IDGuid ==null && list.C_IDGuid == null)
                        {
                            ModelState.AddModelError("file", "Upload Reading Photo!");
                                return View(list);
                        }
                        if (list.IDGuid == null || list.FileName == null || list.Extension == null )  
                        {
                            if(file != null && file.ContentLength>0 )
                            {
                                value.FileName = Path.GetFileName(file.FileName);
                                value.Extension = Path.GetExtension(value.FileName);
                                value.IDGuid = Guid.NewGuid();
                                file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Upload"), value.IDGuid + value.Extension));
                            }
                        
                        }else if (list.C_IDGuid == null || list.C_FileName == null || list.C_Extension == null)
                        {                        
                            if (file != null && file.ContentLength > 0)
                            {
                                value.C_FileName = Path.GetFileName(file.FileName);
                                value.C_Extension = Path.GetExtension(value.C_FileName);
                                value.C_IDGuid = Guid.NewGuid();
                                file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Upload"),value.C_IDGuid + value.C_Extension));
                            }
                        }

                    }

                    /**************************************************************************************************************/
                    if (list.TravelDate == null) value.TravelDate = DateTime.Now;
                    if (list.IDGuid == null && value.IDGuid == null)
                    {
                        ModelState.AddModelError("file", "Upload  file!");
                        return View(list);
                    }
                    var local = _dbContext.Set<DailyTravelMonitoring>()
                             .Local
                             .FirstOrDefault(f => f.ID == value.ID);
                    if (local != null)
                    {
                        _dbContext.Entry(local).State = EntityState.Detached;
                    }
                
                    _dbContext.Entry(value).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return RedirectToAction("Details");
                }
                catch
                {
                    return View("Error");
                }
            }
            return View(list);
        }
        [Authorize(Roles = "Admin,User")]
        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
       
        [Authorize(Roles = "Admin,User")]
        public ActionResult DeleteFile(int id,Guid guid)
        {
            
            try
            {
                //Guid guid = new Guid(id);
                DailyTravelMonitoring value = _dbContext.DailyTravelMonitorings.Find(id);
                if (value != null)
                {
                    if(guid == value.IDGuid)
                    {
                        value.Extension = null;
                        value.FileName = null;
                        value.IDGuid = null;
                    }
                    if(guid == value.C_IDGuid)
                    {
                        value.C_Extension = null;
                        value.C_FileName = null;
                        value.C_IDGuid = null;
                    }
                    _dbContext.Entry(value).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return RedirectToAction("Details");
        }
    }
}