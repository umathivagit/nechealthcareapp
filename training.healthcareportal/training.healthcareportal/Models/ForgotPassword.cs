using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace training.healthcareportal.Models
{
    public class ForgotPassword
    {
        [Display(Name = "User Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Email Id Required")]
        public string EmailId { get; set; }
    }
}