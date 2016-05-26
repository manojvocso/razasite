using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    public class RedeemPointController : BaseController
    {
        private readonly DataRepository _dataRepository;
        public RedeemPointController()
        {
            _dataRepository = new DataRepository();
        }

        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult Index(string id)
        {
            var planInfo = ControllerHelper.GetSinglePlanInfo(id, UserContext.MemberId);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            //var isValidForReedem = ControllerHelper.IsValidForReedemPoints(UserContext);
            //if (!isValidForReedem)
            //{
            //    //    return RedirectToAction("Plan", "Account", new { id = ControllerHelper.Encrypt(id) });
            //}

            var model = new RedeemPointsViewModel();
            var redeemOpt = ControllerHelper.GetReedemPointOptions(UserContext, Convert.ToDecimal(planInfo.ServiceFee));
            if (redeemOpt == null)
            {
                //   return RedirectToAction("Plan", "Account", new { id = ControllerHelper.Encrypt(id) });
            }
            model.RedeemOptions = redeemOpt;
            model.PlanName = planInfo.PlanName;
            return View("RedeemPoints",model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult Index(RedeemPointsViewModel model, string id)
        {
            var planInfo = ControllerHelper.GetSinglePlanInfo(id, UserContext.MemberId);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var serviceFee =
                    ControllerHelper.CalculateServiceFee(
                        string.IsNullOrEmpty(planInfo.ServiceFee) ? 0 : Convert.ToDouble(planInfo.ServiceFee),
                        model.RedeemPointAmount);


                var payPalCheckoutModel = new PayPalCheckoutModel
                {
                    ProcessPlanInfo = new ProcessPlanInfo()
                    {
                        Amount = model.RedeemPointAmount,
                        CurrencyCode = planInfo.CurrencyCode
                    },
                    ProcessPaymentInfo = new ProcessPaymentInfo()
                    {
                        TransactionType = TransactionType.Recharge.ToString(),

                    }
                };

                Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
                return RedirectToAction("ReviewOrder", "RedeemPoint", new { id = ControllerHelper.Encrypt(id) });
            }
            TempData["RedeemPointsViewModel"] = model;
            return RedirectToAction("Index", "RedeemPoint", new { id = ControllerHelper.Encrypt(id) });
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult RedeemPointRecharge(string id)
        {
            var planInfo = ControllerHelper.GetSinglePlanInfo(id, UserContext.MemberId);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckoutModel == null)
                throw new AuthSessionExpiredException();

            var serviceFee =
                ControllerHelper.CalculateServiceFee(
                    string.IsNullOrEmpty(planInfo.ServiceFee) ? 0 : Convert.ToDouble(planInfo.ServiceFee),
                    paypalCheckoutModel.ProcessPlanInfo.Amount);


            string orderId = ControllerHelper.CreateOrderId(TransactionType.Recharge.ToString());

            RechargeInfo rechargeInfo = new RechargeInfo
            {
                OrderId = orderId,
                MemberId = UserContext.MemberId,
                IpAddress = ControllerHelper.GetIpAddressofUser(Request),
                CVV2Response = string.Empty,
                Xid = string.Empty,
                AuthTransactionId = string.Empty,
                AVSResponse = string.Empty,
                Amount = paypalCheckoutModel.ProcessPlanInfo.Amount,
                CardNumber = string.Empty,
                City = UserContext.ProfileInfo.City,
                State = UserContext.ProfileInfo.State,
                CVV2 = string.Empty,
                IsCcProcess = false,
                CoupanCode = string.Empty,
                Cavv = string.Empty,
                EciFlag = string.Empty,
                PaymentTransactionId = string.Empty,
                Address1 = UserContext.ProfileInfo.Address,
                ZipCode = UserContext.ProfileInfo.ZipCode,
                Country = UserContext.ProfileInfo.Country,
                Pin = planInfo.AccountNumber,
                ExpiryDate = string.Empty,
                Address2 = string.Empty,
                ServiceFee = serviceFee,
                IsAutoRefill = string.Empty,
                UserType = "Old",
                PaymentMethod = "Redeem"
            };

            var rechstatus = _dataRepository.Recharge_Pin(rechargeInfo);

            if (rechstatus.Status)
            {
                return RedirectToAction("OrderConfirmation", "Cart");
            }

            return RedirectToAction("OrderFailed", "Cart");
        }

        [EncryptedActionParameter]
        public ActionResult ReviewOrder(string id)
        {
            var planInfo = ControllerHelper.GetSinglePlanInfo(id, UserContext.MemberId);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            var paypalCheckOutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                throw new AuthSessionExpiredException();

            var cartReviewOrderViewModel = new RedeemPointReviewOrderViewModel()
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

                CurrencyCode = paypalCheckOutModel.ProcessPlanInfo.CurrencyCode,
                PaymentType = "Redeem Points",
                Amount = SafeConvert.ToString(paypalCheckOutModel.ProcessPlanInfo.Amount),
                PlanPin = planInfo.AccountNumber

            };

            return View("ReedemPoints_ReviewOrder", cartReviewOrderViewModel);
        }

    }
}
