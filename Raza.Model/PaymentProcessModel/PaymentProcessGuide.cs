using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class PaymentProcessGuide
    {
        public bool AcceptOrder { get; set; }
        public bool DoCcProcess { get; set; }
        public bool CentinelByPass { get; set; }
        public bool AvsByPass { get; set; }
        public bool IsValidPlan { get; set; }
        public string Message { get; set; }
    }
}
