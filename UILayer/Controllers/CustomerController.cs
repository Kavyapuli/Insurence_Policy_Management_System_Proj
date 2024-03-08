using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class CustomerController : Controller
    {
        private InsuranceDbContext dbContext;
        public CustomerController()
        {
            dbContext = new InsuranceDbContext();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Check if UserId is present in the session, otherwise redirect to login
            if (Session["CustomerUserId"] == null)
            {
                filterContext.Result = RedirectToAction("CustomerLogin", "Validation");
            }
        }

        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult GetAllUsers()
        {
            var user = dbContext.Customers.ToList();
            return View(user);
        }
        public ActionResult ViewPoliciesToApply()
        {
            List<Policy> policies = dbContext.Policies.ToList();
            return View(policies);
        }
        public ActionResult Apply(int policyId)
        {
            int customerId = Convert.ToInt32(Session["CustomerUserId"]);

            bool AppliedAlready = dbContext.AppliedPolicies
            .Any(ap => ap.CustomerId == customerId && ap.AppliedPolicyId == policyId);
            if (!AppliedAlready)
            {

                Policy policy = dbContext.Policies.FirstOrDefault(p => p.PolicyId == policyId);

                if (policy != null)
                {

                    AppliedPolicy appliedPolicy = new AppliedPolicy
                    {
                        PolicyNumber = policy.PolicyNumber,
                        AppliedDate = DateTime.Now,
                        Category = policy.Category,
                        policycategory = policy.policycategory,
                        Price = policy.Price,
                        CustomerId = customerId,
                    };

                    dbContext.AppliedPolicies.Add(appliedPolicy);
                    dbContext.SaveChanges();
                }
                else
                {

                }
            }

            return RedirectToAction("AppliedPolicies");
        }
        public ActionResult AppliedPolicies()
        {
            List<AppliedPolicy> appliedPolicies;

            using (var dbContext = new InsuranceDbContext())
            {

                appliedPolicies = dbContext.AppliedPolicies.ToList();
            }
            return View(appliedPolicies);
        }
        public ActionResult Categories()
        {
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }
        public ActionResult AskQuestion()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AskQuestion(QuestionModel questionview)
        {
            if (ModelState.IsValid)
            {
                int customerId = Convert.ToInt32(Session["CustomerUserId"]);
                Questions newQuestion = new Questions
                {
                    Question = questionview.Question,
                    Date = DateTime.Now,
                    Answer = questionview.Answer,
                    CustomerId = customerId
                };

                dbContext.Questions.Add(newQuestion);
                dbContext.SaveChanges();

                return RedirectToAction("Success");
            }

            return View(questionview);
        }
        public ActionResult Success()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AskCustomerId()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DisplayQuestionsByCustomerId()
        {
            int customerId = Convert.ToInt32(Session["CustomerUserId"]);
            if (customerId == null)
            {

                return RedirectToAction("Error");
            }

            var questions = dbContext.Questions.Where(q => q.CustomerId == customerId).ToList();
            ViewBag.CustomerId = customerId;
            return View(questions);
        }

        public ActionResult CancelPolicy(int? AppliedPolicyId)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            if (AppliedPolicyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedPolicy category = context.AppliedPolicies.Find(AppliedPolicyId);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("CancelPolicy")]
     //   [ValidateAntiForgeryToken]
        public ActionResult CancelPolicy(int AppliedPolicyId)
        {
            InsuranceDbContext context = new InsuranceDbContext();
            AppliedPolicy category = context.AppliedPolicies.Find(AppliedPolicyId);
            context.AppliedPolicies.Remove(category);
            context.SaveChanges();
            return RedirectToAction("AppliedPolicies");
        }

        public ActionResult FamilyDetail()
        {
            InsuranceDbContext context = new InsuranceDbContext();
            int customerId = Convert.ToInt32(Session["CustomerUserId"]);
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var fam = context.FamilyMembers.Find(customerId);
                
                if (fam != null)
                {
                    
                    return View(new List<FamilyMember> { fam });
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

       



    }
}