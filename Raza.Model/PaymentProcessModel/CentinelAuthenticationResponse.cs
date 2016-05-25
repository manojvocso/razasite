using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class CentinelAuthenticationResponse
    {
        public string CentinelEciFlag { get; set; }

        public string CentinelPaResStatus { get; set; }

        public string CentinelSignatureVerification { get; set; }

        public string CentinelXid { get; set; }

        public string CentinelCavv { get; set; }

        public string CentinelTransactionId { get; set; }

        public string CcAuthTransactionId { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }


        //for CcCapture
        public bool IsNotCvv2Match { get; set; }
        public bool IsNotAvsAddrMatch { get; set; }
        public bool IsNotAvsZipMatch { get; set; }

        // for paypal 
        public string ConsumerStatus { get; set; }
        public string AddressStatus { get; set; }
        public string ConsumerName { get; set; }
        public string EMail { get; set; }
        public string PayerId { get; set; }

        public string AvsResponse { get; set; }
        public string CvvResponse { get; set; }

    }
}
