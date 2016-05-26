using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class SearchRateViewModel : BaseMobileViewModel
    {
        public SearchRateViewModel()
        {
            CallingToCountries = new List<Country>();
            CountryRegionsList = new List<Country>();
            AutoRefillRates = new List<SearchRateMobileEntity>();
            WithOutAutoRefillRates = new List<SearchRateMobileEntity>();
        }
        public List<Country> CallingToCountries { get; set; }

        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string CountryToName { get; set; }

        public List<Country> CountryRegionsList { get; set; }

        public List<SearchRateMobileEntity> AutoRefillRates { get; set; }
        public List<SearchRateMobileEntity> WithOutAutoRefillRates { get; set; }
        public decimal RatePerMinAutoRefill { get; set; }
        public decimal RatePerMinWithOutAutoRefill { get; set; }

        public string CurrencyCode { get; set; }
        public string PlanName { get; set; }
    }

    public class SearchRateMobileEntity
    {
        public int StrikeAmount { get; set; }
        public decimal Amount { get; set; }
        public int Minute { get; set; }
        public decimal Servicefee { get; set; }
        public string FromToMapping { get; set; }
        public string PlanId { get; set; }
        public string CurrencyCode { get; set; }
    }



}