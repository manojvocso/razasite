using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class MobileTopupViewModel : BaseMobileViewModel
    {
        public MobileTopupViewModel()
        {
            RatesFromCountryList = new List<Country>();
            MobileNumberCountryList = new List<Country>();
            CountryCode = "1";
            MobileCountryId = 314;
        }
        public List<Country> RatesFromCountryList { get; set; }
        public List<Country> MobileNumberCountryList { get; set; }

        [Required(ErrorMessage = "The country is a required field")]
        public int RateFromCountry { get; set; }

        
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "The Mobile number is a required field")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "The country is a required field")]
        public int MobileCountryId { get; set; }
    }
}