using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.App_Start;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Model;
using Raza.Repository;




using System.Net;
using System.IO;
using System.Xml;
using System.Net.Mail;

namespace MvcApplication1.Controllers
{
    [UnRequiresSSL]
    public class PromotionController : BaseController
    {
        private System.Xml.XmlDocument XD = new System.Xml.XmlDocument();

        //
        // GET: /Promotion/
        DataRepository _repository = new DataRepository();
        [CompressFilter]
        public ActionResult BangladeshPromotion()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult OneCentPlan()
        {
            var model = new GenericModel();
            model.CountrybyIp = (int)Session["CountrybyIp"];
            return View(model);
        }



        [CompressFilter]
        [HttpGet]
        public ActionResult Bangladesh1CentNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            return View(model);
        }

        [CompressFilter]
        [Authorize]
        [HttpGet]
        public ActionResult Bangladesh1CentExistCustomer(int countryfrom, int countryto)
        {
            return View(new GenericModel());
        }


        [Authorize]
        public ActionResult PromotionalPlanRecharge(string planname, decimal denomination, int fromcountry, int tocountry, string couponcode = "")
        {
            var planlist = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (planlist == null)
            {
                return RedirectToAction("Index", "Account");
            }
            var premiumplan =
                planlist.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162" || a.PlanId == "102");

            //string orderid, string RecBal, string currencycode, string servicefee, int planid, int countryfrom, int countryto, 
            // string RechAmount="", string CouponCode="", bool IsAutoRefill = false
            if (premiumplan != null)
            {
                return RedirectToAction("Index", "Recharge", new
                {
                    orderid = premiumplan.OrderId,
                    //RecBal = premiumplan.AccountBalance,
                    //currencycode = premiumplan.CurrencyCode,
                    //servicefee = premiumplan.ServiceFee,
                    //planid = premiumplan.PlanId,
                    //countryfrom = fromcountry,
                    //countryto = tocountry,
                    RechAmount = SafeConvert.ToString(denomination),
                    CouponCode = couponcode
                });
            }
            return RedirectToAction("Index", "Account");
        }

        public ActionResult BuyPromotionPlan(int countryfrom, int countryto, decimal denomination, string couponcode = "", decimal servicefee = 0)
        {
            string currencycode = string.Empty;
            int planid = 0;
            string planname = "";
            if (countryfrom == 1)
            {
                planid = 161;
                planname = "One Touch Dial";
                currencycode = "USD";
            }
            if (countryfrom == 2)
            {
                planid = 162;
                planname = "Canada One Touch Dial";
                currencycode = "CAD";
            }

            var countryfromdata = CacheManager.Instance.GetFromCountries();
            var callingfrom = countryfromdata.FirstOrDefault(a => a.Id == SafeConvert.ToString(countryfrom));

            var countrytodata = CacheManager.Instance.GetCountryListTo();
            var callingto = countrytodata.FirstOrDefault(a => a.Id == SafeConvert.ToString(countryto));

            var model = new ShoppingCartModel()
            {
                PlanName = planname,
                FromToMapping = SafeConvert.ToString(planid),
                Price = Convert.ToDecimal(denomination),
                CallingFrom = callingfrom != null ? callingfrom.Name : string.Empty,
                CallingTo = callingto != null ? callingto.Name : string.Empty,
                CurrencyCode = currencycode,
                ServiceFee = servicefee,
                IsAutoRefill = false,
                IsfromSerchrate = false,
                CountryFrom = countryfrom,
                CountryTo = countryto,
                CouponCode = couponcode
            };

            Session["Cart"] = null;
            Session["Cart"] = model;
            return RedirectToAction("Index", "Cart");
        }

        [CompressFilter]
        public ActionResult India250Promotion()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult India250NewCustPromotion()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Pakistan49Promotion()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Pakistan49NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Pakistan49ExistCustomer()
        {
            return View(new GenericModel());
        }

        //[CompressFilter]
        //public ActionResult MothersdayPromotion()
        //{
        //    return View(new GenericModel());
        //}

        //[CompressFilter]
        //public ActionResult MothersdayNewCustomer()
        //{
        //    return View(new GenericModel());
        //}

