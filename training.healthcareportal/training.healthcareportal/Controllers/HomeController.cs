using training.healthcareportal.EDMX;
using training.healthcareportal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;

namespace training.healthcareportal.Controllers
{
    public class HomeController : Controller
    {
        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult MakeAppointment(AppointmentViewModel makeAppointment)
        {
            Membership membership = (Membership)Session["userInfo"];
            int patientID = (int)Session["patientId"];
            //var patientId = Session["patientInfo"];
                Appointment newAppointment = new Appointment();
                newAppointment.Patient_ID = patientID;
                newAppointment.Service_ID = makeAppointment.Service_ID;
                newAppointment.Doctor_ID = makeAppointment.Doctor_ID;
                newAppointment.Time = makeAppointment.Time;
                newAppointment.Date = makeAppointment.Date;
                newAppointment.Symptoms = makeAppointment.Symptoms;
                newAppointment.Appointment_Status = "New";
                newAppointment.Created_At = DateTime.Now;
                newAppointment.Modified_At = DateTime.Now;
                healthCareDB.Appointments.Add(newAppointment);
                healthCareDB.SaveChanges();
                TempData["SuccessNotification"] = "Appointment Created Successfully!";
                //ViewBag.Name = emailData.FirstName + " " + emailData.LastName;
                SendEmail(membership.UserName, makeAppointment.Patient_Name, makeAppointment.Date, makeAppointment.Time).Wait(100);
                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult MakeAppointment()
        {
            ViewBag.Message = "Your Appointment Page";
            int patientID = (int)Session["patientId"];
            List<ServiceModel> servicelist = PopulateServices();
            //List<DoctorModel> doctorList = PopulateDoctorNames();

            AppointmentViewModel currentAppointmentViewModel = new AppointmentViewModel();
            currentAppointmentViewModel.Patient_Name = PatientNameById(patientID);
            currentAppointmentViewModel.Services = servicelist;
            currentAppointmentViewModel.Doctor_Name = Enumerable.Empty<DoctorModel>();

            //new AppointmentViewModel().Services = servicelist;
            return View(currentAppointmentViewModel);

            //return View();
        }

        [HttpPost]
        public JsonResult GetDoctors(int serviceId)
        {
            var doctors = PopulateDoctorNames(serviceId);
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }


        private static string PatientNameById(int Id)
        {
            string patientName = null;
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select FullName from portal.Patient where PatientID="+Id;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            patientName = sdr["FullName"].ToString();
                        }
                    }
                    con.Close();
                }
            }

            return patientName;
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


        private static List<DoctorModel> PopulateDoctorNames(int Sid)
        {
            List<DoctorModel> items = new List<DoctorModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select D.FullName,D.DoctorID from portal.Doctor_Services as DS inner join portal.Doctor as D on D.DoctorID = DS.Doctor_ID where DS.Service_ID ="+Sid;
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
                                FullName = sdr["FullName"].ToString(),
                                DoctorID = Convert.ToInt32(sdr["DoctorID"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        static async Task SendEmail(string patientEmail, string patientName, string date, TimeSpan time)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mustansirsabir53@gmail.com", "Mustansir Sabir");
            var subject = "Reminder for your Appointment";
            var to = new EmailAddress(patientEmail, patientName); 
            var plainTextContent = "Dear "+patientName+" We would like to confirm your appointment for "+date+" on "+time+" with us, please be 15 mins prior to the appointment time";
            var htmlContent = "<strong>Dear " + patientName + " We would like to confirm your appointment for " + date + " on " + time + " with us, please be 15 mins prior to the appointment time</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}