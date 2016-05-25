using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Raza.Model
{
   public class Mobiletopup
    {

        public string TopUpMobileOperator { get; set; }
        public string TopUpMobileCountry { get; set; }
        public string TopUpCountryCode { get; set; }
        public List<MobileTopupOperator> Operatordata { get; set; }
        public List<MobileTopupOperator> AllCountryOperatorList { get; set; }
        public string OperatorImage { get; set; }

       public bool IsAutoOperatorfind { get; set; }
       public string MemberId { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string BillngPhoneNumber { get; set; }
       public string OperatorCode { get; set; }
       public string Operator { get; set; }
       public string CountryId { get; set; }
       public  double SourceAmount { get; set; }
       public double DestinationAmt { get; set; }
       public string DestinationPhoneNumber { get; set; }
       public string SmsTo { get; set; }
       public string PaymentMethod { get; set; }
       public string Pin { get; set; }
       public double PurchaseAmount { get; set; }
       public string CardNumber { get; set; }
       public string ExpDate { get; set; }
       public string Cvv2 { get; set; }
       public string Address1 { get; set; }
       public string Address2 { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string ZipCode { get; set; }
       public string Country { get; set; }
       public string IpAddress { get; set; }
       
       public bool Isccprocess { get; set; }
       public string CouponCode { get; set; }
       public string NewOrderId { get; set; }
       public string PaymentTransactionId { get; set; }
       public string AvsResponse { get; set; }
       public string Cvv2Response { get; set; }
       public string Cavv { get; set; } 
       public string EciFlag { get; set; }
       public string Xid { get; set; }
       public string PayerId { get; set; }
    }
}
