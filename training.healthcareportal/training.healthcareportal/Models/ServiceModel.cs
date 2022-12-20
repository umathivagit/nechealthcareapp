using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.healthcareportal.Models
{
    public class ServiceModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; internal set; }
    }
}