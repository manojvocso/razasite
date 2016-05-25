using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AddNewQuickKeyViewModel : BaseMobileViewModel
    {
        public AddNewQuickKeyViewModel()
        {
            Countries= new List<Country>();
        }
        public List<Country> Countries { get; set; }

        [Required(ErrorMessage = "The country is required")]
        public string CountryCode { get; set; }

        [RegularExpression(@"^[0-9]*$",ErrorMessage = "The phone number is invalid")]
        [Required(ErrorMessage = "The phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The contact name is required ")]
        [MaxLength(15, ErrorMessage = "The contact name should be max 15 char")]
        public string ContactName { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter only digit(s)")]
        [Required(ErrorMessage = "The quick Keys is required")]
        public string QuickKey { get; set; }

        public string PlanPin { get; set; }

    }
}