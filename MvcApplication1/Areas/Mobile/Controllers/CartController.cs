using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Mobile.Models;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Areas.Payment.Controllers;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Mobile.Controllers
{

    [CompressFilter]
    public class CartController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public CartController()
        {
            _dataRepository = new DataRepository();
        }

        private string GetTransactionType(PayPalCheckoutModel payPalCheckoutModel)
        {
            string transactionType = String.Empty;
            if (payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.TopUp.ToString())
                return TransactionType.TopUp.ToString();

            if (payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.Recharge.ToString())
                return TransactionType.Recharge.ToString();

            if (UserContext.ProfileInfo.UserType.ToLower() == "new")
            {
                transactionType = TransactionType.CheckOut.ToString();
            }
            else if (UserContext.ProfileInfo.UserType.ToLower() == "old")
            {
                transactionType = TransactionType.PurchaseNewPlan.ToString();
            }
            else if (payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo != null)
            {
                transactionType = TransactionType.TopUp.ToString();
            }
            return transactionType;
        }

        [NonAction]
        public ActionResult Index()
        {
            return View();
        }

        [UnRequiresSSL]
        [HttpPost]
        public ActionResult BuyTryUsFree(TryUsFreeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["TryUsFreeViewModel"] = model;
                return RedirectToAction("TryUsFree", "Rate");
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
            return RedirectToAction("TryUsFree", "Rate");
        }

        [UnRequiresSSL]
        [HttpPost]
        public ActionResult BuyPlan(ShopCartMobileModel shopCartMobileModel)
        {

            var plansData = _dataRepository.GetRates(new Rates()
            {
                CountryFrom = shopCartMobileModel.CountryFrom,
                CountryTo = shopCartMobileModel.CountryTo
            });

            if (!plansData.Plans.Any())
                return RedirectToAction("SearchRate", "Rate",
                    new { countryfrom = shopCartMobileModel.CountryFrom, countryto = shopCartMobileModel.CountryTo });

            var plan =
                plansData.Plans.FirstOrDefault(
                    a => a.FromToMapping == shopCartMobileModel.FromToMapping && a.PlanId == shopCartMobileModel.PlanId);

            if (plan == null)
                throw new ResourceNotFoundException();

            var processPlanInfo = new ProcessPlanInfo()
            {
                CountryFrom = shopCartMobileModel.CountryFrom,
                CountryTo = shopCartMobileModel.CountryTo,
                IsAutoRefill = shopCartMobileModel.IsAutorefill,
                Amount = Convert.ToDouble(plan.PlanAmount),
                PlanName = plan.CardTypeName,
                CurrencyCode = plan.CurrencyCode,
                CardId = SafeConvert.ToInt32(plan.FromToMapping),
                ServiceFee = Convert.ToDouble(plan.ServiceFee),
                AutoRefillAmount = shopCartMobileModel.IsAutorefill ? Convert.ToDouble(plan.PlanAmount) : 0
            };

            string transType = TransactionType.CheckOut.ToString();
            if (User.Identity.IsAuthenticated && UserContext.UserType.ToLower() == "old")
            {
                transType = TransactionType.PurchaseNewPlan.ToString();
            }
            var payPalCheckoutModel = new PayPalCheckoutModel()
            {
                ProcessPlanInfo = processPlanInfo,
                ProcessPaymentInfo = new ProcessPaymentInfo()
                {
                    TransactionType = transType
                }
            };

            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UpdateBillingInfo", "Cart");
            }
            return RedirectToAction("NewSignUp", "Cart");



        }

        [RequiresSSL]
        [HttpGet]
        public ActionResult NewSignUp()
        {
            var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckoutModel == null)
                throw new AuthSessionExpiredException();

            var model = new CartNewSignupViewModel();

            return View(model);
        }

        [RequiresSSL]
        [HttpPost]
        public ActionResult NewSignUp(CartNewSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();

                string ipAddress = ControllerHelper.GetIpAddressofUser(HttpContext.Request);
                UserContext userContext = new UserContext();

                var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
                if (paypalCheckoutModel == null)
                    return RedirectToAction("Index", "Rate");

                var res = _dataRepository.QuickSignUp(paypalCheckoutModel.ProcessPlanInfo.CountryFrom, paypalCheckoutModel.ProcessPlanInfo.CountryTo, model.Email, model.Password,
                    model.PhoneNumber, ipAddress, ref userContext);

                if (res == "1")
                {
                    var context = Authenticate(model.Email, model.Password);
                    if (context == null)
                    {
                        model.Message = "Invalid Email and Password combination.";
                        model.MessageType = MessageType.Error;

                        return View(model);
                    }
                }
                else
                {
                    model.Message = res;
                    model.MessageType = MessageType.Error;
                    return View(model);
                }
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("UpdateBillingInfo", "Cart");

            }

            TempData["ReturnUrl"] = model.ReturnUrl;
            return View(model);
        }


        [RequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult UpdateBillingInfo(string returnUrl = "")
        {

            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();

            if (ControllerHelper.IsEnforceToRecharge(paypalCheckOutModel, UserContext.MemberId))
            {
                TempData["ViewMessage"] = new ViewMessage()
                {
                    MessageType = MessageType.Error,
                    Message = "You already have purchased the same plan, please recharge."
                };
                return RedirectToAction("MyAccount", "Account");
            }

            var data = _dataRepository.GetBillingInfo(UserContext.MemberId);
            var model = Mapper.Map<BillingInfoViewModel>(data);
            model.ReturnUrl = returnUrl;
            model.Countries = CacheManager.Instance.GetTop3FromCountries();
            var country = model.Countries.FirstOrDefault(a => a.Name == model.Country) ?? new Country() { Id = "1" };
            model.States = _dataRepository.GetStateList(SafeConvert.ToInt32(country.Id));
            model.Country = country.Id;
            model.IsValidForReferer = UserContext.UserType.ToLower() == "new";

            if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.TopUp.ToString())
                model.IsValidForReferer = false;

            return View(model);
        }

        

        [RequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBillingInfo(BillingInfoViewModel model)
        {

            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();

            if (ModelState.IsValid)
            {


                var billingModel = Mapper.Map<BillingInfo>(model);
                billingModel.MemberId = UserContext.MemberId;
                billingModel.Email = UserContext.Email;
                billingModel.UserType = UserContext.UserType;
                billingModel.RefererEmail = string.IsNullOrEmpty(model.RefrerEmail) ? string.Empty : model.RefrerEmail;

                var res = _dataRepository.UpdateBillingInfo(billingModel);
                if (res.Status)
                {
                    var updateBillingInfo = _dataRepository.GetBillingInfo(UserContext.MemberId);
                    var context = new UserContext
                    {
                        ProfileInfo = updateBillingInfo,
                        MemberId = UserContext.MemberId,
                        IsEmailSubscribed = UserContext.IsEmailSubscribed,
                        Email = UserContext.Email,
                        UserType = UserContext.UserType
                    };

                    UpdateUserContext(UserContext.Email, context);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    if (ControllerHelper.IsValidForRegisterPinlessNumbers(paypalCheckOutModel.ProcessPaymentInfo.TransactionType, paypalCheckOutModel.ProcessPlanInfo.Pin))
                        return RedirectToAction("RegisterPinlessNumbers");

                    return RedirectToAction("PaymentInfo", "Cart");

                }
                else
                {
                    model.Message = res.Errormsg;
                    model.MessageType = MessageType.Error;
                }

            }

            model.Countries = CacheManager.Instance.GetTop3FromCountries();
            var country = model.Countries.FirstOrDefault(a => a.Name == model.Country) ?? new Country() { Id = "1" };
            model.States = _dataRepository.GetStateList(SafeConvert.ToInt32(country.Id));
            model.Country = country.Id;
            return View(model);
        }

        [RequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult PaymentInfo(string returnUrl)
        {
            var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckoutModel == null)
                throw new AuthSessionExpiredException();


            if (TempData["IsInvalidCouponCode"] != null)
            {
                if ((bool)TempData["IsInvalidCouponCode"])
                    ModelState.AddModelError("CouponCode", AppMessage.ErrorInvalidCouponCode);
            }


            var savedCardDetails = _dataRepository.Get_top_CreditCard(UserContext.MemberId);
            if (savedCardDetails.GetCardList.Any())
            {
                var savedPaymentInfoViewModel = new SavedPaymentInfoViewModel();
                bool isNewCardAdded = false;
                if (TempData["NewCardAdded"] != null)
                {
                    isNewCardAdded = (bool)TempData["NewCardAdded"];
                }

                if (isNewCardAdded)
                {
                    var newCardInfo = savedCardDetails.GetCardList.First();
                    savedPaymentInfoViewModel.CreditCardId = SafeConvert.ToString(newCardInfo.CreditCardId);
                }
                else
                {
                    if (paypalCheckoutModel.ProcessPaymentInfo != null &&
                        !string.IsNullOrEmpty(paypalCheckoutModel.ProcessPaymentInfo.CardNumber))
                    {
                        var cardDetails =
                            savedCardDetails.GetCardList.FirstOrDefault(
                                a => a.CreditCardNumber == paypalCheckoutModel.ProcessPaymentInfo.CardNumber);
                        if (cardDetails != null)
                        {
                            savedPaymentInfoViewModel.CreditCardId = SafeConvert.ToString(cardDetails.CreditCardId);
                        }
                    }
                }
                savedPaymentInfoViewModel.CouponCode = paypalCheckoutModel.ProcessPaymentInfo != null ? paypalCheckoutModel.ProcessPaymentInfo.CouponCode : String.Empty;
                savedPaymentInfoViewModel.IsPaypalDisabled = paypalCheckoutModel.ProcessPaymentInfo != null && paypalCheckoutModel.ProcessPaymentInfo.IsPaypalDisabled;
                savedPaymentInfoViewModel.ReturnUrl = returnUrl;
                savedPaymentInfoViewModel.UserCreditCards =
                            Mapper.Map<List<SelectListItem>>(savedCardDetails.GetCardList);

                return View("SavedPaymentInfo", savedPaymentInfoViewModel);
            }

            var model = new CartPaymentInfoViewModel() { ReturnUrl = returnUrl };



            if (paypalCheckoutModel.ProcessPaymentInfo != null)
            {
                model.CardType = paypalCheckoutModel.ProcessPaymentInfo.CardType;
                model.CreditCardNumber = paypalCheckoutModel.ProcessPaymentInfo.CardNumber;
                model.ExpMonth = paypalCheckoutModel.ProcessPaymentInfo.ExpMonth;
                model.ExpYear = paypalCheckoutModel.ProcessPaymentInfo.ExpYear;
                model.PaymentType = paypalCheckoutModel.ProcessPaymentInfo.PaymentType;
                model.CouponCode = paypalCheckoutModel.ProcessPaymentInfo.CouponCode;
                model.IsPaypalDisabled = paypalCheckoutModel.ProcessPaymentInfo.IsPaypalDisabled;
            }

            return View(model);
        }

        [RequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        public ActionResult PaymentInfo(CartPaymentInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);



            var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckoutModel == null)
                return RedirectToAction("Index", "Rate");

            if (model.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
            {
                var paymentInfo = new ProcessPaymentInfo()
                {
                    CardNumber = string.Empty,
                    CardType = string.Empty,
                    ExpMonth = string.Empty,
                    ExpYear = string.Empty,

                    PaymentType = model.PaymentType,
                    CouponCode = model.CouponCode,
                    IpAddress = ControllerHelper.GetIpAddressofUser(Request),
                    TransactionType = GetTransactionType(paypalCheckoutModel)
                };
                paypalCheckoutModel.ProcessPaymentInfo = paymentInfo;
            }
            else
            {
                var paymentInfo = new ProcessPaymentInfo()
                {
                    CardNumber = model.CreditCardNumber,
                    CardType = model.CardType,
                    CouponCode = model.CouponCode,
                    ExpMonth = model.ExpMonth,
                    ExpYear = model.ExpYear,
                    Cvv = model.Cvv,
                    TransactionType = GetTransactionType(paypalCheckoutModel),
                    PaymentType = model.PaymentType,
                    IpAddress = ControllerHelper.GetIpAddressofUser(Request)
                };
                paypalCheckoutModel.ProcessPaymentInfo = paymentInfo;
            }

            if (!ControllerHelper.IsValidCouponCode(UserContext, paypalCheckoutModel))
            {
                ModelState.AddModelError("CouponCode", AppMessage.ErrorInvalidCouponCode);
                return View(model);
            }

            Session[GlobalSetting.CheckOutSesionKey] = paypalCheckoutModel;

            if (!string.IsNullOrEmpty(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("ReviewOrder", "Cart");
        }

        [RequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        public ActionResult SavedDetailsUpdated(SavedPaymentInfoViewModel model)
        {
            PayPalCheckoutModel payPalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (payPalCheckoutModel == null)
                return null;

            if (ModelState.IsValid)
            {
                var cardInfo = ControllerHelper.GetCreditCardByCardId(UserContext.MemberId, SafeConvert.ToInt32(model.CreditCardId));

                if (payPalCheckoutModel.ProcessPaymentInfo == null)
                    payPalCheckoutModel.ProcessPaymentInfo = new ProcessPaymentInfo();

                payPalCheckoutModel.ProcessPaymentInfo.TransactionType = GetTransactionType(payPalCheckoutModel);
                if (model.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    payPalCheckoutModel.ProcessPaymentInfo.CardNumber = string.Empty;
                    payPalCheckoutModel.ProcessPaymentInfo.CardType = string.Empty;
                    payPalCheckoutModel.ProcessPaymentInfo.ExpMonth = string.Empty;
                    payPalCheckoutModel.ProcessPaymentInfo.ExpYear = string.Empty;
                    payPalCheckoutModel.ProcessPaymentInfo.Cvv = null;
                    payPalCheckoutModel.ProcessPaymentInfo.CouponCode = model.CouponCode;
                    payPalCheckoutModel.ProcessPaymentInfo.PaymentType = model.PaymentType;
                    payPalCheckoutModel.ProcessPaymentInfo.IpAddress = ControllerHelper.GetIpAddressofUser(Request);
                }
                else if (model.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    payPalCheckoutModel.ProcessPaymentInfo.CardNumber = cardInfo.CreditCardNumber;
                    payPalCheckoutModel.ProcessPaymentInfo.CardType = cardInfo.CreditCardType;
                    payPalCheckoutModel.ProcessPaymentInfo.ExpMonth = cardInfo.ExpiryMonth;
                    payPalCheckoutModel.ProcessPaymentInfo.ExpYear = DateTime.Now.Year.ToString().Substring(0, 2) + cardInfo.ExpiryYear;
                    payPalCheckoutModel.ProcessPaymentInfo.Cvv = model.Cvv;
                    payPalCheckoutModel.ProcessPaymentInfo.CouponCode = model.CouponCode;
                    payPalCheckoutModel.ProcessPaymentInfo.PaymentType = model.PaymentType;
                    payPalCheckoutModel.ProcessPaymentInfo.IpAddress = ControllerHelper.GetIpAddressofUser(Request);

                }
                else
                {
                    model.Message = "Invalid Payment Type";
                    model.MessageType = MessageType.Error;
                    TempData["SavedPaymentInfoViewModel"] = model;
                    return RedirectToAction("PaymentInfo", "Cart");
                }

                if (!ControllerHelper.IsValidCouponCode(UserContext, payPalCheckoutModel))
                {
                    ModelState.AddModelError("CouponCode", AppMessage.ErrorInvalidCouponCode);
                    TempData["SavedPaymentInfoViewModel"] = model;
                    TempData["IsInvalidCouponCode"] = true;
                    return RedirectToAction("PaymentInfo", "Cart");
                }

                Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("ReviewOrder", "Cart");

            }
            TempData["SavedPaymentInfoViewModel"] = model;
            return RedirectToAction("PaymentInfo", "Cart");
        }

        [RequiresSSL]
        public ActionResult Checkout()
        {
            return View();
        }

        /// <summary>
        /// Return When transaction is failed.
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderFailed()
        {
            DeleteCartSession();
            return View("Failed/TransactionFailed");
        }

        public ActionResult OrderConfirmation()
        {

            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();

            var transactonType = paypalCheckOutModel.ProcessPaymentInfo.TransactionType;

            var localAccessNumbers = new List<LocalAccessNumber>();
            if (transactonType == TransactionType.CheckOut.ToString())
            {
                var model = new LocalAccessNumberViewModel()
                {
                    PhoneNumber = UserContext.ProfileInfo.PhoneNumber,
                    AccessCountry = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country).Id,
                    AccessState = UserContext.ProfileInfo.State
                };
                localAccessNumbers = ControllerHelper.GetLocalAccessNumbers(model);
            }

            if (transactonType == TransactionType.Recharge.ToString())
            {
                return View("Success/RechargeConfirmation");
            }
            else if (transactonType == TransactionType.PurchaseNewPlan.ToString())
            {
                return View("Success/PurchaseNewPlanSuccess");
            }
            else if (transactonType == TransactionType.TopUp.ToString())
            {
                return View("Success/TopUpSuccessfull");
            }
            else if (transactonType == TransactionType.CheckOut.ToString())
            {
                return View("Success/CheckOutSuccess", localAccessNumbers.Take(5));
            }
            else if (transactonType == TransactionType.SaveOrder.ToString())
            {
                return View("Success/PurchaseNewPlanSuccess");
            }

            throw new AuthSessionExpiredException();
        }

        [RequiresSSL]
        [MobileAuthorize]
        public ActionResult ReviewOrder(string id = "")
        {
            var payPalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (payPalCheckoutModel == null)
            {
                throw new AuthSessionExpiredException();
                //return !string.IsNullOrEmpty(id)
                //    ? RedirectToAction("Index", "Recharge", new { id = ControllerHelper.Encrypt(id) })
                //    : RedirectToAction("Index", "Rate");
            }

            //do validation that you want before Review Order...
            #region ForMobile Top country check

            if (payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo != null
                && payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.TopUp.ToString() &&
                payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupType == TopupType.International)
            {
                var userCountryInfo = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
                if (payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupFromCountry !=
                    SafeConvert.ToInt32(userCountryInfo.Id))
                {
                    TempData["ViewMessage"] = new ViewMessage()
                    {
                        MessageType = MessageType.Error,
                        Message = "Sorry! You can not make recharge from this country, Please try Again."
                    };
                    return RedirectToAction("International", "TopUpMobile");
                }
            }

            #endregion

            #region check for TransactionType and update for OldCustomer

            if (UserContext.UserType.ToLower() == "old"
                && payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
            {
                payPalCheckoutModel.ProcessPaymentInfo.TransactionType = TransactionType.PurchaseNewPlan.ToString();
            }
            #endregion

            var model = new CartReviewOrderViewModel
            {
                FirstName = UserContext.ProfileInfo.FirstName,
                LastName = UserContext.ProfileInfo.LastName,
                PhoneNumber = UserContext.ProfileInfo.PhoneNumber,
                Email = UserContext.ProfileInfo.Email,
                Address = UserContext.ProfileInfo.Address,
                City = UserContext.ProfileInfo.City,
                State = UserContext.ProfileInfo.State,
                Country = UserContext.ProfileInfo.Country,
                ZipCode = UserContext.ProfileInfo.ZipCode,

                CurrencyCode = payPalCheckoutModel.ProcessPlanInfo.CurrencyCode,
                Amount = SafeConvert.ToString(payPalCheckoutModel.ProcessPlanInfo.Amount),
                ServiceFee = SafeConvert.ToString(payPalCheckoutModel.ProcessPlanInfo.ServiceFee),
                TotalAmount = SafeConvert.ToString(payPalCheckoutModel.ProcessPlanInfo.TotalAmount),
                CouponCode = payPalCheckoutModel.ProcessPaymentInfo.CouponCode,
                CardType = payPalCheckoutModel.ProcessPaymentInfo.CardType,
                CardNumber = ControllerHelper.MaskedCreditCard(payPalCheckoutModel.ProcessPaymentInfo.CardNumber),
                PaymentType = payPalCheckoutModel.ProcessPaymentInfo.PaymentType,

                Message = payPalCheckoutModel.Message != null ? payPalCheckoutModel.Message.Message : String.Empty,
                MessageType = payPalCheckoutModel.Message != null ? payPalCheckoutModel.Message.MessageType : String.Empty
            };
            payPalCheckoutModel.Message = new ViewMessage();
            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
            return View(model);
        }

        [RequiresSSL]
        [MobileAuthorize]
        public ActionResult RegisterPinlessNumbers()
        {
            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();
            ViewBag.IsTextDisable = paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString();
            return View();
        }

        [HttpPost]
        [RequiresSSL]
        [MobileAuthorize]
        public ActionResult RegisterPinlessNumbers(List<string> pinlessNumber)
        {
            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();

            paypalCheckOutModel.ProcessPlanInfo.PinlessNumbers = pinlessNumber;
            return RedirectToAction("PaymentInfo", "Cart");

        }

    }
}
