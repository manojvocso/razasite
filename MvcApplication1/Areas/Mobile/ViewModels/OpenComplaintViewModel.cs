using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class OpenComplaintViewModel : BaseMobileViewModel
    {
        public OpenComplaintViewModel()
        {
            CountryFromList = new List<Country>();
            CountryToList = new List<Country>();
            UserPlanList = new List<SelectListItem>();
            DescriptionList = new List<string>()
            {
                "About our New Website",
                "I am unable to Login",
                "I am unable to Recharge",
                "Remove Numbers from PinLess SetUp",
                "The Access Number(s)are not working",
                "I want to Cancel My Auto Refill",
                "My Plan is not recharged yet",
                "I was Charged by PAYPAL but order could not be processed",
                "Other Issue"
            };
        }

        public List<Country> CountryFromList { get; set; }
        public List<Country> CountryToList { get; set; }
        public List<string> DescriptionList { get; set; }
        public List<SelectListItem> UserPlanList { get; set; }

        [Required(ErrorMessage = "The firstname is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The lastname is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "The plan name is required")]
        public string PlanOrderId { get; set; }

        [Required(ErrorMessage = "The destination number is required")]
        public string DestinationNumber { get; set; }

        [Required(ErrorMessage = "The calling from is required")]
        public string CallingFromCountry { get; set; }

        [Required(ErrorMessage = "The calling to is required")]
        public string CallingToCountry { get; set; }

        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The comment is required")]
        public string Comment { get; set; }
    }
}