using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public enum PlanType
    {
        MobileDirect = 1,
        CityDirect = 2,
        OneTouchDial = 3
    }

    public class RateModel : BaseRazaViewModel
    {
        public RateModel()
        {
            //OneTouchDialRateCards = new List<PlanModel>();
            //CityDirectRateCards = new List<PlanModel>();
            //MobileDirectRateCards = new List<PlanModel>();
            //MobileCityList = new List<Country>();
            //CountryMobileList = new List<Country>();
            //CountryCityList = new List<Country>();
            //IndiaCentPlan = new List<Indiacentpromotionalplanmodel>();
            //UnlimitedIndiaPlan = new List<Unlimitedindiapromotionalplan>();

            //CountryFrom = -1;
            //CountryTo = -1;
        }
        //public List<PlanModel> OneTouchDialRateCards { get; set; }
        //public List<PlanModel> CityDirectRateCards { get; set; }
        //public List<PlanModel> MobileDirectRateCards { get; set; }
        //public List<Indiacentpromotionalplanmodel> IndiaCentPlan { get; set; }
        //public List<Unlimitedindiapromotionalplan> UnlimitedIndiaPlan { get; set; }
        //public List<Country> CountryFromList { get; set; }
        //public List<Country> CountryToList { get; set; }
        //public List<Country> MobileCityList { get; set; }
        //public List<Country> CountryMobileList { get; set; }
        //public List<Country> CountryCityList { get; set; }

        //public Country CountryFrom { get; set; }
        //public Country CountryTo { get; set; }
        //public string CountryCode { get; set; }

        public string MobileOrGlobalPlan { get; set; }

        //public string AutoRefill { get; set; }
        //public string FromToMapping { get; set; }
        //public string PlanId { get; set; }
        //public string SearchType { get; set; }
        //public string RateperMinSign { get; set; }
        public int CountryFromQuery { get; set; }
        public string CountryBannerPath { get; set; }

    }

    public class PlanModel //: BaseRazaViewModel
    {
        public string CardTypeName { get; set; }
        public PlanType CardType { get; set; }
        public string FromToMapping { get; set; }
        public string PlanId { get; set; }
        public decimal PlanAmount { get; set; }
        public decimal RatePerMin { get; set; }
        public decimal Discount { get; set; }
        public decimal ServiceFee { get; set; }
        public float Minute { get; set; }
        public float StrikeoutAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public decimal TotalMinutes { get; set; }
        public bool IsAutoRefill { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
    }
    
    public class OverviewModel
    {
    }
    public class CompareModel
    {
    }
    public class SearchModel
    {
    }

    public class PromotionPlanModel : BaseRazaViewModel
    {
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
    }


    public class RateViewModelJson
    {
        public RateViewModelJson()
        {
            OneTouchDialRateCards = new List<PlanModel>();
            CityDirectRateCards = new List<PlanModel>();
            MobileDirectRateCards = new List<PlanModel>();
            MobileCityList = new List<Country>();
            CountryMobileList = new List<Country>();
            CountryCityList = new List<Country>();
            IndiaCentPlan = new List<Indiacentpromotionalplanmodel>();
            UnlimitedIndiaPlan = new List<Unlimitedindiapromotionalplan>();
        }

        public List<PlanModel> OneTouchDialRateCards { get; set; }
        public List<PlanModel> CityDirectRateCards { get; set; }
        public List<PlanModel> MobileDirectRateCards { get; set; }
        public List<Indiacentpromotionalplanmodel> IndiaCentPlan { get; set; }
        public List<Unlimitedindiapromotionalplan> UnlimitedIndiaPlan { get; set; }
        public List<Country> CountryFromList { get; set; }
        public List<Country> CountryToList { get; set; }
        public List<Country> MobileCityList { get; set; }
        public List<Country> CountryMobileList { get; set; }
        public List<Country> CountryCityList { get; set; }

        public Country CountryFrom { get; set; }
        public Country CountryTo { get; set; }
        public string CountryCode { get; set; }

        public string MobileOrGlobalPlan { get; set; }

        public string AutoRefill { get; set; }
        public string FromToMapping { get; set; }
        public string PlanId { get; set; }
        public string SearchType { get; set; }
        public string RateperMinSign { get; set; }
        public int CountryFromQuery { get; set; }
        public string CountryBannerPath { get; set; }

    }
}
