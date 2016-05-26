using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class CentinelAuthenticateResponse
    {
        public string CentinelEciFlag { get; set; }

        public string CentinelPAResStatus { get; set; }

        public string CentinelSignatureVerification { get; set; }

        public string Centinel_XID { get; set; }

        public string Centinel_CAVV { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }

        // for paypal 

        public string ConsumerStatus { get; set; }
        public string AddressStatus { get; set; }
        public string ConsumerName { get; set; }
        public string EMail { get; set; }
        public string PayerId { get; set; }

        public string AVSResponse { get; set; }
        public string CVVResponse { get; set; }
        
    }
}