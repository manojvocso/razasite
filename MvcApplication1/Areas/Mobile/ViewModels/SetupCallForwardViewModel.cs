using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class SetupCallForwardViewModel : BaseMobileViewModel
    {
        public SetupCallForwardViewModel()
        {
            CallForwarded800NumberList = new List<GetList800>();
            CountryToList = new List<Country>();
        }
        public List<GetList800> CallForwarded800NumberList { get; set; }
        public List<Country> CountryToList { get; set; }
        public string PlanPin { get; set; }

        [Required(ErrorMessage = "The 1-800 Number is required")]
        public string CallForwarding800Number { get; set; }

        [Required(ErrorMessage = "The country is required")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        public string DestinationPhoneNumber { get; set; }

        [Required(ErrorMessage = "The activation date is required")]
        public string ActivationDate { get; set; }

        [Required(ErrorMessage = "The expiry date is required")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "The call forwarding name is required")]
        [MaxLength(15, ErrorMessage = "The call forwarding name should be max 15 char")]
        public string ForwardedName { get; set; }


    }
}