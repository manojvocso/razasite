using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PayPal;
using Raza.Common;

namespace MvcApplication1.Controllers
{
    using System.Web.Caching;

    using Raza.Model;
    using Raza.Repository;

    public class CacheManager
    {
        private Cache _cache;


        private DataRepository _repository = new DataRepository();

        private static CacheManager _instance = new CacheManager();

        public static CacheManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private readonly string CACHE_LOWEST_RATES_KEY = "LOWESTRATES";

        private readonly string CACHE_SEARCH_RATES_KEY = "SEARCHRATES";

        private readonly string CACHE_CUSTOMER_PLAN_LIST = "CUSTOMERPLANLIST";

        private string CACHE_COUNTRYLISTTO = "COUNTRYLISTTO";

        private string CACHE_COUNTRYLISTTOFULL = "COUNTRYLISTTOFULL";

        private readonly string CACHE_FREETRIALCOUNTRYLIST = "CACHE_FREETRIALCOUNTRYLIST";

        private readonly string CACHE_FROMCOUNTRYLIST = "CACHE_FROMCOUNTRYLIST";

        private readonly string CACHE_TOP3FROMCOUNTRYLIST = "CACHE_TOP3FROMCOUNTRYLIST";

        private readonly string CACHE_COUNTRYOPERATORS = "CACHE_COUNTRYOPERATORS";

        private readonly string CACHE_ONLYMOBILECOUNTRY = "CACHE_ONLYMOBILECOUNTRY";

        private readonly string CACHE_ONLYCITYCOUNTRY = "CACHE_ONLYCITYCOUNTRY";

        private readonly string CACHE_OPERATORIMAGES = "CACHE_OPERATORIMAGES";

        private readonly string CACHE_ALLCOUNTRYWITHLOWESTRATES = "CACHE_ALLCOUNTRYWITHLOWESTRATES";

        public GetLowestRate GetLowestRatesCache(int countryFrom, int countryTo)
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            string key = string.Format("{0}-{1}", countryFrom, countryTo);

            if (_cache[CACHE_LOWEST_RATES_KEY] == null)
            {
                _cache.Add(
                    CACHE_LOWEST_RATES_KEY,
                    new Dictionary<string, GetLowestRate>(),
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            var cachedObject = (Dictionary<string, GetLowestRate>)_cache[CACHE_LOWEST_RATES_KEY];

            if ((cachedObject.ContainsKey(key)))
            {
                return cachedObject[key];
            }

            var data = _repository.GetLowestRates(countryFrom, countryTo);
            cachedObject.Add(key, data);
            return data;

        }



        public OrderHistorySnapshot GetCustomerPlanListCache(string memberId)
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            string plankey = memberId;

            if (_cache[CACHE_CUSTOMER_PLAN_LIST] == null)
            {
                _cache.Add(
                    CACHE_CUSTOMER_PLAN_LIST,
                    new Dictionary<string, OrderHistorySnapshot>(),
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            var cachedObject = (Dictionary<string, OrderHistorySnapshot>)_cache[CACHE_CUSTOMER_PLAN_LIST];

            if ((cachedObject.ContainsKey(plankey)))
            {
                return cachedObject[plankey];
            }

            var data = _repository.GetCustomerPlanList(memberId);
            cachedObject.Add(plankey, data);
            return data;

        }


        public List<Country> GetCountryListTo()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYLISTTO] == null)
            {
                var countries = _repository.GetCountryList("to");
                _cache.Add(
                    CACHE_COUNTRYLISTTO,
                    countries,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_COUNTRYLISTTO] as List<Country>;

        }

