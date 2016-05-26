using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class RechargeAutorefillSetupViewModel : BaseMobileViewModel
    {
        public RechargeAutorefillSetupViewModel()
        {
            AutoRefillOptionsList = new List<double>();
        }
        public List<double> AutoRefillOptionsList { get; set; }

        [Required(ErrorMessage = "Please select a autorefill amount.")]
        public double AutoRefillAmout { get; set; }

        public string PlanPin { get; set; }
        public string CurrencyCode { get; set; }
    }
}