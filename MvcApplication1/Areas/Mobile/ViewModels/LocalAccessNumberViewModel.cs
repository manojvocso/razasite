using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class LocalAccessNumberViewModel : BaseMobileViewModel
    {
        public LocalAccessNumberViewModel()
        {
            CountryFromList = new List<Country>()
            {
                new Country()
                {
                    
                }
            };
            StateList = new List<State>();
            LocalAccessNumbers = new List<LocalAccessNumber>();
        }

        public List<LocalAccessNumber> LocalAccessNumbers { get; set; }
        public List<Country> CountryFromList { get; set; }
        public List<State> StateList { get; set; }

        [Required(ErrorMessage = "The country is required")]
        public string AccessCountry { get; set; }

        public string AccessState { get; set; }

        [Phone(ErrorMessage = "The phone number is invalid")]
        public string PhoneNumber { get; set; }

    }
}