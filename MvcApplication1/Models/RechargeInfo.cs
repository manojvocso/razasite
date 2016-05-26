using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class RechargeInfo : BaseRazaViewModel
    {
        public string MemberId { get; set; }
        public string UserType { get; set; }
        public int CardId { get; set; }
        public string Pin { get; set; }
        public double Amount { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string IpAddress { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string CoupanCode { get; set; }
        public string AniNumber { get; set; }
        public string PaymentMethod { get; set; }

        public string PaymentTransactionId { get; set; }
        public string EciFlag { get; set; }

        public string Cavv { get; set; }
        public string Xid { get; set; }

        public string CVV2Response { get; set; }

        public string AVSResponse { get; set; }

        public bool IsCcProcess { get; set; }
    }
}