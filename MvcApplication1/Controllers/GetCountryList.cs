using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Raza.Model;
using Raza.Repository;

namespace MvcApplication1.Controllers
{
    public class GetCountryList
    {
        DataRepository _repository = new DataRepository();
        public List<Country> GetFullCountryList()
        {
            //var countrylist = _repository.GetCountryList("to");
            var countrylist = CacheManager.Instance.GetCountryListTo();
            var list = countrylist.Select(country => new Country()
            {
                CountCode = country.CountCode,
                CountryCode = country.CountryCode,
                Name = country.Name,
                Id = country.Id,
                RateType = "Country"
            }).ToList();

            //var countrycitylist = _repository.GetCountryList("to", "1");
            var countrycitylist = CacheManager.Instance.GetOnlyCityCountry();
            list.AddRange(countrycitylist.Select(country => new Country()
            {
                CountCode = country.CountCode,
                CountryCode = country.CountryCode,
                Name = country.Name,
                Id = country.Id,
                RateType = "City"
            }));

            //var countrymobilelist = _repository.GetCountryList("to", "2");
            var countrymobilelist = CacheManager.Instance.GetOnlyMobileCountry();
            list.AddRange(countrymobilelist.Select(country => new Country()
            {
                CountCode = country.CountCode,
                CountryCode = country.CountryCode,
                Name = country.Name,
                Id = country.Id,
                RateType = "Mobile"
            }));

            list = list.OrderBy(a => a.Name).ToList();
            return list;

        } 
    }
}