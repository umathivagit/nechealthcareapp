using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.necsws.healthcareportal.Controllers
{
    public class PatientProfileController : Controller
    {
        // GET: PatientProfile
        public ActionResult Index()
        {
            return View();
        }
    }
}