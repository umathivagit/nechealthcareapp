using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using training.healthcareportal.EDMX;
using training.healthcareportal.Models;

namespace training.healthcareportal.Controllers
{
    public class StatusController : Controller
    {
        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        // GET: Status
        public ActionResult ApproveStatus(int AppointmentId)
        {
            var statusToApprove = AppointmentList(AppointmentId);

            statusToApprove.Status = GetStatuses();
            return View(statusToApprove);
        }

        [HttpPost]
        public ActionResult ApproveStatus(AppointmentViewModel appointment)
        {
            var appointmentToUpdate = healthCareDB.Appointments.Where(x => x.Appointment_ID == appointment.AppointmentId).FirstOrDefault();
            var statusToApprove = AppointmentList(appointmentToUpdate.Appointment_ID);
            if (appointmentToUpdate.Appointment_ID != 0)
            {
                appointmentToUpdate.Status = appointment.StatusId;
                healthCareDB.SaveChanges();
            }
            HomeController.SendEmail(statusToApprove.Email, statusToApprove.Patient_Name, statusToApprove.Date, statusToApprove.Time);
            return RedirectToAction("DoctorPage", "DoctorRegistration");
        }
        public ActionResult Delete()
        {

            return View();
        }

        [HttpGet]
        public ActionResult ConfirmedAppointments()
        {
            int Id = (int)Session["doctorId"];
            var approvedStatuses = ApprovedAppointments(Id);
            return View(approvedStatuses);
        }


        [HttpGet]
        public ActionResult UpcomingAppointments()
        {
            int Id = (int)Session["doctorId"];
            var approvedStatuses = ApprovedAppointments(Id);
            return View(approvedStatuses);
        }

        private static AppointmentViewModel AppointmentList(int Id)
        {
            AppointmentViewModel appointment = null;
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select A.Appointment_ID,P.FullName,P.Gender,P.ContactNumber,A.Date,A.Time,A.Symptoms,P.Email,P.HealthInsuranceID,P.BloodGroup,P.EmergencyContactPersonDetails from portal.Appointment as A inner join portal.Patient as P on P.PatientID = A.Patient_ID where A.Appointment_ID =" + Id;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            appointment = new AppointmentViewModel
                            {
                                AppointmentId = Convert.ToInt32(sdr["Appointment_ID"]),
                                Patient_Name = sdr["FullName"].ToString(),
                                Gender = sdr["Gender"].ToString(),
                                Phone_Number = sdr["ContactNumber"].ToString(),
                                Date = sdr["Date"].ToString(),
                                Time = (TimeSpan)sdr["Time"],
                                Symptoms = sdr["Symptoms"].ToString(),
                                HealthInsuranceID = sdr["HealthInsuranceID"].ToString(),
                                BloodGroup = sdr["BloodGroup"].ToString(),
                                EmergencyContact = sdr["EmergencyContactPersonDetails"].ToString(),
                                Email = sdr["Email"].ToString()
                            };
                        }
                    }
                    con.Close();
                }
            }

            return appointment;
        }


        private static List<StatusModel> GetStatuses()
        {
            List<StatusModel> items = new List<StatusModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select * from portal.Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new StatusModel
                            {
                                StatusId = Convert.ToInt32(sdr["Id"]),
                                StatusName = sdr["StatusName"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }


        private static List<AppointmentViewModel> ApprovedAppointments(int Id)
        {
            List<AppointmentViewModel> appointment = new List<AppointmentViewModel>();
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select A.Appointment_ID,P.FullName,P.Gender,P.ContactNumber,A.Date,A.Time,A.Symptoms,P.HealthInsuranceID,P.BloodGroup,P.EmergencyContactPersonDetails,A.Status from portal.Appointment as A inner join portal.Patient as P on P.PatientID = A.Patient_ID where A.Doctor_ID =" + Id + " and A.Status = 2"; 
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            appointment.Add(new AppointmentViewModel
                            {
                                AppointmentId = Convert.ToInt32(sdr["Appointment_ID"]),
                                Patient_Name = sdr["FullName"].ToString(),
                                Gender = sdr["Gender"].ToString(),
                                Phone_Number = sdr["ContactNumber"].ToString(),
                                Date = sdr["Date"].ToString(),
                                Time = (TimeSpan)sdr["Time"],
                                Symptoms = sdr["Symptoms"].ToString(),
                                HealthInsuranceID = sdr["HealthInsuranceID"].ToString(),
                                BloodGroup = sdr["BloodGroup"].ToString(),
                                EmergencyContact = sdr["EmergencyContactPersonDetails"].ToString(),
                                StatusId = Convert.ToInt32(sdr["Status"])
                            });
                        }
                    }
                    con.Close();
                }
            }

            return appointment;
        }
    }

}