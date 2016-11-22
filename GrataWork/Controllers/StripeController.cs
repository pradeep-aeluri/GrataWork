using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrataWork.DTO;
using Stripe;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using GrataWork.Helpers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GrataWork.Controllers
{
    [Authorize]
    [RoutePrefix("api/stripe")]
    public class StripeController : ApiController
    {
        public StripeController()
        {
        }

        //[HttpGet]
        //public IEnumerable<StripePlan> GetBalance()
        //{
        //    var plansvc = new StripePlanService(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);
        //    return plansvc.List();
        //}

        [HttpPost]
        [Route("recharge")]
        public async Task<string> Recharge(RechargeInput input)
        {            
            var user = AuthHelper.GetCurrentUser();
            var usermanager = AuthHelper.GetUserManager();

            try
            {
                StripeConfiguration.SetApiKey(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);
            }
            catch (Exception ex)
            {
                //log the exception and return the message to the caller
                return ex.Message;
            }

            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var myCharge = new StripeChargeCreateOptions
                    {
                        Amount = input.HoursAdded * PlanHelper.GetPlan(input.PlanID).Rate,
                        Currency = "usd",
                        Description = "Recharge account balance",
                        CustomerId = user.StripeCustomerId,
                    };

                    var chargeService = new StripeChargeService();
                    var stripeCharge = chargeService.Create(myCharge);

                    //Update the account balance in the system
                    user.AccountBalance += input.HoursAdded;
                    user.StripePlanId = input.PlanID;
                    usermanager.Update(user);

                    return stripeCharge.Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }

        [HttpPost]
        [Route("createcustomer")]
        public string CreateCustomer(RechargeInput c)
        {
            var user = AuthHelper.GetCurrentUser();
            var usermanager = AuthHelper.GetUserManager();

            try
            {
                StripeConfiguration.SetApiKey(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);

                var myCustomer = new StripeCustomerCreateOptions();
                myCustomer.Email = user.Email;
                myCustomer.Description = user.Organization + " (" + user.Email + ")";
                myCustomer.SourceToken = c.AuthorizationCode;
                myCustomer.PlanId = c.PlanID;

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Create(myCustomer);

                //Update the user with the stripe customer id and plan info
                user.StripeCustomerId = customer.Id;
                user.StripePlanId = c.PlanID;
                user.AccountBalance = PlanHelper.GetPlan(c.PlanID).Hours;
                usermanager.Update(user);

                return customer.Id;
            }
            catch (Exception ex)
            {
                //log the exception and return the message to the caller
                throw ex;
            }

            //Create a JIRA epic and store it with the user
            try
            {
                WorkItemController wic = new WorkItemController();
                wic.CreateCustomerBacklog();
            }
            catch { }
        }

        [HttpGet]
        [Route("getaccountdetails")]
        public AccountStatus GetAccountDetails()
        {
            var user = AuthHelper.GetCurrentUser();

            return new AccountStatus() { PlanId = user.StripePlanId, AccountBalance = user.AccountBalance };
        }

        [HttpPost]
        [Route("changeplan")]
        public async Task<string> ChangePlan(RechargeInput input)
        {
            var user = AuthHelper.GetCurrentUser();
            var usermanager = AuthHelper.GetUserManager();

            try
            {
                StripeConfiguration.SetApiKey(System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"]);
            }
            catch (Exception ex)
            {
                //log the exception and return the message to the caller
                return ex.Message;
            }

            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var cust = new StripeCustomerService().Get(user.StripeCustomerId);
                    if (cust != null)
                    {
                        var sub_svc = new StripeSubscriptionService();
                        var sub = sub_svc.Get(cust.Id, cust.StripeSubscriptionList.Data[0].Id);

                        var options = new StripeSubscriptionUpdateOptions();
                        options.PlanId = input.PlanID;
                        options.Prorate = false;

                        sub_svc.Update(cust.Id, sub.Id, options);
                    }
                    else
                    {
                        throw new ApplicationException("Could not find the customer in stripe to change the plan");
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }

        [HttpGet]
        [Route("cancelsubscription")]
        public void CancelSubscription()
        {
            var user = AuthHelper.GetCurrentUser();
            var usermanager = AuthHelper.GetUserManager();

            try
            {
                var cust = new StripeCustomerService().Get(user.StripeCustomerId);
                if (cust != null)
                {
                    var sub_svc = new StripeSubscriptionService();
                    var sub = sub_svc.Get(cust.Id, cust.StripeSubscriptionList.Data[0].Id);

                    sub_svc.Cancel(cust.Id, sub.Id, true);
                }
                else
                {
                    throw new ApplicationException("Could not find the customer in stripe to change the plan");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
