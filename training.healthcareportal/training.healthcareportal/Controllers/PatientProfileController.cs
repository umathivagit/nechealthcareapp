using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace training.healthcareportal.Controllers
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