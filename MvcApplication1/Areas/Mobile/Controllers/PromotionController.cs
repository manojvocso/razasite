using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    [UnRequiresSSL]
    [CompressFilter]
    public class PromotionController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public PromotionController()
        {
            _dataRepository = new DataRepository();
        }

        private List<Country> GetPromotionFromCountryList()
        {
            var list = CacheManager.Instance.GetTop3FromCountries();
            return list.Take(2).ToList();
        }



        public ActionResult Faq()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuyPromotion(PromotionRateViewModel model)
        {
            string currencycode = string.Empty;
            int planid = 0;
            string planname = "";
            if (model.CountryFrom == 1)
            {
                planid = 161;
                planname = "One Touch Dial";
                currencycode = "USD";
            }
            if (model.CountryTo == 2)
            {
                planid = 162;
                planname = "Canada One Touch Dial";
                currencycode = "CAD";
            }

            ProcessPlanInfo processPlanInfo = new ProcessPlanInfo()
            {
                CountryFrom = model.CountryFrom,
                CountryTo = model.CountryTo,
                PlanName = planname,
                CurrencyCode = currencycode,
                CardId = planid,
                Amount = model.Denomination,

            };
            ProcessPaymentInfo processPaymentInfo = new ProcessPaymentInfo()
            {
                CouponCode = model.CouponCode
            };

            PayPalCheckoutModel payPalCheckoutModel = new PayPalCheckoutModel()
            {
                ProcessPaymentInfo = processPaymentInfo,
                ProcessPlanInfo = processPlanInfo
            };


            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("NewSignUp", "Cart");
            }
            else
            {
                if (UserContext.UserType.ToLower() == "new")
                {
                    return RedirectToAction("UpdateBillingInfo", "Cart");
                }
            }
            return RedirectToAction("UpdateBillingInfo", "Cart");

        }

        [HttpPost]
        public ActionResult RechargeWithPromotion(PromotionRateViewModel model)
        {
            var premiumPlanInfo = ControllerHelper.GetUserPremiumPlan(UserContext.MemberId);
            if (premiumPlanInfo == null)
                return RedirectPermanent(Request.Url.AbsolutePath);

            RechargeViewModel rechargeViewModel = new RechargeViewModel()
            {
                CouponCode = model.CouponCode,
                RechargeAmount = model.Denomination
            };
            TempData["RechargeViewModel"] = rechargeViewModel;
            return RedirectToAction("Index", "Recharge", new { id = ControllerHelper.Encrypt(premiumPlanInfo.AccountNumber) });

        }

        [HttpGet]
        public ActionResult OneCentPlan()
        {
            
            return View();
        }



        #region Nepal69 Promotion

        public ActionResult Nepal69Promotion()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nepal69Promotion(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("Nepal69NewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("Nepal69Promotion", "Promotion");
        }

        public ActionResult Nepal69NewCustomer(int countryFrom, int countryTo)
        {
            var model = new PromotionRateViewModel
            {
                CountryFrom = countryFrom,
                CountryTo = countryTo,
                FromCountryList = GetPromotionFromCountryList(),
                ToCountryList = new List<Country>()
                {
                    new Country()
                    {
                        Id = "224",
                        Name = "Nepal",
                        CountCode = "977"
                    }
                },
                CouponCode = "NEPSIGNUP",
                IsOnlyForNewCustomer = true,

            };

            if (User.Identity.IsAuthenticated)
            {
                model.IsNewCustomer = UserContext.UserType.ToLower() == "new";
            }
            
            model.Denominations.Add(new DenominationDropdownEntity()
            {
                Denomination = 10,
                RegularMin = 108,
                ExtraMin = 37,
                RatePerMin = 6.9
            });

            return View("PromotionRates", model);
        }

        #endregion









        #region Pakistan 1 Cent Promotion

        public ActionResult Pakistan1Cent()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Pakistan1Cent(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("Pakistan1CentNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("Pakistan1Cent", "Promotion");
        }

        public ActionResult Pakistan1CentNewCustomer(int countryFrom, int countryTo)
        {
            var model = new PromotionRateViewModel
            {
                CountryFrom = countryFrom,
                CountryTo = countryTo,
                FromCountryList = GetPromotionFromCountryList(),
                ToCountryList = new List<Country>()
                {
                    new Country()
                    {
                        Id = "238",
                        Name = "Pakistan",
                        CountCode = "92"
                    }
                },
                CouponCode = "PAK1C",
                IsOnlyForNewCustomer = true,

            };

            if (User.Identity.IsAuthenticated)
            {
                model.IsNewCustomer = UserContext.UserType.ToLower() == "new";
            }

            model.Denominations.Add(new DenominationDropdownEntity()
            {
                Denomination = 10,
                RegularMin = 360,
                ExtraMin = 640,
                RatePerMin = 1
            });

            return View("PromotionRates", model);
        }

        #endregion











        #region SaveBig EveryDay

        /// <summary>
        /// SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveBigEveryDay()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateSaveBigEveryDayModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveBigEveryDay(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("SaveBigEveryDayNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("SaveBigEveryDayExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("SaveBigEveryDay", "Promotion");
        }

        public ActionResult SaveBigEveryDayNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateSaveBigEveryDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult SaveBigEveryDayExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateSaveBigEveryDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }

        /// <summary>
        /// Get Date of Savebig everyday promotion
        /// </summary>
        /// <param name="countryfrom"></param>
        /// <param name="countryto"></param>
        /// <param name="usertype"></param>
        /// <param name="promocode"></param>
        /// <returns></returns>
        private List<DenominationDropdownEntity> GetUpTo_PromotionRate(int countryfrom, int countryto, string promocode = "")
        {

            if (promocode.ToUpper() == "NY2016")
                promocode = "HH2016";

            string filepath = Server.MapPath(@"/Content/GetUpTo_Promo.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.CouponCode == promocode);

            Mapper.CreateMap<NewCustPromotionplanmodel, DenominationDropdownEntity>()
                .ForMember(dest => dest.Denomination, opts => opts.MapFrom(src => src.Denomination))
                .ForMember(dest => dest.RatePerMin, opts => opts.MapFrom(src => src.RateperMIn))
                .ForMember(dest => dest.ExtraMin, opts => opts.MapFrom(src => src.ExtraMin))
                .ForMember(dest => dest.RegularMin, opts => opts.MapFrom(src => src.RegularMin));

            return Mapper.Map<List<DenominationDropdownEntity>>(promotionrates);

        }

        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateSaveBigEveryDayModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "BIG2014";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                }
            };
            return model;
        }

        #endregion





        #region Dussehra 2015

        /// <summary>
        /// Dussehra promotion plan
        /// </summary>
        /// <returns></returns>
        public ActionResult Dussehra()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateDussehraModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Dussehra(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("DussehraNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("DussehraExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("Dussehra", "Promotion");
        }

        public ActionResult DussehraNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateDussehraModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult DussehraExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateDussehraModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateDussehraModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "DUS2015";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                }
            };
            return model;
        }

        #endregion



        public ActionResult PromotionRates()
        {

            return View();
        }







        [HttpGet]
        public ActionResult Diwali_FreeTrial()
        {
            var model = TempData["TryUsFreeViewModel"] as TryUsFreeViewModel ?? new TryUsFreeViewModel();
            var message = TempData["ViewMessage"] as ViewMessage;


            //model.CountryFromList = CacheManager.Instance.GetTop3FromCountries();
            model.CountryFromList = CacheManager.Instance.GetTop3FromCountries().Where(a => (a.Id == "1" || a.Id == "2")).ToList();

            //model.CountryToForFreeTrialList = CacheManager.Instance.FreeTrial_Country_List();
            model.CountryToForFreeTrialList = new List<TrialCountryInfo>()
            {
                new TrialCountryInfo()
                {
                    Id = "130",
                    Name = "India",
                    Minutes = "200",
                    Desc ="India - 200 MIN"
                },
                new TrialCountryInfo()
                {
                    Id = "224",
                    Name = "Nepal",
                    Desc ="Nepal - 50 MIN"
                }
            };



            if (message != null)
            {
                model.Message = message.Message;
                model.MessageType = message.MessageType;
            }
            return View(model);
        }





        [UnRequiresSSL]
        [HttpPost]
        public ActionResult BuyTryUsFree(TryUsFreeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["TryUsFreeViewModel"] = model;
                return RedirectToAction("Diwali_FreeTrial", "Promotion");
            }
            var trialfromcountrylist = CacheManager.Instance.GetFromCountries();
            var trialtocountrylist = _dataRepository.FreeTrial_Country_List();

            if (trialfromcountrylist == null || trialtocountrylist == null)
                throw new ResourceNotFoundException();

            //var currencycodes = FlagDictonary.GetCurrencycodebyCountry();
            string code = string.Empty;
            string planname = string.Empty;
            int cardId = 0;
            switch (model.TrialCountryFrom)
            {
                case 1:
                    code = "USD";
                    planname = "OneTouch Dial";
                    cardId = 161;
                    break;
                case 2:
                    code = "CAD";
                    planname = "Canada OneTouch Dial";
                    cardId = 162;
                    break;
                case 3:
                    code = "GBP";
                    planname = "UK Direct Dial";
                    cardId = 102;
                    break;
            }


            ProcessPlanInfo processPlanInfo = new ProcessPlanInfo()
            {
                CountryFrom = SafeConvert.ToInt32(model.TrialCountryFrom),
                CountryTo = SafeConvert.ToInt32(model.TrialCountryTo),
                PlanName = planname,
                CurrencyCode = code,
                CardId = cardId,
                IsTryUsFree = true
            };
            ProcessPaymentInfo processPaymentInfo = new ProcessPaymentInfo()
            {
                CouponCode = "FREETRIAL",
                IsPaypalDisabled = true

            };

            PayPalCheckoutModel payPalCheckoutModel = new PayPalCheckoutModel()
            {
                ProcessPaymentInfo = processPaymentInfo,
                ProcessPlanInfo = processPlanInfo
            };


            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("NewSignUp", "Cart");
            }
            else
            {
                if (UserContext.UserType.ToLower() == "new")
                {
                    return RedirectToAction("UpdateBillingInfo", "Cart");
                }
            }
            TempData["ViewMessage"] = new ViewMessage()
            {
                MessageType = MessageType.Error,
                Message = AppMessage.ErrorFreeTrialNotValid
            };
            return RedirectToAction("Diwali_FreeTrial", "Promotion");
        }






        #region DIWALI 2015

        /// <summary>
        /// Dussehra promotion plan
        /// </summary>
        /// <returns></returns>
        public ActionResult HappyDiwali()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateDiwaliModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult HappyDiwali(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("HappyDiwaliNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("HappyDiwaliExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("HappyDiwali", "Promotion");
        }

        public ActionResult HappyDiwaliNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateDiwaliModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult HappyDiwaliExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateDiwaliModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateDiwaliModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "DIWALI15";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                }
            };
            return model;
        }

        #endregion










        #region THANKS GIVING 2015

        /// <summary>
        /// Dussehra promotion plan
        /// </summary>
        /// <returns></returns>
        public ActionResult ThanksGiving()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateThanksGivingModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult ThanksGiving(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("ThanksGivingNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("ThanksGivingExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("ThanksGiving", "Promotion");
        }

        public ActionResult ThanksGivingNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateThanksGivingModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult ThanksGivingExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateThanksGivingModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateThanksGivingModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "THANKS2015";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                },
                new Country()
                {
                    Id = "27",
                    Name = "Bangladesh",
                    CountCode ="880"
                },
                new Country()
                {
                    Id = "281",
                    Name = "Sri Lanka",
                    CountCode ="94"
                }
            };
            return model;
        }

        #endregion






        #region HAPPY HOLIDAYS 2015

        /// <summary>
        /// Happy Holidays promotion plan
        /// </summary>
        /// <returns></returns>
        public ActionResult HappyHolidays()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateHappyHolidaysModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult HappyHolidays(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("HappyHolidaysNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("HappyHolidaysExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("HappyHolidays", "Promotion");
        }

        public ActionResult HappyHolidaysNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateHappyHolidaysModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult HappyHolidaysExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateHappyHolidaysModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateHappyHolidaysModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "HH2016";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                },
                new Country()
                {
                    Id = "27",
                    Name = "Bangladesh",
                    CountCode ="880"
                },
                new Country()
                {
                    Id = "281",
                    Name = "Sri Lanka",
                    CountCode ="94"
                }
            };
            return model;
        }






        public ActionResult NewYear()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateNewYearModel();

            return View(model);
        }


        [HttpPost]
        public ActionResult NewYear(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("NewYearNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("NewYearExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("NewYear", "Promotion");
        }


        public ActionResult NewYearNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateNewYearModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult NewYearExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateNewYearModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateNewYearModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "NY2016";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                },
                new Country()
                {
                    Id = "27",
                    Name = "Bangladesh",
                    CountCode ="880"
                },
                new Country()
                {
                    Id = "281",
                    Name = "Sri Lanka",
                    CountCode ="94"
                }
            };
            return model;
        }







        public ActionResult ValentinesDay()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateValentinesDayModel();

            return View(model);
        }


        [HttpPost]
        public ActionResult ValentinesDay(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("ValentinesDayNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("ValentinesDayExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("NewYear", "Promotion");
        }


        public ActionResult ValentinesDayNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateValentinesDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult ValentinesDayExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateValentinesDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for SaveBig Everyday promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateValentinesDayModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "VD2016";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                },
                new Country()
                {
                    Id = "27",
                    Name = "Bangladesh",
                    CountCode ="880"
                },
                new Country()
                {
                    Id = "281",
                    Name = "Sri Lanka",
                    CountCode ="94"
                }
            };
            return model;
        }






















        public ActionResult MothersDay()
        {
            var model = TempData["PromotionRateViewModel"] as PromotionRateViewModel ?? CreateMothersDayModel();

            return View(model);
        }


        [HttpPost]
        public ActionResult MothersDay(PromotionRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CustomerType == "new")
                {
                    return RedirectToAction("MothersDayNewCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
                else if (model.CustomerType == "old")
                {
                    return RedirectToAction("MothersDayExistCustomer", "Promotion",
                        new { countryfrom = model.CountryFrom, countryto = model.CountryTo });
                }
            }

            TempData["PromotionRateViewModel"] = model;
            return RedirectToAction("MothersDay", "Promotion");
        }


        public ActionResult MothersDayNewCustomer(int countryFrom, int countryTo)
        {
            var model = CreateMothersDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateNewCust", model);
        }

        [MobileAuthorize]
        public ActionResult MothersDayExistCustomer(int countryFrom, int countryTo)
        {
            var model = CreateMothersDayModel(countryFrom, countryTo);

            model.Denominations = GetUpTo_PromotionRate(model.CountryFrom, model.CountryTo, model.CouponCode);

            return View("PromotionRateExistCust", model);
        }


        /// <summary>
        /// Bind model for Mothers Day promotion plan
        /// </summary>
        /// <returns></returns>
        private PromotionRateViewModel CreateMothersDayModel(int countryFrom = 0, int countryTo = 0)
        {
            var model = new PromotionRateViewModel();
            model.FromCountryList = GetPromotionFromCountryList();
            model.CouponCode = "MOM2016";
            model.CountryFrom = countryFrom;
            model.CountryTo = countryTo;
            model.ToCountryList = new List<Country>()
            {
                new Country()
                {
                    Id = "130",
                    Name = "India",
                    CountCode ="91"
                },
                new Country()
                {
                    Id = "238",
                    Name = "Pakistan",
                    CountCode ="92"
                },
                new Country()
                {
                    Id = "224",
                    Name = "Nepal",
                    CountCode ="977"
                },
                new Country()
                {
                    Id = "27",
                    Name = "Bangladesh",
                    CountCode ="880"
                },
                new Country()
                {
                    Id = "281",
                    Name = "Sri Lanka",
                    CountCode ="94"
                }
            };
            return model;
        }

        #endregion

    }
}
