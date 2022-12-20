using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace training.healthcareportal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "DoctorRegistration", action = "Index", id = UrlParameter.Optional }
            //);
            //  routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Patient", action = "PatientProfile", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "VewServices",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "ViewDoctor", action = "ViewDoctors", Serviceid = "" }
          );

        }
    }
}
