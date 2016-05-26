using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcApplication1.Areas.Mobile.Models;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AutoRefillViewModel : BaseMobileViewModel
    {
        public AutoRefillViewModel()
        {
            AutoRefillOptions = new List<double>() { 10, 20, 50, 90 };
            UserExistingCards = new List<GetCard>();
        }

        public List<double> AutoRefillOptions { get; set; }
        public List<GetCard> UserExistingCards { get; set; }
        public string PlanPin { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsAutoRefillActivate { get; set; }
        public bool IsDisableAutorefillDeactivate { get; set; }

        [Required(ErrorMessage = "The autorefill amount is required")]
        public double AutoRefillAmount { get; set; }

        [Required(ErrorMessage = "The credit card is required")]
        public int CreditCardId { get; set; }
        [Required(ErrorMessage = "The cvv is required")]
        public int? Cvv { get; set; }

    }
}