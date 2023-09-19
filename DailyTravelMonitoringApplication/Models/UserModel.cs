using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyTravelMonitoringApplication.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string MobileNo { get; set; }        
        public string UniqeId { get; set; }
        public string Password { get; set; }
    }
}