using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;
namespace MvcApplication1.Models
{
    public class BaseRazaViewModel
    {
       
        public BaseRazaViewModel()
        {
            TrialCountriesPlans = CacheManager.Instance.FreeTrial_Country_List();
            ListOfToCountries = CacheManager.Instance.GetAllCountryTo();
            ListOfFromCountries = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).ToList();
            ListOfTop3FromCountries = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();
            CountryListTo = CacheManager.Instance.GetCountryListTo();
           
        }

        public List<Country> ListOfFromCountries { get; set; }
        public List<Country> ListOfToCountries { get; set; }
        public List<Country> ListOfTop3FromCountries { get; set; }
        public List<TrialCountryInfo> TrialCountriesPlans { get; set; }
           public List<Country> CountryListTo{ get; set; }
    

        public int CountrybyIp { get; set; }
        public bool Isnewuser { get; set; }


    }
}