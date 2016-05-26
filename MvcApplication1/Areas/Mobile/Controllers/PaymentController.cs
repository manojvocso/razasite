using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CardinalCommerce;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Mobile.Controllers
{

    [CompressFilter]
    [MobileAuthorize]
    public class PaymentController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public PaymentController()
        {
            _dataRepository = new DataRepository();
        }

        [RequiresSSL]
        [HttpGet]
        public ActionResult UpdateCard(string returnUrl = "")
        {
            var model = TempData["UpdateCreditCardViewModel"] as UpdateCreditCardViewModel ??
                        new UpdateCreditCardViewModel();


            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [RequiresSSL]
        [HttpPost]
        public ActionResult UpdateCard(UpdateCreditCardViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var userCountry =
                    ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);

                var res = _dataRepository.AddCreditCard(UserContext.MemberId, string.Empty, model.CreditCardNumber, model.ExpMonth,
                     model.ExpYear.Substring(2, 2), model.Cvv.ToString(), userCountry.Id, UserContext.ProfileInfo.Address,
                     UserContext.ProfileInfo.City,
                     UserContext.ProfileInfo.State, UserContext.ProfileInfo.ZipCode);
                if (res.Status)
                {
                    //info added to tempdate for show newly updated card in dropdown.
                    TempData["NewCardAdded"] = true;

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    model = new UpdateCreditCardViewModel()
                    {
                        Message = AppMessage.PaymentInfoSuccessfullyUpdated,
                        MessageType = MessageType.Success
                    };
                }
                else
                {
                    model = new UpdateCreditCardViewModel()
                    {
                        Message = res.Errormsg,
                        MessageType = MessageType.Error
                    };
                }
            }
            model.ReturnUrl = returnUrl;
            TempData["UpdateCreditCardViewModel"] = model;
            return RedirectToAction("UpdateCard", "Payment", new { returnUrl = returnUrl });
        }

        [RequiresSSL]
        [HttpPost]
        public ActionResult ProcessPayment(Recharge model)
        {
            return RedirectToAction("ProcessPayment", "Paypal", new { area = "Payment" });
        }

        public ActionResult PasscodeSetup()
        {
            return View();
        }

        [EncryptedActionParameter]
        public ActionResult EditCardInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ResourceNotFoundException();

            var cardInfo = ControllerHelper.GetCreditCardByCardId(UserContext.MemberId, SafeConvert.ToInt32(id));
            if(cardInfo==null)
                throw new ResourceNotFoundException();

            var model = new UpdateExistingCardInfoViewModel()
            {
                CreditCardId = cardInfo.CreditCardId,
                CardType = cardInfo.CreditCardType,
                CreditCardNumber = cardInfo.CreditCardNo,
                ExpMonth = cardInfo.ExpiryMonth,
                ExpYear = cardInfo.ExpiryYear.ToFourDigitYear()
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult EditCardInfo(UpdateExistingCardInfoViewModel model)
        {

            if (ModelState.IsValid)
            {
                var cardInfo = ControllerHelper.GetCreditCardByCardId(UserContext.MemberId, SafeConvert.ToInt32(model.CreditCardId));
                if (cardInfo == null)
                {
                    model.Message = "Unable to update information, Please try again.";
                    model.MessageType = MessageType.Error;
                    return View(model);
                }
                var countryinfo = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
                var res = _dataRepository.AddCreditCard(UserContext.MemberId, UserContext.FullName,
                    cardInfo.CreditCardNumber, model.ExpMonth, model.ExpYear.ToTwoDigitYear(),
                    SafeConvert.ToString(model.Cvv),
                    countryinfo.Id, UserContext.ProfileInfo.Address, UserContext.ProfileInfo.City,
                    UserContext.ProfileInfo.State, UserContext.ProfileInfo.ZipCode);
                if (res.Status)
                {
                    return RedirectToAction("SavedPaymentInfo", "Account");
                }
                model.Message = res.Errormsg;
                model.MessageType = MessageType.Error;
            }
            return View(model);
        }


    }
}
