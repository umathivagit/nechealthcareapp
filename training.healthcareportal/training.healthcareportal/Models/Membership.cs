﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace com.necsws.healthcareportal.Models
{ 
        public class Membership
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
   
}