using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class Recharge //: BaseRazaViewModel
    {
        public bool IsFromReSubmit { get; set; }
        public string Pin { get; set; }
        public string OldOrderId { get; set; }
        public string order_id { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; }
        public string cvv { get; set; }
        public string FirstName { get; set; }
        public string autoRefill { get; set; }
        public string LastName { get; set; }
        public string CardType { get; set; }
        public string autoRefillAmount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string MemberID { get; set; }
        public string PlanName { get; set; }
        public double ServiceFee { get; set; }
        public string UserName { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionType { get; set; }
        public string PaymentType { get; set; }
        public int CardId { get; set; }
        public string CouponCode { get; set; }
        public string PaymentMethod { get; set; }
        public string IpAddress { get; set; }

        public string UserType { get; set; }
        public int Countryfrom { get; set; }
        public int Countryto { get; set; }
        public string AniNumber { get; set; }

        public string IsPasscodeDial { get; set; }
        public string PassCodePin { get; set; }
        public List<PinLessSetupNumbers> PinlessNumbers { get; set; } 

        public  string TopupMobileNumber { get; set; }
        public string TopupCountry { get; set; }
        public string TopupOperatorCode { get; set; }
        public string TopupOperatorName { get; set; }
        public double TopupSourceAmount { get; set; }
        public double TopupDestAmount { get; set; }
        public string EmailId { get; set; }
        public bool TopupStatus { get; set; }
        public string TopUpCountryCode { get; set; }

        public bool AcceptOrder { get; set; }
        public bool DoCcProcess { get; set; }
        public bool CentinelByPass { get; set; }
        public bool AvsByPass { get; set; }
        public string StatusMessage1 { get; set; }
        public string StatusMessage2 { get; set; }

        public bool IsFromMobile { get; set; }
        
    }


}