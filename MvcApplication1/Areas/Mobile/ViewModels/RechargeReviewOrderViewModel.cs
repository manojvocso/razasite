using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class RechargeReviewOrderViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public string CurrencyCode { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string CouponCode { get; set; }
        public string Amount { get; set; }


    }
}