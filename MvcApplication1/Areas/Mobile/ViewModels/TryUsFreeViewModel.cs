using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class TryUsFreeViewModel : BaseMobileViewModel
    {
        public TryUsFreeViewModel()
        {
            CountryFromList = new List<Country>();
            CountryToForFreeTrialList = new List<TrialCountryInfo>();
        }
        public List<Country> CountryFromList { get; set; }
        public List<TrialCountryInfo> CountryToForFreeTrialList { get; set; }

        [Required(ErrorMessage = "The countryfrom is required")]
        public int TrialCountryFrom { get; set; }

        [Required(ErrorMessage = "The countryto is required")]
        public int TrialCountryTo { get; set; }

    }
}