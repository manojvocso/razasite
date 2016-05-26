using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Controllers;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Compression;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Mobile.Controllers
{

    [CompressFilter]
    [RequiresSSL]
    public class TopUpMobileController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public TopUpMobileController()
        {
            _dataRepository = new DataRepository();
        }

        #region Private Controller methods

        private MobileTopupViewModel BindMobileTopupViewModelData(MobileTopupViewModel model)
        {

            model.MobileNumberCountryList = CacheManager.Instance.GetCountryListTo().Where(a => a.Id != "314").ToList();

            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(UserContext.ProfileInfo.Country))
            {
                model.RatesFromCountryList = CacheManager.Instance.GetTop3FromCountries()
                    .Where(a => a.Name == UserContext.ProfileInfo.Country).ToList();
            }
            else
            {
                model.RatesFromCountryList = CacheManager.Instance.GetTop3FromCountries().Take(2).ToList();
            }
            return model;
        }

        private MobileTopupRatesViewModel BindMobileTopupRatesViewModelData(MobileTopupViewModel infoModel)
        {
            var model = new MobileTopupRatesViewModel();

            var operatorInfo = GetMobileOperatorInfo(infoModel.CountryCode, infoModel.MobileNumber);
            if (operatorInfo == null)
            {
                return null;
            }

            model.Carrier = operatorInfo.Item1;
            model.CountryCode = infoModel.CountryCode;
            model.MobileNumber = infoModel.MobileNumber;
            model.CarrierImage = GetTopupCarrierImage(model.Carrier);

            model.DenominationList = GetTopupOptions(infoModel.RateFromCountry, infoModel.MobileCountryId, model.Carrier);
            model.RatesFromCountryList = CacheManager.Instance.GetTop3FromCountries().Take(2).ToList();
            var rateFromCountryInfo = ControllerHelper.GetFromCountryInfoByCountryId(SafeConvert.ToString(infoModel.RateFromCountry));
            model.RateFromCountry = infoModel.RateFromCountry;
            model.RateFromCountryName = rateFromCountryInfo.Name;
            return model;
        }

        private List<MobileTopupOperator> GetTopupOptions(int rateFromCountry, int countryId, string carrierName)
        {

            var topupoperators = _dataRepository.GetMobile_TopupOperator(rateFromCountry, countryId);
            return topupoperators.OperatorList.Where(a => a.OperatorName == carrierName).ToList();
        }

        private Tuple<string, string> GetMobileOperatorInfo(string countryCode, string mobileNumber)
        {
            string destnumber = countryCode.Replace("-","") + mobileNumber;
            var operatorinfo = _dataRepository.TopUpPhoneNumberInfo(destnumber);
            if (string.IsNullOrEmpty(operatorinfo.TopUpMobileOperator))
                return null;

            return Tuple.Create(operatorinfo.TopUpMobileOperator, operatorinfo.TopUpCountryCode);
        }

        public string GetTopupCarrierImage(string carrierName)
        {

            string filepath = Server.MapPath(@"/Content/Operator_images.xml");
            string imagename = String.Empty;
            var operatorImages = CacheManager.Instance.GetOpeartorImages(filepath);
            var operatorimagedata =
                operatorImages.ListOfOperators.FirstOrDefault(
                    a => a.operatorname == carrierName);

            if (operatorimagedata != null)
            {
                imagename = "/images/operator_images/" + operatorimagedata.operatorimage;
            }
            return imagename;
        }

        private List<MobileTopupOperator> GetDomesticOperatorsInfo(string operatorName)
        {
            var data = _dataRepository.GetMobile_TopupOperator(1, 314);
            return data.OperatorList.Where(a => a.OperatorName == operatorName).ToList();
        }

        private DomesticTopupViewModel BindDomesticTopupViewModel(DomesticTopupViewModel model)
        {
            var data = _dataRepository.GetMobile_TopupOperator(1, 314);
            model.CarrierList = data.OperatorList.DistinctBy(a => a.OperatorName).ToList();
            if (!string.IsNullOrEmpty(model.CarrierName))
            {
                model.DenominationList = data.OperatorList.Where(a => a.OperatorName == model.CarrierName).ToList();
            }
            return model;
        }


        #endregion


        [HttpGet]
        public ActionResult International()
        {
            var tempModel = TempData["MobileTopupViewModel"] as MobileTopupViewModel ?? new MobileTopupViewModel();
            var model = BindMobileTopupViewModelData(tempModel);

            var viewMessage = TempData["ViewMessage"] as ViewMessage;
            if (viewMessage != null)
            {
                model.MessageType = viewMessage.MessageType;
                model.Message = viewMessage.Message;
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult International(MobileTopupViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["MobileTopupViewModel"] = model;

                var operatorInfo = GetMobileOperatorInfo(model.CountryCode, model.MobileNumber);
                if (operatorInfo != null)
                    return RedirectToAction("Rates");

                ModelState.AddModelError("MobileNumber", "Invalid Phone Number");


            }

            return View(BindMobileTopupViewModelData(model));
        }

        public ActionResult Rates()
        {
            var mobileTopupViewModel = TempData["MobileTopupViewModel"] as MobileTopupViewModel;
            if (mobileTopupViewModel == null)
                return RedirectToAction("International");


            var model = BindMobileTopupRatesViewModelData(mobileTopupViewModel);

            var viewMessage = TempData["ViewMessage"] as ViewMessage;
            if (viewMessage != null)
            {
                model.MessageType = viewMessage.MessageType;
                model.Message = viewMessage.Message;
            }
            if (model == null)
            {
                mobileTopupViewModel.Message = "Invalid country or Mobile number.";
                mobileTopupViewModel.MessageType = MessageType.Error;
                return RedirectToAction("International");
            }
            TempData["MobileTopupViewModel"] = mobileTopupViewModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult Rates(MobileTopupRatesViewModel model)
        {
            var mobileTopupViewModel = TempData["MobileTopupViewModel"] as MobileTopupViewModel;

            if (ModelState.IsValid)
            {
                if (Math.Abs(model.SourceAmount) <= 0)
                {
                    ModelState.AddModelError("SourceAmount", "Please select a source amount.");
                    TempData["MobileTopupViewModel"] = mobileTopupViewModel;
                    return RedirectToAction("Rates");
                }
                if (User.Identity.IsAuthenticated)
                {
                    if (model.RateFromCountryName != UserContext.ProfileInfo.Country)
                    {
                        TempData["MobileTopupViewModel"] = mobileTopupViewModel;
                        TempData["ViewMessage"] = new ViewMessage()
                        {
                            MessageType = MessageType.Error,
                            Message = "Sorry! You can not make recharge from this country, Please try Again."
                        };
                        return RedirectToAction("Rates");
                    }
                }
                var operatorInfo = GetMobileOperatorInfo(model.CountryCode, model.MobileNumber);

                var processPlanInfo = new ProcessPlanInfo()
                {
                    Amount = model.SourceAmount,
                    CurrencyCode = model.RateFromCountry == 2 ? "CAD" : "USD",
                    CountryFrom = model.RateFromCountry,
                    MobileTopupInfo = new MobileTopupInfo()
                    {
                        TopUpCountryCode = model.CountryCode,
                        TopupCountry = ControllerHelper.GetcountryInfoByCountryCode(model.CountryCode).Id,
                        TopupMobileNumber = model.MobileNumber,
                        TopupDestAmount = model.DestinationAmount,
                        TopupSourceAmount = model.SourceAmount,
                        TopupOperatorName = operatorInfo.Item1,
                        TopupOperatorCode = model.OperatorCode,
                        TopupFromCountry = model.RateFromCountry,
                        TopupType = TopupType.International
                    }
                };

                var processPaymentInfo = new ProcessPaymentInfo()
                {
                    TransactionType = TransactionType.TopUp.ToString(),
                    IpAddress = ControllerHelper.GetIpAddressofUser(Request),
                    IsPaypalDisabled = true
                };

                var payPalCheckoutModel = new PayPalCheckoutModel()
                {
                    ProcessPaymentInfo = processPaymentInfo,
                    ProcessPlanInfo = processPlanInfo,

                };
                Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("NewSignUp", "Cart");

                return RedirectToAction("UpdateBillingInfo", "Cart");
            }

            TempData["MobileTopupViewModel"] = mobileTopupViewModel;
            return RedirectToAction("Rates");

        }

        public ActionResult Domestic()
        {
            var model = TempData["DomesticTopupViewModel"] as DomesticTopupViewModel ?? new DomesticTopupViewModel();

            model = BindDomesticTopupViewModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Domestic(DomesticTopupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mobileInfo = GetMobileOperatorInfo("1", model.MobileNumber);
                if (mobileInfo != null)
                {
                    var operatorsData = GetDomesticOperatorsInfo(model.CarrierName);
                    var operatorInfo = operatorsData.FirstOrDefault(a => a.SourceAmount == model.SourceAmount);
                    if (operatorInfo != null)
                    {
                        var processPlanInfo = new ProcessPlanInfo()
                        {
                            Amount = model.SourceAmount,
                            CurrencyCode = "USD",
                            CountryFrom = 1,
                            MobileTopupInfo = new MobileTopupInfo()
                            {
                                TopUpCountryCode = "1",
                                TopupCountry = "314",
                                TopupMobileNumber = model.MobileNumber,
                                TopupDestAmount = operatorInfo.DestinationAmount,
                                TopupSourceAmount = model.SourceAmount,
                                TopupOperatorName = model.CarrierName,
                                TopupOperatorCode = operatorInfo.OperatorCode,
                                TopupType = TopupType.Domestic
                            }
                        };

                        var processPaymentInfo = new ProcessPaymentInfo()
                        {
                            TransactionType = TransactionType.TopUp.ToString(),
                            IpAddress = ControllerHelper.GetIpAddressofUser(Request)
                        };

                        var payPalCheckoutModel = new PayPalCheckoutModel()
                        {
                            ProcessPaymentInfo = processPaymentInfo,
                            ProcessPlanInfo = processPlanInfo,

                        };
                        Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
                        return RedirectToAction("UpdateBillingInfo", "Cart");
                    }
                }
                else
                {
                    ModelState.AddModelError("MobileNumber", "Invalid MobileNumber");
                    model = BindDomesticTopupViewModel(model);
                    return View(model);
                }
            }
            TempData["DomesticTopupViewModel"] = model;
            return RedirectToAction("Domestic");
        }

        public JsonResult GetCountryCodeofCountry(string countryId)
        {
            var info = ControllerHelper.GetcountryInfoByCountryId(countryId);
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMobileOperatorInfoforDomestic(string operatorName)
        {
            var data = GetDomesticOperatorsInfo(operatorName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public ActionResult ReviewOrder()
        {
            return View();
        }

    }
}
