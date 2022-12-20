using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace training.healthcareportal.Models
{
    public class DoctorModel
    {
        public int DoctorID { get; set; }
        public int IMA_NO { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public string DOB { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int YearsOfExperience { get; set; }

    }
}