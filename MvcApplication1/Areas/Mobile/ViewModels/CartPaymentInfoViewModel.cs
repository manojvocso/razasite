using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;
using MvcApplication1.AppHelper.CustomValidation;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class CartPaymentInfoViewModel : BaseMobileViewModel
    {

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string CardType { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        [MatchWithCardType("CardType", ErrorMessage = "Invalid Card Type.")]
        public string CreditCardNumber { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string ExpMonth { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string ExpYear { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string Cvv { get; set; }

        public string CouponCode { get; set; }

        public string BackButtonUrl { get; set; }

        public string PaymentType { get; set; }

        public string ExpDate
        {
            get
            {
                return !string.IsNullOrEmpty(ExpMonth) ? string.Format("{0}/{1}", ExpMonth, ExpYear) : string.Empty;
            }
        }
        public bool IsPaypalDisabled { get; set; }

    }
}