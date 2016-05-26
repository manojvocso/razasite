using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
  public class RechargeStatusModel
    {
        public string Rechargestatus { get; set; }
        public string RechargeError { get; set; }
        public string OrderId { get; set; }
        public bool Status { get; set; }

    }

    public class CheckoutStatusModel
    {
        public bool IssuenewpinStatus { get; set; }
        public string Errormsg { get; set; }
        public string NewOrderId { get; set; }
        public string NewPin { get; set; }
        public List<string> LocalAccessNumbers { get; set; }
        public string OrderDateTime { get; set; }

    }

    public class CommonStatus
    {
        public bool Status { get; set; }
        public string Errormsg { get; set; }
        
    }
}
