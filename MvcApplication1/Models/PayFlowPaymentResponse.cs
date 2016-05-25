using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class PayFlowPaymentResponse
    {
        public string Id { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public string intent { get; set; }

    }

    public class payer
    {
        public string payment_method { get; set; }
        public List<funding_instrument> funding_instruments { get; set; }
    }

    public class funding_instrument
    {


    }

    public class credit_card
    {

    }
}