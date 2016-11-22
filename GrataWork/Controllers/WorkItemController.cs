using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrataWork.DTO;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using GrataWork.Helpers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GrataWork.Controllers
{
    [Authorize]
    [RoutePrefix("api/workitem")]
    public class WorkItemController : ApiController
    {
        private string JIRAUserName;
        private string JIRAPassword;
        private string JIRAUrl;
        public WorkItemController()
        {
            if (!System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("JIRAUserName"))
            {
                throw new ApplicationException("JIRA credentials are not configured.");
            }

            JIRAUserName = System.Configuration.ConfigurationManager.AppSettings["JIRAUserName"];
            JIRAPassword = System.Configuration.ConfigurationManager.AppSettings["JIRAPassword"];
            JIRAUrl = System.Configuration.ConfigurationManager.AppSettings["JIRAUrl"];
        }

        [HttpGet]
        [Route("getitemsforuser")]
        public async Task<IEnumerable<WorkItem>> GetWorkItems()
        {
            var user = AuthHelper.GetCurrentUser();

            //connect to JIRA and get a list of work items for an Epic
            List<WorkItem> list = new List<WorkItem>();
        
            try
            {
                var settings = new JiraRestClientSettings()
                {
                    EnableRequestTrace = true
                };

                var jira = Jira.CreateRestClient(JIRAUrl, JIRAUserName, JIRAPassword);
                var issues = await jira.Issues.GetIsssuesFromJqlAsync("cf[10003]=" + user.JIRAEpicId);

                foreach (var issue in issues)
                {
                    list.Add(new WorkItem()
                    {
                        Description = issue.Summary,
                        Status = issue.Status.Name,
                        CreatedOn = issue.Created,
                        WorkDone = 0,
                        WorkRemaining = 0
                    });
                }

                //Calculate the account balance for the user
            }
            catch(Exception ex)
            {
                throw ex;
            }            

            return list;                
        }

        [HttpPost]
        [Route("createepic")]
        public void CreateCustomerBacklog()
        {
            //Creates an epic in jira (if one is not associated with the user) 
            //with the name specified and returns the epic id.
            var user = AuthHelper.GetCurrentUser();
            var usermanager = AuthHelper.GetUserManager();

            try
            {
                if (string.IsNullOrEmpty(user.JIRAEpicId))
                {
                    //Create an epic in JIRA
                    var jira = Jira.CreateRestClient(JIRAUrl, JIRAUserName, JIRAPassword);
                    var issue = jira.CreateIssue("QSPEAR");
                    issue.Type = "Epic";
                    issue.Priority = "Medium";
                    issue["Epic Name"] = user.Organization + "Backlog";
                    issue.Summary = user.Organization + "Backlog";

                    issue.SaveChanges();

                    user.JIRAEpicId = issue.Key.Value;

                    if (user.StripePlanId == "small")
                        user.AccountBalance = 20;

                    if (user.StripePlanId == "standard")
                        user.AccountBalance = 40;

                    if (user.StripePlanId == "premium")
                        user.AccountBalance = 80;

                    usermanager.Update(user);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        [HttpPost]
        [Route("create")]
        public async Task<string> CreateNewItem(WorkItem wi)
        {
            var user = AuthHelper.GetCurrentUser();

            try
            {
                //Create an epic in JIRA
                var jira = Jira.CreateRestClient(JIRAUrl, JIRAUserName, JIRAPassword);
                var issue = jira.CreateIssue("QSPEAR");
                issue.Type = "Task";
                issue.Priority = "Medium";
                issue.Summary = wi.Title;
                issue.Description = wi.Description;
                issue["Epic Link"] = user.JIRAEpicId;                             

                await issue.SaveChangesAsync();

                if (!string.IsNullOrEmpty(wi.FileName))
                {
                    UploadAttachmentInfo ai = new UploadAttachmentInfo(wi.FileName,
                        Convert.FromBase64String(wi.Attachment.Replace("data:text/plain;base64,", "")));

                    await issue.AddAttachmentAsync(new UploadAttachmentInfo[] { ai });
                }

                return issue.Key.Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }
    }
}
