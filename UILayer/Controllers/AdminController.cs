using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class AdminController : Controller
    {
        private InsuranceDbContext dbContext;
        // GET: Admin
        public ActionResult Dashboard()
        {
            if (Session["AdminUserId"] != null)
            {
                // User is authenticated, proceed with the action
                return View();
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult GetAllCustomers()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                
                var customers = context.Customers.ToList();
                return View(customers);
            }
            else
            {
               
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
       
        public ActionResult GetAllUsers()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
               
                var users = context.Customers.ToList();
                return View(users);
            }
            else
            {
               
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult PoliciesList()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                // User is authenticated, proceed with the action
                var policies = context.Policies.ToList();
                return View(policies);
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult Categories()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                // User is authenticated, proceed with the action
                var categories = context.Categories.ToList();
                return View(categories);
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }

        public ActionResult AllAppliedPolicies()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                // User is authenticated, proceed with the action
                var appliedPolicies = context.AppliedPolicies.ToList();
                return View(appliedPolicies);
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult ApprovedPolicies()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var approvedPolicies = context.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Approved).ToList();
                return View(approvedPolicies);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        [HttpPost]
        public ActionResult ApprovePolicy(int policyId)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var policy = context.AppliedPolicies.Find(policyId);
                if (policy != null && policy.StatusCode == PolicyStatus.Pending)
                {
                    policy.StatusCode = PolicyStatus.Approved;
                    context.SaveChanges();
                    var Eid = context.Customers.SingleOrDefault(e => e.Id == policy.CustomerId);
                    MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", Eid.Email);
                    MM.Subject = "Policy Approved✔";
                    MM.Body = " Your Policy has been Approved successfully..!!" + "\n\n\n" +
                      $"Policy Numbur     :{policy.PolicyNumber}" +
                      $"Category              :{policy.Category}" + "\n" +
                      $"Policy Category:{policy.policycategory}" + "\n" +
                      $"policy Status    :{policy.StatusCode}" + "\n" +
                      $"Policy Applied Date:{policy.AppliedDate}" + "\n\n\n\n" +
                      "For more details please Login into Your Spark Account.....🔐" + "\n" +
                      "For further assistance, kindly contact our agents" + "\n\n" +
                      "Puli Kavya    Contact Number:9988776655" + "\n" +
                      "Suresh Reddy  Contact Number:6655998877" + "\n\n\n\n" +
                      "Warm regards," + "\n" + "Spark and Allied Insurance Co.Ltd" + "\n" + "🏢 Hyderabad,Telangana";

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
                return RedirectToAction("AllAppliedPolicies");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult DisapprovedPolicies()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var disapprovedPolicies = context.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Disapproved).ToList();
                return View(disapprovedPolicies);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        [HttpPost]
        public ActionResult DisapprovePolicy(int policyId)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var policy = context.AppliedPolicies.Find(policyId);
                if (policy != null && policy.StatusCode == PolicyStatus.Pending)
                {
                    policy.StatusCode = PolicyStatus.Disapproved;
                    context.SaveChanges();
                }
                return RedirectToAction("AllAppliedPolicies");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }


        }

        public ActionResult PendingPolicies()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var pendingPolicies = context.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Pending).ToList();
                return View(pendingPolicies);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult Policy()

        {
            if (Session["AdminUserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }

        public ActionResult Question()
        {
            dbContext = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var questions = dbContext.Questions.ToList();
                return View(questions);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult Reply(int id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var question = context.Questions.Find(id);
                return View(question);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Validation");
            }
        }

        [HttpPost]
        public ActionResult Reply(Questions model)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null && ModelState.IsValid)
            {
                var existingQuestion = context.Questions.Find(model.QuestionId);
                if (existingQuestion != null)
                {
                    existingQuestion.Answer = model.Answer;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult SaveAnswer(int questionId, string answer)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                var existingQuestion = context.Questions.Find(questionId);
                if (existingQuestion != null)
                {
                    existingQuestion.Answer = answer;
                    context.SaveChanges();
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, error = "Question not found" });
        }

        //Customer
        // delete customer details
        public ActionResult DeleteCustomer(int? Id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            var customer = context.Customers.Find(Id);
            if (customer == null)
            {
                // Customer not found, handle accordingly (e.g., show error page)
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteCustomer(int Id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            var customer = context.Customers.Find(Id);
            if (customer == null)
            {
                // Customer not found, handle accordingly (e.g., show error page)
                return HttpNotFound();
            }
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("GetAllCustomers");
        }
        //view customer details

        public ActionResult ViewCustomer(int? Id)
        {
            // Retrieve the customer from the database based on the provided id
            InsuranceDbContext context = new InsuranceDbContext();
            var customer = context.Customers.Find(Id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }


        [HttpPost, ActionName("ViewCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult ViewwCustomer(int Id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            var customer = context.Customers.Find(Id);
            if (customer == null)
            {
                // Customer not found, handle accordingly (e.g., show error page)
                return HttpNotFound();
            }

            return RedirectToAction("GetAllCustomers");
        }

        public ActionResult EditCustomer(int? Id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Id== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customers.Find(Id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,UserName,Password,RoleId")] Customer customer)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (ModelState.IsValid)
            {
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();

                MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", customer.Email);
                MM.Subject = "Updated Deatils Successfully..!!";
                MM.Body = "Dear Customer," + "\n\n"+
                
                "We are pleased to informed you that your details in our portal has been updated successfully!" + "\n\n\n" +
                "Your updated details"+"\n\n"+
                $"Full Name      :{customer.FirstName} {customer.LastName}" + "\n" +
                $"Email Id              :{customer.Email}" + "\n" +
                $"Phone Number :{customer.PhoneNumber}" + "\n" +
                $"User Name         :{customer.UserName}" + "\n" +
                $"Password           :{customer.Password}" + "\n" +
               
                $"Updated Time {DateTime.Now}" + "\n\n\n\n" +
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

                return RedirectToAction("GetAllCustomers");
            }
            return View(customer);
        }


        public ActionResult ViewCategory(int id)
        {
            // Retrieve the customer from the database based on the provided id
            InsuranceDbContext context = new InsuranceDbContext();
            var category = context.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }


        [HttpPost, ActionName("ViewCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult ViewwCategory(int id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            var category = context.Categories.Find(id);
            if (category == null)
            {
                // Customer not found, handle accordingly (e.g., show error page)
                return HttpNotFound();
            }

            return RedirectToAction("Categories");
        }


        public ActionResult DeleteCategory(int? id)
        {
            InsuranceDbContext context=new InsuranceDbContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            Category category = context.Categories.Find(id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("Categories");
        }

        public ActionResult EditCategory(int? id)
        {
            InsuranceDbContext context=new InsuranceDbContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (ModelState.IsValid)
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(category);
        }

        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (ModelState.IsValid)
            {

                Category newCategory = new Category
                {
                    CategoryId = category.CategoryId,
                  CategoryName= category.CategoryName
                };

                context.Categories.Add(newCategory);
                context.SaveChanges();

                return RedirectToAction("Categories");
            }

            return View(category);
        }

      
    }

}