using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.AppHelper;
using Raza.Common;
using Raza.Model.PaymentProcessModel;
using Raza.Model;
using Raza.Repository;
using System.Configuration;

namespace MvcApplication1.Areas.Payment.Controllers
{
    public class PlanHandler
    {
        private readonly DataRepository _dataRepositry;

        public PlanHandler()
        {
            _dataRepositry = new DataRepository();
        }

        #region EmailHandles methods
        public void SendRechargeConfirmMail(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            try
            {
                RazaLogger.WriteInfo("Sending recharge Confirmation mail.");
                string paymentmethod = string.Empty;
                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    paymentmethod = "PayPal";
                }
                else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    int cardlength = payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Length;
                    paymentmethod = payPalCheckoutModel.ProcessPaymentInfo.CardType + ",...." +
                                    payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Substring(cardlength - 4, 4);
                }

                string accountnumber = Helpers.MaskAccountNumber(payPalCheckoutModel.ProcessPlanInfo.Pin, "X");
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = payPalCheckoutModel.ProcessPlanInfo.ServiceFee;
                double totalcharge = payPalCheckoutModel.ProcessPlanInfo.TotalAmount;
                string servername = GlobalSetting.ServerName;

                string helplinenumber = string.Empty;
                var usercountry = ControllerHelper.GetUserCountryId(billingInfo.Country);
                helplinenumber = Helpers.GetHelpLineNumber(SafeConvert.ToInt32(usercountry.Id));

                string redirectlink = "Account/MyAccount";

                string mailbody =
                    System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/email-recharge.html"));

