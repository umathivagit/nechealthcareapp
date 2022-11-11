using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace com.necsws.healthcareportal.Models
{
    public class AppointmentViewModel
    {
        [Display(Name ="Patient Name")]
        public string Patient_Name { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }

        public List<ServiceModel> Services { get; set; }

        public int Service_ID { get; set; }

        [Display(Name ="Doctor Name")]
        public List<DoctorModel> Doctor_Name { get; set; }

        public int Doctor_ID { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public string Symptoms { get; set; }

        public string Gender { get; set; }
        public string HealthInsuranceID { get; set; }
        public string BloodGroup { get; set; }
        public string EmergencyContact { get; set; }


    }
}