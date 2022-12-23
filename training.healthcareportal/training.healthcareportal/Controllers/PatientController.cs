using training.healthcareportal.EDMX;
using training.healthcareportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace training.healthcareportal.Controllers
{
    [AllowAnonymous]
    public class PatientController : Controller
    {
        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        public ActionResult PatientProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientProfile(PatientModel patient)
        {
            if (ModelState.IsValid)
            {
                patient.Password = EncryptPassword.HashPassword(patient.Password);

                var checkUserName = healthCareDB.Users.Where(x => x.Username == patient.Email).FirstOrDefault();
                if (checkUserName == null)
                {
                    var patientDetails = healthCareDB.sp_NewPatientRegistration((patient.FirstName + " " + patient.LastName), patient.DOB, patient.Gender, patient.Email, patient.Password, patient.BloodGroup, patient.Weight, patient.Height, patient.ContactNumber, patient.Address, patient.HealthInsuranceID, patient.EmergencyContact);

                    Session["patientInfo"] = patientDetails;
                    //ViewBag.Name = patient.FirstName + " " + patient.LastName;
                    TempData["PatientNotification"] = "Patient Registered Successfully! Please Login...";
                    return RedirectToAction("Index", "Login");
                    
                }
                else
                {
                    TempData["UserExists"] = patient.Email + " User already exists";
                    return View();
                }
            }
            return View(patient);
        }
    }
}