                string username = String.Format("{0} {1}", billingInfo.FirstName, billingInfo.LastName);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--UserName-->", username);
                mailbody = mailbody.Replace(@"<!--PlanName-->", payPalCheckoutModel.ProcessPlanInfo.PlanName);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", payPalCheckoutModel.OrderId);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(payPalCheckoutModel.ProcessPlanInfo.Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", billingInfo.Email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--DateTime-->", datetime);
                mailbody = mailbody.Replace(@"<!--Currencycode-->", payPalCheckoutModel.ProcessPlanInfo.CurrencyCode);
                mailbody = mailbody.Replace(@"<!--OldOrderId-->", payPalCheckoutModel.ProcessPlanInfo.OrderId);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    billingInfo.Email,
                    ConfigurationManager.AppSettings["Rechargeconfim"],
                    mailbody,
                    true);
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in recharge confirmation mail: " + ex.Message);
            }

        }

        public void SendCallingCardPurchaseMail(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            try
            {
                string paymentmethod = string.Empty;

                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    paymentmethod = "PayPal";
                }
                else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    int cardlength = payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Length;
                    paymentmethod = payPalCheckoutModel.ProcessPaymentInfo.CardType + ",...." +
                                    payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Substring(cardlength - 4, 4);
                }


                string accountnumber = Helpers.MaskAccountNumber(payPalCheckoutModel.ProcessPlanInfo.Pin, "X");

                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = payPalCheckoutModel.ProcessPlanInfo.ServiceFee;
                double totalcharge = payPalCheckoutModel.ProcessPlanInfo.TotalAmount;
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string helplinenumber = string.Empty;
                var usercountry = ControllerHelper.GetUserCountryId(billingInfo.Country);
                helplinenumber = Helpers.GetHelpLineNumber(SafeConvert.ToInt32(usercountry.Id));
                string redirectlink = "Account/MyAccount";

                string bannername = "newplan_details.jpg";

                //if (model.TransactionType == "PurchaseNewPlan") // existing customer new plan
                //{
                //    bannername = "newplan_details.jpg";
                //}
                if (payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString()) //// new customer new plan
                {
                    bannername = "activation-confirmed-banner.jpg";
                }

                string mailbody =
                    System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/calling-card-purchase-details.html"));
                string username = String.Format("{0} {1}", billingInfo.FirstName, billingInfo.LastName);

                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--UserName-->", username);
                mailbody = mailbody.Replace(@"<!--PlanName-->", payPalCheckoutModel.ProcessPlanInfo.PlanName);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", payPalCheckoutModel.OrderId);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(payPalCheckoutModel.ProcessPlanInfo.Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", billingInfo.Email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--Currencycode-->", payPalCheckoutModel.ProcessPlanInfo.CurrencyCode);
                mailbody = mailbody.Replace(@"<!--Banner-->", bannername);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    billingInfo.Email,
                    ConfigurationManager.AppSettings["CallingCardPurchase"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in Mail Send of Calling Card Purchase:" + ex.Message);
            }

        }

        public void SendMobileTopUpMail(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            try
            {


                string paymentmethod = string.Empty;
                string creditcard = String.Empty;
                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    paymentmethod = "PayPal";
                }
                else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    int cardlength = payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Length;
                    creditcard = payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Substring(0, 4) + "......." +
                                 payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Substring(cardlength - 4, 4);

                    paymentmethod = payPalCheckoutModel.ProcessPaymentInfo.CardType + ",...." +
                                    payPalCheckoutModel.ProcessPaymentInfo.CardNumber.Substring(cardlength - 4, 4);
                }

                //string creditcard = model.CardNumber;
                //string paymentmethod = model.PaymentMethod;

                string orderId = payPalCheckoutModel.OrderId;

                string mobileTopupNumber = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopUpCountryCode +
                                           payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupMobileNumber;

                string topupamount = SafeConvert.ToString(payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupSourceAmount);
                string taxamount = "0%";
                string email = billingInfo.Email;
                string datetime = DateTime.Now.ToString();
                string currencycode = payPalCheckoutModel.ProcessPlanInfo.CurrencyCode;


                string helplinenumber = string.Empty;
                var usercountry = ControllerHelper.GetUserCountryId(billingInfo.Country);

                helplinenumber = Helpers.GetHelpLineNumber(SafeConvert.ToInt32(usercountry.Id));
                string redirectlink = string.Empty;
                if (billingInfo.UserType.ToLower() == "old")
                {
                    redirectlink = "Account/MyAccount";
                }



                string mailbody =
                                 System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/mobile-topup.html"));
                string servername = ConfigurationManager.AppSettings["ServerName"];
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--OrderNumber-->", orderId);
                mailbody = mailbody.Replace(@"<!--MobileTopupNumber-->", mobileTopupNumber);
                mailbody = mailbody.Replace(@"<!--Creditcard-->", creditcard);
                mailbody = mailbody.Replace(@"<!--TopupAmount-->", topupamount);
                mailbody = mailbody.Replace(@"<!--PaymentMethod-->", "Credit Card");
                mailbody = mailbody.Replace(@"<!--Amountwithtax-->", topupamount);
                mailbody = mailbody.Replace(@"<!--Taxamount-->", taxamount);
                mailbody = mailbody.Replace(@"<!--CurrencyCode-->", currencycode);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);

                mailbody = mailbody.Replace(@"<!--EmailId-->", email);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    "Your Mobile Topup is successful.",
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteErrorForMobile("Error in sending Mobile topup mail: " + ex.Message);
            }

        }

        #endregion

        public RechargeStatusModel DoProcessFreeTrial(PayPalCheckoutModel paypalCheckOutModel, BillingInfo billingInfo)
        {
            return IssueNewPlan(paypalCheckOutModel, billingInfo);
        }

        public RechargeStatusModel DoProcessTransaction(PayPalCheckoutModel paypalCheckOutModel, BillingInfo billingInfo)
        {
            RechargeStatusModel statusModel = new RechargeStatusModel();
            switch (paypalCheckOutModel.ProcessPaymentInfo.TransactionType)
            {
                case "Recharge":
                    RazaLogger.WriteInfoPartial("Calling Recharge Pin");
                    statusModel = RechargePin(paypalCheckOutModel, billingInfo);
                    break;

                case "PurchaseNewPlan":
                    RazaLogger.WriteInfo("Calling purchase Pin");
                    statusModel = IssueNewPlan(paypalCheckOutModel, billingInfo);
                    break;

                case "CheckOut":
                    RazaLogger.WriteInfoPartial("CustID: " + billingInfo.MemberId + " Calling Issue NewPin");
                    statusModel = IssueNewPlan(paypalCheckOutModel, billingInfo);
                    break;

                case "TopUp":
                    statusModel = MobileTopUpRecharge(paypalCheckOutModel, billingInfo);
                    break;
            }

            RazaLogger.WriteErrorForMobile("****************************************************************");
            RazaLogger.WriteErrorForMobile(
                string.Format(
                    "Process Transaction in mobile Site, OrderId={0}, Memberid={1}, TransactionType={2}, Status={3}",
                    paypalCheckOutModel.OrderId, billingInfo.MemberId,
                    paypalCheckOutModel.ProcessPaymentInfo.TransactionType, statusModel.Status));

            RazaLogger.WriteErrorForMobile("****************************************************************");

            return statusModel;
        }


        /// <summary>
        /// Save Pending Order when authorization complete.
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <param name="billingInfo"></param>
        public bool SaveNewPendingOrder(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            RechargeInfo infomodel = new RechargeInfo()
            {
                MemberId = billingInfo.MemberId,
                OrderId = payPalCheckoutModel.OrderId,
                CardId = payPalCheckoutModel.ProcessPlanInfo.CardId,
                Amount = payPalCheckoutModel.ProcessPlanInfo.Amount,
                CountryFrom = payPalCheckoutModel.ProcessPlanInfo.CountryFrom,
                CountryTo = payPalCheckoutModel.ProcessPlanInfo.CountryTo,
                CoupanCode = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CouponCode,
                PaymentMethod = payPalCheckoutModel.ProcessPaymentInfo.PaymentType,
                CardNumber = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CardNumber) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CardNumber,
                ExpiryDate = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.ExpDate2) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.ExpDate2,
                CVV2 = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.Cvv) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.Cvv,
                FirstName = billingInfo.FirstName,
                LastName = billingInfo.LastName,
                EmailAddress = billingInfo.Email,
                AniNumber = billingInfo.PhoneNumber,
                Address1 = billingInfo.Address,
                Address2 = string.Empty,
                City = billingInfo.City,
                State = billingInfo.State,
                ZipCode = billingInfo.ZipCode,
                Country = billingInfo.Country,
                AuthTransactionId = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CcAuthTransactionId) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CcAuthTransactionId,
                CentinelPayLoad = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAutorizationResponse.CentinelPayload) ? string.Empty : payPalCheckoutModel.CentinelAutorizationResponse.CentinelPayload,
                PayResPayLoad = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAutorizationResponse.PayResPayLoad) ? string.Empty : payPalCheckoutModel.CentinelAutorizationResponse.PayResPayLoad,
                CentinelTransactionId = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId,
                ProcessedBy = string.Empty,
                IpAddress = payPalCheckoutModel.ProcessPaymentInfo.IpAddress,
                PayerId = payPalCheckoutModel.CentinelAuthenticationResponse.PayerId ?? string.Empty,

            };

            if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
            {
                infomodel.PaymentMethod = "Credit Card";
            }
            else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
            {
                infomodel.PaymentMethod = "PayPal";
            }

            return _dataRepositry.SaveNewPendingOrder(infomodel);

        }

        /// <summary>
        /// Recharge existing user plan
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <param name="billingInfo"></param>
        RechargeStatusModel RechargePin(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            var country = ControllerHelper.GetUserCountryId(billingInfo.Country);

            RazaLogger.WriteErrorForMobile(string.Format("Got Pin {0} and new orderid is:{1}", payPalCheckoutModel.ProcessPlanInfo.Pin, payPalCheckoutModel.ProcessPlanInfo.OrderId));

            try
            {
                RechargeInfo rechargeInfo = new RechargeInfo
                {
                    OrderId = payPalCheckoutModel.OrderId,
                    MemberId = billingInfo.MemberId,
                    IpAddress = payPalCheckoutModel.ProcessPaymentInfo.IpAddress,
                    Pin = payPalCheckoutModel.ProcessPlanInfo.Pin,
                    Amount = payPalCheckoutModel.ProcessPlanInfo.Amount,
                    ServiceFee = payPalCheckoutModel.ProcessPlanInfo.ServiceFee,
                    CardNumber = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CardNumber) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CardNumber,
                    City = string.IsNullOrEmpty(billingInfo.City) ? string.Empty : billingInfo.City,
                    State = string.IsNullOrEmpty(billingInfo.State) ? string.Empty : billingInfo.State,
                    CVV2 = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.Cvv) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.Cvv,
                    Address1 = string.IsNullOrEmpty(billingInfo.Address) ? string.Empty : billingInfo.Address,
                    Address2 = "Mobile Site Recharge",
                    ZipCode = string.IsNullOrEmpty(billingInfo.ZipCode) ? string.Empty : billingInfo.ZipCode,
                    Country = country.Id,
                    ExpiryDate = payPalCheckoutModel.ProcessPaymentInfo.ExpDate2,
                    IsCcProcess = payPalCheckoutModel.PaymentProcessGuide.DoCcProcess,
                    EciFlag = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag,
                    CVV2Response = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse,
                    PaymentTransactionId = payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId,
                    Xid = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid,
                    Cavv = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv,
                    AVSResponse = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse,
                    UserType = "Old",
                    CoupanCode = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CouponCode,
                    PayerId = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.PayerId) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.PayerId
                };

                //RazaLogger.WriteErrorForMobile("object is created");
                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    rechargeInfo.PaymentMethod = "PayPal";
                }
                else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }

                if (payPalCheckoutModel.ProcessPlanInfo.IsAutoRefill)
                {
                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = payPalCheckoutModel.ProcessPlanInfo.AutoRefillAmount;
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }

                RazaLogger.WriteErrorForMobile(
   string.Format(
       "Calling Mobile Site Recharge With params {0},{1},{2},{3},{4}", rechargeInfo.MemberId, rechargeInfo.OrderId, rechargeInfo.Pin, rechargeInfo.PaymentMethod, rechargeInfo.Amount));

                var rechstatus = _dataRepositry.Recharge_Pin(rechargeInfo);

                if (rechstatus.Rechargestatus == "1")
                {
                    RazaLogger.WriteErrorForMobile("************Recharge Success.**********");
                    //set passcode but no need of functionality in mobile
                    //if (recharge.IsPasscodeDial == "T" && recharge.PassCodePin != null)
                    //{
                    //    var res = repository.SetPassCode(recharge.Pin, recharge.PassCodePin);

                    //}

                    //send mail to the user..
                    SendRechargeConfirmMail(payPalCheckoutModel, billingInfo);
                    return new RechargeStatusModel()
                    {
                        Status = true
                    };
                }
                else
                {
                    payPalCheckoutModel.FinalTransactionStatus = false;
                    payPalCheckoutModel.Message = new ViewMessage()
                    {
                        Message = string.Format("Transaction is not successfull. {0}", rechstatus.RechargeError),
                        MessageType = MessageType.Error,
                    };


                }
            }
            catch (Exception ex)
            {
                payPalCheckoutModel.FinalTransactionStatus = false;
                RazaLogger.WriteErrorForMobile(ex.StackTrace);
                payPalCheckoutModel.Message = new ViewMessage()
                {
                    Message = string.Format("Transaction is not successfull. {0}", ex.StackTrace),
                    MessageType = MessageType.Error,
                };
            }
            return new RechargeStatusModel()
            {
                Status = false
            };
        }

        /// <summary>
        /// Issue a new pin
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <param name="billingInfo"></param>
        RechargeStatusModel IssueNewPlan(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            var repository = new DataRepository();
            var country = ControllerHelper.GetUserCountryId(billingInfo.Country);

            try
            {
                var rechargeInfo = new RechargeInfo
                {
                    OrderId = payPalCheckoutModel.OrderId,
                    MemberId = billingInfo.MemberId,
                    Address1 = string.IsNullOrEmpty(billingInfo.Address) ? string.Empty : billingInfo.Address,
                    Country = country.Id,
                    ZipCode = string.IsNullOrEmpty(billingInfo.ZipCode) ? string.Empty : billingInfo.ZipCode,
                    State = string.IsNullOrEmpty(billingInfo.State) ? string.Empty : billingInfo.State,
                    City = string.IsNullOrEmpty(billingInfo.City) ? string.Empty : billingInfo.City,
                    Amount = payPalCheckoutModel.ProcessPlanInfo.Amount,
                    ServiceFee = payPalCheckoutModel.ProcessPlanInfo.ServiceFee,
                    CardNumber = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CardNumber) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CardNumber,
                    CVV2 = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.Cvv) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.Cvv,
                    PaymentMethod = payPalCheckoutModel.ProcessPaymentInfo.PaymentType,
                    ExpiryDate = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.ExpDate2) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.ExpDate2,
                    UserType = billingInfo.UserType,
                    CardId = payPalCheckoutModel.ProcessPlanInfo.CardId,
                    CountryFrom = payPalCheckoutModel.ProcessPlanInfo.CountryFrom,
                    CountryTo = payPalCheckoutModel.ProcessPlanInfo.CountryTo,
                    CoupanCode = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CouponCode,
                    AniNumber = string.IsNullOrEmpty(billingInfo.PhoneNumber) ? string.Empty : billingInfo.PhoneNumber,
                    IpAddress = payPalCheckoutModel.ProcessPaymentInfo.IpAddress,
                    //Address2 = "",
                    Address2 = "Mobile Site NewPin",
                    IsCcProcess = payPalCheckoutModel.PaymentProcessGuide.DoCcProcess,
                    EciFlag = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag,
                    CVV2Response = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse,
                    PaymentTransactionId = payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId,
                    Xid = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid,
                    Cavv = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv,
                    AVSResponse = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse,
                    PaymentType = payPalCheckoutModel.ProcessPaymentInfo.PaymentType,
                    PayerId = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.PayerId) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.PayerId
                };

                if (rechargeInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }

                if (payPalCheckoutModel.ProcessPlanInfo.IsTryUsFree)
                {
                    rechargeInfo.PaymentMethod = "Free Trial";
                }
                if (payPalCheckoutModel.ProcessPlanInfo.IsAutoRefill)
                {

                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = payPalCheckoutModel.ProcessPlanInfo.AutoRefillAmount;
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }

                #region Functionality to added pinless numbers (no need in mobile case)
                if (payPalCheckoutModel.ProcessPlanInfo.PinlessNumbers != null && payPalCheckoutModel.ProcessPlanInfo.PinlessNumbers.Any())
                {
                    RazaLogger.WriteErrorForMobile("add all pinless numbers");
                    int count = 1;
                    foreach (var item in payPalCheckoutModel.ProcessPlanInfo.PinlessNumbers)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (count == 1)
                            {
                                rechargeInfo.AniNumber = item;
                                count++;
                            }
                            else
                            {
                                rechargeInfo.AniNumber += "*" + item;
                            }
                        }

                    }
                }

                #endregion

                var response = repository.IssueNewPin(rechargeInfo);
                if (response.IssuenewpinStatus)
                {
                    payPalCheckoutModel.ProcessPlanInfo.Pin = response.NewPin;

                    //no need to set passcode in mobile case
                    //RazaLogger.WriteInfo("passcode parameter :" + model.RechargeValues.IsPasscodeDial + "  " +
                    //                     model.RechargeValues.PassCodePin + "");
                    //if (model.RechargeValues.IsPasscodeDial == "T" && model.RechargeValues.PassCodePin != null)
                    //{

                    //    var res = repository.SetPassCode(response.NewPin, model.RechargeValues.PassCodePin);
                    //    RazaLogger.WriteInfo("setpass code status is:" + res);
                    //    if (res)
                    //    {

                    //    }
                    //}

                    //Call Issnew pin mail
                    SendCallingCardPurchaseMail(payPalCheckoutModel, billingInfo);
                    return new RechargeStatusModel()
                    {
                        Status = true
                    };
                }

            }
            catch (Exception ex)
            {
                RazaLogger.WriteErrorForMobile("Issue New Pin is not successfull due to " + ex.Message + ex.StackTrace);
                //model.FinalResponseMessage = "Transaction is not successfull due to " + ex.Message + ex.StackTrace;
                payPalCheckoutModel.Message = new ViewMessage
                {
                    Message = "Transaction is complete. Please call our Customer Service to receive your Order Detail.",
                    MessageType = MessageType.Error
                };
                payPalCheckoutModel.FinalTransactionStatus = false;
            }
            return new RechargeStatusModel()
            {
                Status = false
            };
        }

        /// <summary>
        /// Call for top mobile.
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <param name="billingInfo"></param>
        /// <returns></returns>
        RechargeStatusModel MobileTopUpRecharge(PayPalCheckoutModel payPalCheckoutModel, BillingInfo billingInfo)
        {
            try
            {
                var country = ControllerHelper.GetUserCountryId(billingInfo.Country);
                string destphonenumber = string.Empty;
                if (payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopUpCountryCode.Contains("-"))
                {
                    destphonenumber = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopUpCountryCode.Replace("-", "") +
                                      payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupMobileNumber;
                }
                else
                {
                    destphonenumber = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopUpCountryCode + payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupMobileNumber;
                }

                var topuprech = new Mobiletopup()
                {
                    NewOrderId = payPalCheckoutModel.OrderId,
                    MemberId = billingInfo.MemberId,
                    OperatorCode = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupOperatorCode,
                    CountryId = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupCountry,
                    SourceAmount = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupSourceAmount,
                    DestinationAmt = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupDestAmount,
                    SmsTo = string.Empty,
                    Pin = string.Empty,
                    PurchaseAmount = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupSourceAmount,
                    CardNumber = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CardNumber) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CardNumber,
                    ExpDate = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.ExpDate2) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.ExpDate2,
                    Cvv2 = (string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.Cvv)) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.Cvv.ToString(),
                    Address1 = billingInfo.Address,
                    Address2 = string.Empty,
                    City = billingInfo.City,
                    State = billingInfo.State,
                    ZipCode = billingInfo.ZipCode,
                    Country = country.Id,
                    IpAddress = payPalCheckoutModel.ProcessPaymentInfo.IpAddress,
                    DestinationPhoneNumber = destphonenumber,
                    Operator = payPalCheckoutModel.ProcessPlanInfo.MobileTopupInfo.TopupOperatorName,
                    CouponCode = string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode) ? string.Empty : payPalCheckoutModel.ProcessPaymentInfo.CouponCode,

                    //Isccprocess = true,
                    Isccprocess = payPalCheckoutModel.PaymentProcessGuide.DoCcProcess,

                    EciFlag = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelEciFlag,
                    Cvv2Response = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse,
                    PaymentTransactionId = payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId,
                    Xid = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelXid,
                    Cavv = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.CentinelCavv,
                    AvsResponse = string.IsNullOrEmpty(payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse) ? string.Empty : payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse,
                    PayerId = string.Empty

                };

                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    topuprech.PaymentMethod = "PayPal";
                }
                else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    topuprech.PaymentMethod = "Credit Card";
                }

                var statusmodel = new RechargeStatusModel();
                var res = _dataRepositry.MobileTopupRecharge(topuprech);
                if (res.Status)
                {
                    RazaLogger.WriteErrorForMobile("Mobile Top Success, Sending Confirmation mail to user");
                    //Send mail of success mobiletop
                    SendMobileTopUpMail(payPalCheckoutModel, billingInfo);
                }
                else
                {
                    RazaLogger.WriteErrorForMobile("Mobile topup failed");
                }
                // RazaLogger.WriteInfo(res.Status ? "seccessfully Mobiletopup recharge" : "Mobile top Recharge failed.");
                statusmodel.Status = res.Status;
                return statusmodel;
            }
            catch (Exception exception)
            {
                RazaLogger.WriteErrorForMobile("exceptopn occurred in mobiletop: " + exception.Message);
                RazaLogger.WriteErrorForMobile("Stack Trace" + exception.StackTrace);
                return new RechargeStatusModel();
            }

        }
    }
}