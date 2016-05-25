using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Payment.Models
{
    public class AuthenticateInfoViewModel
    {
        public string CentinelAcsurl { get; set; }

        public string CentinelPayload { get; set; }

        public string CentinelTermUrl { get; set; }

        public string CentinelTransactionId { get; set; }
    }
}