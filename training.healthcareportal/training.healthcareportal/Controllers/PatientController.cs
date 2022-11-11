using com.necsws.healthcareportal.EDMX;
using com.necsws.healthcareportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.necsws.healthcareportal.Controllers
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
                var patientDetails = healthCareDB.sp_NewPatientRegistration((patient.FirstName + " " + patient.LastName), patient.DOB, patient.Gender, patient.Email, patient.Password, patient.BloodGroup, patient.Weight, patient.Height, patient.ContactNumber, patient.Address, patient.HealthInsuranceID, patient.EmergencyContact);

                Session["patientInfo"] = patientDetails;
                //ViewBag.Name = patient.FirstName + " " + patient.LastName;
                TempData["PatientNotification"] = "Patient Registered Successfully! Please Login...";
                return RedirectToAction("Index", "Login");
            }
            return View(patient);
        }
    }
}
