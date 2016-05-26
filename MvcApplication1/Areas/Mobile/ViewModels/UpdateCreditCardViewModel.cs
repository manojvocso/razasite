using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcApplication1.AppHelper.CustomValidation;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class UpdateCreditCardViewModel : BaseMobileViewModel
    {


        [Required(ErrorMessage = "Card Type is required")]
        public string CardType { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [MatchWithCardType("CardType", ErrorMessage = "Invalid card number.")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Exp Month is required")]
        public string ExpMonth { get; set; }
        [Required(ErrorMessage = "Exp year is required")]
        public string ExpYear { get; set; }
        [Required(ErrorMessage = "Cvv number is required")]
        public int? Cvv { get; set; }

        public string BackButtonUrl { get; set; }

        public string ExpDate
        {
            get
            {
                return !string.IsNullOrEmpty(ExpMonth) ? string.Format("{0}/{1}", ExpMonth, ExpYear) : string.Empty;
            }
        }
    }
}