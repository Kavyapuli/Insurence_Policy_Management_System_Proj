using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using System.Web.Security;
using DataAccessLayer.Services;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace UILayer.Controllers
{
    public class ValidationController : Controller
    {
        private readonly AdminInterface adminRepository;
        private readonly CustomerInterface customerRepository;
        private readonly InsuranceDbContext context;
      
        public ValidationController() :base()
        {
            
        }
        public ValidationController(AdminInterface adminRepository, CustomerInterface customerRepository)
        {
            this.adminRepository = adminRepository;
            this.customerRepository = customerRepository;
            this.context = new InsuranceDbContext();
        }
        // GET: Validation
        public ActionResult Index()
        {
            return View();
        }
        
        private string GenerateAlphanumericCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%&?|0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult CustomerRegistration()
        {
            
            // Generate and store captcha value in session
            var captchaValue = GenerateAlphanumericCaptcha();
            Session["Captcha"] = captchaValue;

            // Pass captcha value to the view
            var user = new UserModel();
            user.CaptchaValue = captchaValue;
            return View(user);
        }
        private bool ValidateCaptcha(string userInput)
        {
            var captchaInSession = Session["Captcha"] as string;
            return !string.IsNullOrEmpty(captchaInSession) && userInput.Trim().Equals(captchaInSession, StringComparison.OrdinalIgnoreCase);
        }
        [HttpPost]
        public ActionResult CustomerRegistration(UserModel user, string captchaInput)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            bool cust = context.Customers.Any(e => e.Email == user.EmailAddress);
            bool cust1 = context.Customers.Any(e => e.UserName == user.UserName);
            if (ModelState.IsValid)
            {
                // Validate captcha
                if (!ValidateCaptcha(captchaInput))
                {
                    ModelState.AddModelError("Captcha", "Captcha verification failed.");
                    return View("Registration", user);
                }

                // Check if email or username already registered
                if (cust)
                {
                    ModelState.AddModelError("Email", "Email already registered with us.");
                    return View("Registration", user);
                }
                else if (cust1)
                {
                    ModelState.AddModelError("UserName", "Username already registered with us.");
                    return View("Registration", user);
                }
                Customer newcustomer = new Customer
                {
                    Email = user.EmailAddress,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = 1,
                    Password = user.Password,
                };
                context.Customers.Add(newcustomer);
                context.SaveChanges();

                MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", user.EmailAddress);
                MM.Subject = "Account Registration Successfull";
                MM.Body = "Dear Customer," + "\n\n" +
                          "We are pleased to informed you that you registration request for SPARK Insurance Company has been successful!" + "\n\n\n" +
                          $"Full Name :{user.FirstName} {user.LastName}" + "\n" +
                          $"Email Id              :{user.EmailAddress}" + "\n" +
                          $"Phone Number  :{user.PhoneNumber}" + "\n" +
                          $"User Name          :{user.UserName}" + "\n" +
                          $"Password           :{user.Password}" + "\n" +
                          $"Registrastion Date {DateTime.Now}" + "\n\n\n\n" +
                         "For further assistance, kindly contact our agents" + "\n\n" +
                         "Puli Kavya    Contact Number:9988776655" + "\n" +
                         "Suresh Reddy  Contact Number:6655998877" + "\n\n\n\n" +
                         "Warm regards," + "\n" + "Spark and Allied Insurance Co.Ltd" + "\n" + "Hyderabad,Telangana";

                MM.IsBodyHtml = false;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("sparkinsurancepolicy@gmail.com", "rtfp wcpa wlbr vibq");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = networkCredential;
                smtpClient.Send(MM);

                return RedirectToAction("Dashboard", "Customer");
            }
            return View(user);
        }

        public ActionResult AdminRegistration()
        {
            // Generate and store captcha value in session
            var captchaValue = GenerateAlphanumericCaptcha();
            Session["Captcha"] = captchaValue;

            // Pass captcha value to the view
            var user = new UserModel();
            user.CaptchaValue = captchaValue;
            return View(user);
        }
        [HttpPost]
        public ActionResult AdminRegistration(UserModel user, string captchaInput)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            bool cust = context.Admins.Any(e => e.EmailAddress == user.EmailAddress);
            bool cust1 = context.Admins.Any(e => e.UserName == user.UserName);
            if (ModelState.IsValid)
            {
                // Validate captcha
                if (!ValidateCaptcha(captchaInput))
                {
                    ModelState.AddModelError("Captcha", "Captcha verification failed.");
                    return View("Registration", user);
                }

                // Check if email or username already registered
                if (cust)
                {
                    ModelState.AddModelError("Email", "Email already registered with us.");
                    return View("Registration", user);
                }
                else if (cust1)
                {
                    ModelState.AddModelError("UserName", "Username already registered with us.");
                    return View("Registration", user);
                }
                Admin newadmin = new Admin
                {
                    EmailAddress = user.EmailAddress,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = 1,
                    Password = user.Password,
                };
                context.Admins.Add(newadmin);
                context.SaveChanges();
                MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", user.EmailAddress);
                MM.Subject = "Account Registration Successfull";
                MM.Body = "Dear Employee," + "\n\n" +
                          "We are pleased to informed you that you registration request for SPARK Insurance Company has been successful!" + "\n\n\n" +
                          $"Full Name :{user.FirstName} {user.LastName}" + "\n" +
                          $"Email Id              :{user.EmailAddress}" + "\n" +
                          $"Phone Number  :{user.PhoneNumber}" + "\n" +
                          $"User Name          :{user.UserName}" + "\n" +
                          $"Password           :{user.Password}" + "\n" +
                          "Role                     :Admin" + "\n" +
                          $"Registrastion Date {DateTime.Now}" + "\n\n\n\n" +
                        
                         "Warm regards," +"\n"+ "Spark and Allied Insurance Co.Ltd"+"\n"+ "Hyderabad,Telangana";
                         
                MM.IsBodyHtml = false;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("sparkinsurancepolicy@gmail.com", "rtfp wcpa wlbr vibq");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = networkCredential;
                smtpClient.Send(MM);


                return RedirectToAction("Dashboard", "Admin");
            }
            return View(user);
        }
        public ActionResult GenerateCaptchaImage()
        {
            var captchaValue = GenerateAlphanumericCaptcha();

            // Store captcha value in session
            Session["Captcha"] = captchaValue;

            // Create an image of the captcha
            var captchaImage = LoginCaptcha.GenerateCaptchaImage(captchaValue);

            // Convert the image to a byte array and return it as an image response
            var imageBytes = LoginCaptcha.ImageToByteArray(captchaImage);

            return File(imageBytes, "image/png");
        }
        public ActionResult CustomerLogin()
        {
            // Generate and store captcha value in session
            var captchaValue = GenerateAlphanumericCaptcha();
            Session["Captcha"] = captchaValue;
            // Pass captcha value to the view
            var user = new LoginModel();
            user.CaptchaValue = captchaValue;
            return View(user);
        }
        [HttpPost]
        public ActionResult CustomerLogin(LoginModel loginView,string captchaInput)
        {

            var isCustomer = Authentication.VerifyCustomerCredentials(loginView.UserName, loginView.Password);
            
            // Validate captcha
            if (!ValidateCaptcha(captchaInput))
            {
                ModelState.AddModelError("Captcha", "Captcha verification failed.");
                return View(loginView);
            }

            if (isCustomer)
            {
               // var user = customerRepository.GetCustomerByUserName(loginView.UserName);
               InsuranceDbContext dbContext = new InsuranceDbContext();
                var user=dbContext.Customers.FirstOrDefault(e=>e.UserName==loginView.UserName);
                Session["CustomerUserId"] = user.Id;
                Session["CustomerUserName"] = user.UserName;
                FormsAuthentication.SetAuthCookie(loginView.UserName, false);
                return RedirectToAction("Dashboard", "Customer");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }
        public ActionResult AdminLogin()
        {
            // Generate and store captcha value in session
            var captchaValue = GenerateAlphanumericCaptcha();
            Session["Captcha"] = captchaValue;
            // Pass captcha value to the view
            var user = new LoginModel();
            user.CaptchaValue = captchaValue;
            return View(user);
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginModel loginView, string captchaInput)
        {
            var isAdmin = Authentication.VerifyAdminCredentials(loginView.UserName, loginView.Password);

            // Validate captcha
            if (!ValidateCaptcha(captchaInput))
            {
                ModelState.AddModelError("Captcha", "Captcha verification failed.");
                return View(loginView);
            }

            if (isAdmin)
            {
             
                Session["AdminUserId"] = loginView.UserName; // Store user identifier in session
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerResetPassword(ResetPasswordModel model,EmailProp emailProp)
        {
            InsuranceDbContext dbContext = new InsuranceDbContext();
            if (ModelState.IsValid)
            {
                var user = dbContext.Customers.FirstOrDefault(e => e.UserName == model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                    return View(model);
                }
                else
                {
                    user.Password = model.Password;
                    dbContext.SaveChanges();
                    MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", user.Email);
                    MM.Subject = "Changing Password";
                    MM.Body = " Your password has been chenged successfully..!!" + "\n\n\n" +
                      $"Your New Password: {user.Password}" +"\n\n\n"+
                    "Warm regards," + "\n" + "Spark and Allied Insurance Co.Ltd" + "\n" + "Hyderabad,Telangana";
                    MM.IsBodyHtml = false;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential("sparkinsurancepolicy@gmail.com", "rtfp wcpa wlbr vibq");
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = networkCredential;
                    smtpClient.Send(MM);
                }
                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("CustomerLogin", "Validation");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminResetPassword(ResetPasswordModel model,EmailProp emailProp)
        {
            InsuranceDbContext dbContext = new InsuranceDbContext();
            if (ModelState.IsValid)
            {
                var user = dbContext.Admins.FirstOrDefault(e=>e.UserName == model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                    return View(model);
                }
                else
                {
                    user.Password=model.Password;
                    dbContext.SaveChanges();
                    MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", user.EmailAddress);
                    MM.Subject = "Changing Password";
                    MM.Body = " Your password has been chenged successfully..!!"+"\n\n\n" +
                          $"Your New Password: {user.Password}" + "\n\n\n" +
                    "Warm regards," + "\n" + "Spark and Allied Insurance Co.Ltd" + "\n" + "Hyderabad,Telangana";
                    MM.IsBodyHtml = false;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential("sparkinsurancepolicy@gmail.com", "rtfp wcpa wlbr vibq");
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = networkCredential;
                    smtpClient.Send(MM);
                }
                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("AdminLogin", "Validation");
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            if (Session["AdminUserId"] != null)
            {
                Session.Remove("AdminUserId");
            }
            else if (Session["CustomerUserId"] != null)
            {
                Session.Remove("CustomerUserId");
                Session.Remove("CustomerUserName");
            }

            Session.Abandon();

            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetExpires(System.DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            return RedirectToAction("Index", "Home");
        }

    }
}