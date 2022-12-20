using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using training.healthcareportal.EDMX;
using training.healthcareportal.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace training.healthcareportal.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {

        HealthCareDBContext2 healthCareDB = new HealthCareDBContext2();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.Membership model)
        {
            Session["userInfo"] = model;
            var currentAccount = healthCareDB.Users.First(x => x.Username.Equals(model.UserName));

            //bool isValidPatient = healthCareDB.Users.Any( && x.Password == pass && x.Role_ID == 2);
            //bool isValidDoctor = healthCareDB.Users.Any(x => x.Username == model.UserName && x.Password == pass && x.Role_ID == 3);

            if (EncryptPassword.ValidatePassword(model.Password,currentAccount.Password) && currentAccount.Role_ID == 2)
            {
                //var patientid = Session["patientInfo"];
                int PatientId = SavePatientID(model.UserName);
                FormsAuthentication.SetAuthCookie(model.UserName, false);

                string EncryptedPatientId = EncryptPassword.EncryptString("b14ca5898a4e4133bbce2ea2315a1916", PatientId);
                Session["patientId"] = PatientId;

                TempData["userName"] = PatientNameById(PatientId);
                TempData.Keep();
                return RedirectToAction("Index", "Home", new { Patientid = EncryptedPatientId });
            }
            else if (EncryptPassword.ValidatePassword(model.Password,currentAccount.Password) && currentAccount.Role_ID == 3)
            {
                int DoctorId = SaveDoctorID(model.UserName);
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                //TempData["doctorId"] = DoctorId;
                string EncryptedDoctorId =  EncryptPassword.EncryptString("b14ca5898a4e4133bbce2ea2315a1916",DoctorId);

                return RedirectToAction("DoctorPage", "DoctorRegistration", new { Doctorid = EncryptedDoctorId } );
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName and Password");
                return View(model);
            }
            

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgotPassword password)
        {
            var objUsr = healthCareDB.Users.Where(x => x.Username == password.EmailId).FirstOrDefault();
            if (objUsr == null)
            {
                TempData["UserFail"] = "Username is not available";
               // healthCareDB.SaveChanges();
            }
            else
            {
                TempData["UserMatch"] = "Reset Link Sent to the Email Id";
                TempData["username"] = objUsr.Username;
                ForgotPasswordEmailToUser(objUsr.Username).Wait(3000);

            }
            
            return View();
        }


        [HttpGet]
        public ActionResult ChangePassword(string username)
        {
            ChangePassword changePass = new ChangePassword();
            changePass.Username = username;
            return View(changePass);
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            changePassword.Password = EncryptPassword.HashPassword(changePassword.Password);
            //var username = TempData["username"];
            //var username = HttpContext.Request.Params.Get("param.1");
            if (ModelState.IsValid)
            {
                healthCareDB.sp_ChangePasswordByUsername(changePassword.Password, changePassword.Username);
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public int SaveDoctorID(string username)
        {
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("[portal].[sp_DoctorIdByUsername]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                cmd.Parameters.Add("@DoctorID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int ret = int.Parse(cmd.Parameters["@DoctorID"].Value.ToString());
                return ret;
            }
        }


        public int SavePatientID(string username)
        {
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("[portal].[sp_PassPatientId]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = username;
                cmd.Parameters.Add("@PatientID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int ret = int.Parse(cmd.Parameters["@PatientID"].Value.ToString());
                return ret;
            }
        }



        public async Task ForgotPasswordEmailToUser(string username)
        {
            var GenerateUserVerificationLink = "/Login/ChangePassword?Username="+username;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenerateUserVerificationLink);
            string password = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Username FROM [portal].[User] WHERE Username = @username"))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            username = sdr["Username"].ToString();
                        }
                    }
                    con.Close();
                }
            }
                if (!string.IsNullOrEmpty(username))
                {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("mustansirsabir53@gmail.com", "Mustansir Sabir");
                var subject = "Password Recovery";
                var to = new EmailAddress(username);
                var plainTextContent = "Here is the link to Reset your Password " 
                    + username+ "<br/><br/><a href="+ link +"</a>";
                var htmlContent = "<strong>Here is the link to Reset your Password</strong>"+
                    "<br/><br/><a href=" + link + ">"+link+"</a>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                
                }
        }


        private static string PatientNameById(int Id)
        {
            string patientName = null;
            string constr = ConfigurationManager.ConnectionStrings["HealthCareDBContext1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " select FullName from portal.Patient where PatientID=" + Id;
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
    }
}