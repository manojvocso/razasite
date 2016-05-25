using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using System.IO;
using System.Diagnostics;
using MvcApplication1.App_Start;


namespace MvcApplication1.Controllers
{
    [UnRequiresSSL]
    public class RateController : BaseController
    {
        DataRepository _repository = new DataRepository();
        //
        // GET: /Rate/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [CompressFilter]
        public ActionResult SearchRate(int countryfrom = 0, int countryto = 0, string mobileorGlobal = "",
            string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            if (countryfrom == 0)
            {
                return RedirectToAction("Plans");
            }

            var ratemodel = new RateModel();

            if (Request.QueryString["countryfrom"] != "")
            {
                ratemodel.CountryFromQuery = Convert.ToInt16(Request.QueryString["countryfrom"]);
                ratemodel.MobileOrGlobalPlan = Request.QueryString["MobileDiect"];
                ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
                    ? SharedResources.CountriesMap[countryto] + ".jpg"
                    : string.Empty;
            }

            ////******************************************* ADDED BY SABAL ON 10/27/2014 TO ADD DYNAMIC META TAGS *********************************************************
            //var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
            //var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            //ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name +" | Raza";
            //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
            //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
            ////***********************************************************************************************************************************************************


            //******************************************* ADDED BY SABAL ON 11/12/2014 TO ADD DYNAMIC META TAGS *********************************************************

            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag = data.RatePageMetaTags.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);

            foreach (var item in MyMetaTag)
            {
                ViewBag.Title = item.Title;
                ViewBag.PageDescription = item.PageDescription;
                ViewBag.PageKeywords = item.PageKeywords;
            }
            if (string.IsNullOrEmpty(ViewBag.Title))
            {
                var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
                var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

                ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name + " | Raza";
                //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
                ViewBag.PageDescription = "Raza: Call " + countrytodata.Name + " from " + countryfromdata.Name + " at attractive rates! Dedicated calling plans for landlines & mobile phones to help you SAVE money!";
                //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
                ViewBag.PageKeywords = "Calling Cards to " + countrytodata.Name + " from " + countryfromdata.Name + ", Cheap Phone Cards to " + countrytodata.Name;
            }
            //***********************************************************************************************************************************************************


            return View("SearchRate2", ratemodel);
            
            //return View(ratemodel);
        }





        //[CompressFilter]
        //public ActionResult SearchRate_MapRoute(string CountryName)
        //{
        //    int countryfrom = 1, countryto = 314;
        //    if (CountryName.ToUpper() == "INDIA")
        //    {
        //        countryfrom = 3;
        //        countryto = 234;
        //    }

        //    var ratemodel = new RateModel();

        //    ratemodel.CountryFromQuery = countryfrom;
        //    ratemodel.MobileOrGlobalPlan = "";
        //    ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
        //        ? SharedResources.CountriesMap[countryto] + ".jpg"
        //        : string.Empty;

        //    //return View("SearchRate2", ratemodel);

        //    //string returnUrl = Url.Action("SearchRate", "Rate", new { countryfrom = countryfrom, countryto = countryto });
        //    //return RedirectPermanent(returnUrl);
        //    //return RedirectToAction("Index", "Account", new { countryfrom = countryfrom, countryto = countryto });
            
            
        //    //return View(ratemodel);
        //    //return RedirectToAction("SearchRate", "Rate", new { countryfrom = countryfrom, countryto = countryto });
        //    return RedirectPermanent("~/Rate/SearchRate?countryfrom=1&countryto=130");


        //    //string returnUrl = Url.Action("SearchRate", "Rate", new { countryfrom = countryfrom, countryto = countryto });
        //    //return RedirectPermanent("~/Rate/SearchRate?countryfrom=3&countryto=238");
        //    //return RedirectPermanent(returnUrl);
            
            
        //    //return RedirectToAction("Index", "Account");
        //}




        public JsonResult GetRatePage_MetaTag(int countryfrom, int countryto)
        {
            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag =
                data.RatePageMetaTags.FirstOrDefault(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto);
                

            return Json(MyMetaTag, JsonRequestBehavior.AllowGet);
        }





        public JsonResult SearchMethod(int countryfrom, int countryto, string mobileorGlobal, string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            var ratemodel = SearchForRateJson(countryfrom, countryto, mobileorGlobal, cardTypeName, callForwarding,
                globalcall);


            return Json(ratemodel, JsonRequestBehavior.AllowGet);
        }

        //public RateModel SearchForRate(int countryfrom, int countryto, string mobileorGlobal, string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        //{Class1.cs
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var ratemodel = new RateModel();
        //    var repository = new DataRepository();

