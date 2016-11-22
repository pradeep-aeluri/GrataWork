using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrataWork.DTO
{
    public class RechargeInput
    {
        public string AuthorizationCode { get; set; }
        public int HoursAdded { get; set; }
        public decimal AmountCharged { get; set; }
        public string PlanID { get; set; }
    }
}