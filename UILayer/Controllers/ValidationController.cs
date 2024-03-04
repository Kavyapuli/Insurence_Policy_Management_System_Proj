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
            return View();
        }
        [HttpPost]
        public ActionResult CustomerLogin(LoginModel loginView)
        {

            var isCustomer = Authentication.VerifyCustomerCredentials(loginView.UserName, loginView.Password);

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
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginModel loginView)
        {
            var isAdmin = Authentication.VerifyAdminCredentials(loginView.UserName, loginView.Password);

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
        public ActionResult CustomerResetPassword(ResetPasswordModel model)
        {
            /* if (ModelState.IsValid)
             {
                 var user = customerRepository.GetCustomerByUserName(model.UserName);

                 if (user == null)
                 {
                     ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                     return View(model);
                 }
                 else
                 {
                     user.Password = model.Password;
                     customerRepository.customerSaveChanges();
                 }

                 TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                 return RedirectToAction("CustomerLogin", "Validation");
             }

             return View(model);*/
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
                }
                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("CustomerLogin", "Validation");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminResetPassword(ResetPasswordModel model)
        {
          /*  if (ModelState.IsValid)
            {
                var user = adminRepository.GetAdminByUserName(model.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                    return View(model);
                }
                else
                {
                    user.Password = model.Password;
                    adminRepository.SaveAdminchanges();
                }

                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("AdminLogin", "Validation");
            }

            return View(model);*/

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