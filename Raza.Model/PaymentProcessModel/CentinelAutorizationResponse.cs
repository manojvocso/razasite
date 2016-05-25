using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class CentinelAutorizationResponse
    {
        public string CentinelPayload { get; set; }

        public string CentinelTermUrl { get; set; }

        public string CentinelAcsurl { get; set; }

        public string CentinelTransactionId { get; set; }

        public string CentinelTransactionType { get; set; }

        public string FinalResponseMessage { get; set; }

        //public bool FinalTransactionStatus { get; set; }

        public string CcAuthTransactionId { get; set; }

        public string PayResPayLoad { get; set; }

        public bool IsCcProcess { get; set; }
    }
}
