using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class PlanInfoViewModel : BaseMobileViewModel
    {
        public string OrderId { get; set; }
        public string PlanName { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionAmount { get; set; }
        public bool AllowRecharge { get; set; }
        public bool AllowCDR { get; set; }
        public bool AllowPinless { get; set; }
        public bool AllowQuickkey { get; set; }
        public string AutoRefillStatus { get; set; }
        public bool AllowCallForwading { get; set; }
        public bool IsActivePlan { get; set; }
        public bool ShowBalance { get; set; }
        public string MyAccountBal { get; set; }
        public string ServiceFee { get; set; }
        public string UsesFrom { get; set; }
        public string PlanId { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public decimal AccountBalance { get; set; }

        public bool IsValidForRedeem { get; set; }

    }
}