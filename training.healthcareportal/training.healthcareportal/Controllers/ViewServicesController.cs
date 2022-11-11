using com.necsws.healthcareportal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.necsws.healthcareportal.Controllers
{
    public class ViewServicesController : Controller
    {
        // GET: ViewServices
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewServices()
        {
            List<ServiceModel> services = PopulateServicesInTable();

            
            ViewBag.Message = "Services of Doctors are provided here";

            return View(services);
        }



        private static List<ServiceModel> PopulateServicesInTable()
        {
            List<ServiceModel> items = new List<ServiceModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT Service_Name, Service_ID,Description FROM portal.Services";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new ServiceModel
                            {
                                ServiceName = sdr["Service_Name"].ToString(),
                                ServiceId = Convert.ToInt32(sdr["Service_ID"]),
                                Description = sdr["Description"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }
    }
}