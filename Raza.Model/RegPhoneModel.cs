using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
   public class RegPhoneModel
    {
        public int Quantity { get; set; }
        public string PlanId { get; set; }
        public string AutoRefill { get; set; }
        public string PlanName { get; set; }
        public string FromToMapping { get; set; }
        public string CallingFrom { get; set; }
        public string CallingTo { get; set; }
        public decimal Price { get; set; }
        public int CountryTo { get; set; }
        public int CountryFrom { get; set; }
        public decimal ServiceFee { get; set; }
        public string Userstatus { get; set; }
        public string CurrencyCode { get; set; }
        public string ExistPin { get; set; }
        public string MyaccBal { get; set; }
        public string Orderid { get; set; }

    }
}