        //    var fromCountries = CacheManager.Instance.GetFromCountries();
        //    ratemodel.CountryFromList = fromCountries;

        //    var toCountries = CacheManager.Instance.GetAllCountryTo();

        //    ratemodel.CountryToList = toCountries;

        //    ratemodel.CountryFrom = ratemodel.CountryFromList.FirstOrDefault(t => t.Id == countryfrom.ToString());
        //    ratemodel.CountryTo = ratemodel.CountryToList.FirstOrDefault(t => t.Id == countryto.ToString());

        //    var countrydata = toCountries.FirstOrDefault(a => a.Id == countryto.ToString());

        //    if (countrydata != null)
        //    {
        //        var countrycode = countrydata.CountCode;
        //        ratemodel.CountryCode = countrycode;
        //        // fill the list for country mobile list.
        //        var countrymobilelist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Mobile").ToList();

        //        //add country mobile to list 
        //        var mobilecitylist = countrymobilelist.Select(country => new Country()
        //        {
        //            CountCode = country.CountCode,
        //            CountryCode = country.CountryCode,
        //            Id = country.Id,
        //            Name = country.Name,
        //        }).ToList();

        //        var countrycitylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "City").ToList();

        //        //add country city to list
        //        mobilecitylist.AddRange(countrycitylist.Select(country => new Country()
        //        {
        //            CountCode = country.CountCode,
        //            CountryCode = country.CountryCode,
        //            Id = country.Id,
        //            Name = country.Name,
        //        }));

        //        var onlycountrylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Country").ToList();
        //        mobilecitylist.AddRange(onlycountrylist.Select(country => new Country()
        //        {
        //            CountCode = country.CountCode,
        //            CountryCode = country.CountryCode,
        //            Id = country.Id,
        //            Name = country.Name,
        //        }));

        //        ratemodel.MobileCityList = mobilecitylist.OrderBy(a => a.Name).ToList();
        //        ratemodel.CountryCityList = countrycitylist;
        //        ratemodel.CountryMobileList = countrymobilelist;

        //        //search rates
        //        Rates rate = new Rates()
        //        {
        //            CardName = string.Empty,
        //            CountryFrom = countryfrom,
        //            CountryTo = countryto
        //        };

        //        //code ends here for India unlimited Plans

        //        // RatePlans rateplan = repository.GetRates(rate); //comment
        //        RatePlans rateplan = CacheManager.Instance.GetRatesFromCache(rate);

        //        var currencydict = FlagDictonary.GetCurrencycode();
        //        var ratepermindict = FlagDictonary.RatePerMinSign();

        //        if (ratepermindict.ContainsKey(countryfrom))
        //        {
        //            ratemodel.RateperMinSign = ratepermindict[countryfrom];
        //        }
        //        else
        //        {
        //            ratemodel.RateperMinSign = "¢/min";

        //        }

        //        switch (cardTypeName)
        //        {
        //            case "Mobile":
        //                var mobileratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

        //                foreach (EachRatePlan erp in mobileratecards)
        //                {
        //                    ratemodel.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }
        //                ratemodel.SearchType = "Mobile";
        //                break;
        //            case "City":
        //                var directratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "3");  // city direct
        //                foreach (EachRatePlan erp in directratecards)
        //                {
        //                    ratemodel.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }
        //                ratemodel.SearchType = "City";
        //                break;
        //            case "Country":
        //                var onetouchdialratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  //onetouch dial
        //                foreach (EachRatePlan erp in onetouchdialratecards)
        //                {
        //                    ratemodel.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }
        //                ratemodel.SearchType = "Country";
        //                break;
        //            default:
        //                var mobileratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); //mobile direct

        //                foreach (EachRatePlan erp in mobileratecards1)
        //                {
        //                    ratemodel.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }

        //                var directratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "3"); // city direct
        //                foreach (EachRatePlan erp in directratecards1)
        //                {
        //                    ratemodel.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }

        //                var onetouchdialratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  // onetouch dial
        //                foreach (EachRatePlan erp in onetouchdialratecards1)
        //                {
        //                    ratemodel.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
        //                }

        //                break;
        //        }

        //        var tocountrydata = toCountries.SingleOrDefault(a => a.Id == countryto.ToString());
        //        if (tocountrydata != null)
        //        {
        //            if (mobileorGlobal == "Mobile")
        //            {

        //                ratemodel.SearchType = "Mobile";

        //            }
        //            else if (mobileorGlobal != "Mobile")
        //            {
        //                ratemodel.SearchType = tocountrydata.RateType;
        //            }
        //            if (tocountrydata.RateType == "Country")
        //            {
        //                var mobiledata =
        //                    toCountries.Where(a => a.RateType == "Mobile")
        //                        .FirstOrDefault(a => a.Name.StartsWith(tocountrydata.Name));

