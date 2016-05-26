using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class RegPhoneModel : BaseRazaViewModel
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

        public string PinlessNumberOne { get; set; }
        public string CountryCodeOne { get; set; }
        public string OrderIdOne { get; set; }

        public string PinlessNumberTwo { get; set; }
        public string CountryCodeTwo { get; set; }
        public string OrderIdTwo { get; set; }

        public string PinlessNumberThree { get; set; }
        public string CountryCodeThree { get; set; }
        public string OrderIdThree { get; set; }

        public string PinlessNumberFour { get; set; }
        public string CountryCodeFour { get; set; }
        public string OrderIdFour { get; set; }

        public string PinlessNumberFive { get; set; }
        public string CountryCodeFive { get; set; }
        public string OrderIdFive { get; set; }

    }
}
