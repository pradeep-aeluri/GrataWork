using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrataWork.Models
{
    public class CreateCustomerViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string PlanID { get; set; }
    }
}