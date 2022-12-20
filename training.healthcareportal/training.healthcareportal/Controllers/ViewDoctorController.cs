using training.healthcareportal.Models;
using training.healthcareportal.EDMX;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace training.healthcareportal.Controllers
{
    public class ViewDoctorController : Controller
    {

        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        // GET: ViewDoctor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewDoctorsInfo(int Serviceid)
        {
            List<DoctorModel> doctors = PopulateDoctorsByService(Serviceid);


            ViewBag.Message = "Specializd Doctors informations are provided here";

            return View(doctors);
        }

        private List<DoctorModel> PopulateDoctorsByService(int Serviceid)
        {
            //  doctorList =  healthCareDB.GetDoctorListByServiceID(Id).ToList<DoctorModel>();
            List<DoctorModel> items = new List<DoctorModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "EXEC [reporting].[GetDoctorListByServiceID] " + Serviceid.ToString();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new DoctorModel
                            {
                                DoctorID = Convert.ToInt32(sdr["DoctorID"]),
                                IMA_NO = Convert.ToInt32(sdr["IMA_NO"]),
                                FullName = sdr["FullName"].ToString(),
                                Gender = sdr["Gender"].ToString(),
                                YearsOfExperience = Convert.ToInt32(sdr["YearsOfExperience"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }


        //private static List<DoctorModel> PopulateDoctorNamesinTable(int Id)
        //{
        //    List<DoctorModel> items = new List<DoctorModel>();
        //    string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        string query = " SELECT Doctor_Id,IMA_NO,FullName,Gender,YearsOfExperience FROM portal.Doctor where Doctor_ID"+Id.ToString();
        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                while (sdr.Read())
        //                {
        //                    items.Add(new DoctorModel
        //                    {
        //                        DoctorID = Convert.ToInt32(sdr["DoctorID"]),
        //                        IMA_NO = Convert.ToInt32(sdr["IMA_NO"]),
        //                        FullName = sdr["FullName"].ToString(),
        //                        Gender = sdr["Gender"].ToString(),
        //                        YearsOfExperience = Convert.ToInt32(sdr["YerasOfExperience"])
        //                    }) ;
        //                }
        //            }
        //            con.Close();
        //        }
        //    }

        //    return items;
        //}
    }
}