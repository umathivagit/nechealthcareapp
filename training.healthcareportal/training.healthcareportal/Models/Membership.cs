using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace training.healthcareportal.Models
{ 
        public class Membership
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
   
}