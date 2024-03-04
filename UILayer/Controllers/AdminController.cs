using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                // User is authenticated, proceed with the action
                var customers = context.Customers.ToList();
                return View(customers);
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        //Action method to get all users
        public ActionResult GetAllUsers()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (Session["AdminUserId"] != null)
            {
                // User is authenticated, proceed with the action
                var users = context.Customers.ToList();
                return View(users);
            }
            else
            {
                // User is not authenticated, redirect to login or unauthorized page
                return RedirectToAction("AdminLogin", "Validation");
            }
        }
        public ActionResult PoliciesList()
        {InsuranceDbContext context= new InsuranceDbContext();  
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
        {InsuranceDbContext context = new InsuranceDbContext();
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
                    dbContext.SaveChanges();
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
        {dbContext=new InsuranceDbContext();
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
    }
}