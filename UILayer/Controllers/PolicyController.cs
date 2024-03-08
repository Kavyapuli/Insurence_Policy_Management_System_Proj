using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class PolicyController : Controller
    {
        private InsuranceDbContext dbContext;

        public PolicyController()
        {
            dbContext = new InsuranceDbContext();
        }
        public ActionResult ShowAllPolicy()
        {
            // List<Policy> allPolicies=dbContext.Policies.ToList();
            var policies = dbContext.Policies.ToList();


            return View(policies);
        }

       public List<Policy> AllP()
        {
            ViewBag.noofpolicies = dbContext.Policies.Count();
            return dbContext.Policies.ToList();   
        }
        public ActionResult AddPolicy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPolicy(PolicyModel policyViewModel)
        {
            if (ModelState.IsValid)
            {

                Policy newPolicy = new Policy
                {
                    PolicyNumber = policyViewModel.PolicyNumber,
                    DateOfCreation = DateTime.Now,
                    Category= policyViewModel.Category,
                    policycategory = policyViewModel.policycategory,
                    Price = policyViewModel.Price
                };

                dbContext.Policies.Add(newPolicy);
                dbContext.SaveChanges();
                InsuranceDbContext db = new InsuranceDbContext();
                List<Customer> customers = db.Customers.ToList();
                foreach (var i in customers)
                {
                    MailMessage MM = new MailMessage("sparkinsurancepolicy@gmail.com", i.Email);
                    MM.Subject = "New Policy";
                    MM.Body = $"Dear {i.FirstName} {i.LastName}" + "\n\n" +
                               "we are adding a new policy" + "\n\n\n" +
                               $"Policy Number  :{policyViewModel.PolicyNumber}" + "\n" +
                               $"Category            :{policyViewModel.Category}" + "\n" +
                               $"Policy Category: {policyViewModel.policycategory}" + "\n\n\n" +
                               "For more details please Login into Your Spark Account.....🔐" + "\n" +
                               "For further assistance, kindly contact our agents" + "\n\n" +
                               "Puli Kavya    Contact Number:9988776655" + "\n" +
                               "Suresh Reddy  Contact Number:6302539493" + "\n\n\n\n" +
                               "Warm regards🤗," + "\n" + "Spark and Allied Insurance Co.Ltd" + "\n" + "🏢 Hyderabad,Telangana";
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
                    return RedirectToAction("AddPolicySuccess");
            }

            return View(policyViewModel);
        }
        public ActionResult AddPolicySuccess()
        {
            return View();
        }
        public ActionResult ShowAllPoliciesEdit()
        {
            List<Policy> allPolicies = dbContext.Policies.ToList();

            List<PolicyModel> viewModels = allPolicies.Select(policy => new PolicyModel
            {
                PolicyNumber = policy.PolicyNumber,
                DateOfCreation = policy.DateOfCreation,
                Category = policy.Category,
                policycategory = policy.policycategory,
                Price = policy.Price
            }).ToList();

            return View(viewModels);
        }
        public ActionResult EditPolicy(string policyNumber)
        {

            if (string.IsNullOrEmpty(policyNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Policy policyToEdit = dbContext.Policies.FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policyToEdit == null)
            {
                return HttpNotFound();
            }

            PolicyModel viewModel = new PolicyModel
            {
                PolicyNumber = policyToEdit.PolicyNumber,
                DateOfCreation = policyToEdit.DateOfCreation,
                Category = policyToEdit.Category,
                policycategory = policyToEdit.policycategory,
                Price = policyToEdit.Price
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPolicy(PolicyModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Policy policyToEdit = dbContext.Policies.FirstOrDefault(p => p.PolicyNumber == viewModel.PolicyNumber);

                if (policyToEdit == null)
                {
                    return HttpNotFound();
                }

                policyToEdit.DateOfCreation = viewModel.DateOfCreation;
                policyToEdit.Category = viewModel.Category;
                policyToEdit.policycategory = viewModel.policycategory;
                policyToEdit.Price = viewModel.Price;

                dbContext.SaveChanges();

                return RedirectToAction("EditPolicySuccess");
            }
            return View(viewModel);
        }
        public ActionResult EditPolicySuccess()
        {
            return View();
        }
        public ActionResult ShowAllPoliciesDelete()
        {
            List<Policy> allPolicies = dbContext.Policies.ToList();
            List<PolicyModel> viewModels = allPolicies.Select(policy => new PolicyModel
            {
                PolicyNumber = policy.PolicyNumber,
                DateOfCreation = policy.DateOfCreation,
                Category = policy.Category,
                policycategory = policy.policycategory,
                Price = policy.Price
            }).ToList();

            return View(viewModels);
        }
        public ActionResult DeletePolicy(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Policy policyToDelete = dbContext.Policies.FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policyToDelete == null)
            {
                return HttpNotFound();
            }

            PolicyModel viewModel = new PolicyModel
            {
                PolicyNumber = policyToDelete.PolicyNumber,
                DateOfCreation = policyToDelete.DateOfCreation,
                Category = policyToDelete.Category,
                policycategory = policyToDelete.policycategory,
                Price = policyToDelete.Price
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeletePolicy(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Policy policyToDelete = dbContext.Policies.FirstOrDefault(p => p.PolicyNumber == policyNumber);

            if (policyToDelete == null)
            {
                return HttpNotFound();
            }

            dbContext.Policies.Remove(policyToDelete);
            dbContext.SaveChanges();

            return RedirectToAction("DeletePolicySuccess");
        }

        public ActionResult DeletePolicySuccess()
        {
            return View();
        }
    }
}