using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class SavedPaymentInfoViewModel : BaseMobileViewModel
    {
        public SavedPaymentInfoViewModel()
        {
            UserCreditCards = new List<SelectListItem>();
        }

        public List<SelectListItem> UserCreditCards { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string CreditCardId { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The cvv number is required")]
        public string Cvv { get; set; }

        public string CouponCode { get; set; }

        public string PaymentType { get; set; }
        public bool IsPaypalDisabled { get; set; }
    }
}