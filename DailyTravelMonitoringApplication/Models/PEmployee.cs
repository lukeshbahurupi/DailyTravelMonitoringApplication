using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyTravelMonitoringApplication.Models
{
    [MetadataType(typeof(EmployeeMetadataType))]
    public partial class Employee
    {
    }
    public class EmployeeMetadataType
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public System.DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter a value equal to 10 digit.")]
        
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public System.DateTime DateOfJoining { get; set; }
        [Required(ErrorMessage = "Please enter a value equal to 12 digit.")]
        
        public string UAN_No { get; set; }
        [Required]
        public string ActiveFlag { get; set; }
    }
}