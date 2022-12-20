using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace training.healthcareportal.Models
{
    public class DoctorRegisterationViewModel
    {
        [Required]
        [Display (Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "IMA NO")]
        public int IMANO { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Qualification")]
        public List<QualifcationModel> Qualification { get; set; }

        public int Qualification_ID { get; set; }

        [Display(Name = "Specialization")]
        public List<ServiceModel> Specialization { get; set; }

        public int Service_ID { get; set; }

        [Required]
        [Display(Name = "Experience")]
        public int Experience { get; set; }

       //public List<ServiceModel> Services { get; set; }

       // public List<QualifcationModel> Qualifcations { get; set; }

    }
}