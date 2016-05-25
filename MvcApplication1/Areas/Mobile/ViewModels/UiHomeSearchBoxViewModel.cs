using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class UiHomeSearchBoxViewModel
    {
        public UiHomeSearchBoxViewModel()
        {
            CallingFromCountries= new List<Country>();
            CallingToCountries = new List<Country>();
            CallingToCountriesWithLowestRate= new List<CountryWithLowestRateModel>();
        }

        public List<Country> CallingFromCountries { get; set; }

        public List<Country> CallingToCountries { get; set; }
        
        public List<CountryWithLowestRateModel> CallingToCountriesWithLowestRate { get; set; }
    }
}