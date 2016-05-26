using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class ShopCartMobileModel
    {
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string FromToMapping { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public bool IsAutorefill { get; set; }
        public decimal Amount { get; set; }
        public decimal ServiceFee { get; set; }

        public decimal TotalAmount
        {
            get { return Amount + (Amount * ServiceFee) / 100; }
        }

        public string CurrencyCode { get; set; }
    }
}