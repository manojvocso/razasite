using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardinalCommerce;
using MvcApplication1.AppHelper;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Payment.Models;
using MvcApplication1.Controllers;
using Raza.Common;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Repository;

namespace MvcApplication1.Areas.Payment.Controllers
{
    [RequiresSSL]
    public class PaypalController : BaseController
    {
        private readonly DataRepository _dataRepository;


        public PaypalController()
        {
            _dataRepository = new DataRepository();
        }


        //
        // GET: /Payment/Paypal/
        [HttpGet]
        [NonAction]
        public ActionResult Index()
        {
            return View();
        }

        #region Private Payment Helper methods

        private string GetRedirectBackByTransactionType(string transactionType, string pin = "")
        {
            if (transactionType == TransactionType.Recharge.ToString())
                return Url.Action("ReviewOrder", "Recharge", new { area = "Mobile", id = ControllerHelper.Encrypt(pin) });

            return String.Empty;
        }

        private void UpdateCheckoutSession(PayPalCheckoutModel payPalCheckoutModel)
        {
            Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
        }

        private string GetCurrencyCodeforPayment(string currency)
        {
            string currencyCode;

            switch (currency)
            {
                case "USD":
                    currencyCode = "840";
                    break;
                case "CAD":
                    currencyCode = "124";
                    break;
                case "GBP":
                    currencyCode = "826";
                    break;
                default:
                    currencyCode = "840";
                    break;
            }
            return currencyCode;

        }

        private string OneDigitPAymentType(string transactionType)
        {
            if (transactionType == PaymentSettings.PaymentType.PayPal.ToString())
            {
                return "P";
            }
            else if (transactionType == PaymentSettings.PaymentType.CreditCard.ToString())
            {
                return "C";
            }
            return String.Empty;
        }

