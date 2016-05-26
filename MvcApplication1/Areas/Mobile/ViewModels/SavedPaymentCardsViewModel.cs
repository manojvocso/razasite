using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class SavedPaymentCardsViewModel : BaseMobileViewModel
    {
        public List<GetCard> ExistingCards { get; set; }
    }

    public class UpdateExistingCardInfoViewModel : BaseMobileViewModel
    {
        public string CardType { get; set; }
        public int CreditCardId { get; set; }
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