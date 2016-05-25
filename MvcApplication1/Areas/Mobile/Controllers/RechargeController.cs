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
    [RequiresSSL]
    [CompressFilter]
    [MobileAuthorize]
    public class RechargeController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public RechargeController()
        {
            _dataRepository = new DataRepository();
        }

        #region Controller Private Methods
        private OrderHistoricPlanInfoSnapshot GetSinglePlanInfo(string pin)
        {
            var data = _dataRepository.GetCustomerPlanList(UserContext.MemberId);
            if (data == null)
                return null;

            var planInfo = data.OrderInfos.FirstOrDefault(a => a.AccountNumber == pin);
            return planInfo;
        }
        #endregion

        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Index(string id)
        {
            var planData = GetSinglePlanInfo(id);
            if (planData == null)
                throw new ResourceNotFoundException();

            var userSavedCreditCards = _dataRepository.Get_top_CreditCard(UserContext.MemberId);

            var model = TempData["RechargeViewModel"] as RechargeViewModel ?? new RechargeViewModel();
            TempData["RechargeViewModel"] = model;

            model.RechargeAmounts = _dataRepository.RechargeAmountVal(Convert.ToInt32(planData.PlanId)).GetAmountlist;
            model.CurrencyCode = planData.CurrencyCode;
            model.UserCreditCards = Mapper.Map<List<SelectListItem>>(userSavedCreditCards.GetCardList);
            model.PaymentType = PaymentSettings.PaymentType.CreditCard.ToString();

            return View(model);
        }

        [HttpPost]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RechargeViewModel rechargeViewModel, string id)
        {
            var planData = GetSinglePlanInfo(id);
            if (planData == null)
                return RedirectToAction("MyAccount", "Account");

            if (ModelState.IsValid)
            {
                var planInfo = GetSinglePlanInfo(id);
                var processPlanInfo = new ProcessPlanInfo()
                {
                    CountryFrom = planInfo.CountryFrom,
                    CountryTo = planInfo.CountryTo,
                    CurrencyCode = planInfo.CurrencyCode,
                    CardId = Convert.ToInt32(planInfo.PlanId),
                    OrderId = planInfo.OrderId,
                    Pin = planInfo.AccountNumber,
                    PlanName = planInfo.PlanName,
                    Amount = rechargeViewModel.RechargeAmount,
                    ServiceFee = ControllerHelper.CalculateServiceFee(Convert.ToDouble(planInfo.ServiceFee), rechargeViewModel.RechargeAmount),

                };
                var payPalCheckoutModel = new PayPalCheckoutModel()
                {
                    ProcessPlanInfo = processPlanInfo,
                    ProcessByUserInfo = UserContext.ProfileInfo
                };

                if (rechargeViewModel.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    payPalCheckoutModel.ProcessPaymentInfo = new ProcessPaymentInfo()
                    {
                        PaymentType = PaymentSettings.PaymentType.PayPal.ToString(),
                        TransactionType = TransactionType.Recharge.ToString(),
                        IpAddress = ControllerHelper.GetIpAddressofUser(Request)
                    };
                }
                else if (rechargeViewModel.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    var cardInfo = ControllerHelper.GetCreditCardByCardId(UserContext.MemberId, SafeConvert.ToInt32(rechargeViewModel.CreditCardId));
                    payPalCheckoutModel.ProcessPaymentInfo = new ProcessPaymentInfo()
                    {
                        CardNumber = cardInfo.CreditCardNumber,
                        CardType = cardInfo.CreditCardType,
                        ExpMonth = cardInfo.ExpiryMonth,
                        ExpYear = DateTime.Now.Year.ToString().Substring(0, 2) + cardInfo.ExpiryYear,
                        Cvv = rechargeViewModel.Cvv,
                        CouponCode = rechargeViewModel.CouponCode,
                        PaymentType = rechargeViewModel.PaymentType,
                        TransactionType = TransactionType.Recharge.ToString(),
                        IpAddress = ControllerHelper.GetIpAddressofUser(Request)
                    };

                }


                if (!ControllerHelper.IsValidCouponCode(UserContext, payPalCheckoutModel))
                {
                    ModelState.AddModelError("CouponCode", AppMessage.ErrorInvalidCouponCode);
                    rechargeViewModel.RechargeAmounts =
                        _dataRepository.RechargeAmountVal(Convert.ToInt32(planData.PlanId)).GetAmountlist;
                    rechargeViewModel.CurrencyCode = planData.CurrencyCode;
                    rechargeViewModel.UserCreditCards =
                        Mapper.Map<List<SelectListItem>>(_dataRepository.Get_top_CreditCard(UserContext.MemberId).GetCardList);

                    return View(rechargeViewModel);
                }



                //set checkout model to session.. 
                Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;

                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                    return RedirectToAction("ReviewOrder", "Cart", new { id = ControllerHelper.Encrypt(id) });

                return RedirectToAction("AutoRefill", "Recharge", new { id = ControllerHelper.Encrypt(id) });
            }

            rechargeViewModel.RechargeAmounts =
                _dataRepository.RechargeAmountVal(Convert.ToInt32(planData.PlanId)).GetAmountlist;
            rechargeViewModel.CurrencyCode = planData.CurrencyCode;
            rechargeViewModel.UserCreditCards =
                Mapper.Map<List<SelectListItem>>(_dataRepository.Get_top_CreditCard(UserContext.MemberId).GetCardList);

            return View(rechargeViewModel);
        }

        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult AutoRefill(string id)
        {
            var planInfo = GetSinglePlanInfo(id);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            var model = new RechargeAutorefillSetupViewModel()
            {
                AutoRefillOptionsList = ControllerHelper.GetAutoRefillOptions(),
                PlanPin = id,
                CurrencyCode = planInfo.CurrencyCode
            };
            return View(model);
        }

        [HttpPost]
        [EncryptedActionParameter]
        public ActionResult AutoRefill(RechargeAutorefillSetupViewModel model, string id)
        {
            if (ModelState.IsValid)
            {
                var payPalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
                if (payPalCheckoutModel == null)
                    return RedirectToAction("Index", "Recharge", new { id = ControllerHelper.Encrypt(id) });

                if (Math.Abs(model.AutoRefillAmout) > 0)
                {
                    payPalCheckoutModel.ProcessPlanInfo.IsAutoRefill = true;
                    payPalCheckoutModel.ProcessPlanInfo.AutoRefillAmount = model.AutoRefillAmout;
                    Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
                }
                return RedirectToAction("ReviewOrder", "Cart", new { id = ControllerHelper.Encrypt(id) });
            }
            return View(model);
        }

        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult ReviewOrder(string id)
        {
            var payPalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (payPalCheckoutModel == null)
                return RedirectToAction("Index", "Recharge", new { id = ControllerHelper.Encrypt(id) });


            var model = new RechargeReviewOrderViewModel()
            {
                FirstName = payPalCheckoutModel.ProcessByUserInfo.FirstName,
                LastName = payPalCheckoutModel.ProcessByUserInfo.LastName,
                PhoneNumber = payPalCheckoutModel.ProcessByUserInfo.PhoneNumber,
                Email = payPalCheckoutModel.ProcessByUserInfo.Email,
                Address = payPalCheckoutModel.ProcessByUserInfo.Address,
                City = payPalCheckoutModel.ProcessByUserInfo.City,
                State = payPalCheckoutModel.ProcessByUserInfo.State,
                Country = payPalCheckoutModel.ProcessByUserInfo.Country,
                ZipCode = payPalCheckoutModel.ProcessByUserInfo.ZipCode,

                CurrencyCode = payPalCheckoutModel.ProcessPlanInfo.CurrencyCode,
                Amount = SafeConvert.ToString(payPalCheckoutModel.ProcessPlanInfo.Amount),
                CouponCode = payPalCheckoutModel.ProcessPaymentInfo.CouponCode,
                CardType = payPalCheckoutModel.ProcessPaymentInfo.CardType,
                CardNumber = ControllerHelper.MaskedCreditCard(payPalCheckoutModel.ProcessPaymentInfo.CardNumber),
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Confirmation()
        {
            return View();
        }

    }
}
