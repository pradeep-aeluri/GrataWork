using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GrataWork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //if the user does not have a valid stripe customer id
            //redirect to the payment page so a customer can be created in stripe
            var usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (usermanager == null)
                throw new ApplicationException("Could not retrieve user manager");
            var user = usermanager.FindByName(HttpContext.User.Identity.Name);
            if (user == null)
                throw new ApplicationException("Could not retrieve user account");
            
            //if there is no link to a stripe customer, redirect to payment page
            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return RedirectToAction("CreateCustomer", "Payment");
            }

            //if there is no jira epic link for the customer, try and create one
            if (string.IsNullOrEmpty(user.JIRAEpicId))
            {
                WorkItemController wic = new WorkItemController();
                wic.CreateCustomerBacklog();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateTask()
        {
            ViewBag.Message = "Create new work item";

            return View();
        }

        public ActionResult Recharge()
        {
            ViewBag.Message = "Recharge";

            return View();
        }

        public ActionResult CancelSubscription()
        {
            ViewBag.Message = "Cancel Subscription";

            return View();
        }
    }
}