        //                if (mobiledata != null)
        //                {
        //                    Rates rt = new Rates()
        //                    {
        //                        CardName = string.Empty,
        //                        CountryFrom = countryfrom,
        //                        CountryTo = SafeConvert.ToInt32(mobiledata.Id),
        //                    };

        //                    //RatePlans dataplan = repository.GetRates(rt); //comment
        //                    RatePlans dataplan = CacheManager.Instance.GetRatesFromCache(rt);
        //                    var mobileratecards1 = dataplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

        //                    foreach (EachRatePlan erp in mobileratecards1)
        //                    {
        //                        ratemodel.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict,
        //                            countryfrom, SafeConvert.ToInt32(mobiledata.Id)));
        //                    }

        //                }

        //            }
        //            if (tocountrydata.RateType == "Country" || tocountrydata.RateType == "Mobile")
        //            {
        //                if (!ratemodel.MobileDirectRateCards.Any())
        //                {
        //                    ratemodel.MobileDirectRateCards = ratemodel.OneTouchDialRateCards;
        //                }
        //            }
        //            else if (tocountrydata.RateType == "City" && !ratemodel.CityDirectRateCards.Any())
        //            {
        //                ratemodel.CityDirectRateCards = ratemodel.OneTouchDialRateCards;
        //            }
        //        }
        //    }

        //    stopwatch.Stop();

        //    ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto) ? SharedResources.CountriesMap[countryto] : string.Empty;
        //    return ratemodel;
        //}

        public RateViewModelJson SearchForRateJson(int countryfrom, int countryto, string mobileorGlobal, string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            // var rateModel = new RateModel();

            var ratemodelJson = new RateViewModelJson();
            var repository = new DataRepository();

            var toCountries = CacheManager.Instance.GetAllCountryTo();

            ratemodelJson.CountryToList = toCountries;

            //  ratemodelJson.CountryFrom = rateModel.ListOfFromCountries.FirstOrDefault(t => t.Id == countryfrom.ToString());
            //  ratemodelJson.CountryTo = rateModel.ListOfToCountries.FirstOrDefault(t => t.Id == countryto.ToString());

            var countrydata = toCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            if (countrydata != null)
            {
                var countrycode = countrydata.CountCode;
                ratemodelJson.CountryCode = countrycode;
                // fill the list for country mobile list.
                var countrymobilelist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Mobile").ToList();

                //add country mobile to list 
                var mobilecitylist = countrymobilelist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }).ToList();

