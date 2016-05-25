using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class ProcessPaymentInfo
    {
        public string TransactionType { get; set; }
        public string PaymentType { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string IpAddress { get; set; }
        public bool IsPaypalDisabled { get; set; }
        public string ExpDate
        {
            get { return string.Format("{0}/{1}", ExpMonth, ExpYear); }
        }

        public string ExpDate2
        {
            get
            {
                if (!string.IsNullOrEmpty(ExpMonth) && !string.IsNullOrEmpty(ExpYear))
                {
                    return string.Format("{0}{1}", ExpMonth, ExpYear.Remove(0, 2));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string Cvv { get; set; }
        public string CouponCode { get; set; }

    }
}
