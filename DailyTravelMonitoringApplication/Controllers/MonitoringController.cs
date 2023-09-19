using DailyTravelMonitoringApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DailyTravelMonitoringApplication.Controllers
{
    public class MonitoringController : Controller
    {
        private readonly TravelingTeam_DB_Context _dbContext = new TravelingTeam_DB_Context();
        // GET: Monitoring
        public ActionResult Index()
        {
            return View(_dbContext.DailyTravelMonitorings.ToList());
        }
        public ActionResult Edit(int? id) 
        {
            if (id != null)
            {
                 DailyTravelMonitoring value = _dbContext.DailyTravelMonitorings.Find(id);
                if(value.TravelDate==null)value.TravelDate=DateTime.Now;
                return View(value);
            }
           
            return RedirectToAction("Index");
            
        }
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
                return RedirectToAction("Index");
            }
            return View(list);
        }
        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
       
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
            return RedirectToAction("Index");
        }
    }
}