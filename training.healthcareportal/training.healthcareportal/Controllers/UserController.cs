using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.necsws.healthcareportal.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChooseAccountType()
        {
            ViewBag.Message = "Your Appointment Page";
            return View();
        }
    }
}