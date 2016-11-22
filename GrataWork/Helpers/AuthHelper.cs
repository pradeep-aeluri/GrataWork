using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading;
using GrataWork.Models;

namespace GrataWork.Helpers
{
    public static class AuthHelper
    {
       public static ApplicationUser GetCurrentUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            var usermanager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = usermanager.FindByName(identity.Name);
            if (identity == null || usermanager == null || user == null)
            {
                throw new Exception("User not authenticated");
            }

            return user;
        }

        public static ApplicationUserManager GetUserManager()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            var usermanager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (identity == null || usermanager == null)
            {
                throw new Exception("User not authenticated");
            }

            return usermanager;
        }
    }
}