                var countrycitylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "City").ToList();

                //add country city to list
                mobilecitylist.AddRange(countrycitylist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }));

                var onlycountrylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Country").ToList();
                mobilecitylist.AddRange(onlycountrylist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }));

                ratemodelJson.MobileCityList = mobilecitylist.OrderBy(a => a.Name).ToList();
                ratemodelJson.CountryCityList = countrycitylist;
                ratemodelJson.CountryMobileList = countrymobilelist;

                //search rates
                Rates rate = new Rates()
                {
                    CardName = string.Empty,
                    CountryFrom = countryfrom,
                    CountryTo = countryto
                };

                //code ends here for India unlimited Plans

                // RatePlans rateplan = repository.GetRates(rate); //comment
                RatePlans rateplan = CacheManager.Instance.GetRatesFromCache(rate);

                var currencydict = FlagDictonary.GetCurrencycode();
                var ratepermindict = FlagDictonary.RatePerMinSign();

                if (ratepermindict.ContainsKey(countryfrom))
                {
                    ratemodelJson.RateperMinSign = ratepermindict[countryfrom];
                }
                else
                {
                    ratemodelJson.RateperMinSign = "¢/min";

                }

                switch (cardTypeName)
                {
                    case "Mobile":
                        var mobileratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

                        foreach (EachRatePlan erp in mobileratecards)
                        {
                            ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "Mobile";
                        break;
                    case "City":
                        var directratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "3");  // city direct
                        foreach (EachRatePlan erp in directratecards)
                        {
                            ratemodelJson.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "City";
                        break;
                    case "Country":
                        var onetouchdialratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  //onetouch dial
                        foreach (EachRatePlan erp in onetouchdialratecards)
                        {
                            ratemodelJson.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "Country";
                        break;
                    default:
                        var mobileratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); //mobile direct

                        foreach (EachRatePlan erp in mobileratecards1)
                        {
                            ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        var directratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "3"); // city direct
                        foreach (EachRatePlan erp in directratecards1)
                        {
                            ratemodelJson.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        var onetouchdialratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  // onetouch dial
                        foreach (EachRatePlan erp in onetouchdialratecards1)
                        {
                            ratemodelJson.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        break;
                }

                var tocountrydata = toCountries.SingleOrDefault(a => a.Id == countryto.ToString());
                if (tocountrydata != null)
                {
                    if (mobileorGlobal == "Mobile")
                    {

                        ratemodelJson.SearchType = "Mobile";

                    }
                    else if (mobileorGlobal != "Mobile")
                    {
                        ratemodelJson.SearchType = tocountrydata.RateType;
                    }
                    if (tocountrydata.RateType == "Country")
                    {
                        var mobiledata =
                            toCountries.Where(a => a.RateType == "Mobile")
                                .FirstOrDefault(a => a.Name.StartsWith(tocountrydata.Name));

                        if (mobiledata != null)
                        {
                            Rates rt = new Rates()
                            {
                                CardName = string.Empty,
                                CountryFrom = countryfrom,
                                CountryTo = SafeConvert.ToInt32(mobiledata.Id),
                            };

                            //RatePlans dataplan = repository.GetRates(rt); //comment
                            RatePlans dataplan = CacheManager.Instance.GetRatesFromCache(rt);
                            var mobileratecards1 = dataplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

                            foreach (EachRatePlan erp in mobileratecards1)
                            {
                                ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict,
                                    countryfrom, SafeConvert.ToInt32(mobiledata.Id)));
                            }

                        }

                    }
                    if (tocountrydata.RateType == "Country" || tocountrydata.RateType == "Mobile")
                    {
                        if (!ratemodelJson.MobileDirectRateCards.Any())
                        {
                            ratemodelJson.MobileDirectRateCards = ratemodelJson.OneTouchDialRateCards;
                        }
                    }
                    else if (tocountrydata.RateType == "City" && !ratemodelJson.CityDirectRateCards.Any())
                    {
                        ratemodelJson.CityDirectRateCards = ratemodelJson.OneTouchDialRateCards;
                    }
                }
            }
           
            ratemodelJson.CountryToList = new List<Country>();

            ratemodelJson.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto) ? SharedResources.CountriesMap[countryto] : string.Empty;
            return ratemodelJson;
        }

        private static PlanModel createPlanModel(bool callForwarding, EachRatePlan erp, IDictionary<string, string> currencydict, int countryfrom, int countryto)
        {
            return new PlanModel
            {
                CardType = (PlanType)erp.CardType,
                CardTypeName = erp.CardTypeName,
                FromToMapping = erp.FromToMapping,
                RatePerMin = callForwarding ? erp.RatePerMin + 2 : erp.RatePerMin,//todo: add 2 cents/per min for call forwarding
                PlanAmount = erp.PlanAmount,
                PlanId = erp.PlanId,
                Discount = erp.Discount,
                ServiceFee = erp.ServiceFee,
                CurrencyCode = erp.CurrencyCode,
                CurrencySign = currencydict[erp.CurrencyCode],
                TotalMinutes = erp.TotalMinutes,
                IsAutoRefill = false,
                CountryFrom = countryfrom,
                CountryTo = countryto

            };
        }

        private List<Unlimitedindiapromotionalplan> UnlimitedPromotionalPlan(int countryfrom, int countryto)
        {
            string filePathName = Server.MapPath(@"/Content/UnlimitedIndiaPlan.xml");
            var dataresult = SerializationUtility<UnlimitedIndiaPlanModel>.DeserializeObject(System.IO.File.ReadAllText(filePathName));
            var result = new List<Unlimitedindiapromotionalplan>();

            var unlimitedPromotonPlan = dataresult.UnlimitedIndiaPlans;

            foreach (var res in unlimitedPromotonPlan)
            {
                result.Add(new Unlimitedindiapromotionalplan()
                {
                    ServiceFee = res.ServiceFee,
                    CountryFrom = countryfrom,
                    CountryTo = countryto,
                    CentPerMinute = res.CentPerMinute,
                    TotalMinute = res.TotalMinute,
                });

            }
            return result;


        }


        private List<Indiacentpromotionalplanmodel> PromotionalPlan(int countryfrom, int countryto)
        {
            string filepath = Server.MapPath(@"/Content/IndiacentPlan.xml");
            var data = SerializationUtility<IndiaCentPlanModel>.DeserializeObject(System.IO.File.ReadAllText(filepath));
            var result = new List<Indiacentpromotionalplanmodel>();
            // var PromotionalCentPlan = data.IndiaCentPlanRate.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);
            var PromotionalCentPlan = data.IndiaCentPlanRate;
            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var res in PromotionalCentPlan)
            {
                result.Add(new Indiacentpromotionalplanmodel()
                {
                    CountryFrom = countryfrom,
                    CountryTo = countryto,
                    ServiceFee = res.ServiceFee,
                    TotalCharge = res.TotalCharge,
                    CentPerMinute = res.CentPerMinute,
                    TotalMinute = res.TotalMinute,
                    CurrencyCode = res.CurrencyCode,
                    CountryCode = res.CountryCode,
                    RatePerMinSign = sign
                });

            }
            return result;


        }


        public ActionResult AddAutoRefill(string MemberID, string Pin, double ReFill_Amount, string Card_Number)
        {
            //var data = _repository.AddAu(MemberID, Pin, ReFill_Amount, Card_Number);
            return null;
        }

        [CompressFilter]
        public ActionResult Plans()
        {
            var plans = new PlanRate();
            return View(plans);
        }

        public ActionResult AdditionalPlans()
        {
            return View(new GenericModel());
        }

        public ActionResult IndiaOneCent()
        {
            return View(new GenericModel());
        }

        public ActionResult IndiaUnlimited()
        {
            return View(new GenericModel());
        }


        [HttpPost]
        public ActionResult BuyAdditionalPlans(ShoppingCartModel model)
        {
            //For India 1 Cent plan
            //163 1 CENT PLAN
            //164 CANADA 1 CENT PLAN
            //125 CANADA TALK CITY
            //126 CANADA TALK MOBILE
            //120 TALK CITY
            //121 TALK MOBILE


            //India unlimited plan

            //176 CANADA MOBILE UNLIMITED $9.99
            //178 CANADA UNLIMITED $14.99
            //180 CANADA UNLIMITED $19.99
            //175 MOBILE UNLIMITED $9.99
            //177 UNLIMITED $14.99
            //179 UNLIMITED $19.99
            if (model.CountryFrom == 0)
            {
                //model.CountryFrom = (int) Session["CountrybyIp"];
                model.CountryFrom = SafeConvert.ToInt32(Session["CountrybyIp"]);

                //**************** UPDATED ON 10/21/2015 BY SABAL TO FIX NULL VALUE OF SESSION["COUNTRYBYIP"] ***********
                //model.CountryFrom = 1;
                //if (Session["CountrybyIp"] != null)
                //{
                //    model.CountryFrom = SafeConvert.ToInt32(Session["CountrybyIp"]);
                //}
                //*******************************************************************************************************
            }
            if (model.CountryFrom == 1)
            {
                switch (model.PlanId)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "175";
                            model.PlanName = "MOBILE UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(9.99);
                            model.ServiceFee = Convert.ToDecimal(3.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "177";
                            model.PlanName = "UNLIMITED $14.99";
                            model.Price = Convert.ToDecimal(14.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "179";
                            model.PlanName = "UNLIMITED $19.99";
                            model.Price = Convert.ToDecimal(19.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "163";
                            model.PlanName = "1 Cent Plan";
                            model.Price = Convert.ToDecimal(15);
                            model.ServiceFee = Convert.ToDecimal(4.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "121";
                            model.PlanName = "Talk Mobile";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "120";
                            model.PlanName = "Talk City";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                }
                model.CurrencyCode = "USD";

            }
            else if (model.CountryFrom == 2)
            {
                switch (model.PlanId)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "176";
                            model.PlanName = "CANADA MOBILE UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(9.99);
                            model.ServiceFee = Convert.ToDecimal(3.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "178";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(14.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "180";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(19.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "164";
                            model.PlanName = "Canada 1 Cent Plan";
                            model.Price = Convert.ToDecimal(15);
                            model.ServiceFee = Convert.ToDecimal(4.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "126";
                            model.PlanName = "Canada Talk Mobile";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "125";
                            model.PlanName = "Canada Talk City";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                }
                model.CurrencyCode = "CAD";
            }
            model.CountryTo = 130;
            var countryfromdata = CacheManager.Instance.GetFromCountries();
            var callingfrom = countryfromdata.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryFrom));

            var countrytodata = CacheManager.Instance.GetCountryListTo();
            var callingto = countrytodata.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryTo));


            model.FromToMapping = SafeConvert.ToString(model.PlanId);

            model.CallingFrom = callingfrom != null ? callingfrom.Name : string.Empty;
            model.CallingTo = callingto != null ? callingto.Name : string.Empty;

            Session["Cart"] = null;
            Session["Cart"] = model;
            return RedirectToAction("Index", "Cart");

        }


    }


    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);
            response.Write(serializedObject);
        }


    }

}
