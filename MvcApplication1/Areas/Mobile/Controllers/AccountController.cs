using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using DevTrends.MvcDonutCaching;
using MvcApplication1.AppHelper;
using MvcApplication1.Areas.Mobile.Models;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;
using System.Web;
using MvcApplication1.App_Start;
using Raza.Model.PaymentProcessModel;
using Raza.Common;

namespace MvcApplication1.Areas.Mobile.Controllers
{

    [CompressFilter]
    [HandleResourceNotFound]
    public class AccountController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public AccountController()
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


        #region Common Partial Actions

        [HttpGet]
        [MobileAuthorize]
        public ActionResult GetStates(int countryId)
        {
            var data = _dataRepository.GetStateList(countryId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLowestRate(string callingFrom, string callingTo)
        {
            if (string.IsNullOrWhiteSpace(callingFrom) || string.IsNullOrWhiteSpace(callingTo) || callingFrom == "undefined" || callingTo == "undefined")
            {
                return null;
            }
            var rates = new LowestRates();
            int countryfrom = SafeConvert.ToInt32(callingFrom);
            int countryto = SafeConvert.ToInt32(callingTo);
            if (countryto != 0)
            {

                var data = CacheManager.Instance.GetLowestRatesCache(countryfrom, countryto);

                var firstOrDefault = data.Rate.FirstOrDefault(a => a.RateFor == "LandLine");

                if (firstOrDefault != null)
                    rates.LandLineRate = SafeConvert.ToString(firstOrDefault.LowestRate);
                var getRateList = data.Rate.FirstOrDefault(a => a.RateFor == "Mobile");
                rates.MobileRate = SafeConvert.ToString(getRateList != null ? getRateList.LowestRate : 0);

                var sign = " ¢/min";
                rates.RateperSign = sign;

                if (countryfrom == 3)
                {
                    sign = " p/min";
                    rates.RateperSign = sign;
                }

                return Json(rates, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string sign = " ¢/min";
                rates.RateperSign = sign;
                return Json(rates, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        /// <summary>
        /// Home Page of The website for Mobile View. Donut Cached.
        /// </summary>
        /// <returns></returns>
        [UnRequiresSSL]
        [DonutOutputCache(Duration = 86400)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [UnRequiresSSL]
        public ActionResult ChangeHeaderCountry(string countryByIp)
        {
            string returnUrl = string.Empty;

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
            {
                if ((Request.UrlReferrer.LocalPath.ToLower() == "/rate/searchrate" || Request.UrlReferrer.LocalPath.ToLower() == "/mobile/rate/searchrate")
                    && Request.UrlReferrer.Query.Length > 0)
                {
                    var countryto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["countryto"];
                    returnUrl = Url.Action("SearchRate", "Rate", new { countryfrom = countryByIp, countryto = countryto });
                }
                else
                {
                    returnUrl = Request.UrlReferrer.ToString();
                }

            }

            Session["CountrybyIp"] = countryByIp;
            string heplNumber = "1-(877) 463-4233";
            if (countryByIp == "2")
            {
                heplNumber = "1-(800) 550-3501";
            }
            else if (countryByIp == "3")
            {
                heplNumber = "44-(800) 520-0329";
            }

            Session["HelpNumber"] = heplNumber;
            //if (!string.IsNullOrEmpty(returnUrl))
            //    return RedirectPermanent(returnUrl);

            //return RedirectToAction("Index", "Account");

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Account");

            if (Request.IsAjaxRequest())
                return Json(new { url = returnUrl });

            return RedirectPermanent(returnUrl);
        }

        #region Authentication Section

        [RequiresSSL]
        [HttpGet]
        public ActionResult LogOn(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("MyAccount", "Account");

            //if (!string.IsNullOrEmpty(returnUrl))
            //    RazaLogger.WriteErrorForMobile("Return Url is: " + returnUrl);

            //if (string.IsNullOrEmpty(returnUrl))
            //    returnUrl = TempData["ReturnUrl"] as string;

            //TempData["ReturnUrl"] = returnUrl;

            return View(new LogOnMobileViewModel() { ReturnUrl = returnUrl });
        }

        [RequiresSSL]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnMobileViewModel logOnModel)
        {
            // TempData["ReturnUrl"] = logOnModel.ReturnUrl ?? String.Empty;
            if (ModelState.IsValid)
            {
                var context = Authenticate(logOnModel.UserEmail, logOnModel.Password, logOnModel.RememberMe);

                if (context != null && !string.IsNullOrEmpty(logOnModel.ReturnUrl))
                {


                    //********************** ADDED BY SABAL ON 10/21/2015 *******************

                    var paypalCheckoutModel = Session[GlobalSetting.CheckOutSesionKey] as PayPalCheckoutModel;
                    if (paypalCheckoutModel != null)
                    {
                        //if ((paypalCheckoutModel.ProcessPlanInfo.CountryFrom != Convert.ToInt32(UserContext.ProfileInfo.Country)) && UserContext.ProfileInfo.Country == "2")
                        //if ((paypalCheckoutModel.ProcessPlanInfo.CountryFrom != Convert.ToInt32(context.ProfileInfo.Country)) && context.ProfileInfo. == "2")
                        if (paypalCheckoutModel.ProcessPlanInfo.CountryFrom == 1 && context.ProfileInfo.Country.ToUpper() == "CANADA")
                        {
                            paypalCheckoutModel.ProcessPlanInfo.CountryFrom = 2;
                            if (paypalCheckoutModel.ProcessPlanInfo.CardId == 175)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 176;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "CANADA MOBILE UNLIMITED $9.99";
                            }
                            else if (paypalCheckoutModel.ProcessPlanInfo.CardId == 177)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 178;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "CANADA UNLIMITED $14.99";
                            }
                            else if (paypalCheckoutModel.ProcessPlanInfo.CardId == 179)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 180;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "CANADA UNLIMITED $19.99";
                            }
                            else if (paypalCheckoutModel.ProcessPlanInfo.CardId == 163)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 164;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "Canada 1 Cent Plan";
                            }
                            else if (paypalCheckoutModel.ProcessPlanInfo.CardId == 121)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 126;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "Canada Talk Mobile";
                            }
                            else if (paypalCheckoutModel.ProcessPlanInfo.CardId == 120)
                            {
                                paypalCheckoutModel.ProcessPlanInfo.CardId = 125;
                                paypalCheckoutModel.ProcessPlanInfo.PlanName = "Canada Talk City";
                            }
                            //payPalCheckoutModel.ProcessPaymentInfo.TransactionType = TransactionType.PurchaseNewPlan.ToString();
                            Session[GlobalSetting.CheckOutSesionKey] = paypalCheckoutModel;
                        }
                    }
                    //**********************************************************************

                    return Redirect(Server.UrlDecode(logOnModel.ReturnUrl));

                    //if (logOnModel.ReturnUrl.ToLower().IndexOf("dussehra") > -1)
                    //    return Redirect("/Mobile/Promotion/DussehraExistCustomer?countryfrom=1&countryto=130");
                    //else
                    //    return Redirect(Server.UrlDecode(logOnModel.ReturnUrl));
                }
                else if (context != null)
                {
                    if (!string.IsNullOrEmpty(logOnModel.ReturnUrl))
                        return Redirect(Server.UrlDecode(logOnModel.ReturnUrl));

                    return RedirectToAction("MyAccount", "Account");
                }
                ModelState.Clear();
                logOnModel.Message = "Invalid email or password.";
                logOnModel.MessageType = MessageType.Error;
            }

            return View(logOnModel);
        }

        [RequiresSSL]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [RequiresSSL]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var password = _dataRepository.GetPassword(model.Email);
                string helplinenumber;
                if (Session["HelpNumber"] != null)
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }
                else
                {
                    helplinenumber = "1-(877) 463-4233";
                }
                string redirectlink = "Account/MyAccount";

