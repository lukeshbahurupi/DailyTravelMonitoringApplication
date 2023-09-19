using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace DailyTravelMonitoringApplication.Models
{
    [MetadataType(typeof(metadataDailyTravelMonitoring))]
    public partial class DailyTravelMonitoring
    {
    }
    public class metadataDailyTravelMonitoring
    {
        
        
        public Nullable<System.DateTime> TravelDate { get; set; }
          
        public string ScheduleRemark { get; set; }
       
        public string VehicleType { get; set; }
       
        [RegularExpression(@"^[A-Z]{2}\d{2}\s[A-Z]{1,2}\d{1,4}$",ErrorMessage = "Plese enter number in 'AA11 AA1111 format")]
       
        public string VehicleNumber { get; set; }

        
        public Nullable<int> StartKmReading { get; set; }
          
        public Nullable<int> EndKmReading { get; set; }
          
        public Nullable<int> TotalKmTravelled { get; set; }
          
        public Nullable<decimal> RatePerKm { get; set; }
          
        public Nullable<decimal> TotalTravelCost { get; set; }
         
        public Nullable<decimal> MeetingExpenses { get; set; }
          
        public Nullable<decimal> StayExpenses { get; set; }
       
        public Nullable<decimal> FoodExpenses { get; set; }
        
        public string DayFinalRemarks { get; set; }
    }
}