        private bool IsBillingInfoUpdated()
        {
            #region Check for the billing info
            if ((UserContext.ProfileInfo.Country.ToUpper() == "U.K." || UserContext.ProfileInfo.Country == "3") && (string.IsNullOrEmpty(UserContext.ProfileInfo.FirstName) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.LastName) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.Address) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.City)
                || string.IsNullOrEmpty(UserContext.ProfileInfo.ZipCode) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.PhoneNumber)))
            {
                //todo: return to the billing info for completion and then continue transaction
                return false;
            }
            if ((UserContext.ProfileInfo.Country.ToUpper() == "U.S.A." || UserContext.ProfileInfo.Country.ToUpper() == "USA" || UserContext.ProfileInfo.Country.ToUpper() == "CANADA" ||
                UserContext.ProfileInfo.Country == "1" || UserContext.ProfileInfo.Country == "2") && (string.IsNullOrEmpty(UserContext.ProfileInfo.FirstName) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.LastName) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.Address) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.City)
                || string.IsNullOrEmpty(UserContext.ProfileInfo.ZipCode) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.State) ||
                string.IsNullOrEmpty(UserContext.ProfileInfo.PhoneNumber)))
            {

                //todo: return to the billing info for completion and then continue transaction
                return false;
            }
            return true;

            #endregion
        }

        private CommonStatus ValidateCardInfo(PayPalCheckoutModel paypalCheckOutModel)
        {

            try
            {
                if (string.IsNullOrEmpty(paypalCheckOutModel.ProcessPaymentInfo.CouponCode))
                {
                    paypalCheckOutModel.ProcessPaymentInfo.CouponCode = string.Empty;
                }


                string transmode = ControllerHelper.GetTransactionMode(paypalCheckOutModel.ProcessPaymentInfo.TransactionType);

                if (transmode == "T")
                    transmode = "R";

                if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
                {
                    //no need to validate redirect to the CcprocessValidationcheck
                    return new CommonStatus
                    {
                        Status = true,
                        Errormsg = string.Empty
                    };
                }

                string memberid = UserContext.MemberId;
                string cardNumber = paypalCheckOutModel.ProcessPaymentInfo.CardNumber ?? string.Empty;

                var res = _dataRepository.ValidateCustomer(memberid, cardNumber,
                    paypalCheckOutModel.ProcessPlanInfo.CardId, paypalCheckOutModel.ProcessPlanInfo.Amount, transmode,
                    paypalCheckOutModel.ProcessPlanInfo.CountryFrom, paypalCheckOutModel.ProcessPlanInfo.CountryTo,
                    paypalCheckOutModel.ProcessPaymentInfo.CouponCode);

                return res;


            }
            catch (Exception ex)
            {

                //error occurred log the error and return back with message...
                throw;
            }

        }

        private PaymentProcessGuide CheckCcProcessGuide(PayPalCheckoutModel paypalCheckOutModel)
        {

            var orderType = PaymentSettings.GetOrderType(paypalCheckOutModel.ProcessPaymentInfo.TransactionType);

            var ccProcessGuide = _dataRepository.CcProcessValidation(UserContext.MemberId, UserContext.UserType,
                orderType, paypalCheckOutModel.ProcessPaymentInfo.PaymentType,
                paypalCheckOutModel.ProcessPaymentInfo.CardNumber ?? string.Empty,
                SafeConvert.ToString(paypalCheckOutModel.ProcessPlanInfo.CountryFrom),
                paypalCheckOutModel.ProcessPlanInfo.Amount);

            return new PaymentProcessGuide()
            {
                AcceptOrder = ccProcessGuide.AcceptOrder,
                AvsByPass = ccProcessGuide.AvsByPass,
                CentinelByPass = ccProcessGuide.CentinelBypass,
                DoCcProcess = ccProcessGuide.DoCcProcess,
                IsValidPlan = ccProcessGuide.IsValidPlan,
                Message = ccProcessGuide.StatusMsg
            };

        }

        private CommonStatus CardLookUpAuthorization(PayPalCheckoutModel payPalCheckoutModel)
        {
            RazaLogger.WriteErrorForMobile("Entered in CardLookUpAuthorization()");

            if (string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode))
                payPalCheckoutModel.ProcessPaymentInfo.CouponCode = string.Empty;

            string currencyCode = GetCurrencyCodeforPayment(payPalCheckoutModel.ProcessPlanInfo.CurrencyCode);

            #region Register Pinless number not using in mobile case so comment this
            //if (Session["RegPinlessNumber"] != null)
            //{
            //    RazaLogger.WriteInfo("Adding number to Model list");
            //    recharge.PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
            //    Session["RegPinlessNumber"] = null;
            //}
            #endregion

            string transmode =
                ControllerHelper.GetTransactionMode(payPalCheckoutModel.ProcessPaymentInfo.TransactionType);

            payPalCheckoutModel.OrderId = ControllerHelper.CreateOrderId(payPalCheckoutModel.ProcessPaymentInfo.TransactionType);


            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_lookup");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);

            double amount;
            if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
            {
                ccRequest.add("TransactionType", "X"); // Express Checkout
                amount = payPalCheckoutModel.ProcessPlanInfo.Amount * 100;
            }
            else if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
            {
                ccRequest.add("TransactionType",
                    OneDigitPAymentType(payPalCheckoutModel.ProcessPaymentInfo.PaymentType));
                ccRequest.add("CardNumber", payPalCheckoutModel.ProcessPaymentInfo.CardNumber);
                ccRequest.add("CardExpMonth", payPalCheckoutModel.ProcessPaymentInfo.ExpMonth);
                ccRequest.add("CardExpYear", payPalCheckoutModel.ProcessPaymentInfo.ExpYear);

                if (payPalCheckoutModel.ProcessPaymentInfo.CardNumber.StartsWith("3"))
                {
                    amount = (payPalCheckoutModel.ProcessPlanInfo.Amount + 0.5) * 100;
                }
                else
                {
                    amount = payPalCheckoutModel.ProcessPlanInfo.Amount * 100;
                }

            }
            else
            {
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = "Invalid Transactions",
                };
            }

            ccRequest.add("OrderNumber", payPalCheckoutModel.OrderId);
            ccRequest.add("OrderDescription", "This is where you put the order description");
            ccRequest.add("Amount", amount.ToString(CultureInfo.InvariantCulture));
            ccRequest.add("CurrencyCode", currencyCode);
            ccRequest.add("UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]);
            ccRequest.add("BrowserHeader", Request.ServerVariables["HTTP_ACCEPT"]);
            ccRequest.add("IPAddress", ControllerHelper.GetIpAddressofUser(Request));

            if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString())
            {
                ccRequest.add("EMail", string.Empty);
                ccRequest.add("TransactionAction", "Authorization");
                ccRequest.add("NoShipping", "N");
                ccRequest.add("OverrideAddress", "N");
                ccRequest.add("ForceAddress", "N");
            }
            else
            {
                ccRequest.add("Installment", "");
                ccRequest.add("EMail", UserContext.Email);
                ccRequest.add("BillingFirstName", UserContext.FirstName);
                ccRequest.add("BillingLastName", UserContext.LastName);
                ccRequest.add("BillingAddress1", UserContext.ProfileInfo.Address);
                ccRequest.add("BillingAddress2", "");
                ccRequest.add("BillingCity", UserContext.ProfileInfo.City);
                ccRequest.add("BillingState", UserContext.ProfileInfo.State);
                ccRequest.add("BillingCountryCode", UserContext.ProfileInfo.Country);
                ccRequest.add("BillingPostalCode", UserContext.ProfileInfo.ZipCode);

                ccRequest.add("ShippingFirstName", UserContext.FirstName);
                ccRequest.add("ShippingLastName", UserContext.LastName);
                ccRequest.add("ShippingAddress1", UserContext.ProfileInfo.Address);

                ccRequest.add("ShippingAddress2", "");
                ccRequest.add("ShippingCity", UserContext.ProfileInfo.City);
                ccRequest.add("ShippingState", UserContext.ProfileInfo.State);
                ccRequest.add("ShippingCountryCode", UserContext.ProfileInfo.Country);
                ccRequest.add("ShippingPostalCode", UserContext.ProfileInfo.ZipCode);

            }


            CentinelResponse ccResponse;
            try
            {
                ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            }
            catch (Exception ex)
            {
                RazaLogger.WriteErrorForMobile(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));
                throw;
            }

            var errorNo = ccResponse.getValue("ErrorNo");
            var errorDesc = ccResponse.getValue("ErrorDesc");
            var enrolled = ccResponse.getValue("Enrolled");
            var payload = ccResponse.getValue("Payload");
            var acsurl = ccResponse.getValue("ACSUrl");
            var transactionId = ccResponse.getValue("TransactionId");

            RazaLogger.WriteErrorForMobile("Transaction Id " + transactionId);
            string message = string.Empty;
            bool status = false;

            if (errorNo == "0" || errorNo == "")
            {
                switch (enrolled)
                {
                    case "Y":
                        message = "Cardholder authentication is available.";
                        status = true;
                        break;
                    case "N":
                        message = "Cardholder not enrolled in authentication program.";
                        break;
                    case "U":
                        message = "Cardholder authentication is unavailable.";
                        break;
                }

                string centinelRetUrl = ConfigurationManager.AppSettings["Centinel_RetUrl"];

                payPalCheckoutModel.CentinelAutorizationResponse = new CentinelAutorizationResponse()
                {
                    CentinelPayload = payload,
                    CentinelTermUrl = centinelRetUrl,
                    CentinelAcsurl = acsurl,
                    CentinelTransactionId = transactionId,
                    CentinelTransactionType = OneDigitPAymentType(payPalCheckoutModel.ProcessPaymentInfo.PaymentType),
                    IsCcProcess = true
                };

                RazaLogger.WriteErrorForMobile(
                    string.Format("Response of CardLookUpAuthorization() is , errorNo: {0}, errorDesc:{1}, message:{2}", errorNo, errorDesc, message));

                //Update Checkout session
                UpdateCheckoutSession(payPalCheckoutModel);
                return new CommonStatus()
                {
                    Status = status,
                    Errormsg = message
                };
            }
            else
            {
                switch (enrolled)
                {
                    case "Y":
                        message =
                            string.Format(
                            "Cardholder authentication is available. However cannot be processed due to server error : Error No: {0} Error Desc: {1}",
                                errorNo, errorDesc);
                        break;
                    case "N":
                        message = string.Format("Card holder not enrolled in authentication program. Error No {0} Desc: {1}", errorNo, errorDesc);
                        break;
                    case "U":
                        message = string.Format("Card holder authentication is unavailable. Error No {0} Desc: {1}", errorNo, errorDesc);
                        break;
                    default:
                        message = errorDesc;
                        break;
                }

                RazaLogger.WriteErrorForMobile(
                    string.Format("Response of CardLookUpAuthorization() is , errorNo: {0}, errorDesc:{1}, message:{2}", errorNo, errorDesc, message));

                return new CommonStatus()
                {
                    Status = status,
                    Errormsg = message
                };
            }
        }

        private CentinelResponse SendCmpiAuthorize(PayPalCheckoutModel payPalCheckoutModel, CentinelResponse ccResponse)
        {
            string currencyCode = GetCurrencyCodeforPayment(payPalCheckoutModel.ProcessPlanInfo.CurrencyCode);

            CentinelRequest ccRequest;
            ccRequest = new CentinelRequest();

            RazaLogger.WriteErrorForMobile("CurrencyCode is :" + currencyCode);
            RazaLogger.WriteErrorForMobile("Txn Id  is :" + payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_authorize");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("TransactionId", payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);

            RazaLogger.WriteErrorForMobile("Txn Id " + payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);
            ccRequest.add("TransactionType", "X");
            //ccRequest.add("TransactionMode", "S");
            //ccRequest.add("CategoryCode", "5400");
            //ccRequest.add("CustomerFlag", "N");
            //ccRequest.add("CustomerRegistrationDate", date);

            ccRequest.add("OrderNumber", payPalCheckoutModel.OrderId);

            RazaLogger.WriteErrorForMobile("Order Number " + payPalCheckoutModel.OrderId);

            ccRequest.add("OrderDescription", "Raza.com Online order.");

            RazaLogger.WriteInfo("TotalAmount with service fee " + payPalCheckoutModel.ProcessPlanInfo.TotalAmount);
            double tmpamount = (payPalCheckoutModel.ProcessPlanInfo.TotalAmount) * 100;
            ccRequest.add("Amount", tmpamount.ToString());

            ccRequest.add("CurrencyCode", currencyCode);

            RazaLogger.WriteInfo("Email " + UserContext.Email);
            ccRequest.add("Email", UserContext.Email);
            ccRequest.add("IPAddress", payPalCheckoutModel.ProcessPaymentInfo.IpAddress);
            ccRequest.add("BillingFirstName", UserContext.FirstName);
            ccRequest.add("BillingLastName", UserContext.LastName);
            ccRequest.add("BillingAddress1", UserContext.ProfileInfo.Address);
            ccRequest.add("BillingAddress2", "");
            ccRequest.add("BillingCity", UserContext.ProfileInfo.City);
            ccRequest.add("BillingState", UserContext.ProfileInfo.State);
            ccRequest.add("BillingCountryCode", UserContext.ProfileInfo.Country);
            ccRequest.add("BillingPostalCode", UserContext.ProfileInfo.ZipCode);
            //ccRequest.add("BillingPhone", UserContext.ProfileInfo.PhoneNumber);
            ccRequest.add("ShippingFirstName", UserContext.FirstName);
            ccRequest.add("ShippingLastName", UserContext.LastName);
            ccRequest.add("ShippingAddress1", UserContext.ProfileInfo.Address);
            ccRequest.add("ShippingAddress2", "");
            ccRequest.add("ShippingCity", UserContext.ProfileInfo.City);
            ccRequest.add("ShippingState", UserContext.ProfileInfo.State);
            ccRequest.add("ShippingCountryCode", UserContext.ProfileInfo.Country);
            ccRequest.add("ShippingPostalCode", UserContext.ProfileInfo.ZipCode);
            //ccRequest.add("ShippingAmount", tmpamount.ToString());

            RazaLogger.WriteInfo("Sending CMPI_Authorization");
            try
            {
                ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo(ex.Message);
            }
            return ccResponse;
        }

        private CentinelResponse SendCmpiSaleMessage(PayPalCheckoutModel payPalCheckoutModel)
        {
            RazaLogger.WriteErrorForMobile("start in SendCmpiSaleMessage()");

            string currencyCode = GetCurrencyCodeforPayment(payPalCheckoutModel.ProcessPlanInfo.CurrencyCode);
            RazaLogger.WriteErrorForMobile("CurrencyCode is :" + currencyCode);
            RazaLogger.WriteErrorForMobile("CentinelAuthenticationResponse is :" + payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);

            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_sale");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("TransactionId", payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);

            RazaLogger.WriteErrorForMobile("Txn Id " + payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId);
            ccRequest.add("TransactionType", "X");

            ccRequest.add("OrderNumber", payPalCheckoutModel.OrderId);

            RazaLogger.WriteErrorForMobile("Order Number " + payPalCheckoutModel.OrderId);

            ccRequest.add("OrderDescription", "Raza.com Online order.");

            RazaLogger.WriteErrorForMobile("totalAmount with servicefee: " + payPalCheckoutModel.ProcessPlanInfo.TotalAmount);
            double tmpamount = (payPalCheckoutModel.ProcessPlanInfo.TotalAmount) * 100;
            ccRequest.add("Amount", tmpamount.ToString());

            ccRequest.add("CurrencyCode", currencyCode);

            RazaLogger.WriteErrorForMobile("Email " + UserContext.Email);
            ccRequest.add("Email", UserContext.Email);
            ccRequest.add("IPAddress", payPalCheckoutModel.ProcessPaymentInfo.IpAddress);
            ccRequest.add("BillingFirstName", UserContext.FirstName);
            ccRequest.add("BillingLastName", UserContext.LastName);
            ccRequest.add("BillingAddress1", UserContext.ProfileInfo.Address);
            ccRequest.add("BillingAddress2", "");
            ccRequest.add("BillingCity", UserContext.ProfileInfo.City);
            ccRequest.add("BillingState", UserContext.ProfileInfo.State);
            ccRequest.add("BillingCountryCode", UserContext.ProfileInfo.Country);
            ccRequest.add("BillingPostalCode", UserContext.ProfileInfo.ZipCode);
            ccRequest.add("ShippingFirstName", UserContext.FirstName);
            ccRequest.add("ShippingLastName", UserContext.LastName);
            ccRequest.add("ShippingAddress1", UserContext.ProfileInfo.Address);
            ccRequest.add("ShippingAddress2", "");
            ccRequest.add("ShippingCity", UserContext.ProfileInfo.City);
            ccRequest.add("ShippingState", UserContext.ProfileInfo.State);
            ccRequest.add("ShippingCountryCode", UserContext.ProfileInfo.Country);
            ccRequest.add("ShippingPostalCode", UserContext.ProfileInfo.ZipCode);

            RazaLogger.WriteErrorForMobile("Sending CMPI_Sale message");


            //if (model.RechargeValues.TransactionType.ToLower() == "recharge")
            //{
            //    RazaLogger.WriteInfoPartial(string.Format("Calling CMPI_Sale_PayPal : {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}", model.Centinel_TransactionId, model.RechargeValues.order_id,
            //        model.RechargeValues.Amount, currencyCode, UserContext.Email, Request.ServerVariables["REMOTE_ADDR"], UserContext.FirstName, UserContext.LastName, UserContext.ProfileInfo.Address, "",
            //        UserContext.ProfileInfo.City, UserContext.ProfileInfo.State, UserContext.ProfileInfo.Country, UserContext.ProfileInfo.ZipCode, UserContext.FirstName, UserContext.LastName, UserContext.ProfileInfo.Address,
            //        "", UserContext.ProfileInfo.City, UserContext.ProfileInfo.State, UserContext.ProfileInfo.Country, UserContext.ProfileInfo.ZipCode));
            //}


            CentinelResponse ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            return ccResponse;
        }


        #endregion

        [HttpPost]
        [MobileAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPayment()
        {
            try
            {
                //return RedirectToAction("ValidateCardInfo");
                var paypalCheckOutModel = Session["PaypalCheckoutModel"] as PayPalCheckoutModel;
                if (paypalCheckOutModel == null)
                    return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });

                #region Check Billing info is updated or not

                if (!IsBillingInfoUpdated())
                {
                    return RedirectToAction("UpdateBillingInfo", "Cart",
                        new { returnUrl = Url.Action("ReviewOrder", "Cart") });
                }

                #endregion


                #region for Validate CardInfo

                var validateCardInfo = ValidateCardInfo(paypalCheckOutModel);
                if (!validateCardInfo.Status)
                {
                    //return back to the page with message
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = validateCardInfo.Errormsg,
                        MessageType = MessageType.Error
                    };
                    UpdateCheckoutSession(paypalCheckOutModel);
                    return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });
                }

                #endregion

                #region Check for CcProcess Validation

                paypalCheckOutModel.PaymentProcessGuide = CheckCcProcessGuide(paypalCheckOutModel);


                if (!paypalCheckOutModel.PaymentProcessGuide.AcceptOrder)
                {

                    paypalCheckOutModel.CentinelAuthenticationResponse = new CentinelAuthenticationResponse();
                    paypalCheckOutModel.CentinelAutorizationResponse = new CentinelAutorizationResponse();
                    paypalCheckOutModel.OrderId = ControllerHelper.CreateOrderId(paypalCheckOutModel.ProcessPaymentInfo.TransactionType);
                    var status = new PlanHandler().SaveNewPendingOrder(paypalCheckOutModel, UserContext.ProfileInfo);

                    if (status)
                    {
                        paypalCheckOutModel.ProcessPaymentInfo.TransactionType = TransactionType.SaveOrder.ToString();
                        UpdateCheckoutSession(paypalCheckOutModel);
                        return RedirectToAction("OrderConfirmation", "Cart", new { Area = "Mobile" });
                    }
                    // return with message that your order is not accepted
                    return RedirectToAction("OrderFailed", "Cart");
                }

                #endregion

                //Update Cart Session before process
                UpdateCheckoutSession(paypalCheckOutModel);

                //Do Process with centinel
                if (paypalCheckOutModel.PaymentProcessGuide.DoCcProcess && !paypalCheckOutModel.PaymentProcessGuide.CentinelByPass)
                {

                    var cardlookupAuthResponse = CardLookUpAuthorization(paypalCheckOutModel);
                    if (cardlookupAuthResponse.Status)
                    {
                        //redirect to Send Automatic redirect Authenticate page
                        return RedirectToAction("AuthenticateInfo", "Paypal");
                    }
                    //redirect back to the page with error message that authenticate info is not valid 
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "Your information is not valid, please try with valid information.",
                        MessageType = MessageType.Error
                    };
                    return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });

                }
                //Do Process with ByPass the centinel
                else if (paypalCheckOutModel.PaymentProcessGuide.DoCcProcess && paypalCheckOutModel.PaymentProcessGuide.CentinelByPass)
                {
                    return ProcessCreditCard(paypalCheckOutModel);
                }
                //Send All info to Api and Api will Process the CreditCard.
                else if (!paypalCheckOutModel.PaymentProcessGuide.DoCcProcess)
                {
                    return ProcessApi(paypalCheckOutModel);
                }
                else
                {
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "Sorry!! We can not process your order, Please check your information and retry again.",
                        MessageType = MessageType.Error
                    };
                    UpdateCheckoutSession(paypalCheckOutModel);
                    return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteErrorForMobile(string.Format("message:{0},  Inner Exception: {1}", ex.Message, ex.InnerException));
                throw;
            }

        }

        [MobileAuthorize]
        public ActionResult AuthenticateInfo()
        {
            var payPalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
            if (payPalCheckoutModel == null)
            {
                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
            }

            var authenticateInfoViewModel = new AuthenticateInfoViewModel()
            {
                CentinelAcsurl = payPalCheckoutModel.CentinelAutorizationResponse.CentinelAcsurl,
                CentinelTransactionId = payPalCheckoutModel.CentinelAutorizationResponse.CentinelTransactionId,
                CentinelPayload = payPalCheckoutModel.CentinelAutorizationResponse.CentinelPayload,
                CentinelTermUrl = payPalCheckoutModel.CentinelAutorizationResponse.CentinelTermUrl
            };

            return View(authenticateInfoViewModel);
        }

        [MobileAuthorize]
        public ActionResult PaymentVerifier()
        {

            RazaLogger.WriteErrorForMobile("Mobile: processing start In PaymentVerifier method.");
            //CreditCartHandler creditCartHandler = new CreditCartHandler();
            PlanHandler planHandler = new PlanHandler();
            PaymentHandler paymentHandler = new PaymentHandler();

            var paypalCheckOutModel = Session["PaypalCheckoutModel"] as PayPalCheckoutModel;
            if (paypalCheckOutModel == null)
                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });


            #region CentinelProcessing

            var pares = TempData["PaRes"] as string;
            //Request.Form["PaRes"]; 
            //var merchant_data = Request.Form["MD"];
            RazaLogger.WriteErrorForMobile("Pares Id is:" + pares);

            if (string.IsNullOrEmpty(pares))
            {
                RazaLogger.WriteErrorForMobile("pares id is null or empty");
                // No Response from server so redirect it to transaction failed..
                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
            }

            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_authenticate");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("PAResPayload", pares);
            ccRequest.add("TransactionId", paypalCheckOutModel.CentinelAutorizationResponse.CentinelTransactionId);

            RazaLogger.WriteErrorForMobile(string.Format("PaymentType {0}", paypalCheckOutModel.ProcessPaymentInfo.PaymentType));

            if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString() || paypalCheckOutModel.ProcessPaymentInfo.PaymentType == "X")
            {
                // Set Transaction Type 'X' for Paypal..
                ccRequest.add("TransactionType", "X"); // Express Checkout    

            }
            else
            {
                // Set Transaction Type 'C' for CreditCard..
                ccRequest.add("TransactionType", "C");
            }

            //'=====================================================================================
            //' Send the XML Msg to the MAPS Server
            //' SendHTTP will send the cmpi_authenticate message to the MAPS Server (requires fully qualified Url)
            //' The Response is the CentinelResponse Object
            //'=====================================================================================

            //'=====================================================================================
            //' Send the XML Msg to the MAPS Server
            //' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
            //' The Response is the CentinelResponse Object
            //'=====================================================================================
            CentinelResponse ccResponse;
            try
            {
                RazaLogger.WriteErrorForMobile("01. sending request to centinel........");
                ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            }
            catch (Exception ex)
            {
                RazaLogger.WriteErrorForMobile(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));

                // Redirect to Transaction Error Page
                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });

            }

            var errorNo = ccResponse.getValue("ErrorNo");
            var errorDesc = ccResponse.getValue("ErrorDesc");

            paypalCheckOutModel.CentinelAuthenticationResponse = new CentinelAuthenticationResponse()
            {
                CentinelPaResStatus = ccResponse.getValue("PAResStatus"),
                CentinelSignatureVerification = ccResponse.getValue("SignatureVerification"),
                CentinelEciFlag = ccResponse.getValue("EciFlag"),
                CentinelCavv = ccResponse.getValue("Cavv"),
                CentinelXid = ccResponse.getValue("Xid"),
                ConsumerName = ccResponse.getValue("ConsumerStatus"),
                AddressStatus = ccResponse.getValue("AddressStatus"),

            };

            if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString()) //Paypal
            {
                #region Centinel Process for PayPal

                RazaLogger.WriteErrorForMobile(string.Format("Your CentinelAuthenticateResponse: errorNo {0} errorDesc {1} CentinelPAResStatus {2}", errorNo, errorDesc, paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus));

                if (errorNo == "0" && (paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "Y" || paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "P"))
                {
                    RazaLogger.WriteErrorForMobile("Your authentication completed successfully.");

                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = true;
                }
                else if (errorNo == "0" && paypalCheckOutModel.CentinelAuthenticationResponse.CentinelSignatureVerification == "Y" &&
                         paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "D")
                {

                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "You cancelled your transaction for Data Reasons. Please update your information and select another form of payment.",
                        MessageType = MessageType.Error
                    };
                }
                else if (errorNo == "0" && paypalCheckOutModel.CentinelAuthenticationResponse.CentinelSignatureVerification == "Y" &&
                         paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "X")
                {
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "Your transaction was canceled prior to completion. Please provide another form of payment.",
                        MessageType = MessageType.Error
                    };
                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;

                }
                else
                {
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "PayPal express checkout payment was unable to complete. Please provide another form of payment to complete your transaction.",
                        MessageType = MessageType.Error
                    };
                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;

                }
                #endregion
            }
            else if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString()) //Credit Card
            {
                #region Centinel Process for Credit Card

                RazaLogger.WriteErrorForMobile(string.Format("Your CentinelAuthenticateResponse: errorNo {0} errorDesc {1} CentinelPAResStatus {2}", errorNo, errorDesc, paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus));

                if (paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "Y" || paypalCheckOutModel.CentinelAuthenticationResponse.CentinelPaResStatus == "A")
                {
                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = true;
                }
                else
                {
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        Message = "Your transaction could not complete. Please provide another form of payment.",
                        MessageType = MessageType.Error
                    };
                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                }
                #endregion
            }

            #endregion

            //RazaLogger.WriteInfo("status of ccresponse is : " + response.Status.ToString());

            if (paypalCheckOutModel.CentinelAuthenticationResponse.Status)
            {
                try
                {

                    #region Paypal Txn


                    if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.PayPal.ToString()) //Paypal
                    {
                        //ConsumerStatus, AddressStatus, ConsumerName, EMail, PayerId
                        RazaLogger.WriteErrorForMobile("Status is true for paypal so start processing for CmpiSale");

                        paypalCheckOutModel.CentinelAuthenticationResponse.ConsumerName = ccResponse.getValue("ConsumerName");
                        paypalCheckOutModel.CentinelAuthenticationResponse.ConsumerStatus = ccResponse.getValue("ConsumerStatus");
                        paypalCheckOutModel.CentinelAuthenticationResponse.AddressStatus = ccResponse.getValue("AddressStatus");
                        paypalCheckOutModel.CentinelAuthenticationResponse.EMail = ccResponse.getValue("EMail");
                        paypalCheckOutModel.CentinelAuthenticationResponse.PayerId = ccResponse.getValue("Payer");
                        var cent_transId = ccResponse.getValue("TransactionId");

                        #region only for checkout TransactionType

                        if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                        {
                            // check for payerId if already exists then return error message
                            // Write in Log table and send message that Payment is not successfull"

                            if (_dataRepository.IsPayerIdExist(paypalCheckOutModel.CentinelAuthenticationResponse.PayerId, paypalCheckOutModel.CentinelAuthenticationResponse.ConsumerStatus,
                                paypalCheckOutModel.CentinelAuthenticationResponse.AddressStatus, paypalCheckOutModel.ProcessPlanInfo.CountryFrom.ToString()))
                            {
                                RazaLogger.WriteInfoPartial(string.Format("CustID {0} PayerId {1} is already exist ", UserContext.MemberId, paypalCheckOutModel.CentinelAuthenticationResponse.PayerId));
                                ccResponse = SendCmpiAuthorize(paypalCheckOutModel, ccResponse);
                                errorNo = ccResponse.getValue("ErrorNo");
                                errorDesc = ccResponse.getValue("ErrorDesc");
                                var stcode = ccResponse.getValue("StatusCode");
                                RazaLogger.WriteErrorForMobile("Response of cmpiauthorization: errorno=" + errorNo + "statuscode=" + stcode + "errordesc=" + errorDesc);

                                RazaLogger.WriteErrorForMobile("CustID: " + UserContext.MemberId +
                                                            " Response of cmpiauthorization: errorno=" + errorNo +
                                                            "statuscode=" +
                                                            stcode + "errordesc=" + errorDesc);

                                if (errorNo == "0" && stcode == "Y")
                                {

                                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                                    //model.CentinelAuthenticateResponse = response;
                                    //Session["CartAuthenticateModel"] = model;
                                    paypalCheckOutModel.Message = new ViewMessage()
                                    {
                                        MessageType = MessageType.Error,
                                        Message = "Customer is not new"
                                    };

                                    paypalCheckOutModel.FinalTransactionStatus = false;
                                    if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                                    {
                                        paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = ccResponse.getValue("TransactionId");
                                        paypalCheckOutModel.CentinelAuthenticationResponse.CcAuthTransactionId = paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId;
                                        RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + " PayerId exist, calling saveNewPendingOrder");

                                        // Call Save New Pending Order 
                                        planHandler.SaveNewPendingOrder(paypalCheckOutModel, UserContext.ProfileInfo);
                                        return RedirectToAction("OrderConfirmation", "Cart", new { area = "Mobile" });
                                    }
                                }
                                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" }); //
                            }
                            else
                            {

                                if (paypalCheckOutModel.CentinelAuthenticationResponse.ConsumerStatus.ToLower() != "verified" || paypalCheckOutModel.CentinelAuthenticationResponse.AddressStatus.ToLower() != "confirmed"
                                    && (UserContext.ProfileInfo.Country == "U.S.A." || UserContext.ProfileInfo.Country.ToLower() == "canada"))
                                {
                                    ccResponse = SendCmpiAuthorize(paypalCheckOutModel, ccResponse);
                                    errorNo = ccResponse.getValue("ErrorNo");
                                    errorDesc = ccResponse.getValue("ErrorDesc");
                                    var stcode = ccResponse.getValue("StatusCode");
                                    if (errorNo == "0" && stcode == "Y")
                                    {
                                        paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                                        paypalCheckOutModel.Message = new ViewMessage()
                                        {
                                            MessageType = MessageType.Error,
                                            Message = "Customer is not new"
                                        };
                                        paypalCheckOutModel.FinalTransactionStatus = false;
                                        if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                                        {
                                            paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = ccResponse.getValue("TransactionId");
                                            paypalCheckOutModel.CentinelAuthenticationResponse.CcAuthTransactionId = paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId;

                                            RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + ",Country: " + UserContext.ProfileInfo.Country + " Customer Not Verified, calling saveNewPendingOrder");

                                            //Call Save new pending order
                                            planHandler.SaveNewPendingOrder(paypalCheckOutModel, UserContext.ProfileInfo);
                                            return RedirectToAction("OrderConfirmation", "Cart", new { area = "Mobile" });
                                        }
                                    }
                                    return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
                                }
                            }

                        }
                        #endregion

                        // Calling CmpiSale 
                        ccResponse = SendCmpiSaleMessage(paypalCheckOutModel);


                        errorNo = ccResponse.getValue("ErrorNo");

                        errorDesc = ccResponse.getValue("ErrorDesc");

                        paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = ccResponse.getValue("TransactionId");

                        var statusCode = ccResponse.getValue("StatusCode");

                        paypalCheckOutModel.CentinelAuthenticationResponse.AvsResponse = ccResponse.getValue("AVSResult");

                        RazaLogger.WriteErrorForMobile(
                            string.Format(
                                "CustID: {0}. Get response of CMPI_Sale message as error no: errorNo={1},errorDesc={2},StatusCode={3}, AVSResult={4}, PayPal_TransactionID={5}",
                                UserContext.MemberId, errorNo, errorDesc, statusCode, paypalCheckOutModel.CentinelAuthenticationResponse.AvsResponse, paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId));

                        paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;

                        if (errorNo == "0" && statusCode == "Y")
                        {
                            paypalCheckOutModel.Message = new ViewMessage
                            {
                                Message = "Congratulations! Your Order is Approved.",
                                MessageType = MessageType.Success
                            };
                            paypalCheckOutModel.CentinelAuthenticationResponse.Status = true;
                        }
                        else if (errorNo == "0" && statusCode == "N")
                        {
                            paypalCheckOutModel.Message = new ViewMessage
                            {
                                Message = "Transaction Declined",
                                MessageType = MessageType.Error
                            };
                            paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                        }
                        else if (errorNo == "0" && statusCode == "E")
                        {
                            paypalCheckOutModel.Message = new ViewMessage
                            {
                                Message = "Transaction resulted in Error",
                                MessageType = MessageType.Error
                            };
                            paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                        }
                        else
                        {
                            paypalCheckOutModel.Message = new ViewMessage
                            {
                                Message = errorDesc,
                                MessageType = MessageType.Error
                            };
                            paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                        }

                        paypalCheckOutModel.FinalTransactionStatus = paypalCheckOutModel.CentinelAuthenticationResponse.Status;

                        UpdateCheckoutSession(paypalCheckOutModel);
                        if (paypalCheckOutModel.CentinelAuthenticationResponse.Status)
                        {
                            var rechargeStatus = planHandler.DoProcessTransaction(paypalCheckOutModel, UserContext.ProfileInfo);
                            return RedirectToAction(rechargeStatus.Status ? "OrderConfirmation" : "OrderFailed", "Cart", new { area = "Mobile" });
                        }

                    }

                    #endregion

                    #region Credit Card Processing

                    if (paypalCheckOutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                    {
                        //model.CentinelAuthenticateResponse = response;
                        //Session["CartAuthenticateModel"] = model;
                        RazaLogger.WriteErrorForMobile(paypalCheckOutModel.ProcessPaymentInfo.TransactionType);
                        RazaLogger.WriteErrorForMobile("Credit Card Processing..");


                        // Payflow Pro implementations
                        //in case of tryus fee only authorize thae card.
                        RazaLogger.WriteErrorForMobile("Calling CreditCardAuthorize");


                        //Call creditCardAuthorize..
                        var result = paymentHandler.CreditCardAuthorize(paypalCheckOutModel, UserContext.ProfileInfo);

                        if (result.RESULT == "0" && !string.IsNullOrEmpty(result.IAVS) &&
                            ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") &&
                            (paypalCheckOutModel.PaymentProcessGuide.AvsByPass || (paypalCheckOutModel.PaymentProcessGuide.AvsByPass != true && result.AVSADDR == "Y" && result.AVSZIP == "Y"))))
                        {
                            #region PROCESS TRANSACTION AFTER SUCCESSFULL CREDIT CARD AUTHORIZATION
                            RazaLogger.WriteInfo2("Calling CreditCardPayment");
                            // for tryusfree txns
                            #region Tryus Free Section
                            RazaLogger.WriteInfo2("CardID is :" + paypalCheckOutModel.ProcessPlanInfo.CardId);
                            if (paypalCheckOutModel.ProcessPlanInfo.IsTryUsFree)
                            {
                                if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                                {
                                    RazaLogger.WriteInfo2("calling issue new pin for free trial plan");
                                    paypalCheckOutModel.CentinelAuthenticationResponse.AvsResponse = result.AVSADDR + result.AVSZIP;
                                    //model.Centinel_TransactionId = result.PNREF;
                                    paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = result.PNREF;
                                    paypalCheckOutModel.CentinelAuthenticationResponse.CvvResponse = result.CVV2MATCH;

                                    //call issue new pin for free trial
                                    var rechargeStatus = planHandler.DoProcessFreeTrial(paypalCheckOutModel, UserContext.ProfileInfo);
                                    //call creditcardVoid for void the transaction..
                                    var void_auth = paymentHandler.CreditCardVoid(paypalCheckOutModel, UserContext.ProfileInfo, result.PNREF);

                                    return RedirectToAction(rechargeStatus.Status ? "OrderConfirmation" : "OrderFailed", "Cart", new { area = "Mobile" });
                                }

                            }
                            #endregion

                            //Call credit capture for make payment
                            var cap_auth = paymentHandler.CreditCardCapture(paypalCheckOutModel, UserContext.ProfileInfo, result.PNREF);
                            // CVV response ="Y" OR "X"
                            // AVS ADDR =="Y" and AVSZIP ="Y"


                            if (cap_auth.RESULT == "0")
                            {
                                paypalCheckOutModel.CentinelAuthenticationResponse.Status = true;
                                paypalCheckOutModel.CentinelAuthenticationResponse.AvsResponse = result.AVSADDR + result.AVSZIP;
                                paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = cap_auth.PNREF;
                                paypalCheckOutModel.CentinelAuthenticationResponse.CvvResponse = result.CVV2MATCH;

                            }
                            else
                            {
                                //capture response not valid , so void the transaction
                                var void_auth = paymentHandler.CreditCardVoid(paypalCheckOutModel, UserContext.ProfileInfo, result.PNREF);

                                paypalCheckOutModel.Message = new ViewMessage
                                {
                                    MessageType = MessageType.Error,
                                    Message = "Payment authorization failed."
                                };
                            }
                            #endregion
                        }
                        else
                        {
                            RazaLogger.WriteErrorForMobile(string.Format("In CreditCard Else: {0},{1},{2},{3},{4}", result.RESULT,
                                result.IAVS,
                                result.CVV2MATCH, result.AVSADDR, result.AVSZIP));
                            #region Payment Info Not Matched

                            if ((result.RESULT == "0" || result.RESULT == "114") && !string.IsNullOrEmpty(result.IAVS) &&
                                ((result.CVV2MATCH != "Y" || result.CVV2MATCH != "X")) || (result.AVSADDR != "Y" || result.AVSZIP != "Y"))
                            {
                                if (result.CVV2MATCH == "N" && result.AVSADDR == "N" && result.AVSZIP == "N")
                                {
                                    paypalCheckOutModel.Message = new ViewMessage
                                    {
                                        Message = "Credit Card’s CVV doesn’t match.<br/>Your address or zip code/postal code do not match with your credit card billing address.",
                                        MessageType = MessageType.Error
                                    };
                                }
                                else if (result.CVV2MATCH.Trim() == "N")
                                {
                                    paypalCheckOutModel.Message = new ViewMessage
                                    {
                                        Message = "Credit Card’s CVV doesn’t match",
                                        MessageType = MessageType.Error
                                    };

                                }
                                else if (result.AVSADDR.Trim() == "N" || result.AVSZIP.Trim() == "N")
                                {
                                    paypalCheckOutModel.Message = new ViewMessage
                                    {
                                        Message = "Your address or zip code/postal code do not match with your credit card billing address.",
                                        MessageType = MessageType.Error
                                    };
                                }
                                UpdateCheckoutSession(paypalCheckOutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" }); // Redirect to review order page for resubmit.
                            }

                            #endregion

                            #region Address and Postal code not matched
                            if ((result.RESULT == "0" || result.RESULT == "114") && !string.IsNullOrEmpty(result.IAVS) &&
                                ((result.AVSADDR != "Y" || result.AVSZIP != "Y")))
                            {
                                paypalCheckOutModel.Message = new ViewMessage
                                {
                                    MessageType = MessageType.Error,
                                    Message = "Address or Zip Code / Postal Code does not match."
                                };
                                UpdateCheckoutSession(paypalCheckOutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" }); // Redirect to review order page for resubmit.
                            }

                            #endregion

                            if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                            {
                                paypalCheckOutModel.CentinelAuthenticationResponse.CentinelTransactionId = ccResponse.getValue("TransactionId");
                                paypalCheckOutModel.CentinelAuthenticationResponse.CcAuthTransactionId = result.PNREF;

                                // call save new pending order..
                                planHandler.SaveNewPendingOrder(paypalCheckOutModel, UserContext.ProfileInfo);
                                return RedirectToAction("Confirmation", "Cart", new { area = "Mobile" });
                            }

                            paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                            paypalCheckOutModel.FinalTransactionStatus = paypalCheckOutModel.CentinelAuthenticationResponse.Status;

                            //UpdateCheckoutSession(paypalCheckOutModel);

                            if (paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                            {

                                RazaLogger.WriteErrorForMobile("credit card authorization failed, calling saveNewPendingOrder");
                                planHandler.SaveNewPendingOrder(paypalCheckOutModel, UserContext.ProfileInfo);

                            }

                            // redirect to the order failed
                            return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });

                        }

                        if (paypalCheckOutModel.CentinelAuthenticationResponse.Status) //Successful messages
                        {
                            #region on Success transaction Call api methods.
                            var rechargeStatus = planHandler.DoProcessTransaction(paypalCheckOutModel, UserContext.ProfileInfo);
                            return RedirectToAction(rechargeStatus.Status ? "OrderConfirmation" : "OrderFailed", "Cart", new { area = "Mobile" });

                            #endregion
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    paypalCheckOutModel.CentinelAuthenticationResponse.Status = false;
                    paypalCheckOutModel.Message = new ViewMessage()
                    {
                        MessageType = MessageType.Error,
                        Message = "Internel error occurred when make payment."
                    };
                    RazaLogger.WriteErrorForMobile("Exception Message:" + ex.Message);
                    RazaLogger.WriteErrorForMobile("Inner Exception:" + ex.InnerException.Message);
                    RazaLogger.WriteErrorForMobile("Stack Trace:" + ex.InnerException.Message);
                }
            }


            RazaLogger.WriteInfo("Invalid Transaction in cc verifier..., status of ccresponse is" + paypalCheckOutModel.CentinelAuthenticationResponse.Status);
            Session[GlobalSetting.CheckOutSesionKey] = paypalCheckOutModel;
            paypalCheckOutModel.Message = new ViewMessage
            {
                Message = "Invalid Transaction, Please try Again",
                MessageType = MessageType.Error
            };
            paypalCheckOutModel.FinalTransactionStatus = false;

            return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
        }

        /// <summary>
        /// Process Credit card Directly with bypass the centinel process.
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <returns></returns>
        private ActionResult ProcessCreditCard(PayPalCheckoutModel payPalCheckoutModel)
        {

            if (payPalCheckoutModel == null)
                throw new AuthSessionExpiredException();

            PaymentHandler paymentHandler = new PaymentHandler();
            PlanHandler planHandler = new PlanHandler();

            RazaLogger.WriteErrorForMobile("Processing start in ProcessCreditCard");
            if (payPalCheckoutModel.ProcessPaymentInfo.CouponCode == null || payPalCheckoutModel.ProcessPaymentInfo.CouponCode.ToLower() == "null")
            {
                payPalCheckoutModel.ProcessPaymentInfo.CouponCode = string.Empty;
            }

            //if (Session["RegPinlessNumber"] != null)
            //{
            //    RazaLogger.WriteInfo("Adding number to list");
            //    recharge.PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
            //    Session["RegPinlessNumber"] = null;
            //}


            payPalCheckoutModel.OrderId = ControllerHelper.CreateOrderId(payPalCheckoutModel.ProcessPaymentInfo.TransactionType);

            try
            {

                #region Credit Card Processing

                if (payPalCheckoutModel.ProcessPaymentInfo.PaymentType == PaymentSettings.PaymentType.CreditCard.ToString())
                {
                    RazaLogger.WriteErrorForMobile("Credit Card Processing..");

                    // Payflow Pro implementations

                    //in case of tryus fee only authorize thae card.
                    RazaLogger.WriteErrorForMobile("Calling CreditCardAuthorize");
                    payPalCheckoutModel.CentinelAuthenticationResponse = new CentinelAuthenticationResponse();
                    var result = paymentHandler.CreditCardAuthorize(payPalCheckoutModel, UserContext.ProfileInfo);


                    //if (result.RESULT == "0" && !string.IsNullOrEmpty(result.IAVS) &&
                    //    ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") && (model.RechargeValues.AvsByPass == true || (model.RechargeValues.AvsByPass != true && result.AVSADDR == "Y" && result.AVSZIP == "Y"))))
                    if (result.RESULT == "0")
                    {
                        if ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") && (payPalCheckoutModel.PaymentProcessGuide.AvsByPass || (result.AVSADDR == "Y" && result.AVSZIP == "Y")))
                        {
                            #region TO PROCESS TRANSACTION AFTER SUCCESSFULL AUTHORIZATION

                            RazaLogger.WriteErrorForMobile("Calling CreditCardPayment");

                            // for tryusfree txns
                            if (payPalCheckoutModel.ProcessPlanInfo.IsTryUsFree)
                            {

                                if (payPalCheckoutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString())
                                {

                                    payPalCheckoutModel.CentinelAuthenticationResponse.Status = true;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse = result.AVSADDR + result.AVSZIP;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId = result.PNREF;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse = result.CVV2MATCH;

                                    RazaLogger.WriteInfo("calling issue new pin for free trial plan");

                                    //planHandler.IssueNewPlan(payPalCheckoutModel, UserContext.ProfileInfo);
                                    var void_auth = paymentHandler.CreditCardVoid(payPalCheckoutModel, UserContext.ProfileInfo, result.PNREF);
                                }
                                //returl = Url.Action("OrderConfirmation", "Cart");

                            }
                            else
                            {
                                //  var result = creditCartHandler.CreditCardPayment(model, UserContext);
                                var cap_auth = paymentHandler.CreditCardCapture(payPalCheckoutModel, UserContext.ProfileInfo, result.PNREF);
                                // CVV response ="Y" OR "X"
                                // AVS ADDR =="Y" and AVSZIP ="Y"
                                RazaLogger.WriteErrorForMobile(string.Format("response of credit card capture: cvv2match={0},AVSADDR={1},AVSZIP={2}", result.CVV2MATCH, result.AVSADDR, result.AVSZIP));
                                if (cap_auth.RESULT == "0")
                                {
                                    payPalCheckoutModel.CentinelAuthenticationResponse.Status = true;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.AvsResponse = result.AVSADDR + result.AVSZIP;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.CentinelTransactionId = cap_auth.PNREF;
                                    payPalCheckoutModel.CentinelAuthenticationResponse.CvvResponse = result.CVV2MATCH;

                                }
                                else
                                {
                                    var void_auth = paymentHandler.CreditCardVoid(payPalCheckoutModel, UserContext.ProfileInfo, result.PNREF);
                                    payPalCheckoutModel.Message = new ViewMessage
                                    {
                                        MessageType = MessageType.Error,
                                        Message = "Payment authorization failed."
                                    };
                                    payPalCheckoutModel.CentinelAuthenticationResponse.Status = false;
                                }
                            }
                            #endregion
                        }
                        else
                        {

                            #region TO DO AFTER SUCCESS FULL AUTHORIZATION BUT UNSUCCESSFULL CVV OR AVS RESPONSE

                            if (string.IsNullOrEmpty(result.IAVS))
                            {
                                RazaLogger.WriteErrorForMobile("No AVS Response Received");
                                payPalCheckoutModel.Message = new ViewMessage
                                {
                                    Message = "Invalid Credit card information. Please try with a valid card.",
                                    MessageType = MessageType.Error
                                };
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotCvv2Match = true;
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsAddrMatch = true;
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsZipMatch = true;
                                UpdateCheckoutSession(payPalCheckoutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });

                            }
                            else if (result.CVV2MATCH == "N" && (result.AVSADDR == "N" || result.AVSZIP == "N"))
                            {
                                RazaLogger.WriteErrorForMobile("Cvv and address doesn't match. redirect to RevieOrder");
                                payPalCheckoutModel.Message = new ViewMessage
                                {
                                    Message = "Credit Card’s CVV doesn’t match.<br/>Your address or zip code/postal code do not match with your credit card billing address.",
                                    MessageType = MessageType.Error
                                };
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotCvv2Match = true;
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsAddrMatch = true;
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsZipMatch = true;
                                UpdateCheckoutSession(payPalCheckoutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });
                            }
                            else if ((result.AVSADDR != null && result.AVSADDR.Trim() == "N") || (result.AVSZIP != null && result.AVSZIP.Trim() == "N"))
                            {
                                RazaLogger.WriteErrorForMobile("Adress and Zipcode/PostalCode  doesn't match. redirect to RevieOrder");
                                payPalCheckoutModel.Message = new ViewMessage
                                {
                                    Message = "Your address or zip code/postal code do not match with your credit card billing address.",
                                    MessageType = MessageType.Error
                                };
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsAddrMatch = true;
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotAvsZipMatch = true;
                                UpdateCheckoutSession(payPalCheckoutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });

                            }
                            else if (result.CVV2MATCH != null && result.CVV2MATCH.Trim() == "N")
                            {
                                RazaLogger.WriteErrorForMobile("Cvv doesn't match. redirect to RevieOrder");
                                payPalCheckoutModel.Message = new ViewMessage
                                {
                                    Message = "Credit Card’s CVV doesn’t match.",
                                    MessageType = MessageType.Error
                                };
                                payPalCheckoutModel.CentinelAuthenticationResponse.IsNotCvv2Match = true;
                                UpdateCheckoutSession(payPalCheckoutModel);
                                return RedirectToAction("ReviewOrder", "Cart", new { area = "Mobile" });

                            }
                            else
                            {
                                RazaLogger.WriteErrorForMobile("Invalid Credit card Information. Please try again.");
                                return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        Session["IsPaymentComplete"] = "N";
                        //THIS ELSE WAS USED ON 11/10/14 TO TRAP THE CREDIT CARD ERROR WHICH DOES NOT HAVE RESULT = 0
                        RazaLogger.WriteInfo("There is problem with your credit card. Please try again later");
                        //return Json(new { status = false, code = "U", returl = "" });
                        payPalCheckoutModel.CentinelAuthenticationResponse.Status = false;

                        return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });
                    }


                    if (payPalCheckoutModel.CentinelAuthenticationResponse.Status) //Successful messages
                    {
                        var rechargerStatus = planHandler.DoProcessTransaction(payPalCheckoutModel, UserContext.ProfileInfo);
                        UpdateCheckoutSession(payPalCheckoutModel);
                        return RedirectToAction(rechargerStatus.Status ? "OrderConfirmation" : "OrderFailed", "Cart", new { area = "Mobile" });
                    }

                    return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });


                }
                #endregion
            }
            catch (Exception ex)
            {
                payPalCheckoutModel.CentinelAuthenticationResponse.Status = false;
                payPalCheckoutModel.Message = new ViewMessage()
                {
                    MessageType = MessageType.Error,
                    Message = "Internel error occurred !!!.."
                };
                RazaLogger.WriteInfo(ex.Message);
            }



            RazaLogger.WriteInfo("Invalid Transaction in processcredit card..., status of ccresponse is" + payPalCheckoutModel.CentinelAuthenticationResponse.Status);
            UpdateCheckoutSession(payPalCheckoutModel);
            return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });

        }

        /// <summary>
        /// Process Api directly and Api will process the credit card. 
        /// </summary>
        /// <param name="payPalCheckoutModel"></param>
        /// <returns></returns>
        private ActionResult ProcessApi(PayPalCheckoutModel payPalCheckoutModel)
        {

            if (payPalCheckoutModel == null)
                throw new AuthSessionExpiredException();

            PlanHandler planHandler = new PlanHandler();

            RazaLogger.WriteErrorForMobile("Processing start in ProcessCreditCard");
            if (payPalCheckoutModel.ProcessPaymentInfo.CouponCode == null || payPalCheckoutModel.ProcessPaymentInfo.CouponCode.ToLower() == "null")
            {
                payPalCheckoutModel.ProcessPaymentInfo.CouponCode = string.Empty;
            }

            payPalCheckoutModel.OrderId = ControllerHelper.CreateOrderId(payPalCheckoutModel.ProcessPaymentInfo.TransactionType);

            try
            {

                // Set all data related to centinel response empty.
                payPalCheckoutModel.CentinelAuthenticationResponse = new CentinelAuthenticationResponse
                {
                    CentinelEciFlag = string.Empty,
                    CvvResponse = string.Empty,
                    CentinelTransactionId = string.Empty,
                    CentinelXid = string.Empty,
                    CentinelCavv = string.Empty,
                    AvsResponse = string.Empty
                };

                var rechargerStatus = planHandler.DoProcessTransaction(payPalCheckoutModel, UserContext.ProfileInfo);
                UpdateCheckoutSession(payPalCheckoutModel);
                return RedirectToAction(rechargerStatus.Status ? "OrderConfirmation" : "OrderFailed", "Cart", new { area = "Mobile" });

            }
            catch (Exception ex)
            {
                payPalCheckoutModel.CentinelAuthenticationResponse.Status = false;
                payPalCheckoutModel.Message = new ViewMessage()
                {
                    MessageType = MessageType.Error,
                    Message = "Internel error occurred !!!.."
                };
                RazaLogger.WriteInfo(ex.Message);
            }

            RazaLogger.WriteInfo("Invalid Transaction in Process Api...,");
            UpdateCheckoutSession(payPalCheckoutModel);
            return RedirectToAction("OrderFailed", "Cart", new { area = "Mobile" });

        }

    }
}