        //[CompressFilter]
        //public ActionResult MothersdayExistCustomer()
        //{
        //    return View(new GenericModel());
        //}

        [CompressFilter]
        public ActionResult Nepal69Promotion()
        {
            ViewBag.UserType = true;
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Nepal69NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }

            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Nepal69ExistCustomer()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult RamadanSpecial()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult RamadanSpecialNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }

        [CompressFilter]
        [Authorize]
        public ActionResult RamadanSpecialExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }

        public JsonResult GetNewCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/RamadamSpecial.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.TotalMinute =
                    SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
                item.RateperSign = sign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetExistCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/RamadamSpecial.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }





        [CompressFilter]
        public ActionResult NavaRatri()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult NavaRatriNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }







        [CompressFilter]
        [Authorize]
        public ActionResult NavaRatriExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }





        public JsonResult GetNavaRatriNewCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/NavaRatriSpecial.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.TotalMinute =
                    SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
                item.RateperSign = sign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetNavaRatriExistCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/NavaRatriSpecial.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }




        //Created on October 01 for Bakra Eid Promotion 2014
        [CompressFilter]
        public ActionResult EidMubarak()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult EidMubarakNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }







        [CompressFilter]
        [Authorize]
        public ActionResult EidMubarakExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }






        public JsonResult GetEidMubarakPromotionRate(int countryfrom, int countryto, string planname, string usertype)
        {
            string filepath = Server.MapPath(@"/Content/GetUpTo_Offer.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            if (usertype == "new")
            {
                foreach (var item in promotionrates)
                {
                    item.TotalMinute = SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
                    item.CurrencyCode = currencycode;
                    item.CurrencySign = currencysign;
                    item.RateperSign = sign;
                }
                return Json(promotionrates, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in promotionrates)
                {
                    item.CurrencyCode = currencycode;
                    item.CurrencySign = currencysign;
                }
                return Json(promotionrates, JsonRequestBehavior.AllowGet);
            }


            //string filepath = Server.MapPath(@"/Content/NavaRatriSpecial.xml");
            //var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            //var promotionrates =
            //    data.NewCustomerPlans.Where(
            //        a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            //var dict = FlagDictonary.GetCurrencycodebyCountry();
            //var currencycode = dict[countryfrom];

            //var dict2 = FlagDictonary.RatePerMinSign();
            //var sign = dict2[countryfrom];

            //var dict3 = FlagDictonary.GetCurrencycode();
            //var currencysign = dict3[currencycode];

            //foreach (var item in promotionrates)
            //{
            //    item.CurrencyCode = currencycode;
            //    item.CurrencySign = currencysign;
            //}


            //return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }







        ////Created on October 14 for Diwali Promotion 2014
        //[CompressFilter]
        //public ActionResult HappyDiwali()
        //{
        //    return View(new GenericModel());
        //}



        //[CompressFilter]
        //public ActionResult HappyDiwaliNewCustomer(int countryfrom, int countryto)
        //{
        //    ViewBag.CountryFromPromotion = countryfrom;
        //    ViewBag.CountryToPromotion = countryto;
        //    return View(new GenericModel());
        //}







        //[CompressFilter]
        //[Authorize]
        //public ActionResult HappyDiwaliExistCustomer(int countryfrom, int countryto)
        //{
        //    ViewBag.CountryFromPromotion = countryfrom;
        //    ViewBag.CountryToPromotion = countryto;
        //    var res = _repository.GetCustomerPlanList(UserContext.MemberId);
        //    if (res.OrderInfos != null)
        //    {
        //        var premiumplan =
        //            res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

        //        if (premiumplan != null)
        //        {
        //            ViewBag.OrderId = premiumplan.OrderId;
        //            ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
        //            ViewBag.CurrencyCode = premiumplan.CurrencyCode;
        //        }
        //        else
        //        {
        //            return RedirectToAction("NewCustomer", "Account");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("NewCustomer", "Account");
        //    }

        //    return View(new GenericModel());
        //}






        public JsonResult GetUpTo_PromotionRate(int countryfrom, int countryto, string planname, string usertype, string promocode = "")
        {
            if (promocode.ToUpper() == "NY2016")
                promocode = "HH2016";

            string filepath = Server.MapPath(@"/Content/GetUpTo_Promo.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname && a.CouponCode == promocode);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            if (usertype == "new")
            {
                foreach (var item in promotionrates)
                {
                    item.TotalMinute = SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
                    item.CurrencyCode = currencycode;
                    item.CurrencySign = currencysign;
                    item.RateperSign = sign;
                }
                return Json(promotionrates, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in promotionrates)
                {
                    item.CurrencyCode = currencycode;
                    item.CurrencySign = currencysign;
                }
                return Json(promotionrates, JsonRequestBehavior.AllowGet);
            }
        }





        //Created on October 21 for Save Big EveryDay Pakistan Promotion
        [CompressFilter]
        public ActionResult SaveBigEveryDay()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult SaveBigEveryDayNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }







        [CompressFilter]
        [Authorize]
        public ActionResult SaveBigEveryDayExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }




        //Created on October 29 for Buy 1 Get 1 Offer
        [CompressFilter]
        public ActionResult Buy1Get1_NewYear()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult Buy1Get1NewCustomer_NewYear(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null)
            {
                if (UserContext.UserType.ToLower() == "old")
                    ViewBag.UserType = true;
                else
                    ViewBag.UserType = false;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;
            ViewBag.Countryto = countryto;

            return View(model);
        }





        //Created on October 29 for Buy 1 Get 1 Offer
        [CompressFilter]
        public ActionResult Buy1Get1_Current()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult Buy1Get1NewCustomer_Current(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null)
            {
                if (UserContext.UserType.ToLower() == "old")
                    ViewBag.UserType = true;
                else
                    ViewBag.UserType = false;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;
            ViewBag.Countryto = countryto;

            return View(model);
        }




        //Created on Feb 29 2016 to include multiple countries
        [CompressFilter]
        public ActionResult Buy1Get1()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult Buy1Get1NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null)
            {
                if (UserContext.UserType.ToLower() == "old")
                    ViewBag.UserType = true;
                else
                    ViewBag.UserType = false;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;
            ViewBag.Countryto = countryto;

            return View(model);
        }




        //Created on February 17th for ICC Cricket Promotion 2015
        [CompressFilter]
        public ActionResult IccContest()
        {
            return View("IccContest.html");
        }




        [CompressFilter]
        public ActionResult Pakistan29Promotion()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Pakistan29NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Pakistan29ExistingCustomer()
        {
            return View(new GenericModel());
        }






        //Created on May 04 for MothersDay Promotion 2015
        [CompressFilter]
        public ActionResult MothersDay_2015()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult MothersDayNewCustomer_2015(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult MothersDayExistCustomer_2015(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult MaaContest()
        {
            return View(new GenericModel());
        }



        public JsonResult MaaContest_Submit(string FullName, string Email, string Phone, string City, string Address, string ZipCode, string State, string Message)
        {




            #region SEND ALL THE USER INPUT TO SERVER USING XMLHTTP WEB REQUEST

            //string strXML = "<?xml version=\"1.0\"?>\n";
            string strXML = "<xml>\n";
            strXML += "<MaaContest_Data>\n";
            strXML += "<FullName>" + FullName + "</FullName>\n";
            strXML += "<Email>" + Email + "</Email>\n";
            strXML += "<Phone>" + Phone + "</Phone>\n";
            strXML += "<Address>" + Address + "</Address>\n";
            strXML += "<City>" + City + "</City>\n";
            strXML += "<State>" + State + "</State>\n";
            strXML += "<ZipCode>" + ZipCode + "</ZipCode>\n";
            strXML += "<Message>" + Message + "</Message>\n";
            strXML += "</MaaContest_Data>\n";
            strXML += "</xml>\n";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.hotphonecard.com/services/maacontest.aspx");

            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.ContentLength = strXML.Length;

            //WebProxy myProxy = new WebProxy();
            //myProxy = (WebProxy)req.Proxy;

            string RespCode = "", RespMsg = "";

            req.Method = "POST";
            try
            {
                System.IO.Stream stream = req.GetRequestStream();
                byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strXML);
                stream.Write(arrBytes, 0, arrBytes.Length);
                stream.Close();

                string strResponse = "";
                WebResponse resp = req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                strResponse = rdr.ReadToEnd();

                XD = new XmlDocument();
                XD.LoadXml(strResponse);
                XmlNodeList nList = XD.GetElementsByTagName("Result");


                if (nList.Count > 0)
                {
                    XmlNodeList nList1 = nList[0].ChildNodes;

                    RespCode = nList1[0].InnerText.ToString();
                    if (RespCode == "1")
                        RespMsg = "Record Successfully Submitted. Thank You.";
                    else if (RespCode == "0")
                        RespMsg = "Error: Trying to Submit Duplicate Record.";
                    else
                        RespMsg = nList1[1].InnerText.ToString();
                }
                else
                {
                    RespCode = "-1";
                    RespMsg = "There is Problem. Please try again later.";
                }

            }
            catch (Exception Ex)
            {
                //RespCode = "-2";
                //RespMsg = "Execpton Error";

                RespCode = "-2";
                //RespMsg = Ex.ToString();
                RespMsg = "There is Problem. Please Try again later.";
            }
            #endregion

            return Json(RespMsg, JsonRequestBehavior.AllowGet);










            //var promotionrates =
            //    data.NewCustomerPlans.Where(
            //        a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname && a.CouponCode == promocode);

            //var dict = FlagDictonary.GetCurrencycodebyCountry();
            //var currencycode = dict[countryfrom];

            //var dict2 = FlagDictonary.RatePerMinSign();
            //var sign = dict2[countryfrom];

            //var dict3 = FlagDictonary.GetCurrencycode();
            //var currencysign = dict3[currencycode];

            //if (usertype == "new")
            //{
            //    foreach (var item in promotionrates)
            //    {
            //        item.TotalMinute = SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
            //        item.CurrencyCode = currencycode;
            //        item.CurrencySign = currencysign;
            //        item.RateperSign = sign;
            //    }
            //    return Json(promotionrates, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    foreach (var item in promotionrates)
            //    {
            //        item.CurrencyCode = currencycode;
            //        item.CurrencySign = currencysign;
            //    }
            //    return Json(promotionrates, JsonRequestBehavior.AllowGet);
            //}
        }



        [CompressFilter]
        public ActionResult PaaContest()
        {
            return View(new GenericModel());
        }


        public JsonResult PaaContest_Submit(string FullName, string Email, string Phone, string City, string Address, string ZipCode, string State, string Message)
        {

            #region SEND ALL THE USER INPUT TO SERVER USING XMLHTTP WEB REQUEST

            //string strXML = "<?xml version=\"1.0\"?>\n";
            string strXML = "<xml>\n";
            strXML += "<PaaContest_Data>\n";
            strXML += "<FullName>" + FullName + "</FullName>\n";
            strXML += "<Email>" + Email + "</Email>\n";
            strXML += "<Phone>" + Phone + "</Phone>\n";
            strXML += "<Address>" + Address + "</Address>\n";
            strXML += "<City>" + City + "</City>\n";
            strXML += "<State>" + State + "</State>\n";
            strXML += "<ZipCode>" + ZipCode + "</ZipCode>\n";
            strXML += "<Message>" + Message + "</Message>\n";
            strXML += "</PaaContest_Data>\n";
            strXML += "</xml>\n";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.hotphonecard.com/services/paacontest.aspx");

            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.ContentLength = strXML.Length;

            //WebProxy myProxy = new WebProxy();
            //myProxy = (WebProxy)req.Proxy;

            string RespCode = "", RespMsg = "";

            req.Method = "POST";
            try
            {
                System.IO.Stream stream = req.GetRequestStream();
                byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strXML);
                stream.Write(arrBytes, 0, arrBytes.Length);
                stream.Close();

                string strResponse = "";
                WebResponse resp = req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                strResponse = rdr.ReadToEnd();

                XD = new XmlDocument();
                XD.LoadXml(strResponse);
                XmlNodeList nList = XD.GetElementsByTagName("Result");


                if (nList.Count > 0)
                {
                    XmlNodeList nList1 = nList[0].ChildNodes;

                    RespCode = nList1[0].InnerText.ToString();
                    if (RespCode == "1")
                        RespMsg = "Record Successfully Submitted. Thank You.";
                    else if (RespCode == "0")
                        RespMsg = "Error: Trying to Submit Duplicate Record.";
                    else
                        RespMsg = nList1[1].InnerText.ToString();
                }
                else
                {
                    RespCode = "-1";
                    RespMsg = "There is Problem. Please try again later.";
                }

            }
            catch (Exception Ex)
            {
                //RespCode = "-2";
                //RespMsg = "Execpton Error";

                RespCode = "-2";
                //RespMsg = Ex.ToString();
                RespMsg = "There is Problem. Please Try again later.";
            }
            #endregion

            return Json(RespMsg, JsonRequestBehavior.AllowGet);

        }







        [CompressFilter]
        public ActionResult SelfieContest()
        {
            //return View(new GenericModel());
            
            
            
            //return View();
            return RedirectToAction("Index", "Account");
        }




        [HttpPost]
        public ActionResult SelfieContest(MvcApplication1.Models.MailModel objModelMail, HttpPostedFileBase fileUploader)
        {




            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(objModelMail.FirstName) || string.IsNullOrEmpty(objModelMail.LastName))
                {
                    ViewBag.Message = "invalid name";
                    return View("SelfieContest", objModelMail);
                }
                else if (string.IsNullOrEmpty(objModelMail.ActorName))
                {
                    ViewBag.Message = "invalid salman";
                    return View("SelfieContest", objModelMail);
                }
                else if (string.IsNullOrEmpty(objModelMail.Email))
                {
                    ViewBag.Message = "invalid email";
                    return View("SelfieContest", objModelMail);
                }
                else if (string.IsNullOrEmpty(objModelMail.Mobile))
                {
                    ViewBag.Message = "invalid mobile";
                    return View("SelfieContest", objModelMail);
                }
                else if (objModelMail.TermsnCond == false)
                {
                    ViewBag.Message = "invalid terms";
                    return View("SelfieContest", objModelMail);
                }
                else if (fileUploader == null)
                {
                    ViewBag.Message = "invalid photo";
                    return View("SelfieContest", objModelMail);
                }
                else
                {

                    //decimal filesize = ((fileUploader.PostedFile.ContentLength) / 1024) / 1024;


                    string fileName = Path.GetFileName(fileUploader.FileName);
                    decimal filesize = ((fileUploader.ContentLength) / 1024) / 1024;
                    if (filesize > 2)
                    {
                        ViewBag.Message = "invalid size";
                        return View("SelfieContest", objModelMail);
                    }
                    else if (Path.GetExtension(fileUploader.FileName).ToLower() != ".jpg" && Path.GetExtension(fileUploader.FileName).ToLower() != ".png" && Path.GetExtension(fileUploader.FileName).ToLower() != ".gif" && Path.GetExtension(fileUploader.FileName).ToLower() != ".jpeg")
                    {
                        ViewBag.Message = "invalid photo";
                        return View("SelfieContest", objModelMail);
                    }
                    else
                    {


                        #region SEND ALL THE USER INPUT TO SERVER USING XMLHTTP WEB REQUEST

                        //string strXML = "<?xml version=\"1.0\"?>\n";
                        string strXML = "<xml>\n";
                        strXML += "<SelfieContest_Data>\n";
                        strXML += "<FullName>" + objModelMail.FirstName + " " + objModelMail.LastName + "</FullName>\n";
                        strXML += "<Email>" + objModelMail.Email + "</Email>\n";
                        strXML += "<Phone>" + objModelMail.Mobile + "</Phone>\n";
                        strXML += "<Address>" + "" + "</Address>\n";
                        strXML += "<City>" + "" + "</City>\n";
                        strXML += "<State>" + "" + "</State>\n";
                        strXML += "<ZipCode>" + "" + "</ZipCode>\n";
                        strXML += "<Message>" + objModelMail.ActorName + "</Message>\n";
                        strXML += "</SelfieContest_Data>\n";
                        strXML += "</xml>\n";

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.hotphonecard.com/services/SelfieContest.aspx");

                        req.ContentType = "text/xml;charset=\"utf-8\"";
                        req.Accept = "text/xml";
                        req.ContentLength = strXML.Length;

                        //WebProxy myProxy = new WebProxy();
                        //myProxy = (WebProxy)req.Proxy;

                        string RespCode = "", RespMsg = "";

                        req.Method = "POST";
                        try
                        {
                            System.IO.Stream stream = req.GetRequestStream();
                            byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strXML);
                            stream.Write(arrBytes, 0, arrBytes.Length);
                            stream.Close();

                            string strResponse = "";
                            WebResponse resp = req.GetResponse();
                            Stream respStream = resp.GetResponseStream();
                            StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                            strResponse = rdr.ReadToEnd();

                            XD = new XmlDocument();
                            XD.LoadXml(strResponse);
                            XmlNodeList nList = XD.GetElementsByTagName("Result");


                            if (nList.Count > 0)
                            {
                                XmlNodeList nList1 = nList[0].ChildNodes;

                                RespCode = nList1[0].InnerText.ToString();
                                if (RespCode == "1")
                                {
                                    RespMsg = "Record Successfully Submitted. Thank You.";


                                    string from = "info@raza.com";
                                    string to = "sabal.raza@hotmail.com,nagesh@raza.com,nagesh0515@gmail.com,myflag786@gmail.com";
                                    //string to = "sabal@raza.com";
                                    //using (MailMessage mail = new MailMessage(from, objModelMail.To))
                                    using (MailMessage mail = new MailMessage(from, to))
                                    {
                                        string FullName = objModelMail.FirstName + " " + objModelMail.LastName;
                                        string ActorsName = objModelMail.ActorName;
                                        string EmailAddress = objModelMail.Email;
                                        string Phone = objModelMail.Mobile;

                                        //mail.Subject = objModelMail.Subject;
                                        mail.Subject = "Selfie Contest";

                                        //mail.Body = objModelMail.Body;
                                        mail.Body = "Contest Name: " + FullName + System.Environment.NewLine + "EmailAddress: " + EmailAddress + System.Environment.NewLine + "Phone: " + Phone + System.Environment.NewLine + "Actor Name: " + ActorsName;

                                        mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                                        //if (fileUploader != null)
                                        //{
                                        //    string fileName = Path.GetFileName(fileUploader.FileName);
                                        //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                                        //}

                                        mail.IsBodyHtml = false;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = "smtp.raza.com";
                                        smtp.EnableSsl = false;
                                        NetworkCredential networkCredential = new NetworkCredential(from, "harlem5219");
                                        smtp.UseDefaultCredentials = true;
                                        smtp.Credentials = networkCredential;
                                        smtp.Port = 25;
                                        smtp.Send(mail);

                                        ViewBag.Message = "Sent";
                                        //ViewBag.Message = "Sent";
                                        return View("SelfieContest", objModelMail);
                                    }
                                }
                                else if (RespCode == "0")
                                {
                                    ViewBag.Message = "duplicate record";
                                    return View("SelfieContest", objModelMail);
                                }
                                else
                                {
                                    //RespMsg = nList1[1].InnerText.ToString();
                                    ViewBag.Message = "try again";
                                    return View("SelfieContest", objModelMail);
                                }
                            }
                            else
                            {
                                ViewBag.Message = "try again";
                                return View("SelfieContest", objModelMail);
                            }

                        }
                        catch (Exception Ex)
                        {
                            //RespCode = "-2";
                            ViewBag.Message = "try again";
                            return View("SelfieContest", objModelMail);
                        }
                    }
                    #endregion

                }



            }
            else
            {
                return View();
            }














            //if (ModelState.IsValid)
            //{
            //    if (string.IsNullOrEmpty(objModelMail.FirstName) || string.IsNullOrEmpty(objModelMail.LastName))
            //    {
            //        ViewBag.Message = "invalid name";
            //        return View("SelfieContest", objModelMail);
            //    }
            //    else if (string.IsNullOrEmpty(objModelMail.ActorName))
            //    {
            //        ViewBag.Message = "invalid salman";
            //        return View("SelfieContest", objModelMail);
            //    }
            //    else if (string.IsNullOrEmpty(objModelMail.Email))
            //    {
            //        ViewBag.Message = "invalid email";
            //        return View("SelfieContest", objModelMail);
            //    }
            //    else if (string.IsNullOrEmpty(objModelMail.Mobile))
            //    {
            //        ViewBag.Message = "invalid mobile";
            //        return View("SelfieContest", objModelMail);
            //    }
            //    else if (objModelMail.TermsnCond == false)
            //    {
            //        ViewBag.Message = "invalid terms";
            //        return View("SelfieContest", objModelMail);
            //    }
            //    else
            //    {

            //        string from = "info@raza.com";
            //        string to = "sabal.raza@hotmail.com";
            //        //string to = "sabal@raza.com";
            //        //using (MailMessage mail = new MailMessage(from, objModelMail.To))
            //        using (MailMessage mail = new MailMessage(from, to))
            //        {
            //            string FullName = objModelMail.FirstName + " " + objModelMail.LastName;
            //            string ActorsName = objModelMail.ActorName;
            //            string EmailAddress = objModelMail.Email;
            //            string Phone = objModelMail.Mobile;

            //            //mail.Subject = objModelMail.Subject;
            //            mail.Subject = "Selfie Contest";

            //            //mail.Body = objModelMail.Body;
            //            mail.Body = "Contest Name: " + FullName + System.Environment.NewLine + "EmailAddress: " + EmailAddress + System.Environment.NewLine + "Phone: " + Phone + System.Environment.NewLine + "Actor Name: " + ActorsName;

            //            if (fileUploader != null)
            //            {
            //                string fileName = Path.GetFileName(fileUploader.FileName);
            //                mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
            //            }

            //            mail.IsBodyHtml = false;
            //            SmtpClient smtp = new SmtpClient();
            //            smtp.Host = "smtp.raza.com";
            //            smtp.EnableSsl = false;
            //            NetworkCredential networkCredential = new NetworkCredential(from, "harlem5219");
            //            smtp.UseDefaultCredentials = true;
            //            smtp.Credentials = networkCredential;
            //            smtp.Port = 25;
            //            smtp.Send(mail);

            //            ViewBag.Message = "Sent";
            //            //ViewBag.Message = "Sent";
            //            return View("SelfieContest", objModelMail);
            //        }
            //    }



            //}
            //else
            //{
            //    return View();
            //}
        }



        public JsonResult SelfieContest_Submit(string FullName, string Email, string Phone, string City, string Address, string ZipCode, string State, string Message)
        {

            #region SEND ALL THE USER INPUT TO SERVER USING XMLHTTP WEB REQUEST

            //string strXML = "<?xml version=\"1.0\"?>\n";
            string strXML = "<xml>\n";
            strXML += "<SelfieContest_Data>\n";
            strXML += "<FullName>" + FullName + "</FullName>\n";
            strXML += "<Email>" + Email + "</Email>\n";
            strXML += "<Phone>" + Phone + "</Phone>\n";
            strXML += "<Address>" + Address + "</Address>\n";
            strXML += "<City>" + City + "</City>\n";
            strXML += "<State>" + State + "</State>\n";
            strXML += "<ZipCode>" + ZipCode + "</ZipCode>\n";
            strXML += "<Message>" + Message + "</Message>\n";
            strXML += "</SelfieContest_Data>\n";
            strXML += "</xml>\n";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.hotphonecard.com/services/SelfieContest.aspx");

            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.ContentLength = strXML.Length;

            //WebProxy myProxy = new WebProxy();
            //myProxy = (WebProxy)req.Proxy;

            string RespCode = "", RespMsg = "";

            req.Method = "POST";
            try
            {
                System.IO.Stream stream = req.GetRequestStream();
                byte[] arrBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strXML);
                stream.Write(arrBytes, 0, arrBytes.Length);
                stream.Close();

                string strResponse = "";
                WebResponse resp = req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                strResponse = rdr.ReadToEnd();

                XD = new XmlDocument();
                XD.LoadXml(strResponse);
                XmlNodeList nList = XD.GetElementsByTagName("Result");


                if (nList.Count > 0)
                {
                    XmlNodeList nList1 = nList[0].ChildNodes;

                    RespCode = nList1[0].InnerText.ToString();
                    if (RespCode == "1")
                        RespMsg = "Record Successfully Submitted. Thank You.";
                    else if (RespCode == "0")
                        RespMsg = "Error: Trying to Submit Duplicate Record.";
                    else
                        RespMsg = nList1[1].InnerText.ToString();
                }
                else
                {
                    RespCode = "-1";
                    RespMsg = "There is Problem. Please try again later.";
                }

            }
            catch (Exception Ex)
            {
                //RespCode = "-2";
                //RespMsg = "Execpton Error";

                RespCode = "-2";
                //RespMsg = Ex.ToString();
                RespMsg = "There is Problem. Please Try again later.";
            }
            #endregion

            return Json(RespMsg, JsonRequestBehavior.AllowGet);

        }







        //Created on June 16 2015 for Ramadan Promotion 2015
        [CompressFilter]
        public ActionResult Ramadan()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult RamadanNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult RamadanExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }





        //Created on June 18 2015 for Fathers Day Promotion 2015
        [CompressFilter]
        public ActionResult FathersDay()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult FathersDayNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult FathersDayExistCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }





        //Created on July 14 2015 for Eid Promotion 2015
        [CompressFilter]
        public ActionResult Eid()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult EidNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult EidExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }






        [CompressFilter]
        public ActionResult Eid1Cent()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Eid1CentPakNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Eid1CentIndNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Eid1CentBanglaNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }

        [CompressFilter]
        public ActionResult Eid1CentExistingCustomer()
        {
            return View(new GenericModel());
        }





        //Created on August 12 2015 for Pakistan Independence Day Promotion 2015
        [CompressFilter]
        public ActionResult August14()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult August14NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }


        [CompressFilter]
        [Authorize]
        public ActionResult August14ExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }







        //Created on August 12 2015 for Pakistan Independence Day Promotion 2015
        [CompressFilter]
        public ActionResult August15()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult August15NewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }


        [CompressFilter]
        [Authorize]
        public ActionResult August15ExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }







        [CompressFilter]
        public ActionResult Rakhi()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult RakhiNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }


        [CompressFilter]
        [Authorize]
        public ActionResult RakhiExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }




        [CompressFilter]
        public ActionResult Pakistan1Cent()
        {
            return View(new GenericModel());
        }

        [CompressFilter]
        public ActionResult Pakistan1CentNewCustomer(int countryfrom, int countryto)
        {
            var model = new PromotionPlanModel();
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                ViewBag.UserType = true;
            }
            else
            {
                ViewBag.UserType = false;
            }
            ViewBag.Countryfrom = countryfrom;

            return View(model);
        }






        //Created on July 14 2015 for Dussehra Promotion 2015
        [CompressFilter]
        public ActionResult Dussehra()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult DussehraNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult DussehraExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }


        [CompressFilter]
        public ActionResult Diwali_FreeTrial()
        {
            //var model = new GenericModel();
            //return View(model);
            return RedirectToAction("Index", "FreeTrial");
        }





        //Created on July 14 2015 for Dussehra Promotion 2015
        [CompressFilter]
        public ActionResult HappyDiwali()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult HappyDiwaliNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult HappyDiwaliExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }








        //Created on November 24 2015 for Thanks Giving Promotion 2015
        [CompressFilter]
        public ActionResult ThanksGiving()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult ThanksGivingNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult ThanksGivingExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }













        //Created on December 22 2015 for Holiday and Christmas Promotion 2015
        [CompressFilter]
        public ActionResult HappyHolidays()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult HappyHolidaysNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult HappyHolidaysExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }






        //Created on December 22 2015 for Holiday and Christmas Promotion 2015
        [CompressFilter]
        public ActionResult NewYear()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult NewYearNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult NewYearExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }





        //Created on Feb 11 2016 for Valentines Day Promotion 2016
        [CompressFilter]
        public ActionResult ValentinesDay()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult ValentinesDayNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult ValentinesDayExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }





        //Created on Feb 11 2016 for Valentines Day Promotion 2016
        [CompressFilter]
        public ActionResult MothersDay()
        {
            return View(new GenericModel());
        }



        [CompressFilter]
        public ActionResult MothersDayNewCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            return View(new GenericModel());
        }


        [CompressFilter]
        [Authorize]
        public ActionResult MothersDayExistingCustomer(int countryfrom, int countryto)
        {
            ViewBag.CountryFromPromotion = countryfrom;
            ViewBag.CountryToPromotion = countryto;
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }




    }
}

