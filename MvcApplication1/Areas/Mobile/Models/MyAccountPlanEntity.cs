using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class MyAccountPlanEntity
    {
        public string OrderId { get; set; }
        public string PlanName { get; set; }
        public string LastTransactionDate { get; set; }
        public bool IsActivePlan { get; set; }
        public string PlanPin { get; set; }
    }
}