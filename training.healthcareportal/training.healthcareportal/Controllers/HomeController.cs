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
using System.Security.Cryptography;
using System.Web.Security;

namespace training.healthcareportal.Controllers
{
    public class HomeController : Controller
    {
        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        
        public ActionResult Index()
        {
            var patientId = Session["patientId"];
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
            Models.Membership membership = (Models.Membership)Session["userInfo"];
            int patientID = (int)Session["patientId"];
            //var patientId = Session["patientInfo"];
                Appointment newAppointment = new Appointment();
                newAppointment.Patient_ID = patientID;
                newAppointment.Service_ID = makeAppointment.Service_ID;
                newAppointment.Doctor_ID = makeAppointment.Doctor_ID;
                newAppointment.Time = makeAppointment.Time;
                newAppointment.Date = makeAppointment.Date;
                newAppointment.Symptoms = makeAppointment.Symptoms;
                newAppointment.Status = makeAppointment.StatusId;
                //newAppointment.Appointment_Status = "New";
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

            currentAppointmentViewModel.StatusId = 1;
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

        [HttpGet]
        public ActionResult MakeAppointmentOfDoctor(int Id)
        {
            AppointmentViewModel appointmentByDoctor = new AppointmentViewModel();
            //Here we have taken two session which will populate the services id and patient id from the View Services controller
            int patientID = (int)Session["patientId"];
            int serviceid = (int)Session["serviceId"];

            //So here we will take the service name from the id which we got from the View Doctors controller
            var service = GetServiceById(serviceid);
            List<DoctorModel> doctorName = (List<DoctorModel>)Session["doctorName"];
            
            //And we will populate that service here
            appointmentByDoctor.Services = service;

            foreach (var doctor in doctorName)
            {
                if (doctor.DoctorID == Id)
                {
                    appointmentByDoctor.Doctor_Name = GetDoctorById(doctor.DoctorID);
                }
            }
            //Populating the doctorname from the session of view doctors controller

            appointmentByDoctor.StatusId = 1;
            appointmentByDoctor.Patient_Name = PatientNameById(patientID);
            return View(appointmentByDoctor);
        }

        [HttpPost]
        public ActionResult MakeAppointmentOfDoctor(AppointmentViewModel makeAppointment)
        {
            Models.Membership membership = (Models.Membership)Session["userInfo"];
            int patientID = (int)Session["patientId"];
            //var patientId = Session["patientInfo"];
            Appointment newAppointment = new Appointment();
            newAppointment.Patient_ID = patientID;
            newAppointment.Service_ID = makeAppointment.Service_ID;
            newAppointment.Doctor_ID = makeAppointment.Doctor_ID;
            newAppointment.Time = makeAppointment.Time;
            newAppointment.Date = makeAppointment.Date;
            newAppointment.Symptoms = makeAppointment.Symptoms;
            newAppointment.Status = makeAppointment.StatusId;
            //newAppointment.Appointment_Status = "New";
            newAppointment.Created_At = DateTime.Now;
            newAppointment.Modified_At = DateTime.Now;
            healthCareDB.Appointments.Add(newAppointment);
            healthCareDB.SaveChanges();
            TempData["SuccessNotification"] = "Appointment Created Successfully!";
            //ViewBag.Name = emailData.FirstName + " " + emailData.LastName;
            //SendEmail(membership.UserName, makeAppointment.Patient_Name, makeAppointment.Date, makeAppointment.Time).Wait(100);
            return RedirectToAction("Index", "Home");
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

        public static async Task SendEmail(string patientEmail, string patientName, string date, TimeSpan time)
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

        private static List<ServiceModel> GetServiceById(int id)
        {
            var serviceModel = new List<ServiceModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select Service_ID,Service_Name from portal.Services where Service_ID =" + id;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            serviceModel.Add(new ServiceModel
                            {
                                ServiceId = Convert.ToInt32(sdr["Service_ID"]),
                                ServiceName = sdr["Service_Name"].ToString()
                            });
                        }
                    }
                    return serviceModel;
                }
            }
        }


        private static List<DoctorModel> GetDoctorById(int id)
        {
            var doctorModel = new List<DoctorModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select DoctorID,FullName from portal.Doctor where DoctorID =" + id;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            doctorModel.Add(new DoctorModel
                            {
                                DoctorID = Convert.ToInt32(sdr["DoctorID"]),
                                FullName = sdr["FullName"].ToString()
                            });
                        }
                    }
                    return doctorModel;
                }
            }
        }

    }
}