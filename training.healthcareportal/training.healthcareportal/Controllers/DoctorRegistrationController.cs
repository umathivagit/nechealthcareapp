using com.necsws.healthcareportal.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.necsws.healthcareportal.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace com.necsws.healthcareportal.Controllers
{
    public class DoctorRegistrationController : Controller
    {
        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();

        public ActionResult DoctorPage(int Doctorid)
        {
            List<AppointmentViewModel> appointments = PopulateAppointmentByDoctorID(Doctorid);
            return View(appointments);
        }
        [AllowAnonymous]
        public ActionResult Index()
        {

            List<QualifcationModel> Qualifactions =  PopulateQualification();

            List<ServiceModel> services = PopulateServices();

            //Populate the Qualifications which are fetched from the database
            DoctorRegisterationViewModel docViewModel = new DoctorRegisterationViewModel();
            docViewModel.Qualification = Qualifactions;
            docViewModel.Specialization = services;

            return View(docViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(DoctorRegisterationViewModel docViewModel)
        {
            if (ModelState.IsValid)
            {
                docViewModel.Password = EncryptPassword.HashPassword(docViewModel.Password);
                healthCareDB.sp_NewDoctorRegistration(docViewModel.IMANO, (docViewModel.FirstName +" "+ docViewModel.LastName), Convert.ToDateTime(docViewModel.DOB), docViewModel.Gender, docViewModel.Email, docViewModel.Experience, docViewModel.Password, docViewModel.Qualification_ID, docViewModel.Service_ID, 2, docViewModel.Email);
                return RedirectToAction("Index","Login");
            }
            return View(docViewModel);
        }


        private static List<ServiceModel> PopulateServices()
        {
            List<ServiceModel> items = new List<ServiceModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT Service_Name, Service_ID FROM portal.Services";
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
                                ServiceId = Convert.ToInt32(sdr["Service_ID"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }

        /// <summary>
        /// This method pulls the maste table records from Qualifcation table
        /// </summary>
        /// <returns></returns>

        private static List<QualifcationModel> PopulateQualification()
        {
            List<QualifcationModel> quaLificationList = new List<QualifcationModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT Qualification_Name, Qualification_ID FROM portal.Qualification";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            quaLificationList.Add(new QualifcationModel
                            {
                                QualificationName = sdr["Qualification_Name"].ToString(),
                                QualificationId = Convert.ToInt32(sdr["Qualification_ID"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return quaLificationList;
        }


        private List<AppointmentViewModel> PopulateAppointmentByDoctorID(int Doctorid)
        {
            //  doctorList =  healthCareDB.GetDoctorListByServiceID(Id).ToList<DoctorModel>();
            List<AppointmentViewModel> items = new List<AppointmentViewModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "EXEC [portal].[sp_ViewPatientAppointment]" + Doctorid.ToString();
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new AppointmentViewModel
                            {
                                Patient_Name = sdr["FullName"].ToString(),
                                Gender = sdr["Gender"].ToString(),
                                HealthInsuranceID = sdr["HealthInsuranceID"].ToString(),
                                BloodGroup = sdr["BloodGroup"].ToString(),
                                EmergencyContact = sdr["EmergencyContactPersonDetails"].ToString(),
                                Date= sdr["Date"].ToString(),
                                Time = (TimeSpan)sdr["Time"],
                                Symptoms = sdr["Symptoms"].ToString(),
                                
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