                if (!string.IsNullOrWhiteSpace(password))
                {
                    try
                    {
                        string servername = ConfigurationManager.AppSettings["ServerName"];
                        string mailbody =
                            System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/forgot-password.html"));
                        mailbody = mailbody.Replace(@"<!--username-->", model.Email);
                        mailbody = mailbody.Replace(@"<!--password-->", password);
                        mailbody = mailbody.Replace(@"<!--EmailId-->", model.Email);
                        mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                        mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                        mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);

                        Helpers.SendEmail(
                            ConfigurationManager.AppSettings["senderemailaddress"],
                            ConfigurationManager.AppSettings["sendername"],
                            model.Email,
                            ConfigurationManager.AppSettings["forgotpasswordsubject"],
                            mailbody,
                            true);


                        model.Message = "Your password sent successfully.";
                        model.MessageType = MessageType.Success;
                    }
                    catch (Exception ex)
                    {
                        model.Message = "Internel Error occurred, Email does not sent.";
                        model.MessageType = MessageType.Error;
                    }
                }
                else
                {
                    model.Message = "Your password sent successfully.";
                    model.MessageType = MessageType.Success;
                    model.Email = String.Empty;

                }
            }

            return View(model);
        }

        [RequiresSSL]
        [HttpGet]
        public ActionResult SignUp()
        {
            var model = new SignUpViewModel
            {
                FromCountryList = CacheManager.Instance.GetTop3FromCountries(),
                ToCountryList = CacheManager.Instance.GetCountryListTo(),
            };
            return View(model);
        }

        [RequiresSSL]
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();

                string ipAddress = ControllerHelper.GetIpAddressofUser(HttpContext.Request);
                UserContext userContext = new UserContext();
                var res = _dataRepository.QuickSignUp(model.CountryFrom, model.CountryTo, model.Email, model.Password,
                    model.PhoneNumber, ipAddress, ref userContext);

                if (res == "1")
                {
                    var context = Authenticate(model.Email, model.Password);
                    if (context == null)
                    {
                        model.Message = "Invalid Email and Password combination.";
                        model.MessageType = MessageType.Error;
                        model.FromCountryList = CacheManager.Instance.GetTop3FromCountries();
                        model.ToCountryList = CacheManager.Instance.GetCountryListTo();
                        return View(model);
                    }
                }
                else
                {
                    model.Message = res;
                    model.MessageType = MessageType.Error;
                    model.FromCountryList = CacheManager.Instance.GetTop3FromCountries();
                    model.ToCountryList = CacheManager.Instance.GetCountryListTo();
                    return View(model);
                }

                return RedirectToAction("SearchRate", "Rate", new { countryfrom = model.CountryFrom, countryto = model.CountryTo });

            }

            model.FromCountryList = CacheManager.Instance.GetTop3FromCountries();
            model.ToCountryList = CacheManager.Instance.GetCountryListTo();
            //TempData["ReturnUrl"] = model.ReturnUrl;
            return View(model);
        }



        [HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }

        [RequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult BillingInfo()
        {
            string returnUrl = TempData["ReturnUrl"] as string ?? String.Empty;
            TempData["ReturnUrl"] = returnUrl;

            var data = _dataRepository.GetBillingInfo(UserContext.MemberId);
            //var model = new BillingInfoViewModel();

            var model = Mapper.Map<BillingInfoViewModel>(data);

            model.Countries = CacheManager.Instance.GetTop3FromCountries();
            var country = model.Countries.FirstOrDefault(a => a.Name == model.Country) ?? new Country() { Id = "1" };
            model.States = _dataRepository.GetStateList(SafeConvert.ToInt32(country.Id));
            model.Country = country.Id;
            return View(model);
        }

        [RequiresSSL]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BillingInfo(BillingInfoViewModel model)
        {
            string returnUrl = TempData["ReturnUrl"] as string ?? String.Empty;
            TempData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {

                var billingModel = Mapper.Map<BillingInfo>(model);
                billingModel.MemberId = UserContext.MemberId;
                billingModel.Email = UserContext.Email;
                billingModel.RefererEmail = string.Empty;
                billingModel.UserType = UserContext.UserType;
                var res = _dataRepository.UpdateBillingInfo(billingModel);
                if (!res.Status)
                {

                    model.Message = res.Errormsg;
                    model.MessageType = MessageType.Error;
                }

                //update user password
                if (!string.IsNullOrEmpty(model.OldPwd) && !string.IsNullOrEmpty(model.NewPwd))
                {
                    _dataRepository.UpdatePassword(UserContext.MemberId, model.OldPwd, model.NewPwd);
                }

                //Updating Seesion of Usercontext
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

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                model.Message = AppMessage.SuccessBillingInfoUpdate;
                model.MessageType = MessageType.Success;

            }
            else
            {
                model.IsExpand = true;
            }
            model.Countries = CacheManager.Instance.GetTop3FromCountries();
            var country = model.Countries.FirstOrDefault(a => a.Name == model.Country) ?? new Country() { Id = "1" };
            model.States = _dataRepository.GetStateList(SafeConvert.ToInt32(country.Id));
            model.Country = country.Id;
            return View(model);
        }

        #endregion


        #region MyAccount Section

        [HttpGet]
        [MobileAuthorize]
        public ActionResult MyAccount()
        {
            var data = _dataRepository.GetCustomerPlanList(UserContext.MemberId);

            var model = new MyAccountMobileViewModel
            {
                CustomerPlanList = Mapper.Map<List<MyAccountPlanEntity>>(data.OrderInfos),
                UserName = UserContext.FullName,
                Email = UserContext.Email
            };
            if (TempData["ViewMessage"] != null)
            {
                var viewMessage = TempData["ViewMessage"] as ViewMessage ?? new ViewMessage();
                model.MessageType = viewMessage.MessageType;
                model.Message = viewMessage.Message;
            }

            return View(model);
        }

        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult Plan(string id)
        {
            var planInfo = GetSinglePlanInfo(id);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            var model = Mapper.Map<PlanInfoViewModel>(planInfo);


            if (model == null)
            {
                model = new PlanInfoViewModel
                {
                    MessageType = MessageType.Error,
                    Message = "You have selected a invalid plan."
                };
            }
            else if (!model.IsActivePlan)
            {
                model.MessageType = MessageType.Warning;
                model.Message = "This plan is currently Inactive, Please contact to our customer service center.";
            }
            model.IsValidForRedeem = ControllerHelper.IsValidForReedemPoints(UserContext);
            return View(model);
        }
        [RequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult OrderHistory()
        {
            var data = _dataRepository.GetOrderHistory(UserContext.MemberId);

            var model = new OrderHistoryViewModel();
            foreach (var item in data.Orders)
            {
                model.Orders.Add(new OrderHistoryEntity()
                {
                    PlanName = item.PlanName,
                    OrderId = item.OrderId,
                    Pin = item.AccountNumber,
                    TransactionDate =
                        SafeConvert.ToString(Convert.ToDateTime(String.Format("{0:G}", item.TransactionDate))),
                    TransactionAmount = item.TransactionAmount,
                    CurrencyCode = item.CurrencyCode
                });
            }
            return View(model);
        }


        #region My Account Plan Features


        #region Call Detail Report
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult CallDetailReport(string id, string date = "")
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
            var dt = Convert.ToDateTime(date);

            var startdate = new DateTime(dt.Year, dt.Month, 1);
            var enddate = startdate.AddMonths(1).AddDays(-1);

            var allCallData = _dataRepository.GetCallHistory(id, startdate.ToShortDateString(), enddate.ToShortDateString());

            var currentMonthData = allCallData.AllCalls.Where(a => a.CallDate.Month == dt.Month).ToList();

            var model = new CallDetailsViewModel();
            foreach (var eachcall in currentMonthData)
            {
                model.AllCalls.Add(new EachCall()
                {
                    CallDate = Convert.ToDateTime(String.Format("{0:G}", eachcall.CallDate)),
                    SourceNumber = eachcall.SourceNumber,
                    DestinationNumber = eachcall.DestinationNumber,
                    DestinationCity = eachcall.DestinationCity,
                    CallDuration = eachcall.CallDuration,
                    CallAmount = eachcall.CallAmount
                });
            }
            model.PlanPin = id;
            return View(model);

        }

        #endregion

        #region Pinless Setup

        /// <summary>
        /// Registered pinless setup of a plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult PinLessSetupEdit(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["PinlessSetupViewModel"] as PinlessSetupViewModel ?? new PinlessSetupViewModel();
            var data = _dataRepository.GetPinLessNumber(id);
            model.RegisteredPinlessNumbers = data.PinlessNumberList;
            model.PlanPin = id;
            return View(model);
        }

        /// <summary>
        /// Add New Pinless Setup (Get)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult AddNewPinlessNumber(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["addNewPinlessNumberViewModel"] as AddNewPinlessNumberViewModel ??
                        new AddNewPinlessNumberViewModel();
            model.PlanPin = id;
            return View(model);
        }


        /// <summary>
        /// Add New Pinless setup (post)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewPinlessNumber(string id, AddNewPinlessNumberViewModel model)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            model.PlanPin = id;
            if (ModelState.IsValid)
            {
                var userCountryInfo = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
                var res = _dataRepository.PinLessSetUpEdit(UserContext.FullName, UserContext.MemberId,
                    string.Empty, id, model.NewPinlessNumber, userCountryInfo.CountCode
                    );

                model = new AddNewPinlessNumberViewModel();
                if (res.Status)
                {
                    model.Message = AppMessage.SuccessAddNewPinlessNumber;
                    model.MessageType = MessageType.Success;
                }
                else
                {
                    // access number can not be used for pinless setup
                    // if that phone number already exist, then you will get response message as "already exist"
                    // and if aninumber is not 10 digit number then you will receive err msg as "invalid ani number"
                    if (res.Errormsg == "access number can not be used for pinless setup")
                    {
                        model.Message = "Raza.Com Access Numbers Can Not Be Used as PinLess Phone Number !!! ";
                    }
                    else if (res.Errormsg == "already exist")
                    {
                        model.Message = "This number is already registered on a different plan.";
                    }
                    else if (res.Errormsg == "invalid ani number")
                    {
                        model.Message = "Invalid Ani number, please try with a valid number. ";
                    }
                    else
                    {
                        model.Message = "Unable to process your request, please try again.";
                    }
                    model.MessageType = MessageType.Error;
                }
            }
            TempData["addNewPinlessNumberViewModel"] = model;
            return RedirectToAction("AddNewPinlessNumber", "Account", new { id = ControllerHelper.Encrypt(id) });

        }

        /// <summary>
        /// Delete Pinless setup of a user plan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletePinlessSetupModel"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePinlessSetup(string id, DeletePinlessSetupModel deletePinlessSetupModel)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var res = _dataRepository.DeletePinLessSetUp(id, deletePinlessSetupModel.PinlessNumber,
                    deletePinlessSetupModel.CountryCode, UserContext.FullName);

                var model = new PinlessSetupViewModel();
                if (!string.IsNullOrEmpty(res))
                {
                    model.Message = AppMessage.SuccessDeletePinlessNumber;
                    model.MessageType = MessageType.Success;
                }
                else
                {
                    model.Message = AppMessage.FailureDeletePinlessNumber;
                    model.MessageType = MessageType.Error;
                }

                TempData["PinlessSetupViewModel"] = model;
            }
            return RedirectToAction("PinLessSetupEdit", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        #endregion

        #region OneTouch Setup

        /// <summary>
        /// OneTouch setup of plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult OnetouchSetup(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var data = _dataRepository.GetOneTouchSetup(id);
            var model = TempData["OneTouchSetupViewModel"] as OneTouchSetupViewModel ?? new OneTouchSetupViewModel();

            model.RegisteredOnetouchs = data.oneTouchList;
            model.PlanPin = id;

            return View(model);
        }

        /// <summary>
        /// Add New Onetouch setup of plan (Get)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult AddNewOnetouch(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["AddNewOnetouchViewModel"] as AddNewOnetouchViewModel ?? new AddNewOnetouchViewModel();

            model.CallingToCountries = ControllerHelper.GetCountryToWithCountryCodeInId();
            model.PlanPin = id;
            model.CallingFromCountries = CacheManager.Instance.GetTop3FromCountries();

            return View(model);
        }

        /// <summary>
        /// Add New onetouch setup (Post)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewOnetouch(string id, AddNewOnetouchViewModel model)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var res = _dataRepository.OneTouchSetup(id, model.CallingAvailableNumber,
                    model.CallingToCountryCode + model.CallingToPhoneNumber,
                    model.RefrenceName, UserContext.FullName);
                if (res.Status)
                {
                    model = new AddNewOnetouchViewModel
                    {
                        Message = AppMessage.SuccessNewOneTouchSetup,
                        MessageType = MessageType.Success
                    };
                }
                else
                {
                    model.Message = res.Errormsg;
                    model.MessageType = MessageType.Error;

                    model.States = _dataRepository.GetStateList(SafeConvert.ToInt32(model.CallingFromCountry));
                    model.AreaCodes = _dataRepository.GetAreacode(model.CallingFromState);
                    model.AvailableNumbers = _dataRepository.GetAvailableOneTouchNumbers(id,
                        SafeConvert.ToInt32(model.CallingFromCountry), model.CallingFromState, model.CallingFromAreaCode);

                }
            }

            TempData["AddNewOnetouchViewModel"] = model;
            return RedirectToAction("AddNewOnetouch", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        /// <summary>
        /// Delete existing onetouch setup.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteOnetouchSetup"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOnetouchSetup(string id, DeleteOnetouchSetupModel deleteOnetouchSetup)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var res = _dataRepository.DeleteOneTouchSetup(id, deleteOnetouchSetup.OnetouchNumber, UserContext.FullName);
            var model = new OneTouchSetupViewModel();
            if (res)
            {
                model.Message = AppMessage.SuccessOneTouchSetupDelete;
                model.MessageType = MessageType.Success;
            }
            else
            {
                model.Message = AppMessage.FailureOneTouchSetupDelete;
                model.MessageType = MessageType.Error;
            }
            TempData["OneTouchSetupViewModel"] = model;
            return RedirectToAction("OnetouchSetup", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        /// <summary>
        /// Get Areacode of a state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult GetOneTouchAreaCode(string state)
        {
            var data = _dataRepository.GetAreacode(state);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Available numbers for onetoch setup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="countryId"></param>
        /// <param name="state"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public JsonResult GetAvailableNumbers(string id, int countryId, string state, string areaCode)
        {
            var data = _dataRepository.GetAvailableOneTouchNumbers(id, countryId, state, areaCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region QuickKeys(s) Setup

        /// <summary>
        /// Existing quick keys of user plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult QuickKeysSetupEdit(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["QuickkeysSetupsViewModel"] as QuickkeysSetupsViewModel ??
                        new QuickkeysSetupsViewModel();
            var data = _dataRepository.GetQuickKeyNumbers(id);

            model.QuickKeysList = data.quickeyList;
            model.Planpin = id;

            return View(model);
        }


        /// <summary>
        /// Get Action methos for Add new quick key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult AddNewQuickKey(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["AddNewQuickKeyViewModel"] as AddNewQuickKeyViewModel ?? new AddNewQuickKeyViewModel();
            model.Countries = ControllerHelper.GetCountryToWithCountryCodeInId();
            model.PlanPin = id;

            return View(model);
        }

        /// <summary>
        /// Add new quick keys for a plan (Post)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewQuickKey(string id, AddNewQuickKeyViewModel model)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var res = _dataRepository.QuickKeySetUp(UserContext.MemberId, id, model.CountryCode, model.PhoneNumber,
                    model.QuickKey, model.ContactName);

                if (res.Status)
                {
                    model = new AddNewQuickKeyViewModel
                    {
                        Message = "QuickKey successfully added.",
                        MessageType = MessageType.Success
                    };
                }
                else
                {
                    model.Message = res.Errormsg;
                    model.MessageType = MessageType.Error;
                }

                ModelState.Clear();
            }
            TempData["AddNewQuickKeyViewModel"] = model;
            return RedirectToAction("AddNewQuickKey", "Account", new { id = ControllerHelper.Encrypt(id) });

        }

        /// <summary>
        /// Delete quick keys for a specific plan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuickeys(string id, DeleteQuickKeysModel model)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var quickkeySetupModel = new QuickkeysSetupsViewModel();
            if (ModelState.IsValid)
            {
                var res = _dataRepository.DeleteQuickKeySetUp(id, model.DestinationNumber, model.QuickKeys, string.Empty);

                if (res)
                {
                    quickkeySetupModel.Message = AppMessage.SuccessQuickKeysDelete;
                    quickkeySetupModel.MessageType = MessageType.Success;
                }
                else
                {
                    quickkeySetupModel.Message = AppMessage.FailureQuickKeysDelete;
                    quickkeySetupModel.MessageType = MessageType.Error;
                }
            }
            TempData["QuickkeysSetupsViewModel"] = quickkeySetupModel;
            return RedirectToAction("QuickKeysSetupEdit", "Account", new { id = ControllerHelper.Encrypt(id) });
        }
        #endregion

        #region AutoRefill
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult AutoRefill(string id)
        {
            var planinfo = GetSinglePlanInfo(id);
            if (planinfo == null)
                throw new ResourceNotFoundException();

            var userExistingCards = ControllerHelper.GetUserExistingCreditCard(UserContext.MemberId);

            var model = TempData["AutoRefillModel"] as AutoRefillViewModel ?? new AutoRefillViewModel();

            model.CurrencyCode = planinfo.CurrencyCode;
            model.UserExistingCards = userExistingCards;
            model.PlanPin = id;
            model.IsAutoRefillActivate = planinfo.AutoRefillStatus == "A";

            if (Helpers.CheckMandatoryAutorefill(Convert.ToInt32(planinfo.PlanId)) != "nm")
            {
                model.IsDisableAutorefillDeactivate = true;
            }

            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateAutoRefill(string id, AutoRefillViewModel autoRefillViewModel)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var creditCardInfoTask = Task.Run(() => ControllerHelper.GetUserExistingCreditCard(UserContext.MemberId));

                var planinfoTask = Task.Run(() => GetSinglePlanInfo(id));

                Task.WaitAll(creditCardInfoTask);
                var creditCardInfo =
                    creditCardInfoTask.Result.FirstOrDefault(
                        a => a.CreditCardId == autoRefillViewModel.CreditCardId);

                var res = creditCardInfo != null && _dataRepository.AutoReFilledit(UserContext.MemberId, id, autoRefillViewModel.AutoRefillAmount,
                    creditCardInfo.CreditCardNumber);

                Task.WaitAll(planinfoTask);

                if (res)
                {
                    autoRefillViewModel = new AutoRefillViewModel
                    {
                        Message = AppMessage.SuccessAutorefillActivation,
                        MessageType = MessageType.Success
                    };
                }
                else
                {
                    autoRefillViewModel = new AutoRefillViewModel
                    {
                        Message = AppMessage.FailureAutoRefillActivation,
                        MessageType = MessageType.Error
                    };
                }

                //autoRefillViewModel.PlanPin = id;
                //autoRefillViewModel.CurrencyCode = planinfo.CurrencyCode;
                //autoRefillViewModel.IsAutoRefillActivate = planinfo.AutoRefillStatus == "A";
                //autoRefillViewModel.UserExistingCards = creditCardInfoTask.Result;

            }
            TempData["AutoRefillModel"] = autoRefillViewModel;
            return RedirectToAction("AutoRefill", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateAutoRefiil(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var userIp = ControllerHelper.GetIpAddressofUser(Request);
            var creditCardInfoTask = Task.Run(() => ControllerHelper.GetUserExistingCreditCard(UserContext.MemberId));
            var planinfoTask = Task.Run(() => GetSinglePlanInfo(id));

            var res = _dataRepository.AutoRefillRemove(UserContext.MemberId, id, userIp);
            Task.WaitAll(planinfoTask, creditCardInfoTask);
            var model = new AutoRefillViewModel();

            if (res)
            {
                model.Message = AppMessage.SuccessAutorefillDeactivation;
                model.MessageType = MessageType.Success;
            }
            else
            {
                model.Message = AppMessage.FailureAutoRefillDeactivation;
                model.MessageType = MessageType.Error;
            }
            TempData["AutoRefillModel"] = model;
            return RedirectToAction("AutoRefill", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        #endregion

        #region CallForwarding
        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult CallForwarding(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var message = TempData["ViewMessage"] as ViewMessage ?? new ViewMessage();
            var data = _dataRepository.GetCallForwarding(id);
            var model = new CallForwardingViewModel
            {
                ExistingForwardedNumbers = data.GetNumberList,
                Message = message.Message,
                MessageType = message.MessageType,
                PlanPin = id
            };

            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [EncryptedActionParameter]
        [MobileAuthorize]
        public ActionResult DeleteCallForwardSetup(string id, int serialNumber)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var res = _dataRepository.DeleteForwardNumber(serialNumber, id, UserContext.MemberId);
            if (res)
            {
                var viewMessage = new ViewMessage()
                {
                    Message = AppMessage.SuccessCallForwardDelete,
                    MessageType = MessageType.Success
                };
                TempData["ViewMessage"] = viewMessage;
                return RedirectToAction("CallForwarding", "Account", new { id = ControllerHelper.Encrypt(id) });
            }
            var message = new ViewMessage()
            {
                Message = AppMessage.ErrorCallForwardDelete,
                MessageType = MessageType.Error
            };
            TempData["ViewMessage"] = message;
            return RedirectToAction("CallForwarding", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        [UnRequiresSSL]
        [HttpGet]
        [EncryptedActionParameter]
        [MobileAuthorize]
        public ActionResult SetupCallForwarding(string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var model = TempData["SetupCallForwardViewModel"] as SetupCallForwardViewModel ?? new SetupCallForwardViewModel();
            if (!_dataRepository.ValidateCustomerPlan(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            var listof1800Numbers = _dataRepository.GetNumber800();


            model.CountryToList = ControllerHelper.GetCountryToWithCountryCodeInId();
            model.CallForwarded800NumberList = listof1800Numbers.GetNumberList;
            model.PlanPin = id;

            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [EncryptedActionParameter]
        [MobileAuthorize]
        public ActionResult SetupCallForwarding(SetupCallForwardViewModel model, string id)
        {
            if (!ControllerHelper.ValidateCustomerPlanPin(UserContext.MemberId, id))
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var res = _dataRepository.CallForwarding(id, model.CallForwarding800Number, model.CountryCode,
                     model.DestinationPhoneNumber, model.ActivationDate, model.ExpiryDate, model.ForwardedName,
                     string.Empty);
                if (res.Status)
                {
                    model = new SetupCallForwardViewModel()
                    {
                        MessageType = MessageType.Success,
                        Message = AppMessage.SuccessSetupCallForward
                    };
                }
                else
                {
                    model.MessageType = MessageType.Error;
                    model.Message = res.Errormsg;
                }
            }
            TempData["SetupCallForwardViewModel"] = model;
            return RedirectToAction("SetupCallForwarding", "Account", new { id = ControllerHelper.Encrypt(id) });

        }




        #endregion

        #endregion

        #region RazaReward
        [UnRequiresSSL]
        [MobileAuthorize]
        [HttpGet]
        public ActionResult RazaReward()
        {
            var model = new RazaRewardsViewModel();

            var result = _dataRepository.GetReferFriendPt(UserContext.MemberId);
            model.ReferAFriend = result.Count == 0 ? 0 : result.Sum(a => a.Points);

            var data = _dataRepository.RedeemPt(UserContext.MemberId);
            model.PointsRedeemed = data.Count == 0 ? 0 : data.Sum(a => a.RedeemPoints);


            model.TotalAvailablePoints = SafeConvert.ToInt32(_dataRepository.GetRazaReward(UserContext.MemberId));

            var points = _dataRepository.GetRazaRewardPt(UserContext.MemberId).Sum(a => a.Points);
            model.TotalEarnedPointsByRecharge = SafeConvert.ToInt32(points);

            return View(model);
        }

        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult RedeemPoints(string id)
        {
            var planInfo = GetSinglePlanInfo(id);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            //var isValidForReedem = ControllerHelper.IsValidForReedemPoints(UserContext);
            //if (!isValidForReedem)
            //{
            //    //    return RedirectToAction("Plan", "Account", new { id = ControllerHelper.Encrypt(id) });
            //}

            if (TempData["cartReviewOrderViewModel"] != null)
            {
                var cartReviewModel = TempData["cartReviewOrderViewModel"] as RedeemPointReviewOrderViewModel;
                TempData["cartReviewOrderViewModel"] = cartReviewModel;
                return View("ReedemPoints_ReviewOrder", cartReviewModel);
            }



            var model = new RedeemPointsViewModel();
            var redeemOpt = ControllerHelper.GetReedemPointOptions(UserContext, Convert.ToDecimal(planInfo.ServiceFee));
            if (redeemOpt == null)
            {
                //   return RedirectToAction("Plan", "Account", new { id = ControllerHelper.Encrypt(id) });
            }
            model.RedeemOptions = redeemOpt;
            model.PlanName = planInfo.PlanName;
            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult RedeemPoints(RedeemPointsViewModel model, string id)
        {
            var planInfo = GetSinglePlanInfo(id);
            if (planInfo == null)
                throw new ResourceNotFoundException();

            if (ModelState.IsValid)
            {
                var serviceFee =
                    ControllerHelper.CalculateServiceFee(
                        string.IsNullOrEmpty(planInfo.ServiceFee) ? 0 : Convert.ToDouble(planInfo.ServiceFee),
                        model.RedeemPointAmount);

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

                    CurrencyCode = planInfo.CurrencyCode,
                    PaymentType = "Redeem Points",
                    Amount = SafeConvert.ToString(model.RedeemPointAmount),
                    PlanPin = planInfo.AccountNumber

                };
                var payPalCheckoutModel = new PayPalCheckoutModel
                {
                    ProcessPlanInfo = new ProcessPlanInfo()
                    {
                        Amount = model.RedeemPointAmount
                    },
                    ProcessPaymentInfo = new ProcessPaymentInfo()
                    {
                        TransactionType = TransactionType.Recharge.ToString()
                    }
                };

                Session[GlobalSetting.CheckOutSesionKey] = payPalCheckoutModel;
                TempData["cartReviewOrderViewModel"] = cartReviewOrderViewModel;
                return RedirectToAction("RedeemPoints", "Account", new { id = ControllerHelper.Encrypt(id) });
            }
            TempData["RedeemPointsViewModel"] = model;
            return RedirectToAction("RedeemPoints", "Account", new { id = ControllerHelper.Encrypt(id) });
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        [EncryptedActionParameter]
        public ActionResult RedeemPointRecharge(string id)
        {
            var planInfo = GetSinglePlanInfo(id);
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
            var country = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);

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
                Country = country.Id,
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

        #endregion

        [MobileAuthorize]
        [HttpGet]
        public ActionResult SavedPaymentInfo()
        {
            var cards = _dataRepository.Get_top_CreditCard(UserContext.MemberId);
            var model = new SavedPaymentCardsViewModel()
            {
                ExistingCards = cards.GetCardList
            };
            var viewMessage = TempData["ViewMessage"] as ViewMessage;
            if (viewMessage != null)
            {
                model.Message = viewMessage.Message;
                model.MessageType = viewMessage.MessageType;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteCreditCard(string creditCardId)
        {
            if (!string.IsNullOrEmpty(creditCardId))
            {
                var cardInfo = ControllerHelper.GetCreditCardByCardId(UserContext.MemberId, SafeConvert.ToInt32(creditCardId));
                var res = _dataRepository.DeleteCreditCard(UserContext.MemberId, cardInfo.CreditCardNumber);
                if (res.Status)
                {
                    TempData["ViewMessage"] = new ViewMessage()
                    {
                        Message = "Credit Card successfully deleted.",
                        MessageType = MessageType.Success
                    };
                }
                else
                {
                    TempData["ViewMessage"] = new ViewMessage()
                    {
                        Message = res.Errormsg,
                        MessageType = MessageType.Error
                    };

                }
            }
            return RedirectToAction("SavedPaymentInfo");
        }

        [HttpPost]
        [MobileAuthorize]
        public ActionResult DeleteExistingCard(int id)
        {
            return RedirectToAction("SavedPaymentInfo");
        }

        #endregion



        [UnRequiresSSL]
        [HttpGet]
        public ActionResult AgentSignUp()
        {
            var model = TempData["AgentSignupViewModel"] as AgentSignupViewModel ?? new AgentSignupViewModel();
            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        public ActionResult AgentSignUp(AgentSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                //ModelState.Clear();

                var res = _dataRepository.AgentSignUp(model.FirstName, model.LastName, model.Email, model.PhoneNumber, string.Empty,
                    string.Empty, string.Empty, model.AgentMessage, string.Empty, string.Empty);
                if (res.Status)
                {
                    SendAgentSignuptoRaza(model);
                    ModelState.Clear();

                    model = new AgentSignupViewModel()
                    {
                        MessageType = MessageType.Success,
                        Message =
                            "Thank you for your interest to join our Raza Agent Program. " + Environment.NewLine +
                            "One of our agent specialists will contact you through phone or e-mail within 24-48 business hours."
                    };
                    //SendAgentSignuptoRaza(model);
                }
                else
                {
                    model.MessageType = MessageType.Error;
                    model.Message = res.Errormsg;
                }
            }
            TempData["AgentSignupViewModel"] = model;
            return RedirectToAction("AgentSignUp");
        }

        private void SendAgentSignuptoRaza(AgentSignupViewModel model)
        {
            // string email = string.Empty;  
            string razarecipent = "azim@raza.com,salim@raza.com,sabal.raza@gmail.com";
            string[] allrows = razarecipent.Split(',');
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = string.Empty;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                redirectlink = "Account/MyAccount";
            }
            string subject = "Sub: Raza Agent program signup Information !!";


            string userfeedback = "<strong>Dear Raza, <strong><br/> <br/> " + subject + "<br/><br/><br/> Name :" + model.FirstName +
                                  " " +
                                  model.LastName + "<br/><br/>"
                                  + "Email: " + model.Email +
                                  "<br/><br/>Phone Number: " + model.PhoneNumber;

            //if (!string.IsNullOrEmpty(model.Message))
            if (!string.IsNullOrEmpty(model.AgentMessage))
            {
                userfeedback += "<br/><br/> Message: " + model.AgentMessage;
            }

            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/feedback-toraza.html"));
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--CustomerFeedback-->", userfeedback);

                foreach (var email in allrows)
                {


                    Helpers.SendEmail(
                        ConfigurationManager.AppSettings["senderemailaddress"],
                        ConfigurationManager.AppSettings["sendername"],
                        email,
                        subject,
                        mailbody,
                        true,
                        razarecipent);
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending AgentSignupmailtoRaza : " + ex.Message);
            }

        }

        public ActionResult SwitchView()
        {
            Session["SiteView"] = "Y";
            return RedirectToAction("Index", "Account", new { area = "" });
        }

    }
}