        public List<Country> GetAllCountryTo()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYLISTTOFULL] == null)
            {
                var list = _repository.GetCountryList("to", "");
                _cache.Add(
                    CACHE_COUNTRYLISTTOFULL,
                    list,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_COUNTRYLISTTOFULL] as List<Country>;
        }


        public List<Country> GetFromCountries()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_FROMCOUNTRYLIST] == null)
            {
                var countries = _repository.GetCountryList("from", "");
                _cache.Add(
                    CACHE_FROMCOUNTRYLIST,
                    countries,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_FROMCOUNTRYLIST] as List<Country>;
        }

        public List<Country> GetTop3FromCountries()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_TOP3FROMCOUNTRYLIST] == null)
            {
                var countries = _repository.GetCountryList("from", "").Take(3).ToList();
                _cache.Add(
                    CACHE_TOP3FROMCOUNTRYLIST,
                    countries,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_TOP3FROMCOUNTRYLIST] as List<Country>;
        }

        public List<TrialCountryInfo> FreeTrial_Country_List()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_FREETRIALCOUNTRYLIST] == null)
            {
                var countries = _repository.FreeTrial_Country_List();
                _cache.Add(
                    CACHE_FREETRIALCOUNTRYLIST,
                    countries,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_FREETRIALCOUNTRYLIST] as List<TrialCountryInfo>;
        }

        public TopUpOperator GetCountryOperators(int countryid)
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYOPERATORS] == null)
            {
                var operators = _repository.GetMobile_TopupOperator(countryid);
                _cache.Add(
                    CACHE_COUNTRYOPERATORS,
                    operators,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_COUNTRYOPERATORS] as TopUpOperator;
        }


        public List<Country> GetOnlyMobileCountry()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYOPERATORS] == null)
            {
                var list = _repository.GetCountryList("to", "2");
                _cache.Add(
                    CACHE_ONLYMOBILECOUNTRY,
                    list,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_ONLYMOBILECOUNTRY] as List<Country>;
        }

        public List<Country> GetOnlyCityCountry()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYOPERATORS] == null)
            {
                var list = _repository.GetCountryList("to", "1");
                _cache.Add(
                    CACHE_ONLYCITYCOUNTRY,
                    list,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_ONLYCITYCOUNTRY] as List<Country>;
        }

        public OperatorImages GetOpeartorImages(string filepath)
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_COUNTRYOPERATORS] == null)
            {

                OperatorImages operatorImages = SerializationUtility<OperatorImages>.DeserializeObject(File.ReadAllText(filepath));

                _cache.Add(
                    CACHE_OPERATORIMAGES,
                    operatorImages,
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            return _cache[CACHE_OPERATORIMAGES] as OperatorImages;
        }

        public RatePlans GetRatesFromCache(Rates rates)
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            string key = string.Format("{0}-{1}", rates.CountryFrom, rates.CountryTo);

            if (_cache[CACHE_SEARCH_RATES_KEY] == null)
            {
                _cache.Add(
                    CACHE_SEARCH_RATES_KEY,
                    new Dictionary<string, RatePlans>(),
                    null,
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(100),
                    CacheItemPriority.Normal,
                    null);
            }

            var cachedObject = (Dictionary<string, RatePlans>)_cache[CACHE_SEARCH_RATES_KEY];

            if ((cachedObject.ContainsKey(key)))
            {
                return cachedObject[key];
            }
            var data = _repository.GetRates(rates);
            cachedObject.Add(key, data);
            return data;

        }

        public List<CountryWithLowestRateModel> GetAllCountriesWithLowestRates()
        {
            if (_cache == null)
            {
                _cache = HttpContext.Current.Cache;
            }

            if (_cache[CACHE_FROMCOUNTRYLIST] == null)
            {
                var countries = _repository.GetCountryWithLowestRates();
                if (countries.Any())
                {
                    _cache.Add(
                        CACHE_ALLCOUNTRYWITHLOWESTRATES,
                        countries,
                        null,
                        Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(100),
                        CacheItemPriority.Normal,
                        null);
                }
                else
                {
                    RazaLogger.WriteErrorForMobile("****************** XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX ***********************");
                    RazaLogger.WriteErrorForMobile("Home page country list is null from service");
                    RazaLogger.WriteErrorForMobile("****************** XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX ***********************");
                }
            }

            return _cache[CACHE_ALLCOUNTRYWITHLOWESTRATES] as List<CountryWithLowestRateModel>;
        }

    }

}