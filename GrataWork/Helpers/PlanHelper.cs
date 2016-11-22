using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrataWork.Helpers
{
    public static class PlanHelper
    {
        public static Plan Premium = new Plan { Name = "premium", Hours = 80, Rate = 10000 };
        public static Plan Standard = new Plan { Name = "standard", Hours = 40, Rate = 11000 };
        public static Plan Small = new Plan { Name = "small", Hours = 20, Rate = 12000 };

        public static Plan GetPlan(string planId)
        {
            switch (planId.Trim().ToLower())
            {
                case "premium":
                    return Premium;
                case "standard":
                    return Standard;
                case "small":
                    return Small;
                default:
                    return null;            
            }
        }
    }

    public class Plan
    {
        public string Name { get; set; }
        public int Hours { get; set; }
        
        //Stripe accepts the amount to be charged in cents, so this is an int and represents the rate in cents.
        public int Rate { get; set; }
    }
}