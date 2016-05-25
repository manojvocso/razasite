using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class RedeemPointsViewModel : BaseMobileViewModel
    {
        public RedeemPointsViewModel()
        {
            RedeemOptions = new List<SelectListItem>();
        }
        public List<SelectListItem> RedeemOptions { get; set; }

        [Required(ErrorMessage = "Please select point to redeem")]
        public double RedeemPointAmount { get; set; }
        public string PlanName { get; set; }

    }
}