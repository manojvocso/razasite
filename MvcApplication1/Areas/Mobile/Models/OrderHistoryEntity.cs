using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.Models
{
    public class OrderHistoryEntity
    {
        public string PlanName { get; set; }
        public string Pin { get; set; }
        public string OrderId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
    }
}