using com.necsws.healthcareportal.EDMX;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace com.necsws.healthcareportal.Models
{
    public class PatientModel { 

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords Do Not Match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Health Insurance Id")]
        public string HealthInsuranceID { get; set; }

        [Required]
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }

    }
}