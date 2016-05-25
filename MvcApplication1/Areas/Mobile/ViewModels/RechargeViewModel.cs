using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using MvcApplication1.AppHelper;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class RechargeViewModel : BaseMobileViewModel
    {

        public RechargeViewModel()
        {
            RechargeAmounts = new List<GetAmount>();
            UserCreditCards = new List<SelectListItem>();
        }

        public List<GetAmount> RechargeAmounts { get; set; }
        public List<SelectListItem> UserCreditCards { get; set; }
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "The recharge amount is required")]
        public double RechargeAmount { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The credit card is required")]
        public string CreditCardId { get; set; }

        [RequiredIfNot("PaymentType", "PayPal", ErrorMessage = "The cvv number is required")]
        public string Cvv { get; set; }

        public string CouponCode { get; set; }

        public string PaymentType { get; set; }

    }
}