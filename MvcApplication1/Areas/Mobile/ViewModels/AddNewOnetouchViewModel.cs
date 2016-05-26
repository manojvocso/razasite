using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class AddNewOnetouchViewModel : BaseMobileViewModel
    {
        public AddNewOnetouchViewModel()
        {
            CallingFromCountries = new List<Country>();
            CallingToCountries = new List<Country>();
            States = new List<State>();
            AreaCodes = new List<Areacode>();
            AvailableNumbers = new List<Availnumber>();
        }

        public List<Country> CallingFromCountries { get; set; }
        public List<Country> CallingToCountries { get; set; }
        public List<State> States { get; set; }
        public List<Areacode> AreaCodes { get; set; }
        public List<Availnumber> AvailableNumbers { get; set; }


        [Required(ErrorMessage = "The country is required")]
        public string CallingFromCountry { get; set; }

        [Required(ErrorMessage = "The area code required")]
        public string CallingFromAreaCode { get; set; }

        [Required(ErrorMessage = "The state is required")]
        public string CallingFromState { get; set; }

        [Required(ErrorMessage = "The available number is required")]
        public string CallingAvailableNumber { get; set; }


        [Required(ErrorMessage = "The country is required")]
        public string CallingToCountryCode { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "The phone number is invalid")]
        [Required(ErrorMessage = "The phone number is required")]
        public string CallingToPhoneNumber { get; set; }

        [Required(ErrorMessage = "The reference name is required")]
        [MaxLength(15,ErrorMessage = "The reference name should be max 15 char")]
        public string RefrenceName { get; set; }

        public string PlanPin { get; set; }
    }
}