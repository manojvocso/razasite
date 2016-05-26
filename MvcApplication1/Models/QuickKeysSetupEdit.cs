using System;
using System.Collections.Generic;
using Raza.Model;

namespace MvcApplication1.Models
{
    using System.Web.Mvc;
    public class QuickeysSetupEdit : BaseRazaViewModel
    {
        public string MemberID { get; set; }
        public string CountryCode { get; set; }
        public string SpeedDialNumber { get; set; }
        public string NickName { get; set; }
        public string PinNumber { get; set; }
        public string Pin { get; set; }
        public string DestinationNumber { get; set; }
        public List<Country> list { get; set; }
        public string OrderId { get; set; }
        public DateTime returndate { get; set; }
        public string planname { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public int PlanId { get; set; }
        public List<GetAmount> AmountRecharge { get; set; }
        public string ServiceFee { get; set; }
        public string RechargeBalance { get; set; }
        public string countrycityphone
        {
            get
            {
                return CountryCode + "" + DestinationNumber;

            }

        }
        public string Email { get; set; }
        public double RechargePin { get; set; }
        public List<QuickeySetups> QuickeyEdit { get; set; }
        public List<QuickeysSetupEdit> SetupEdit { get; set; }
        public SelectList ToCountryList { get; set; }
        public SelectList CountryList { get; set; }
        public string CurrencyCode { get; set; }
        public List<Country> Tocountry { get; set; } 

        public QuickeysSetups QuickeysSetups { get; set; }
    }
}