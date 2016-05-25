using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.AppHelper;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;
using WebGrease.Css.Extensions;
using System.Threading.Tasks;
using MvcApplication1.App_Start;
using MvcApplication1.Compression;
using Raza.Model.PaymentProcessModel;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    [CompressFilter]
    [UnRequiresSSL]
    public class RateController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public RateController()
        {
            _dataRepository = new DataRepository();
        }



        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchRate(int countryfrom, int countryto)
        {
            //int countryfrom = 1;
            //int countryto = 130;
            if (countryfrom == 0 || countryto == 0)
                return RedirectToAction("Index", "Rate");

            var countryToInfoTask =
                Task.Run(() => ControllerHelper.GetcountryInfoByCountryId(SafeConvert.ToString(countryto)));

            var dataTask = Task.Run(
                    () => _dataRepository.GetRates(new Rates()
                    {
                        CountryFrom = countryfrom,
                        CountryTo = countryto,
                        CardName = string.Empty
                    }));

            var mobileCountryTask =
              Task.Run(
                    () => CacheManager.Instance.GetOnlyMobileCountry().Where(a => a.CountCode == countryToInfoTask.Result.CountCode).ToList());

            var landlineCountryTask =
                Task.Run(
                    () => CacheManager.Instance.GetOnlyCityCountry().Where(a => a.CountCode == countryToInfoTask.Result.CountCode).ToList());


            Task.WaitAll(dataTask);
            var modelCreateTask =
                Task.Run(
                    () =>
                        CreateSearchModel(dataTask.Result.Plans.Where(a => a.PlanCategoryId == "1").ToList(),
                            countryfrom));



            #region Fill Country data in Model

            var model = modelCreateTask.Result;
            model.CountryFrom = countryfrom;
            model.CountryTo = countryto;
            model.CountryToName = countryToInfoTask.Result.Name;

            model.CountryRegionsList.AddRange(mobileCountryTask.Result);
            model.CountryRegionsList.AddRange(landlineCountryTask.Result);

            #endregion

            return View(model);


        }

        private SearchRateViewModel CreateSearchModel(List<EachRatePlan> planData, int countryFrom)
        {
            var model = new SearchRateViewModel();


            foreach (var item in planData)
            {
                #region Caclulation for AutoRefill Rates

                var min = Math.Round(item.TotalMinutes + (item.TotalMinutes * (item.Discount / 100)));
                var strikeAmount = 0;
                if (item.PlanAmount == 90 && countryFrom != 3)
                {
                    model.RatePerMinAutoRefill = Math.Round((item.PlanAmount / min) * 100, 1);
                    strikeAmount = 100;
                }
                else if (item.PlanAmount == 50 && countryFrom == 3)
                {
                    model.RatePerMinAutoRefill = Math.Round((item.PlanAmount / min) * 100, 1);
                }

                model.AutoRefillRates.Add(new SearchRateMobileEntity()
                {
                    Amount = item.PlanAmount,
                    Minute = SafeConvert.ToInt32(Math.Round(min)),
                    StrikeAmount = strikeAmount,
                    Servicefee = item.ServiceFee,
                    PlanId = item.PlanId,
                    FromToMapping = item.FromToMapping,
                    CurrencyCode = item.CurrencyCode
                });

                #endregion

                #region Calculation for Without Autorefill Rates

                model.RatePerMinWithOutAutoRefill = item.RatePerMin;
                model.WithOutAutoRefillRates.Add(new SearchRateMobileEntity()
                {
                    StrikeAmount = strikeAmount,
                    Amount = item.PlanAmount,
                    Minute = SafeConvert.ToInt32(item.TotalMinutes),
                    Servicefee = item.ServiceFee,
                    PlanId = item.PlanId,
                    FromToMapping = item.FromToMapping,
                    CurrencyCode = item.CurrencyCode
                });

                model.PlanName = item.CardTypeName;

                #endregion
            }

            return model;

        }

        [HttpGet]
        public ActionResult TryUsFree()
        {
            var model = TempData["TryUsFreeViewModel"] as TryUsFreeViewModel ?? new TryUsFreeViewModel();
            var message = TempData["ViewMessage"] as ViewMessage;


            model.CountryFromList = CacheManager.Instance.GetTop3FromCountries();
            model.CountryToForFreeTrialList = CacheManager.Instance.FreeTrial_Country_List();

            if (message != null)
            {
                model.Message = message.Message;
                model.MessageType = message.MessageType;
            }
            return View(model);
        }

        public ActionResult Faq()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IndiaOneCent()
        {
            return View();
        }

        private AdditionalPlanInfoModel GetAdditionalPlanInfo(int countryFrom, string planType)
        {

            var model = new AdditionalPlanInfoModel();
            if (countryFrom == 0)
            {
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(UserContext.ProfileInfo.Country))
                {
                    var country = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
                    countryFrom = SafeConvert.ToInt32(country.Id);
                }
                else
                {
                    countryFrom = SafeConvert.ToInt32(Session["CountrybyIp"]);
                }
            }

            model.CountryFrom = countryFrom;
            if (countryFrom == 1)
            {
                switch (planType)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "175";
                            model.PlanName = "MOBILE UNLIMITED $9.99";
                            model.Price = 9.99;
                            model.ServiceFee = 3.99;
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "177";
                            model.PlanName = "UNLIMITED $14.99";
                            model.Price = 14.99;
                            model.ServiceFee = 7.99;
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "179";
                            model.PlanName = "UNLIMITED $19.99";
                            model.Price = 19.99;
                            model.ServiceFee = 7.99;
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "163";
                            model.PlanName = "1 Cent Plan";
                            model.Price = 15;
                            model.ServiceFee = 4.99;
                            model.IsAutoRefill = true;
                            model.AutoRefillAmount = 15;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "121";
                            model.PlanName = "Talk Mobile";
                            model.Price = 10;
                            model.ServiceFee = 2.99;
                            model.IsAutoRefill = true;
                            model.AutoRefillAmount = 10;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "120";
                            model.PlanName = "Talk City";
                            model.Price = 10;
                            model.ServiceFee = 2.99;
                            model.AutoRefillAmount = 10;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                }
                model.CurrencyCode = "USD";

            }
            else if (countryFrom == 2)
            {
                switch (planType)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "176";
                            model.PlanName = "CANADA MOBILE UNLIMITED $9.99";
                            model.Price = 9.99;
                            model.ServiceFee = 3.99;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "178";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = 14.99;
                            model.ServiceFee = 7.99;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "180";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = 19.99;
                            model.ServiceFee = 7.99;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "164";
                            model.PlanName = "Canada 1 Cent Plan";
                            model.Price = 15;
                            model.ServiceFee = 4.99;
                            model.IsAutoRefill = true;
                            model.AutoRefillAmount = 15;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "126";
                            model.PlanName = "Canada Talk Mobile";
                            model.Price = 10;
                            model.ServiceFee = 2.99;
                            model.AutoRefillAmount = 10;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "125";
                            model.PlanName = "Canada Talk City";
                            model.Price = 10;
                            model.AutoRefillAmount = 10;
                            model.ServiceFee = 2.99;
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                }
                model.CurrencyCode = "CAD";
            }
            model.CountryTo = 130;

            return model;
        }

        private void BuyAdditionalPlan(AdditionalPlanInfoModel model)
        {
            var processPlanInfo = new ProcessPlanInfo()
            {
                CountryFrom = model.CountryFrom,
                CountryTo = model.CountryTo,
                Amount = model.Price,
                PlanName = model.PlanName,
                CurrencyCode = model.CurrencyCode,
                CardId = SafeConvert.ToInt32(model.PlanId),
                ServiceFee = model.ServiceFee,
                IsAutoRefill = model.IsAutoRefill,
                AutoRefillAmount = model.AutoRefillAmount
            };

            var processPaymentInfo = new ProcessPaymentInfo()
            {
                IsPaypalDisabled = true,
                TransactionType = User.Identity.IsAuthenticated ? TransactionType.PurchaseNewPlan.ToString() : TransactionType.CheckOut.ToString()
            };
            var payPalCheckoutModel = new PayPalCheckoutModel()
            {
                ProcessPlanInfo = processPlanInfo,
                ProcessPaymentInfo = processPaymentInfo
            };

            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

        }

        [HttpPost]
        public ActionResult IndiaOneCent(AdditionalPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var additionalPlan = GetAdditionalPlanInfo(model.CountryFrom, model.PlanType);
                BuyAdditionalPlan(additionalPlan);
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("UpdateBillingInfo", "Cart");
                }
                return RedirectToAction("NewSignUp", "Cart");
            }
            return RedirectToAction("IndiaOneCent", "Rate");
        }

        [HttpGet]
        public ActionResult IndiaUnlimited()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndiaUnlimited(AdditionalPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var additionalPlan = GetAdditionalPlanInfo(model.CountryFrom, model.PlanType);
                BuyAdditionalPlan(additionalPlan);
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("UpdateBillingInfo", "Cart");
                }
                return RedirectToAction("NewSignUp", "Cart");
            }
            return RedirectToAction("IndiaUnlimited", "Rate");
        }



    }
}
