using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AdditionalPlanViewModel
    {
        [Required(ErrorMessage = "")]
        public string PlanType { get; set; }

        public int CountryFrom { get; set; }

    }
}