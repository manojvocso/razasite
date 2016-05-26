using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api.Payments;

namespace MvcApplication1.Models
{
    public class CartAuthenticateModel : BaseRazaViewModel
    {
        public string CentinelPayload { get; set; }

        public string CentinelTermURL { get; set; }

        public string Centinel_ACSURL { get; set; }

        public string Centinel_TransactionId { get; set; }

        public string Centinel_TransactionType { get; set; }

        public Recharge RechargeValues { get; set; }

        public CentinelAuthenticateResponse CentinelAuthenticateResponse { get; set; }

        public List<string> LocalAccesNumberList { get; set; } 

        public bool IsCcProcess { get; set; } // if credit card processing should be done by centinel

        public Payment PayPalPaymentResponse { get; set; }

        public string FinalResponseMessage { get; set; }

        public bool FinalTransactionStatus { get; set; }

        public string CcAuthTransactionId { get; set; }

        public string PayResPayLoad { get; set; }

    }
}