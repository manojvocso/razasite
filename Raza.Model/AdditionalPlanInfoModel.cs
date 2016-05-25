using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class AdditionalPlanInfoModel
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public double Price { get; set; }
        public double ServiceFee { get; set; }
        public string CouponCode { get; set; }
        public int CountryTo { get; set; }
        public int CountryFrom { get; set; }
        public bool IsAutoRefill { get; set; }
        public double AutoRefillAmount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
