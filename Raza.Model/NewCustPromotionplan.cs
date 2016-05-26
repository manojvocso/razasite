using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class NewCustomerPromotionPlan
    {
        public List<NewCustPromotionplanmodel> NewCustomerPlans { get; set; }
    }

    public class NewCustPromotionplanmodel
    {
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string Denomination { get; set; }
        public string RegularMin { get; set; }
        public string ExtraMin { get; set; }
        public string RateperMIn { get; set; }
        public string TotalMinute { get; set; }
        public string CurrencySign { get; set; }
        public string CurrencyCode { get; set; }
        public string RateperSign { get; set; }
        public string PlanName { get; set; }
        public string PlanId { get; set; }
        public decimal ServiceFee { get; set; }
        public string CouponCode { get; set; }


    }
}
