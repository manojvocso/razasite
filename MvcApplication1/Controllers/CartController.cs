using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
//using Antlr.Runtime.JavaExtensions;
using CardinalCommerce;
using MvcApplication1.AppHelper;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using PayPal.Api.Payments;
using PayPal.OpenIdConnect;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    public class CartController : BaseController
    {
        
        // GET: /Cart/
        private DataRepository _repository = new DataRepository();

        [CompressFilter]
        [RequiresSSL]
        public ActionResult Index()
        {
            Session["IsTransactionSuccess"] = false;
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Plans", "Rate");
            }

            return View(new GenericModel());
        }


        public JsonResult GetCartData()
        {
            ShoppingCartModel shopmodel = Session["Cart"] as ShoppingCartModel ?? new ShoppingCartModel();

            if (shopmodel != null && Session["countryfrom"] != null)
            {
                var countryFrom = Session["countryfrom"];
                var countryTo = Session["countryto"];
                shopmodel.CountryFrom = SafeConvert.ToInt32(countryFrom);
                shopmodel.CountryTo = SafeConvert.ToInt32(countryTo);
            }

            if (!User.Identity.IsAuthenticated)
            {
                shopmodel.Userstatus = "New";
            }
            else
            {
                BillingInfo billInf = _repository.GetBillingInfo(UserContext.MemberId);

                if (billInf.City != "" && billInf.Address != "")
                {
                    shopmodel.Userstatus = "Old";
                }
                else
                {
                    shopmodel.Userstatus = "QuickSignUp";
                }
            }

            if (string.IsNullOrEmpty(shopmodel.CouponCode))
            {
                shopmodel.CouponCode = string.Empty;
            }

            shopmodel.IsMandatoryAutorefill = Helpers.CheckMandatoryAutorefill(SafeConvert.ToInt32(shopmodel.PlanId));
            Session["Cart"] = shopmodel;
            return Json(shopmodel, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DeleteCartItem(RateModel model)
        {
            var shopmodel = new ShoppingCartModel();
            //var cartmodel = new CartModel();
            Session["Cart"] = shopmodel;

            return RedirectToAction("Index", "Cart");

        }

        [CompressFilter]
        [HttpGet]
        public ActionResult PromotionalPlans(string countryfrom, string countryto, string amount, string currencycode)
        {
            var repository = new DataRepository();
            decimal price = Convert.ToDecimal(amount);
            List<Country> fromcountrylist = repository.GetCountryList("From");
            var countryFrom = fromcountrylist.FirstOrDefault(a => a.Id == countryfrom).Name;

            var tocountrylist = repository.GetCountryList("to");
            var toCountry = tocountrylist.FirstOrDefault(a => a.Id == countryto).Name;

            var shoppingcartmodel = new ShoppingCartModel
            {
                CallingFrom = countryFrom,
                CallingTo = toCountry,
                Price = price,
                PlanName = "New Year Special",
                CurrencyCode = currencycode
            };

            Session["Cart"] = shoppingcartmodel;
            return View("Index", new GenericModel());

        }

        [CompressFilter]
        [ActionName("GetFreeTrialPlan")]
        [RequiresSSL]
        public ActionResult Index(string trialcountryfrom, string trialcountryto)
        {
            var repository = new DataRepository();

            List<Country> trialfromcountrylist = CacheManager.Instance.GetFromCountries();
            var fromtrialcountry = trialfromcountrylist.FirstOrDefault(a => a.Id == trialcountryfrom).Name;

            var trialtocountrylist = repository.FreeTrial_Country_List();
            var totrialcountry = trialtocountrylist.FirstOrDefault(a => a.Id == trialcountryto).Name;
            //var currencycodes = FlagDictonary.GetCurrencycodebyCountry();
            string code = string.Empty;
            string planname = string.Empty;
            switch (SafeConvert.ToInt32(trialcountryfrom))
            {
                case 1:
                    code = "USD";
                    planname = "OneTouch Dial";
                    break;
                case 2:
                    code = "CAD";
                    planname = "Canada OneTouch Dial";
                    break;
                case 3:
                    code = "GBP";
                    planname = "UK Direct Dial";
                    break;
            }
            var shoppingcartmodel = new ShoppingCartModel
            {
                CallingFrom = fromtrialcountry,
                CallingTo = totrialcountry,
                PlanName = planname,
                CurrencyCode = code,
                FromToMapping = "9999",
                PlanId = "9999",
                CountryFrom = SafeConvert.ToInt32(trialcountryfrom),
                CountryTo = SafeConvert.ToInt32(trialcountryto),
                CouponCode = "FREETRIAL"
            };
            Session["Cart"] = null;
            Session["Cart"] = shoppingcartmodel;
            return RedirectToAction("Index", "Cart");
            return View("Index", new GenericModel());

        }

        public JsonResult BuyPlan(string PlanId, string FromToMapping, int FromCountry, int ToCountry, bool AutoRefill, bool IsfromSerchrate, bool isEnrollToExtraMinute)
        {
            Session["Cart"] = null;
            //var countryfrom = Session["countryfrom"];
            //var countryto = Session["countryto"];

            var countryfrom = FromCountry;
            var countryto = ToCountry;

            var rate = new Rates()
            {
                CardName = "",//"HOT DIRECT",
                CountryFrom = SafeConvert.ToInt32(countryfrom),
                CountryTo = SafeConvert.ToInt32(countryto)
            };

            var repository = new DataRepository();
            var shoppingcartmodel = new ShoppingCartModel();

            //string fromcountry = "", tocountry = "";
            List<Country> fromcountrylist = CacheManager.Instance.GetFromCountries();
            var fromcountry = fromcountrylist.FirstOrDefault(a => a.Id == countryfrom.ToString()).Name;

            List<Country> tocountrylist = CacheManager.Instance.GetAllCountryTo();
            var tocountry = tocountrylist.FirstOrDefault(a => a.Id == countryto.ToString()).Name;

            RatePlans rateplan = repository.GetRates(rate);

            var mobileratecards = rateplan.Plans.Where(t => t.FromToMapping == FromToMapping && t.PlanId == PlanId);
            var ratelist = rateplan.Plans.Where(a => a.FromToMapping == FromToMapping);
            var amountlist = ratelist.Select(item => SafeConvert.ToString(item.PlanAmount)).ToList();

            foreach (EachRatePlan erp in mobileratecards)
            {
                shoppingcartmodel.PlanName = erp.CardTypeName;
                shoppingcartmodel.FromToMapping = erp.FromToMapping;
                shoppingcartmodel.Price = erp.PlanAmount;
                shoppingcartmodel.PlanId = erp.PlanId;
                shoppingcartmodel.CallingFrom = fromcountry;
                shoppingcartmodel.CallingTo = tocountry;
                shoppingcartmodel.CurrencyCode = erp.CurrencyCode;
                shoppingcartmodel.ServiceFee = erp.ServiceFee;
                shoppingcartmodel.IsAutoRefill = AutoRefill;
                shoppingcartmodel.IsfromSerchrate = IsfromSerchrate;
                shoppingcartmodel.CountryFrom = FromCountry;
                shoppingcartmodel.CountryTo = ToCountry;

            }
            shoppingcartmodel.IsEnrollToExtraMinute = isEnrollToExtraMinute;
            shoppingcartmodel.AmountList = amountlist;

            Session["Cart"] = shoppingcartmodel;

            return
                    Json(
                        new
                        {
                            ok = true,
                            newurl = Url.Action("Index", "Cart")
                        });

        }

        [HttpPost]
        public JsonResult BuyPrmotionplans(string countryfrom, string countryto, string planname, string amount, string currencycode, string servicefee)
        {
            var shoppingcartmodel = new ShoppingCartModel();
            shoppingcartmodel.PlanName = planname;
            shoppingcartmodel.Price = Convert.ToDecimal(amount);
            shoppingcartmodel.CallingFrom = countryfrom;
            shoppingcartmodel.CallingTo = countryto;
            shoppingcartmodel.CurrencyCode = currencycode;
            shoppingcartmodel.ServiceFee = Convert.ToInt32(servicefee);

            Session["Cart"] = shoppingcartmodel;
            return
                            Json(
                                new
                                {
                                    ok = true,
                                    newurl = Url.Action("Index", "Cart")
                                });

        }

        [HttpPost]
        public JsonResult BuyCustomerPromotion(NewCustPromotionplanmodel model)
        {
            var shoppingcartmodel = new ShoppingCartModel();

            var countrytolist = CacheManager.Instance.GetCountryListTo();
            var countryfromlist = CacheManager.Instance.GetFromCountries();

            var callingfrom = countryfromlist.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryFrom));
            var callingto = countrytolist.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryTo));

            shoppingcartmodel.PlanName = model.PlanName;
            shoppingcartmodel.Price = Convert.ToDecimal(model.Denomination);
            shoppingcartmodel.CountryFrom = model.CountryFrom;
            shoppingcartmodel.CountryTo = model.CountryTo;
            if (callingfrom != null) shoppingcartmodel.CallingFrom = callingfrom.Name;
            if (callingto != null) shoppingcartmodel.CallingTo = callingto.Name;
            shoppingcartmodel.CurrencyCode = model.CurrencyCode;
            shoppingcartmodel.ServiceFee = model.ServiceFee;
            shoppingcartmodel.PlanId = model.PlanId;
            shoppingcartmodel.CouponCode = model.CouponCode;
            shoppingcartmodel.IsPromotionPlan = true;

            Session["Cart"] = shoppingcartmodel;
            return
                            Json(
                                new
                                {
                                    ok = true,
                                    newurl = Url.Action("Index", "Cart")
                                });

        }


        [HttpPost]
        [RequiresSSL]
        public ActionResult CheckOutCart(decimal planAmount)
        {
            if (Session["Cart"] == null)
            {
                //return RedirectToAction("searchrate", "Rate");
                return Json(new
                {
                    status = true,
                    user = "newplan",
                    newurl = Url.Action("searchrate", "Rate"),

                });
            }
            var shopmodel = new ShoppingCartModel();
            shopmodel = Session["Cart"] as ShoppingCartModel;
            var rates = new Rates()
            {
                CountryFrom = shopmodel.CountryFrom,
                CountryTo = shopmodel.CountryTo,
                CardName = string.Empty
            };

            var data = _repository.GetRates(rates);
            var plandata = data.Plans.FirstOrDefault(a => a.FromToMapping == shopmodel.FromToMapping && a.PlanAmount == planAmount);
            if (plandata != null)
            {
                shopmodel.Price = plandata.PlanAmount;
                shopmodel.ServiceFee = plandata.ServiceFee;
                shopmodel.PlanId = plandata.PlanId;
            }

            Session["Cart"] = shopmodel;

            //BillingInfo billInf = _repository.GetBillingInfo(UserContext.MemberId);
            if (User.Identity.IsAuthenticated)
            {
                BillingInfo billInf = _repository.GetBillingInfo(UserContext.MemberId);
                var planlist = new DataRepository().GetCustomerPlanList(UserContext.MemberId);
                var resp = planlist.OrderInfos.FirstOrDefault(a => a.PlanId == shopmodel.FromToMapping);

                if (planlist.OrderInfos.Count == 0 && UserContext.UserType.ToLower() == "new")
                {
                    if ((billInf.City != "" && billInf.Address != "" && billInf.Country != "") &&
                        (billInf.City != null && billInf.Address != null && billInf.Country != null))
                    {
                        Session["Cart"] = shopmodel;

                        return Json(new
                        {
                            status = true,
                            user = "newact",
                            newurl = Url.Action("CheckOut", "Cart"),
                            IsAutoRefill = shopmodel.IsAutoRefill
                        });
                    }
                    else
                    {
                        Session["Cart"] = shopmodel;
                        return Json(new
                        {
                            status = true,
                            user = "exist-newact",
                            newurl = Url.Action("CheckOut", "Cart"),
                            IsAutoRefill = shopmodel.IsAutoRefill
                        });
                    }

                }
                else
                {
                    if (resp != null &&
                        (shopmodel.FromToMapping == "161" || shopmodel.FromToMapping == "162" ||
                         shopmodel.FromToMapping == "102"))
                    {
                        shopmodel.ExistPin = resp.AccountNumber;
                        shopmodel.MyaccBal = resp.MyAccountBal;
                        shopmodel.Orderid = resp.OrderId;
                        Session["Cart"] = shopmodel;
                        return Json(new
                        {
                            status = true,
                            user = "existplan",
                            newurl = Url.Action("Index", "Recharge", new
                            {
                                order_id = shopmodel.Orderid,
                                RecBal = shopmodel.MyaccBal,
                                currencycode = shopmodel.CurrencyCode,
                                servicefee = shopmodel.ServiceFee,
                                IsAutoRefill = shopmodel.IsAutoRefill
                            })
                        });

                    }
                    else
                    {
                        Session["Cart"] = shopmodel;

                        return Json(new
                        {
                            status = true,
                            user = "newplan",
                            newurl = Url.Action("Regphone", "Recharge"),
                            IsAutoRefill = shopmodel.IsAutoRefill
                        });
                    }
                }

            }
            else
            {
                Session["Cart"] = shopmodel;
                return Json(new
                {
                    status = true,
                    user = "newact",
                    newurl = Url.Action("CheckOut", "Cart"),
                    IsAutoRefill = shopmodel.IsAutoRefill
                });
            }


        }


        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult CheckOut()
        {
            if ((bool)Session["IsTransactionSuccess"])
            {
                return RedirectToAction("MyAccount", "Account", new { rf = "ai" });
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("SearchRate", "Rate");
            }

            var result = new CheckOutModel();
            var fs = new List<State>();
            var repository = new DataRepository();
            var data = CacheManager.Instance.GetFromCountries();
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(CacheManager.Instance.GetFromCountries()));

            foreach (var res in data)
            {
                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            result.States = fs;
            result.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = result.Statelist;
            if (User.Identity.IsAuthenticated)
            {
                result.AniNumber = UserContext.ProfileInfo.PhoneNumber;
                result.FirstName = UserContext.ProfileInfo.FirstName;
                result.LastName = UserContext.ProfileInfo.LastName;
                result.ZipCode = UserContext.ProfileInfo.ZipCode;
                result.Address = UserContext.ProfileInfo.Address;
                result.City = UserContext.ProfileInfo.City;
                result.State = string.IsNullOrEmpty(UserContext.ProfileInfo.State) ? string.Empty : UserContext.ProfileInfo.State;
                var cdata = CacheManager.Instance.GetFromCountries();
                var firstOrDefault = cdata.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                if (firstOrDefault != null)
                    result.Country = firstOrDefault.Id;
                result.Email = UserContext.Email;
                result.IsSignuped = true;
            }


            return View(result);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AuthCheckOut()
        {
            return View();
        }

        [RequiresSSL]
        public JsonResult ValidateCouponCode(Recharge validateCoupon)
        {
            if (validateCoupon.CouponCode == null || validateCoupon.CouponCode.ToLower() == "null")
            {
                return Json(new
                {
                    status = false
                });
            }
            string transmode = string.Empty;
            switch (validateCoupon.TransactionType)
            {
                case "Recharge":
                    transmode = "R";
                    break;
                case "PurchaseNewPlan":
                    transmode = "S";
                    break;
                case "Checkout":
                    transmode = "A";
                    break;
                case "TopUp":
                    transmode = "T";
                    break;
            }

            if (validateCoupon.CardId == 9999)
            {
                switch (SafeConvert.ToInt32(validateCoupon.Countryfrom))
                {
                    case 1:
                        validateCoupon.CardId = 161;
                        break;
                    case 2:
                        validateCoupon.CardId = 162;
                        break;
                    case 3:
                        validateCoupon.CardId = 102;
                        break;
                }
            }

            validateCoupon.MemberID = UserContext.MemberId;

            var res = _repository.ValidateCouponCode(validateCoupon.MemberID, validateCoupon.CouponCode,
                validateCoupon.CardId, validateCoupon.Countryfrom, validateCoupon.Countryto, validateCoupon.Amount,
                transmode);
            if (res)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }

        [RequiresSSL]
        public JsonResult ValidateCardInfo(Recharge validatecard)
        {
            try
            {
                if (validatecard.CouponCode == null || validatecard.CouponCode.ToLower() == "null")
                {
                    validatecard.CouponCode = string.Empty;
                }

                string transmode = string.Empty;
                if (validatecard.TransactionType == "Recharge")
                {
                    transmode = "R";
                }

                else if (validatecard.TransactionType == "PurchaseNewPlan")
                {
                    transmode = "S";
                }

                else if (validatecard.TransactionType == "Checkout")
                {
                    transmode = "S";
                }

                //if (string.IsNullOrEmpty(UserContext.ProfileInfo.FirstName) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.LastName) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.Address) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.City)
                //    || string.IsNullOrEmpty(UserContext.ProfileInfo.ZipCode) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.State) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.State) ||
                //    string.IsNullOrEmpty(UserContext.ProfileInfo.PhoneNumber))
                if ((UserContext.ProfileInfo.Country.ToUpper() == "U.K." || UserContext.ProfileInfo.Country == "3") && (string.IsNullOrEmpty(UserContext.ProfileInfo.FirstName) ||
                    string.IsNullOrEmpty(UserContext.ProfileInfo.LastName) ||
                    string.IsNullOrEmpty(UserContext.ProfileInfo.Address) ||
                    string.IsNullOrEmpty(UserContext.ProfileInfo.City)
                    || string.IsNullOrEmpty(UserContext.ProfileInfo.ZipCode) ||
                    string.IsNullOrEmpty(UserContext.ProfileInfo.PhoneNumber)))
                {
                    return
                       Json(
                           new
                           {
                               status = false,
                               Type = "B",
                               Message = "You have to complete your billing information before recharge.Please complete your billing info first."
                           }, JsonRequestBehavior.AllowGet);
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
                    return
                       Json(
                           new
                           {
                               status = false,
                               Type = "B",
                               Message = "You have to complete your billing information before recharge.Please complete your billing info first."
                           }, JsonRequestBehavior.AllowGet);
                }
                if (validatecard.PaymentType == "P")
                {
                    return
                       Json(
                           new
                           {
                               status = true
                           }, JsonRequestBehavior.AllowGet);
                }

                string memberid = UserContext.MemberId;
                string cardNumber = validatecard.CardNumber ?? string.Empty;

                var res = _repository.ValidateCustomer(memberid, cardNumber, validatecard.CardId, validatecard.Amount, transmode, validatecard.Countryfrom, validatecard.Countryto, validatecard.CouponCode);
                if (res.Status)
                {
                    return
                       Json(
                           new
                           {
                               status = true
                           }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return
                       Json(
                           new
                           {
                               status = false,
                               Type = "A",
                               Message = "Invalid Credit card Information. Please try with a valid credit card."
                           }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return
                       Json(
                           new
                           {
                               status = false
                           }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        [RequiresSSL]
        public JsonResult CcProcessValidation(string userType, string ordertype, string PaymentMethod, string CardNumber,
            string Country, double TransAmount = 0, string validateplan = "Y")
        {

            if (Session["Cart"] != null)
            {
                ShoppingCartModel shopmodel = Session["Cart"] as ShoppingCartModel ?? new ShoppingCartModel();
                var cfromdata =
                    CacheManager.Instance.GetFromCountries()
                        .FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                if (SafeConvert.ToInt32(cfromdata.Id) != shopmodel.CountryFrom)
                {
                    var model = new CcValidationModel()
                    {
                        IsValidPlan = false,
                        StatusMsg = "You have selected wrong plan from your country."
                    };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }


            string paymentmethod = string.Empty;
            if (PaymentMethod == "P")
            {
                paymentmethod = "Paypal";
            }
            else if (PaymentMethod == "C")
            {
                paymentmethod = "Credit Card";
            }
            else if (PaymentMethod == "F")
            {
                paymentmethod = "Free Trial";
            }

            //        string tempcc = "5434322234212345";
            //        var response = _repository.CcProcessValidation(UserContext.MemberId, userType, ordertype, paymentmethod,
            //tempcc, Country, TransAmount);

            if (Session["IsPaymentComplete"] != null && Session["IsPaymentComplete"] as string == "Y")
                return null;




            //************************** ADDED ON 01/13/2015 to TRAK USER AGENT  **************************
            if (PaymentMethod == "P")
            {
                string userAgent = Request.UserAgent.ToLower();
                string DeviceInfo = "";
                if (userAgent != null)
                {
                    if (userAgent.Contains("blackberry"))
                        DeviceInfo = "BlaceBerry";
                    else if (userAgent.Contains("iphone"))
                        DeviceInfo = "IPhone";
                    else if (userAgent.Contains("ipad"))
                        DeviceInfo = "IPad";
                    else if (userAgent.Contains("Android"))
                        DeviceInfo = "Android";
                }
                if (DeviceInfo.Length > 0)
                    RazaLogger.WriteInfoPartial("CustomerID: " + UserContext.MemberId + ",User Device " + DeviceInfo);
            }
            //*********************************************************************************************




            Session["IsPaymentComplete"] = "Y";
            var response = _repository.CcProcessValidation(UserContext.MemberId, userType, ordertype, paymentmethod,
                CardNumber, Country, TransAmount);

            response.IsValidPlan = true;
            if (response.AcceptOrder)
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [Authorize]
        [RequiresSSL]
        public JsonResult IsCentinelAllowed(string userType, string orderType)
        {
            try
            {
                bool ccProcess = _repository.IsCentinelProcess(UserContext.MemberId, userType, orderType);
                //usertype, UserType = new , old
                if (ccProcess)
                {
                    return
                        Json(
                            new
                            {
                                status = true,
                            });
                }
                else
                {
                    return
                        Json(

                            new
                            {
                                status = false
                            });
                }
            }

            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        // For Issue new pin for a new plan
        [HttpPost]
        [Authorize]
        [RequiresSSL]
        public ActionResult IssueNewPin(CheckOutModel checkOutModel)
        {
            try
            {
                if ((bool)Session["IsTransactionSuccess"])
                {
                    var model = new CheckoutStatusModel
                    {
                        IssuenewpinStatus = true,
                        Errormsg = "Transaction is arleady successfull, Please check your order history."
                    };
                    return Json(model);
                }

                string prefix = string.Empty;
                if (checkOutModel.TransactionType == "PurchaseNewPlan") // existing customer new plan
                {
                    prefix = "OS";
                }
                else if (checkOutModel.TransactionType == "CheckOut") //// new customer new plan
                {
                    prefix = "OA";
                }

                string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

                var repository = new DataRepository();

                string country = string.Empty;
                var cFromList = CacheManager.Instance.GetFromCountries();
                if (cFromList != null)
                {
                    var cdata = cFromList.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                    if (cdata != null) country = cdata.Id;
                }

                var rechargeInfo = new RechargeInfo
                {
                    OrderId = orderId,
                    Address1 = checkOutModel.Address,
                    Amount = checkOutModel.Amount,
                    ServiceFee = checkOutModel.ServiceFee,
                    Country = country,
                    ZipCode = checkOutModel.ZipCode,
                    State = string.IsNullOrEmpty(checkOutModel.State) ? string.Empty : checkOutModel.State,
                    MemberId = UserContext.MemberId,
                    City = checkOutModel.City,
                    CardNumber = checkOutModel.CardNumber,
                    CVV2 = checkOutModel.CvvNumber,
                    PaymentMethod = checkOutModel.PaymentMethod,
                    ExpiryDate = checkOutModel.ExpDate,
                    UserType = checkOutModel.UserType,
                    CardId = checkOutModel.CardId,
                    CountryFrom = checkOutModel.CountryFrom,
                    CountryTo = checkOutModel.CountryTo,
                    CoupanCode = string.IsNullOrEmpty(checkOutModel.CoupanCode) ? string.Empty : checkOutModel.CoupanCode,
                    AniNumber = checkOutModel.AniNumber,
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"],
                    Address2 = "",
                    PaymentType = checkOutModel.PaymentType,
                    IsCcProcess = false,
                    AVSResponse = string.Empty,
                    CVV2Response = string.Empty,
                    Cavv = string.Empty,
                    EciFlag = string.Empty,
                    PaymentTransactionId = string.Empty,
                    Xid = string.Empty,


                };
                if (Session["RegPinlessNumber"] != null)
                {
                    var PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
                    Session["RegPinlessNumber"] = null;
                    RazaLogger.WriteInfo("add all pinless numbers");
                    int count = 1;
                    if (PinlessNumbers != null)
                    {
                        foreach (var item in PinlessNumbers)
                        {
                            if (item.PinlessNumber != null)
                            {
                                if (count == 1)
                                {
                                    rechargeInfo.AniNumber = item.PinlessNumber;
                                }
                                else
                                {
                                    rechargeInfo.AniNumber += "*" + item.PinlessNumber;
                                }
                            }

                        }
                    }

                }


                if (rechargeInfo.PaymentType == "P")
                {
                    rechargeInfo.PaymentMethod = "PayPal";
                    rechargeInfo.CardNumber = string.Empty;
                    rechargeInfo.ExpiryDate = string.Empty;
                    rechargeInfo.CVV2 = string.Empty;
                }
                else if (rechargeInfo.PaymentType == "C")
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }
                if (checkOutModel.autoRefill == "E")
                {
                    rechargeInfo.IsAutoRefill = string.Empty;
                    rechargeInfo.AutoRefillAmount = 0;
                }
                else if (checkOutModel.autoRefill == "T")
                {
                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = Convert.ToDouble(checkOutModel.autoRefillAmount);
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }

                if (rechargeInfo.CardId == 9999)
                {
                    rechargeInfo.PaymentMethod = "Free Trial";
                    rechargeInfo.CardId = 0;
                    switch (SafeConvert.ToInt32(rechargeInfo.CountryFrom))
                    {
                        case 1:
                            rechargeInfo.CardId = 161;
                            break;
                        case 2:
                            rechargeInfo.CardId = 162;
                            break;
                        case 3:
                            rechargeInfo.CardId = 102;
                            break;
                    }
                }

                var response = repository.IssueNewPin(rechargeInfo);

                if (response.IssuenewpinStatus)
                {
                    Session["IsTransactionSuccess"] = true;

                    response.NewOrderId = rechargeInfo.OrderId;
                    if (checkOutModel.IsPasscodeDial == "T" && checkOutModel.PassCodePin != null)
                    {
                        var res = repository.SetPassCode(response.NewPin, checkOutModel.PassCodePin);
                    }

                    response.NewOrderId = orderId;
                    var data = repository.GetLocalAccessNumber(rechargeInfo.State,
                        SafeConvert.ToString(rechargeInfo.CountryFrom), rechargeInfo.AniNumber,
                        "A");
                    var numberList = data.Take(5).Select(item => item.AccessNumber).ToList();
                    response.LocalAccessNumbers = numberList;
                    response.OrderDateTime = SafeConvert.ToString(DateTime.Now);
                    var creditCartHandler = new CreditCartHandler();
                    var rech = new Recharge()
                    {
                        PlanName = checkOutModel.PlanName,
                        order_id = rechargeInfo.OrderId,
                        Amount = rechargeInfo.Amount,
                        ServiceFee = rechargeInfo.ServiceFee,
                        CardNumber = rechargeInfo.CardNumber,
                        PaymentType = rechargeInfo.PaymentType,
                        CardType = checkOutModel.CardType,
                        Pin = response.NewPin,
                        CurrencyCode = checkOutModel.CurrencyCode,

                    };
                    creditCartHandler.CallingCardPurchaseMail(rech, UserContext.Email);
                    return Json(response);
                }
                else
                {
                    return Json(response);
                }

            }

            catch (Exception ex)
            {
                var model = new CheckoutStatusModel
                {
                    IssuenewpinStatus = false,
                    Errormsg = "Transaction Declined, Please Try again."
                };
                //Response.StatusCode = 500;
                //return Json(new { Message = ex.Message });
                return Json(model);
            }

            return null;

        }


        [HttpPost]
        [Authorize]
        [RequiresSSL]
        public ActionResult ProcessCreditCard(Recharge recharge)
        {
            CreditCartHandler creditCartHandler = new CreditCartHandler();
            CentinelAuthenticateResponse response = new CentinelAuthenticateResponse();

            CartAuthenticateModel model = new CartAuthenticateModel();


            RazaLogger.WriteInfo("Processing start in ProcessCreditCard");
            if (recharge.CouponCode == null || recharge.CouponCode.ToLower() == "null")
            {
                recharge.CouponCode = string.Empty;
            }
            if (recharge.ExpDate != null && recharge.ExpDate.Contains("/"))
            {
                var expdate = recharge.ExpDate.Replace("/", "");
                recharge.ExpDate = expdate;
            }


            string ClientIPAddress = Request.ServerVariables["REMOTE_ADDR"];
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ClientIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //recharge.IpAddress = Request.ServerVariables["REMOTE_ADDR"];
            recharge.IpAddress = ClientIPAddress;

            string prefix = string.Empty;

            string transmode = string.Empty;

            if (recharge.TransactionType == "PurchaseNewPlan") // existing customer new plan
            {
                prefix = "OS";
                transmode = "S";
            }
            else if (recharge.TransactionType == "Recharge") //// existing customer existing plan
            {
                prefix = "OR";
                transmode = "R";
            }
            else if (recharge.TransactionType == "CheckOut") //// new customer new plan
            {
                prefix = "OA";
                transmode = "S";
            }
            else if (recharge.TransactionType == "TopUp") //// new customer new plan
            {
                prefix = "TP";
                transmode = "S";
            }

            if (Session["RegPinlessNumber"] != null)
            {
                RazaLogger.WriteInfo("Adding number to list");
                recharge.PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
                Session["RegPinlessNumber"] = null;
            }

            if (recharge.TransactionType == "Recharge")
            {
                recharge.OldOrderId = recharge.order_id;
            }

            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

            //if (recharge.TransactionType == "PurchaseNewPlan" || recharge.TransactionType == "CheckOut")
            recharge.order_id = orderId;



            model.RechargeValues = recharge;
            string returl = string.Empty;

            try
            {


                #region Credit Card Processing

                if (model.RechargeValues.PaymentType == "C")
                {
                    model.CentinelAuthenticateResponse = response;
                    Session["CartAuthenticateModel"] = model;
                    RazaLogger.WriteInfo(model.RechargeValues.TransactionType);
                    RazaLogger.WriteInfo("Credit Card Processing..");
                    // Payflow Pro implementations

                    // var result = new Payment();
                    //in case of tryus fee only authorize thae card.
                    RazaLogger.WriteInfo("Calling CreditCardAuthorize");
                    var result = creditCartHandler.CreditCardAuthorize(model, UserContext);
















                    /****************************************************** CODE COMMMENTED ON 11/11/14 TO CLEAN UP LOGIC BY SABALE *******************************


                    //if (!string.IsNullOrEmpty(creditCartHandler.CreditCardAuthorize(model, UserContext).IAVS))
                    if (result.RESULT == "0" && !string.IsNullOrEmpty(result.IAVS) &&
                        ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") && (model.RechargeValues.AvsByPass == true || (model.RechargeValues.AvsByPass != true && result.AVSADDR == "Y" && result.AVSZIP == "Y"))))
                    {
                        RazaLogger.WriteInfo("Calling CreditCardPayment");

                        // for tryusfree txns
                        if (model.RechargeValues.CardId == 9999)
                        {
                            if (model.RechargeValues.TransactionType == "CheckOut")
                            {
                                model.CentinelAuthenticateResponse.AVSResponse = "YY";
                                RazaLogger.WriteInfo("calling issue new pin for free trial plan");
                                creditCartHandler.IssueNewPlan(model, UserContext);
                                var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                            }
                            returl = Url.Action("OrderConfirmation", "Cart");

                        }

                        //  var result = creditCartHandler.CreditCardPayment(model, UserContext);
                        var cap_auth = creditCartHandler.CreditCardCapture(model, UserContext, result.PNREF);
                        // CVV response ="Y" OR "X"
                        // AVS ADDR =="Y" and AVSZIP ="Y"
                        RazaLogger.WriteInfo(string.Format("response of credit card payment: cvv2match={0},AVSADDR={1},AVSZIP={2}", result.CVV2MATCH, result.AVSADDR, result.AVSZIP));
                        if (cap_auth.RESULT == "0")
                        {
                            response.Status = true;
                            model.CentinelAuthenticateResponse.AVSResponse = result.AVSADDR + result.AVSZIP;
                            model.Centinel_TransactionId = cap_auth.PNREF;
                            model.CentinelAuthenticateResponse.CVVResponse = result.CVV2MATCH;

                        }
                        else
                        {
                            var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                            response.Message = "Payment authorization failed.";
                            response.Status = false;
                        }
                    }
                    else
                    {
                        //if ((result.RESULT == "0" || result.RESULT == "114") && !string.IsNullOrEmpty(result.IAVS) &&
                        //       ((result.CVV2MATCH != "Y" || result.CVV2MATCH != "X")) || (result.AVSADDR != "Y" || result.AVSZIP != "Y"))
                        if (result.RESULT == "0" || result.RESULT == "114")
                        {
                            if((result.CVV2MATCH != "Y" || result.CVV2MATCH != "X") || (result.AVSADDR != "Y" || result.AVSZIP != "Y"))
                            {
                                if(string.IsNullOrEmpty(result.IAVS))
                                {
                                    RazaLogger.WriteInfo("No AVS Response Received");
                                    return Json(new { status = false, cvv2match = "N", avsaddr = "N", returl = "" });
                                }
                                else if (result.CVV2MATCH == "N" && (result.AVSADDR == "N" || result.AVSZIP == "N"))
                                {
                                    RazaLogger.WriteInfo("Cvv and address doesn't match. redirect to RevieOrder");
                                    return Json(new { status = false, cvv2match = "N", avsaddr = "N", returl = "" });
                                }
                                else if ((result.AVSADDR != null && result.AVSADDR.Trim() == "N") ||
                                         (result.AVSZIP != null && result.AVSZIP.Trim() == "N"))
                                {
                                    RazaLogger.WriteInfo(
                                        "Adress and Zipcode/PostalCode  doesn't match. redirect to RevieOrder");
                                    return Json(new { status = false, avsaddr = "N", returl = "" });

                                }
                                else if (result.CVV2MATCH != null && result.CVV2MATCH.Trim() == "N")
                                {
                                    RazaLogger.WriteInfo("Cvv doesn't match. redirect to RevieOrder");
                                    return Json(new { status = false, cvv2match = "N", returl = "" });

                                }
                                else
                                {
                                    RazaLogger.WriteInfo("Invalid Credit card Information. Please try again.");
                                    switch (model.RechargeValues.TransactionType)
                                    {
                                        case "Recharge":
                                        case "PurchaseNewPlan":
                                            returl = Url.Action("OrderFailed", "Recharge");
                                            break;
                                        case "CheckOut":
                                            var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                            returl = Url.Action("OrderFailed", "Cart");
                                            break;
                                        case "TopUp":
                                            returl = Url.Action("TopupFailed", "TopUpMobile");
                                            break;
                                    }
                                    return Json(new { status = false, returl = returl });
                                }
                                //  return RedirectToAction("ReviewOrder", "Cart");
                            }

                        }
                        else if (result.RESULT != "0")
                        {
                            //THIS ELSE WAS USED ON 11/10/14 TO TRAP THE CREDIT CARD ERROR WHICH DOES NOT HAVE RESULT = 0
                            RazaLogger.WriteInfo("There is problem with your credit card. Please try again later");
                            return Json(new { status = false, code = "U", returl = "" });
                        }

                        response.Status = false;
                        model.FinalResponseMessage = response.Message;
                        model.FinalTransactionStatus = response.Status;
                        model.CentinelAuthenticateResponse = response;
                        Session["CartAuthenticateModel"] = model;

                        switch (model.RechargeValues.TransactionType)
                        {
                            case "Recharge":
                            case "PurchaseNewPlan":
                                returl = Url.Action("OrderFailed", "Recharge");
                                break;
                            case "CheckOut":
                                var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                returl = Url.Action("OrderFailed", "Cart");
                                break;
                            case "TopUp":
                                returl = Url.Action("TopupFailed", "TopUpMobile");
                                break;
                        }
                    }

                    ****************************************************************************************************************************************************************/

















                    //if (result.RESULT == "0" && !string.IsNullOrEmpty(result.IAVS) &&
                    //    ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") && (model.RechargeValues.AvsByPass == true || (model.RechargeValues.AvsByPass != true && result.AVSADDR == "Y" && result.AVSZIP == "Y"))))
                    if (result.RESULT == "0")
                    {
                        if ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") && (model.RechargeValues.AvsByPass == true || (result.AVSADDR == "Y" && result.AVSZIP == "Y")))
                        {
                            #region TO PROCESS TRANSACTION AFTER SUCCESSFULL AUTHORIZATION
                            RazaLogger.WriteInfo("Calling CreditCardPayment");

                            // for tryusfree txns
                            if (model.RechargeValues.CardId == 9999)
                            {
                                //******************* COMMENTED ON 11/14/14 TO FIX FREE TRIAL CC AUTHORIZATION BY WEBSITE ************************
                                //if (model.RechargeValues.TransactionType == "CheckOut")
                                //{
                                //    model.CentinelAuthenticateResponse.AVSResponse = "YY";
                                //    RazaLogger.WriteInfo("calling issue new pin for free trial plan");
                                //    creditCartHandler.IssueNewPlan(model, UserContext);
                                //    var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                                //}
                                //returl = Url.Action("OrderConfirmation", "Cart");
                                //*****************************************************************************************************************





                                if (model.RechargeValues.TransactionType == "CheckOut")
                                {
                                    //******************* ADDED ON 11/14/14 TO FIX FREE TRIAL CC AUTHORIZATION BY WEBSITE ************************
                                    response.Status = true;
                                    model.CentinelAuthenticateResponse.AVSResponse = result.AVSADDR + result.AVSZIP;
                                    model.Centinel_TransactionId = result.PNREF;
                                    model.CentinelAuthenticateResponse.CVVResponse = result.CVV2MATCH;
                                    //************************************************************************************************************


                                    RazaLogger.WriteInfo("calling issue new pin for free trial plan");
                                    //creditCartHandler.IssueNewPlan(model, UserContext);
                                    var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                                }
                                //returl = Url.Action("OrderConfirmation", "Cart");

                            }
                            else
                            {
                                //  var result = creditCartHandler.CreditCardPayment(model, UserContext);
                                var cap_auth = creditCartHandler.CreditCardCapture(model, UserContext, result.PNREF);
                                // CVV response ="Y" OR "X"
                                // AVS ADDR =="Y" and AVSZIP ="Y"
                                RazaLogger.WriteInfo(string.Format("response of credit card payment: cvv2match={0},AVSADDR={1},AVSZIP={2}", result.CVV2MATCH, result.AVSADDR, result.AVSZIP));
                                if (cap_auth.RESULT == "0")
                                {
                                    response.Status = true;
                                    model.CentinelAuthenticateResponse.AVSResponse = result.AVSADDR + result.AVSZIP;
                                    model.Centinel_TransactionId = cap_auth.PNREF;
                                    model.CentinelAuthenticateResponse.CVVResponse = result.CVV2MATCH;

                                }
                                else
                                {
                                    var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                                    response.Message = "Payment authorization failed.";
                                    response.Status = false;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region TO DO AFTER SUCCESS FULL AUTHORIZATION BUT UNSUCCESSFULL CVV OR AVS RESPONSE
                            if (string.IsNullOrEmpty(result.IAVS))
                            {
                                Session["IsPaymentComplete"] = "N";
                                RazaLogger.WriteInfo("No AVS Response Received");
                                return Json(new { status = false, cvv2match = "N", avsaddr = "N", returl = "" });
                            }
                            else if (result.CVV2MATCH == "N" && (result.AVSADDR == "N" || result.AVSZIP == "N"))
                            {
                                Session["IsPaymentComplete"] = "N";
                                RazaLogger.WriteInfo("Cvv and address doesn't match. redirect to RevieOrder");
                                return Json(new { status = false, cvv2match = "N", avsaddr = "N", returl = "" });
                            }
                            else if ((result.AVSADDR != null && result.AVSADDR.Trim() == "N") || (result.AVSZIP != null && result.AVSZIP.Trim() == "N"))
                            {
                                Session["IsPaymentComplete"] = "N";
                                RazaLogger.WriteInfo("Adress and Zipcode/PostalCode  doesn't match. redirect to RevieOrder");
                                return Json(new { status = false, avsaddr = "N", returl = "" });

                            }
                            else if (result.CVV2MATCH != null && result.CVV2MATCH.Trim() == "N")
                            {
                                Session["IsPaymentComplete"] = "N";
                                RazaLogger.WriteInfo("Cvv doesn't match. redirect to RevieOrder");
                                return Json(new { status = false, cvv2match = "N", returl = "" });

                            }
                            else
                            {
                                Session["IsPaymentComplete"] = "N";
                                RazaLogger.WriteInfo("Invalid Credit card Information. Please try again.");
                                switch (model.RechargeValues.TransactionType)
                                {
                                    case "Recharge":
                                    case "PurchaseNewPlan":
                                        returl = Url.Action("OrderFailed", "Recharge");
                                        break;
                                    case "CheckOut":
                                        //var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                        returl = Url.Action("OrderFailed", "Cart");
                                        break;
                                    case "TopUp":
                                        returl = Url.Action("TopupFailed", "TopUpMobile");
                                        break;
                                }
                                return Json(new { status = false, returl = returl });
                            }
                            //  return RedirectToAction("ReviewOrder", "Cart");
                            #endregion
                        }
                    }
                    else
                    {
                        Session["IsPaymentComplete"] = "N";
                        //THIS ELSE WAS USED ON 11/10/14 TO TRAP THE CREDIT CARD ERROR WHICH DOES NOT HAVE RESULT = 0
                        RazaLogger.WriteInfo("There is problem with your credit card. Please try again later");
                        return Json(new { status = false, code = "U", returl = "" });

                        response.Status = false;
                        model.FinalResponseMessage = response.Message;
                        model.FinalTransactionStatus = response.Status;
                        model.CentinelAuthenticateResponse = response;
                        Session["CartAuthenticateModel"] = model;

                        switch (model.RechargeValues.TransactionType)
                        {
                            case "Recharge":
                            case "PurchaseNewPlan":
                                returl = Url.Action("OrderFailed", "Recharge");
                                break;
                            case "CheckOut":
                                var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                returl = Url.Action("OrderFailed", "Cart");
                                break;
                            case "TopUp":
                                returl = Url.Action("TopupFailed", "TopUpMobile");
                                break;
                        }
                    }



















                    if (response.Status) //Successful messages
                    {
                        //switch (model.RechargeValues.TransactionType)
                        //{
                        //    case "Recharge":
                        //        creditCartHandler.RechargePin(model, UserContext);
                        //        returl = Url.Action("RechargeConfirmation", "Recharge");
                        //        break;
                        //    case "PurchaseNewPlan":
                        //        creditCartHandler.IssueNewPlan(model, UserContext);
                        //        returl = Url.Action("OrderConfirmation", "Cart");
                        //        break;
                        //    case "CheckOut":
                        //        creditCartHandler.IssueNewPlan(model, UserContext);
                        //        returl = Url.Action("OrderConfirmation", "Cart");
                        //        break;
                        //    case "TopUp":
                        //        var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                        //        Session["TopupStatus"] = res;
                        //        returl = Url.Action("TopupConfirmation", "TopUpMobile");
                        //        break;
                        //}


                        switch (model.RechargeValues.TransactionType)
                        {
                            case "Recharge":
                                creditCartHandler.RechargePin(model, UserContext);
                                returl = Url.Action("RechargeConfirmation", "Recharge");
                                return Json(new { status = true, returl = returl });
                            case "PurchaseNewPlan":
                                creditCartHandler.IssueNewPlan(model, UserContext);
                                returl = Url.Action("OrderConfirmation", "Cart");
                                return Json(new { status = true, returl = returl });
                            case "CheckOut":
                                creditCartHandler.IssueNewPlan(model, UserContext);
                                returl = Url.Action("OrderConfirmation", "Cart");
                                return Json(new { status = true, returl = returl });
                            case "TopUp":
                                var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                Session["TopupStatus"] = res;
                                returl = Url.Action("TopupConfirmation", "TopUpMobile");
                                return Json(new { status = true, returl = returl });
                        }

                    }
                    else
                    {

                        switch (model.RechargeValues.TransactionType)
                        {
                            case "Recharge":
                            case "PurchaseNewPlan":
                                returl = Url.Action("OrderFailed", "Recharge");
                                break;
                            case "CheckOut":
                                creditCartHandler.SaveNewPendingOrder(model, UserContext);
                                returl = Url.Action("OrderFailed", "Cart");
                                break;
                            case "TopUp":
                                returl = Url.Action("TopupFailed", "TopUpMobile");
                                break;
                        }

                    }
                    return Json(new { status = response.Status, returl = returl });
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                RazaLogger.WriteInfo(ex.Message);
            }



            RazaLogger.WriteInfo("Invalid Transaction in processcredit card..., status of ccresponse is" + response.Status);
            model.CentinelAuthenticateResponse = response;
            Session["CartAuthenticateModel"] = model;
            model.FinalResponseMessage = string.Format("Invalid Transaction {0}", response.Message);
            model.FinalTransactionStatus = false;

            switch (model.RechargeValues.TransactionType)
            {
                case "Recharge":
                case "PurchaseNewPlan":
                    returl = Url.Action("OrderFailed", "Recharge");
                    break;
                case "CheckOut":
                    returl = Url.Action("OrderFailed", "Cart");
                    break;
                case "TopUp":
                    returl = Url.Action("TopupFailed", "TopUpMobile");
                    break;
            }


            return Json(new { returl = returl });



        }


        #region Previous Commented ProcessCardLookUp

        //[HttpPost]
        //[Authorize]
        //public JsonResult ProcessCardLookUp(Recharge recharge)
        //{
        //    if (recharge.CouponCode == null || recharge.CouponCode.ToLower() == "null")
        //    {
        //        recharge.CouponCode = string.Empty;
        //    }
        //    if (recharge.ExpDate !=null && recharge.ExpDate.Contains("/"))
        //    {
        //        var expdate = recharge.ExpDate.Replace("/", "");
        //        recharge.ExpDate = expdate;
        //    }
        //    recharge.IpAddress = Request.ServerVariables["REMOTE_ADDR"];

        //    string currencyCode = string.Empty;

        //    switch (recharge.CurrencyCode)
        //    {
        //        case "USD":
        //            currencyCode = "840";
        //            break;
        //        case "CAD":
        //            currencyCode = "124";
        //            break;
        //        case "GBP":
        //            currencyCode = "826";
        //            break;
        //        default:
        //            currencyCode = "840";
        //            break;
        //    }

        //    if (Session["RegPinlessNumber"] != null)
        //    {
        //        recharge.PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
        //        Session["RegPinlessNumber"] = null;
        //    }

        //    string prefix = string.Empty;

        //    string transmode = string.Empty;


        //    if (recharge.TransactionType == "PurchaseNewPlan") // existing customer new plan
        //    {
        //        prefix = "OS";
        //        transmode = "S";
        //    }
        //    else if (recharge.TransactionType == "Recharge") //// existing customer existing plan
        //    {
        //        prefix = "OR";
        //        transmode = "R";
        //    }
        //    else if (recharge.TransactionType == "CheckOut") //// new customer new plan
        //    {
        //        prefix = "OA";
        //        transmode = "S";
        //    }
        //    else if (recharge.TransactionType == "TopUp") //// new customer new plan
        //    {
        //        prefix = "TP";
        //        transmode = "S";
        //    }


        //    if (recharge.TransactionType == "Recharge")
        //    {
        //        recharge.OldOrderId = recharge.order_id;
        //    }

        //    string orderId = string.Format("{0}{1}", prefix,
        //        Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

        //    //if (recharge.TransactionType == "PurchaseNewPlan" || recharge.TransactionType == "CheckOut")
        //    recharge.order_id = orderId;


        //    var ccRequest = new CentinelRequest();

        //    ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
        //    ccRequest.add("MsgType", "cmpi_lookup");
        //    ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
        //    ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
        //    ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);

        //    if (recharge.PaymentType == "P")
        //    {
        //        ccRequest.add("TransactionType", "X"); // Express Checkout
        //    }
        //    else if (recharge.PaymentType == "C")
        //    {
        //        if (recharge.CardNumber.StartsWith("3")) // Amex card add 50
        //        {
        //            // EXTRA 50¢ WILL BE CHARGED FOR AMEX CREDIT CARD
        //            recharge.Amount += 0.5;
        //        }

        //        if (string.IsNullOrEmpty(recharge.ExpMonth))
        //        {
        //            var listOfCards = _repository.Get_top_CreditCard(UserContext.MemberId);

        //            var selectedCard = listOfCards.GetCardList.FirstOrDefault(a => a.CreditCardNumber == recharge.CardNumber);

        //            if (selectedCard != null)
        //            {
        //                recharge.ExpMonth = selectedCard.ExpiryMonth;
        //                recharge.ExpYear = selectedCard.ExpiryYear;
        //            }
        //            else
        //            {
        //                return Json(new { Message = "Incomplete Card Information", status = false }, JsonRequestBehavior.AllowGet);
        //            }

        //            //call the web service method for validate the card - mohi 
        //            if (recharge.CouponCode == null)
        //            {
        //                recharge.CouponCode = string.Empty;
        //            }
        //            var res = _repository.ValidateCustomer(UserContext.MemberId, recharge.CardNumber, recharge.CardId,
        //                recharge.Amount, transmode, recharge.Countryfrom, recharge.Countryto, recharge.CouponCode);
        //            if (!res)
        //            {
        //                //return if validation  fail.
        //                return Json(new { Message = "Invalid Card Information", status = false }, JsonRequestBehavior.AllowGet);
        //            }

        //        }

        //        ccRequest.add("TransactionType", recharge.PaymentType);
        //        ccRequest.add("CardNumber", recharge.CardNumber);
        //        ccRequest.add("CardExpMonth", recharge.ExpMonth);
        //        ccRequest.add("CardExpYear", DateTime.Now.Year.ToString().Substring(0, 2) + recharge.ExpYear);
        //    }
        //    else
        //    {
        //        return Json(new { Message = "Invalid Transactions", status = false }, JsonRequestBehavior.AllowGet);
        //    }

        //    var randomOrderNumber = new Random().Next(10000000, 99999999);
        //    double amount = recharge.Amount * 100;

        //    ccRequest.add("OrderNumber", recharge.order_id == null ? orderId : recharge.order_id);// );
        //    ccRequest.add("OrderDescription", "This is where you put the order description");
        //    ccRequest.add("Amount", amount.ToString());
        //    ccRequest.add("CurrencyCode", currencyCode); // todo:remove hard coded currency
        //    ccRequest.add("UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]);
        //    ccRequest.add("BrowserHeader", Request.ServerVariables["HTTP_ACCEPT"]);
        //    ccRequest.add("IPAddress", Request.ServerVariables["REMOTE_ADDR"]);

        //    if (recharge.PaymentType == "P")
        //    {
        //        ccRequest.add("EMail", string.Empty);
        //        ccRequest.add("TransactionAction", "Authorization");
        //        ccRequest.add("NoShipping", "N");
        //        ccRequest.add("OverrideAddress", "N");
        //        ccRequest.add("ForceAddress", "N");
        //    }
        //    else
        //    {
        //        ccRequest.add("Installment", "");
        //        ccRequest.add("EMail", UserContext.Email);
        //        ccRequest.add("BillingFirstName", UserContext.FirstName);
        //        ccRequest.add("BillingLastName", UserContext.LastName);
        //        ccRequest.add("BillingAddress1", UserContext.ProfileInfo.Address);
        //        ccRequest.add("BillingAddress2", "");
        //        ccRequest.add("BillingCity", UserContext.ProfileInfo.City);
        //        ccRequest.add("BillingState", UserContext.ProfileInfo.State);
        //        ccRequest.add("BillingCountryCode", UserContext.ProfileInfo.Country);
        //        ccRequest.add("BillingPostalCode", UserContext.ProfileInfo.ZipCode);

        //        ccRequest.add("ShippingFirstName", UserContext.FirstName);
        //        ccRequest.add("ShippingLastName", UserContext.LastName);
        //        ccRequest.add("ShippingAddress1", UserContext.ProfileInfo.Address);

        //        ccRequest.add("ShippingAddress2", "");
        //        ccRequest.add("ShippingCity", UserContext.ProfileInfo.City);
        //        ccRequest.add("ShippingState", UserContext.ProfileInfo.State);
        //        ccRequest.add("ShippingCountryCode", UserContext.ProfileInfo.Country);
        //        ccRequest.add("ShippingPostalCode", UserContext.ProfileInfo.ZipCode);

        //    }


        //    CentinelResponse ccResponse;
        //    try
        //    {
        //        ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
        //    }
        //    catch (Exception ex)
        //    {
        //        RazaLogger.WriteInfo(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));
        //        return Json(new
        //        {
        //            Message = "Internal error with payment gateway. Please try later.",
        //            status = false,
        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //    var errorNo = ccResponse.getValue("ErrorNo");
        //    var errorDesc = ccResponse.getValue("ErrorDesc");
        //    var enrolled = ccResponse.getValue("Enrolled");
        //    var payload = ccResponse.getValue("Payload");
        //    var acsurl = ccResponse.getValue("ACSUrl");
        //    var transactionId = ccResponse.getValue("TransactionId");

        //    RazaLogger.WriteInfo("Transaction Id " + transactionId);
        //    string message = string.Empty;
        //    bool status = false;

        //    if (errorNo == "0" || errorNo == "")
        //    {
        //        switch (enrolled)
        //        {
        //            case "Y":
        //                message = "Cardholder authentication is available.";
        //                status = true;
        //                break;
        //            case "N":
        //                message = "Cardholder not enrolled in authentication program.";
        //                break;
        //            case "U":
        //                message = "Cardholder authentication is unavailable.";
        //                break;
        //        }

        //        string Centinel_RetUrl = ConfigurationManager.AppSettings["Centinel_RetUrl"];
        //        var cartAuthenticateModel = new CartAuthenticateModel()
        //        {
        //            CentinelPayload = payload,
        //            CentinelTermURL = Centinel_RetUrl,
        //            Centinel_ACSURL = acsurl,
        //            Centinel_TransactionId = transactionId,
        //            Centinel_TransactionType = recharge.PaymentType,
        //            RechargeValues = recharge,
        //            IsCcProcess = true
        //        };

        //        Session["CartAuthenticateModel"] = cartAuthenticateModel;


        //        return Json(new
        //        {
        //            Message = message,
        //            status = status,
        //            CentinelPayload = payload,
        //            Centinel_ACSURL = acsurl,
        //            CentinelTermURL = Centinel_RetUrl,
        //            Centinel_TransactionId = transactionId,

        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        switch (enrolled)
        //        {
        //            case "Y":
        //                message =
        //                    string.Format(
        //                    "Cardholder authentication is available. However cannot be processed due to server error : Error No: {0} Error Desc: {1}",
        //                        errorNo, errorDesc);
        //                break;
        //            case "N":
        //                message = string.Format("Card holder not enrolled in authentication program. Error No {0} Desc: {1}", errorNo, errorDesc);
        //                break;
        //            case "U":
        //                message = string.Format("Card holder authentication is unavailable. Error No {0} Desc: {1}", errorNo, errorDesc);
        //                break;
        //            default:
        //                message = errorDesc;
        //                break;
        //        }

        //        return Json(new { Message = message, status = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion


        [HttpPost]
        [Authorize]
        [RequiresSSL]
        public JsonResult ProcessCardLookUp(Recharge recharge)
        {
            var model = CardLookUp(recharge);

            return Json(new
               {
                   Message = model.Errormsg,
                   status = model.Status,
                   //CentinelPayload = payload,
                   //Centinel_ACSURL = acsurl,
                   //CentinelTermURL = Centinel_RetUrl,
                   //Centinel_TransactionId = transactionId,

               }, JsonRequestBehavior.AllowGet);

        }

        [CompressFilter]
        [RequiresSSL]
        public ActionResult AuthenticateInfo()
        {
            if (Session["CartAuthenticateModel"] != null)
            {


                CartAuthenticateModel model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
                RazaLogger.WriteInfo(string.Format("Parameter In AuthenticateInfo: Centinel_ACSURL={0},CentinelPayload={1},CentinelTermURL={2},Centinel_TransactionId={3}", model.Centinel_ACSURL, model.CentinelPayload,
                    model.CentinelTermURL, model.Centinel_TransactionId));
                return View(model);
            }

            return View();
        }

        [RequiresSSL]
        public ActionResult ccVerifier()
        {
            if (Session[GlobalSetting.CheckOutSesionKey] != null)
            {
                TempData["PaRes"] = Request.Form["PaRes"];
                return RedirectToAction("PaymentVerifier", "Paypal", new { area = "Payment" });
            }

            //RazaLogger.WriteInfo("processing start In ccVerifier method.");

            RazaLogger.WriteInfo2("processing start In ccVerifier method.");
            CreditCartHandler creditCartHandler = new CreditCartHandler();

            CartAuthenticateModel model = Session["CartAuthenticateModel"] as CartAuthenticateModel;



            if (model == null)
            {
                return View(new CCVerifyModel { Message = "Cart is blank." });
            }

            if (model.RechargeValues.TransactionType.ToLower() == "recharge" && model.RechargeValues.PaymentType == "P")
            {
                //RazaLogger.WriteInfo2("processing start In ccVerifier method. with Order Id:" +
                //                      model.RechargeValues.order_id);
            }


            #region CentinelProcessing

            var pares = Request.Form["PaRes"];
            var merchant_data = Request.Form["MD"];

            if (string.IsNullOrEmpty(pares))
            {
                return View(new CCVerifyModel { Message = "No Response from Server" });
            }
            model.RechargeValues.IpAddress = Request.ServerVariables["REMOTE_ADDR"];

            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_authenticate");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("PAResPayload", pares);
            ccRequest.add("TransactionId", model.Centinel_TransactionId);

            //if (model.RechargeValues.TransactionType == "Recharge")
            //RazaLogger.WriteInfoPartial(string.Format("PaymentType {0}", model.RechargeValues.PaymentType));

            if (model.RechargeValues.PaymentType == "P" || model.RechargeValues.PaymentType == "X")
            {
                //RazaLogger.WriteInfo("Payment Type " + model.RechargeValues.PaymentType);
                //if (model.RechargeValues.TransactionType == "Recharge")
                //RazaLogger.WriteInfoPartial("Payment Type " + model.RechargeValues.PaymentType);
                ccRequest.add("TransactionType", "X"); // Express Checkout    
                //Centinel_TransactionType
            }
            else
            {
                ccRequest.add("TransactionType", "C");

                //Added on 11/18/14 TO WRITE LOG
                //if (model.RechargeValues.PaymentType == "C" && model.RechargeValues.TransactionType == "CheckOut")
                //    RazaLogger.WriteInfoPartial("Payment Type " + model.RechargeValues.PaymentType);
            }

            //        '=====================================================================================
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
                ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            }
            catch (Exception ex)
            {
                //if (model.RechargeValues.PaymentType == "C")
                //{
                //    if (model.RechargeValues.TransactionType == "CheckOut")
                //        RazaLogger.WriteInfoPartial(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));
                //}

                //if (model.RechargeValues.TransactionType == "Recharge")
                //RazaLogger.WriteInfoPartial(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));

                return RedirectToAction("OrderFailed", "Cart");
                return Json(new
                {
                    Message = "Internal error with payment gateway. Please try later.",
                    status = false,
                }, JsonRequestBehavior.AllowGet);
            }

            var errorNo = ccResponse.getValue("ErrorNo");
            var errorDesc = ccResponse.getValue("ErrorDesc");

            CentinelAuthenticateResponse response = new CentinelAuthenticateResponse()
            {
                CentinelPAResStatus = ccResponse.getValue("PAResStatus"),
                CentinelSignatureVerification = ccResponse.getValue("SignatureVerification"),
                CentinelEciFlag = ccResponse.getValue("EciFlag"),
                Centinel_CAVV = ccResponse.getValue("Cavv"),
                Centinel_XID = ccResponse.getValue("Xid"),
                ConsumerName = ccResponse.getValue("ConsumerStatus"),
                AddressStatus = ccResponse.getValue("AddressStatus"),

            };

            model.CentinelAuthenticateResponse = response;


            if (model.RechargeValues.PaymentType == "P") //Paypal
            {
                #region Centinel Process for PayPal

                //if (errorNo == "0" && response.CentinelSignatureVerification == "Y" &&
                //    response.CentinelPAResStatus == "Y")
                if (errorNo == "0" && (response.CentinelPAResStatus == "Y" || response.CentinelPAResStatus == "P"))
                {
                    //response.Message =
                    //RazaLogger.WriteInfo("Your authentication completed successfully.");

                    if (model.RechargeValues.TransactionType == "CheckOut")

                        RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + " Your PayPal authentication completed successfully.");

                    response.Status = true;
                }
                //else if (errorNo == "0" && response.CentinelSignatureVerification == "Y" &&
                //         response.CentinelPAResStatus == "P")
                //{
                //    RazaLogger.WriteInfo("Your authentication completed however payment is currently pending.");
                //    // authorize and saveneworder, consumerstatus== verified, addressstatus== confirmed.
                //    if (model.RechargeValues.TransactionType == "CheckOut" &&
                //        response.ConsumerStatus.ToLower() == "verified" &&
                //        response.ConsumerStatus.ToLower() == "confirmed")
                //    {
                //        RazaLogger.WriteInfo("savepending order in paypal case");
                //        creditCartHandler.SaveNewPendingOrder(model, UserContext);
                //    }

                //    response.Status = false;
                //}
                else if (errorNo == "0" && response.CentinelSignatureVerification == "Y" &&
                         response.CentinelPAResStatus == "D")
                {
                    response.Message =
                        "You cancelled your transaction for Data Reasons. Please update your information and select another form of payment.";
                    response.Status = false;
                }
                else if (errorNo == "0" && response.CentinelSignatureVerification == "Y" &&
                         response.CentinelPAResStatus == "X")
                {
                    response.Message =
                        "Your transaction was canceled prior to completion. Please provide another form of payment.";
                    response.Status = false;
                }
                else
                {
                    response.Message =
                        "PayPal express checkout payment was unable to complete. Please provide another form of payment to complete your transaction.";

                    //RazaLogger.WriteInfo(string.Format("Your authentication failed with error no and desc. {0} {1}", errorNo, errorDesc));

                    //if (model.RechargeValues.TransactionType == "CheckOut")
                    //    RazaLogger.WriteInfoPartial(string.Format("Your authentication failed with error no and desc. {0} {1}", errorNo, errorDesc));
                    response.Status = false;
                }
                #endregion
            }
            else if (model.RechargeValues.PaymentType == "C")
            {
                #region Centinel Process for Credit Card

                //if (model.RechargeValues.TransactionType == "CheckOut")
                //    RazaLogger.WriteInfoPartial(string.Format("Your CentinelAuthenticateResponse: errorNo {0} errorDesc {1} CentinelPAResStatus {2}", errorNo, errorDesc, response.CentinelPAResStatus));

                if (response.CentinelPAResStatus == "Y" || response.CentinelPAResStatus == "A")
                {
                    response.Status = true;
                }
                else
                {
                    response.Message = "Your transaction could not complete. Please provide another form of payment.";
                    response.Status = false;
                }
                #endregion
            }

            #endregion

            //RazaLogger.WriteInfo("status of ccresponse is : " + response.Status.ToString());
            //if (model.RechargeValues.PaymentType == "C" && model.RechargeValues.TransactionType == "CheckOut")
            //    RazaLogger.WriteInfoPartial("status of ccresponse is : " + response.Status.ToString());

            if (response.Status)
            {
                try
                {

                    #region Paypal Txn
                    if (model.RechargeValues.PaymentType == "P")
                    {
                        //ConsumerStatus, AddressStatus, ConsumerName, EMail, PayerId

                        response.ConsumerName = ccResponse.getValue("ConsumerName");
                        response.ConsumerStatus = ccResponse.getValue("ConsumerStatus");
                        response.AddressStatus = ccResponse.getValue("AddressStatus");
                        response.EMail = ccResponse.getValue("EMail");
                        response.PayerId = ccResponse.getValue("Payer");
                        var cent_transId = ccResponse.getValue("TransactionId");

                        if (model.RechargeValues.TransactionType == "CheckOut")
                        {
                            // check for payerId if already exists then return error message
                            // Write in Log table and send message that Payment is not successfull"

                            if (_repository.IsPayerIdExist(response.PayerId, response.ConsumerStatus,
                                response.AddressStatus, model.RechargeValues.Countryfrom.ToString()))
                            {
                                RazaLogger.WriteInfoPartial(string.Format("CustID {0} PayerId {1} is already exist ", UserContext.MemberId, response.PayerId));
                                ccResponse = SendCmpiAuthorize(model, ccResponse);
                                errorNo = ccResponse.getValue("ErrorNo");
                                errorDesc = ccResponse.getValue("ErrorDesc");
                                var stcode = ccResponse.getValue("StatusCode");
                                //RazaLogger.WriteInfo("Response of cmpiauthorization: errorno=" + errorNo + "statuscode=" + stcode + "errordesc=" + errorDesc);


                                RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + " Response of cmpiauthorization: errorno=" + errorNo + "statuscode=" +
                     stcode + "errordesc=" + errorDesc);
                                if (errorNo == "0" && stcode == "Y")
                                {

                                    response.Status = false;
                                    model.CentinelAuthenticateResponse = response;
                                    Session["CartAuthenticateModel"] = model;
                                    model.FinalResponseMessage = "Customer is not new";
                                    model.FinalTransactionStatus = false;
                                    if (model.RechargeValues.TransactionType == "CheckOut")
                                    {
                                        model.Centinel_TransactionId = ccResponse.getValue("TransactionId");
                                        model.CcAuthTransactionId = model.Centinel_TransactionId;
                                        RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + " PayerId exist, calling saveNewPendingOrder");
                                        creditCartHandler.SaveNewPendingOrder(model, UserContext);
                                        return RedirectToAction("Confirmation", "Cart");
                                    }
                                }
                                return RedirectToAction("OrderFailed", "Cart");
                            }
                            else
                            {

                                if (response.ConsumerStatus.ToLower() != "verified" || response.AddressStatus.ToLower() != "confirmed"
                                    && (model.RechargeValues.Country == "U.S.A." || model.RechargeValues.Country.ToLower() == "canada"))
                                {
                                    ccResponse = SendCmpiAuthorize(model, ccResponse);
                                    errorNo = ccResponse.getValue("ErrorNo");
                                    errorDesc = ccResponse.getValue("ErrorDesc");
                                    var stcode = ccResponse.getValue("StatusCode");
                                    if (errorNo == "0" && stcode == "Y")
                                    {
                                        //RazaLogger.WriteInfo(
                                        //    string.Format(
                                        //            "transaction failed in paypal, consumerstatus={0},AddressStatus={1}",
                                        //            response.ConsumerStatus, response.AddressStatus));

                                        //                                    if (model.RechargeValues.TransactionType == "Recharge")
                                        //                                    RazaLogger.WriteInfoPartial(
                                        //string.Format(
                                        //        "transaction failed in paypal, consumerstatus={0},AddressStatus={1}",
                                        //        response.ConsumerStatus, response.AddressStatus));

                                        response.Status = false;
                                        model.CentinelAuthenticateResponse = response;
                                        Session["CartAuthenticateModel"] = model;
                                        model.FinalResponseMessage = "Customer is not new";
                                        model.FinalTransactionStatus = false;
                                        if (model.RechargeValues.TransactionType == "CheckOut")
                                        {
                                            model.Centinel_TransactionId = ccResponse.getValue("TransactionId");
                                            model.CcAuthTransactionId = model.Centinel_TransactionId;
                                            RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + ",Country: " + model.RechargeValues.Country + " Customer Not Verified, calling saveNewPendingOrder");
                                            creditCartHandler.SaveNewPendingOrder(model, UserContext);
                                            return RedirectToAction("Confirmation", "Cart");
                                        }
                                    }
                                    return RedirectToAction("OrderFailed", "Cart");

                                }

                            }

                        }

                        // Send CMPI Sale Message
                        //if (model.RechargeValues.CardId == 9999)
                        //{
                        //    if (model.RechargeValues.TransactionType == "CheckOut")
                        //    {
                        //        RazaLogger.WriteInfo2("calling issue new pin for free trial plan");
                        //        creditCartHandler.IssueNewPlan(model, UserContext);
                        //    }
                        //    return RedirectToAction("OrderConfirmation", "Cart");

                        //}


                        //RazaLogger.WriteInfoPartial("CustID: " + model.RechargeValues.MemberID + " Sending SendCmpiSaleMessage");

                        ccResponse = SendCmpiSaleMessage(model, ccResponse);


                        errorNo = ccResponse.getValue("ErrorNo");

                        errorDesc = ccResponse.getValue("ErrorDesc");

                        var Centinel_PaypalTransactionId = ccResponse.getValue("TransactionId");

                        var StatusCode = ccResponse.getValue("StatusCode");

                        var AVSResult = ccResponse.getValue("AVSResult");

                        //RazaLogger.WriteInfo(string.Format("Get response of CMPI_Sale message as error no: {0} {1} {2} {3}", errorNo, errorDesc, StatusCode, AVSResult));
                        if (model.RechargeValues.TransactionType == "CheckOut")
                            RazaLogger.WriteInfoPartial(
                                string.Format(
                                    "CustID: {0}. Get response of CMPI_Sale message as error no: errorNo={1},errorDesc={2},StatusCode={3}, AVSResult={4}, PayPal_TransactionID={5}",
                                    UserContext.MemberId, errorNo, errorDesc, StatusCode, AVSResult, Centinel_PaypalTransactionId));

                        response.Status = false;

                        if (errorNo == "0" && StatusCode == "Y")
                        {
                            response.Message = "Congratulations! Your Order is Approved.";

                            response.Status = true;
                        }
                        else if (errorNo == "0" && StatusCode == "N")
                        {
                            response.Message = "Transaction Declined";
                            response.Status = false;
                        }
                        else if (errorNo == "0" && StatusCode == "E")
                        {
                            response.Message = "Transaction resulted in Error";
                            response.Status = false;
                        }
                        else
                        {
                            response.Message = errorDesc;
                            response.Status = false;
                        }

                        model.FinalResponseMessage = response.Message;
                        model.FinalTransactionStatus = response.Status;
                        response.AVSResponse = AVSResult;
                        model.CentinelAuthenticateResponse = response;
                        model.Centinel_TransactionId = Centinel_PaypalTransactionId;
                        Session["CartAuthenticateModel"] = model;

                        //if (model.RechargeValues.TransactionType == "Recharge")
                        //RazaLogger.WriteInfoPartial("transaction type is " + model.RechargeValues.TransactionType + "");

                        switch (model.RechargeValues.TransactionType)
                        {
                            case "Recharge":
                                if (response.Status)
                                {
                                    //RazaLogger.WriteInfoPartial("Calling Recharge Pin");
                                    creditCartHandler.RechargePin(model, UserContext);
                                }

                                return RedirectToAction("RechargeConfirmation", "Recharge");

                                break;
                            case "PurchaseNewPlan":
                                if (response.Status)
                                {
                                    RazaLogger.WriteInfo("Calling purchase Pin");
                                    creditCartHandler.IssueNewPlan(model, UserContext);
                                }
                                return RedirectToAction("OrderConfirmation", "Cart");
                                break;
                            case "CheckOut":
                                if (response.Status)
                                {
                                    RazaLogger.WriteInfoPartial("CustID: " + UserContext.MemberId + " Calling Issue NewPin");
                                    creditCartHandler.IssueNewPlan(model, UserContext);
                                }
                                return RedirectToAction("OrderConfirmation", "Cart");
                                break;

                        }
                    }

                    #endregion

                    #region Credit Card Processing

                    if (model.RechargeValues.PaymentType == "C")
                    {
                        model.CentinelAuthenticateResponse = response;
                        Session["CartAuthenticateModel"] = model;
                        RazaLogger.WriteInfo(model.RechargeValues.TransactionType);
                        RazaLogger.WriteInfo("Credit Card Processing..");
                        // Payflow Pro implementations

                        // var result = new Payment();
                        //in case of tryus fee only authorize thae card.
                        RazaLogger.WriteInfo("Calling CreditCardAuthorize");
                        var result = creditCartHandler.CreditCardAuthorize(model, UserContext);


                        //if (!string.IsNullOrEmpty(creditCartHandler.CreditCardAuthorize(model, UserContext).IAVS))
                        if (result.RESULT == "0" && !string.IsNullOrEmpty(result.IAVS) &&
                            ((result.CVV2MATCH == "Y" || result.CVV2MATCH == "X") &&
                            (model.RechargeValues.AvsByPass == true || (model.RechargeValues.AvsByPass != true && result.AVSADDR == "Y" && result.AVSZIP == "Y"))))
                        {
                            #region PROCESS TRANSACTION AFTER SUCCESSFULL CREDIT CARD AUTHORIZATION
                            RazaLogger.WriteInfo2("Calling CreditCardPayment");

                            // for tryusfree txns
                            RazaLogger.WriteInfo2("CardID is :" + model.RechargeValues.CardId);
                            if (model.RechargeValues.CardId == 9999)
                            {
                                if (model.RechargeValues.TransactionType == "CheckOut")
                                {
                                    model.CentinelAuthenticateResponse.AVSResponse = "YY";
                                    RazaLogger.WriteInfo2("calling issue new pin for free trial plan");
                                    model.CentinelAuthenticateResponse.AVSResponse = result.AVSADDR + result.AVSZIP;
                                    model.Centinel_TransactionId = result.PNREF;
                                    model.CentinelAuthenticateResponse.CVVResponse = result.CVV2MATCH;

                                    creditCartHandler.IssueNewPlan(model, UserContext);
                                    var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                                }

                                return RedirectToAction("OrderConfirmation", "Cart");

                            }

                            //  var result = creditCartHandler.CreditCardPayment(model, UserContext);
                            var cap_auth = creditCartHandler.CreditCardCapture(model, UserContext, result.PNREF);
                            // CVV response ="Y" OR "X"
                            // AVS ADDR =="Y" and AVSZIP ="Y"

                            if (model.RechargeValues.TransactionType == "CheckOut")
                                RazaLogger.WriteInfoPartial(string.Format("response of credit card payment: cvv2match={0},AVSADDR={1},AVSZIP={2}", result.CVV2MATCH, result.AVSADDR, result.AVSZIP));

                            if (cap_auth.RESULT == "0")
                            {
                                response.Status = true;
                                model.CentinelAuthenticateResponse.AVSResponse = result.AVSADDR + result.AVSZIP;
                                model.Centinel_TransactionId = cap_auth.PNREF;
                                model.CentinelAuthenticateResponse.CVVResponse = result.CVV2MATCH;

                            }
                            else
                            {

                                var void_auth = creditCartHandler.CreditCardVoid(model, UserContext, result.PNREF);
                                response.Message = "Payment authorization failed.";
                                response.Status = false;
                            }
                            #endregion
                        }
                        else
                        {
                            RazaLogger.WriteInfo(string.Format("In CreditCard Else: {0},{1},{2},{3},{4}", result.RESULT,
                                result.IAVS,
                                result.CVV2MATCH, result.AVSADDR, result.AVSZIP));
                            if ((result.RESULT == "0" || result.RESULT == "114") && !string.IsNullOrEmpty(result.IAVS) &&
                                ((result.CVV2MATCH != "Y" || result.CVV2MATCH != "X")) || (result.AVSADDR != "Y" || result.AVSZIP != "Y"))
                            {
                                if (result.CVV2MATCH == "N" && result.AVSADDR == "N" && result.AVSZIP == "N")
                                {
                                    //if (model.RechargeValues.TransactionType == "CheckOut")
                                    //    RazaLogger.WriteInfoPartial("Cvv and address doesn't match. redirect to RevieOrder");
                                    model.FinalResponseMessage = "A";
                                    Session["CartAuthenticateModel"] = model;

                                }
                                else if (result.CVV2MATCH.Trim() == "N")
                                {
                                    //if (model.RechargeValues.TransactionType == "CheckOut")
                                    //    RazaLogger.WriteInfoPartial("Cvv doesn't match. redirect to RevieOrder");
                                    model.FinalResponseMessage = "B";
                                    Session["CartAuthenticateModel"] = model;

                                }
                                else if (result.AVSADDR.Trim() == "N" || result.AVSZIP.Trim() == "N")
                                {
                                    //if (model.RechargeValues.TransactionType == "CheckOut")
                                    //    RazaLogger.WriteInfoPartial("Adress and Zipcode/PostalCode  doesn't match. redirect to RevieOrder");
                                    model.FinalResponseMessage = "C";
                                    Session["CartAuthenticateModel"] = model;

                                }
                                return RedirectToAction("ReviewOrder", "Cart");
                            }
                            if ((result.RESULT == "0" || result.RESULT == "114") && !string.IsNullOrEmpty(result.IAVS) &&
                                ((result.AVSADDR != "Y" || result.AVSZIP != "Y")))
                            {
                                //if (model.RechargeValues.TransactionType == "CheckOut")
                                //    RazaLogger.WriteInfoPartial("Cvv doesn't match. redirect to RevieOrder");
                                Session["CartAuthenticateModel"] = model;
                                model.FinalResponseMessage = "Address or Zip Code / Postal Code does not match.";
                                return RedirectToAction("ReviewOrder", "Cart");
                            }
                            if (model.RechargeValues.TransactionType == "CheckOut")
                            {
                                model.Centinel_TransactionId = ccResponse.getValue("TransactionId");
                                model.CcAuthTransactionId = result.PNREF;

                                //if (model.RechargeValues.TransactionType == "CheckOut")
                                //    RazaLogger.WriteInfoPartial("creditcard authorization failed, calling saveNewPendingOrder");

                                creditCartHandler.SaveNewPendingOrder(model, UserContext);
                                return RedirectToAction("Confirmation", "Cart");
                            }
                            response.Status = false;
                            model.FinalResponseMessage = response.Message;
                            model.FinalTransactionStatus = response.Status;
                            model.CentinelAuthenticateResponse = response;
                            Session["CartAuthenticateModel"] = model;

                            switch (model.RechargeValues.TransactionType)
                            {
                                case "Recharge":
                                case "PurchaseNewPlan":
                                    return RedirectToAction("OrderFailed", "Recharge"); //redirect it to RechargeFailed
                                case "CheckOut":
                                    RazaLogger.WriteInfo("credit card authorization failed, calling saveNewPendingOrder");
                                    creditCartHandler.SaveNewPendingOrder(model, UserContext);
                                    return RedirectToAction("OrderFailed", "Cart");
                                case "TopUp":
                                    return RedirectToAction("TopupFailed", "TopUpMobile");
                            }
                        }

                        if (response.Status) //Successful messages
                        {
                            switch (model.RechargeValues.TransactionType)
                            {
                                case "Recharge":
                                    creditCartHandler.RechargePin(model, UserContext);
                                    return RedirectToAction("RechargeConfirmation", "Recharge");
                                    break;
                                case "PurchaseNewPlan":
                                    creditCartHandler.IssueNewPlan(model, UserContext);
                                    return RedirectToAction("OrderConfirmation", "Cart");
                                    break;
                                case "CheckOut":
                                    creditCartHandler.IssueNewPlan(model, UserContext);
                                    return RedirectToAction("OrderConfirmation", "Cart");
                                    break;
                                case "TopUp":
                                    var res = creditCartHandler.MobileTopUpRecharge(model, UserContext);
                                    Session["TopupStatus"] = res;
                                    return RedirectToAction("TopupConfirmation", "TopUpMobile");
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.Message = ex.Message;
                    RazaLogger.WriteInfo(ex.Message);
                }
            }


            RazaLogger.WriteInfo("Invalid Transaction in cc verifier..., status of ccresponse is" + response.Status);
            model.CentinelAuthenticateResponse = response;
            Session["CartAuthenticateModel"] = model;
            model.FinalResponseMessage = string.Format("Invalid Transaction {0}", response.Message);
            model.FinalTransactionStatus = false;

            switch (model.RechargeValues.TransactionType)
            {
                case "Recharge":
                case "PurchaseNewPlan":
                    return RedirectToAction("OrderFailed", "Recharge"); //redirect it to failed recharge
                case "CheckOut":
                    return RedirectToAction("OrderFailed", "Cart");
                case "TopUp":
                    return RedirectToAction("TopupFailed", "TopUpMobile");
            }

            return RedirectToAction("OrderFailed", "Cart");
            return View();

        }


        private CentinelResponse SendCmpiAuthorize(CartAuthenticateModel model, CentinelResponse ccResponse)
        {
            string currencyCode = string.Empty;
            switch (model.RechargeValues.CurrencyCode)
            {
                case "USD":
                    currencyCode = "840";
                    break;//todo: Add more currencies
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


            CentinelRequest ccRequest;
            ccRequest = new CentinelRequest();
            string date = DateTime.Now.ToShortDateString();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_authorize");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("TransactionId", model.Centinel_TransactionId);

            RazaLogger.WriteInfo("Txn Id " + model.Centinel_TransactionId);
            ccRequest.add("TransactionType", "X");
            //ccRequest.add("TransactionMode", "S");
            //ccRequest.add("CategoryCode", "5400");
            //ccRequest.add("CustomerFlag", "N");
            //ccRequest.add("CustomerRegistrationDate", date);

            ccRequest.add("OrderNumber", model.RechargeValues.order_id);

            RazaLogger.WriteInfo("Order Number " + model.RechargeValues.order_id);

            ccRequest.add("OrderDescription", "Raza.com Online order.");

            RazaLogger.WriteInfo("Amount " + model.RechargeValues.Amount);
            double tmpamount = (model.RechargeValues.Amount + model.RechargeValues.ServiceFee) * 100;
            ccRequest.add("Amount", tmpamount.ToString());

            ccRequest.add("CurrencyCode", currencyCode);

            RazaLogger.WriteInfo("Email " + UserContext.Email);
            ccRequest.add("Email", UserContext.Email);
            ccRequest.add("IPAddress", Request.ServerVariables["REMOTE_ADDR"]);
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


        private CentinelResponse SendCmpiSaleMessage(CartAuthenticateModel model, CentinelResponse ccResponse)
        {
            string currencyCode = string.Empty;
            switch (model.RechargeValues.CurrencyCode)
            {
                case "USD":
                    currencyCode = "840";
                    break;//todo: Add more currencies
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

            CentinelRequest ccRequest;
            ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_sale");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);
            ccRequest.add("TransactionId", model.Centinel_TransactionId);

            RazaLogger.WriteInfo("Txn Id " + model.Centinel_TransactionId);
            ccRequest.add("TransactionType", "X");

            ccRequest.add("OrderNumber", model.RechargeValues.order_id);

            RazaLogger.WriteInfo("Order Number " + model.RechargeValues.order_id);

            ccRequest.add("OrderDescription", "Raza.com Online order.");

            RazaLogger.WriteInfo("Amount " + model.RechargeValues.Amount);
            double tmpamount = (model.RechargeValues.Amount + model.RechargeValues.ServiceFee) * 100;
            ccRequest.add("Amount", tmpamount.ToString());

            ccRequest.add("CurrencyCode", currencyCode);

            RazaLogger.WriteInfo("Email " + UserContext.Email);
            ccRequest.add("Email", UserContext.Email);
            ccRequest.add("IPAddress", Request.ServerVariables["REMOTE_ADDR"]);
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

            //RazaLogger.WriteInfo("Sending CMPI_Sale message");


            //if (model.RechargeValues.TransactionType.ToLower() == "recharge")
            //{
            //    RazaLogger.WriteInfoPartial(string.Format("Calling CMPI_Sale_PayPal : {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}", model.Centinel_TransactionId, model.RechargeValues.order_id,
            //        model.RechargeValues.Amount, currencyCode, UserContext.Email, Request.ServerVariables["REMOTE_ADDR"], UserContext.FirstName, UserContext.LastName, UserContext.ProfileInfo.Address, "",
            //        UserContext.ProfileInfo.City, UserContext.ProfileInfo.State, UserContext.ProfileInfo.Country, UserContext.ProfileInfo.ZipCode, UserContext.FirstName, UserContext.LastName, UserContext.ProfileInfo.Address,
            //        "", UserContext.ProfileInfo.City, UserContext.ProfileInfo.State, UserContext.ProfileInfo.Country, UserContext.ProfileInfo.ZipCode));
            //}


            ccResponse = ccRequest.sendHTTP(ConfigurationManager.AppSettings["CentinelTxnUrl"], 10000);
            return ccResponse;
        }

        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult OrderFailed()
        {
            var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
            Session["CartAuthenticateModel"] = null;
            if (model == null)
            {
                model = new CartAuthenticateModel();
            }
            model.FinalResponseMessage = "Transaction does not success, Please try again!!";
            return View(model);
        }


        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult OrderConfirmation()
        {
            RazaLogger.WriteInfo("order confirmation page.");
            try
            {
                //if (Session["CartAuthenticateModel"] as CartAuthenticateModel == null)
                //{
                //    RazaLogger.WriteInfo("Session[CartAuthenticateModel] is null.");
                //    return RedirectToAction("Confirmation", "Cart");
                //}
                var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
                Session["CartAuthenticateModel"] = null;
                if (model == null)
                {
                    RazaLogger.WriteInfo("cartauth model null");
                    model = new CartAuthenticateModel();
                    model.RechargeValues = new Recharge();
                }
                string newRegNumber = string.Empty;
                string searchtype = "S";
                if (model.RechargeValues.TransactionType == "PurchaseNewPlan" && (model.RechargeValues.PinlessNumbers != null && model.RechargeValues.PinlessNumbers.Any()))
                {
                    var PinlessNumbers = model.RechargeValues.PinlessNumbers.First();
                    newRegNumber = PinlessNumbers.PinlessNumber;
                    searchtype = "A";

                }
                else if (model.RechargeValues.TransactionType == "CheckOut")
                {
                    newRegNumber = model.RechargeValues.AniNumber;
                    searchtype = "A";
                }

                var accessnumbers = _repository.GetLocalAccessNumber(model.RechargeValues.State,
                    SafeConvert.ToString(model.RechargeValues.Countryfrom), newRegNumber, searchtype);
                model.RechargeValues.AniNumber = newRegNumber;
                var list = new List<string>();
                if (accessnumbers != null)
                    foreach (var number in accessnumbers.Take(5))
                    {
                        list.Add(number.AccessNumber);
                    }
                if (list.Count == 0)
                {
                    list.Add("1-877-777-3971 (U.S.A)");
                    list.Add("1-877-777-1674 (CANADA)");
                    list.Add("1-866-397-2880 (HAWAII, PUERTO RICO, US VIRGIN ISLANDS)");
                }

                model.LocalAccesNumberList = list;

                return View(model);
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo(ex.Message);
                return RedirectToAction("Confirmation");
            }
        }

        [Authorize]
        [HttpPost]
        [RequiresSSL]
        public JsonResult SavePendingOrder(RechargeInfo info)
        {
            info.MemberId = UserContext.MemberId;
            info.EmailAddress = UserContext.Email;
            if (info.PaymentType == "P")
            {
                info.PaymentMethod = "Paypal";
                info.CardNumber = string.Empty;
                info.CVV2 = string.Empty;
                info.ExpiryDate = string.Empty;

            }
            else if (info.PaymentType == "C")
            {
                info.PaymentMethod = "Credit Card";
                if (info.ExpiryDate != null && info.ExpiryDate.Contains("/"))
                {
                    var exp = info.ExpiryDate.Replace("/", "");
                    info.ExpiryDate = exp;
                }

            }

            string prefix = "OA";
            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));
            info.OrderId = orderId;

            if (string.IsNullOrEmpty(info.CoupanCode))
            {
                info.CoupanCode = string.Empty;
            }

            info.ProcessedBy = string.Empty;
            info.Address2 = string.Empty;
            info.IpAddress = Request.ServerVariables["REMOTE_ADDR"];
            if (UserContext.ProfileInfo.PhoneNumber.Contains("-"))
            {
                info.AniNumber = UserContext.ProfileInfo.PhoneNumber.Replace("-", "");
            }
            else
            {
                info.AniNumber = UserContext.ProfileInfo.PhoneNumber;
            }



            info.AuthTransactionId = string.Empty;
            info.CentinelPayLoad = string.Empty;
            info.CentinelTransactionId = string.Empty;
            info.PayResPayLoad = string.Empty;
            info.PayerId = string.Empty;

            var res = _repository.SaveNewPendingOrder(info);
            if (res)
            {
                return Json(new { status = true });
            }
            return Json(new { status = false });
        }

        [HttpPost]
        [Authorize]
        [RequiresSSL]
        public ActionResult GlobalPlanIssue(CheckOutModel checkOutModel)
        {
            try
            {

                var repository = new DataRepository();

                string country = string.Empty;
                var cFromList = CacheManager.Instance.GetFromCountries();
                if (cFromList != null)
                {
                    var cdata = cFromList.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                    if (cdata != null) country = cdata.Id;
                }
                if (checkOutModel.Country == "U.S.A.")
                {
                    checkOutModel.CardId = 161;
                }
                else if (checkOutModel.Country == "CANADA")
                {
                    checkOutModel.CardId = 162;
                }
                else if (checkOutModel.Country == "U.K.")
                {
                    checkOutModel.CardId = 102;

                }
                var rechargeInfo = new RechargeInfo
                {

                    Address1 = checkOutModel.Address,
                    Amount = checkOutModel.Amount,
                    ServiceFee = 0,
                    Country = country,
                    ZipCode = checkOutModel.ZipCode,
                    State = checkOutModel.State,
                    MemberId = UserContext.MemberId,
                    City = checkOutModel.City,
                    CardNumber = checkOutModel.CardNumber,
                    CVV2 = checkOutModel.CvvNumber,
                    PaymentMethod = checkOutModel.PaymentMethod,
                    ExpiryDate = string.Empty,
                    UserType = "Old",
                    CardId = checkOutModel.CardId,
                    CountryFrom = checkOutModel.CountryFrom,
                    CountryTo = checkOutModel.CountryTo,
                    CoupanCode = string.Empty,
                    AniNumber = UserContext.ProfileInfo.PhoneNumber,
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"],
                    Address2 = "",
                    PaymentType = checkOutModel.PaymentType,
                    IsCcProcess = false,
                    AVSResponse = string.Empty,
                    CVV2Response = string.Empty,
                    Cavv = string.Empty,
                    EciFlag = string.Empty,
                    PaymentTransactionId = string.Empty,
                    Xid = string.Empty,


                };

                var response = repository.IssueNewPin(rechargeInfo);

                if (response.IssuenewpinStatus)
                {
                    return Json(response);
                }
                else
                {
                    return Json(response);
                }

            }

            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { Message = ex.Message });
            }

            return null;

        }

        [CompressFilter]
        [Authorize]
        [HttpGet]
        [RequiresSSL]
        public ActionResult ReviewOrder()
        {
            var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
            //model.FinalResponseMessage = "Please Review your information.";

            string CardType = "Visa";
            if (model.RechargeValues.CardNumber.StartsWith("5"))
                CardType = "MasterCard";
            if (model.RechargeValues.CardNumber.StartsWith("3"))
                CardType = "Amex";
            if (model.RechargeValues.CardNumber.StartsWith("6"))
                CardType = "Discover";
            model.RechargeValues.CardType = CardType;

            //RazaLogger.WriteInfoPartial(string.Format("ReviewOrder with values cardnumber: {0},CardType: {1},ExpMonth: {2},ExpYear: {3},PlanName: {4},FirstName: {5},LastName: {6},State: {7},City: {8},ZipCode: {9},Country: {10},Address: {11},Amount: {12},ServiceFee: {13},CurrencyCode: {14},TransactionType: {15},error msg: {16},FullName: {17}" + model.RechargeValues.CardNumber, model.RechargeValues.CardNumber, model.RechargeValues.ExpMonth.ToString(), model.RechargeValues.ExpYear.ToString(), model.RechargeValues.PlanName, UserContext.ProfileInfo.FirstName, UserContext.ProfileInfo.LastName, UserContext.ProfileInfo.State, UserContext.ProfileInfo.City, UserContext.ProfileInfo.ZipCode, UserContext.ProfileInfo.Country, UserContext.ProfileInfo.Address, model.RechargeValues.Amount.ToString(), model.RechargeValues.ServiceFee.ToString(), model.RechargeValues.CurrencyCode, model.RechargeValues.TransactionType, model.FinalResponseMessage, UserContext.FullName));

            try
            {
                var checkOutModel = new CheckOutModel();
                //var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
                if (Session["CartAuthenticateModel"] == null)
                {
                    RazaLogger.WriteInfo("Session is null");
                }
                if (model == null)
                {
                    RazaLogger.WriteInfo("model is null");
                    RedirectToAction("Index", "Cart");
                }
                RazaLogger.WriteInfo(string.Format("Data in Revieworder page: {0},{1},{2}" +
                    model.RechargeValues.CardNumber,
                    model.RechargeValues.CardType, model.RechargeValues.ExpMonth, model.RechargeValues.ExpYear));

                checkOutModel.NameOnCard = UserContext.FullName;
                //string cardnumber = model.RechargeValues.CardNumber.Substring(0, 4) + "XXXXXXXX" +
                //                       model.RechargeValues.CardNumber.Substring(acc - 5, 4);
                checkOutModel.CardNumber = model.RechargeValues.CardNumber;
                checkOutModel.CardType = model.RechargeValues.CardType;
                checkOutModel.ExpMonth = model.RechargeValues.ExpMonth;
                checkOutModel.ExpYear = DateTime.Now.Year.ToString().Substring(0, 2) + model.RechargeValues.ExpYear;
                checkOutModel.PlanName = model.RechargeValues.PlanName;
                checkOutModel.FirstName = UserContext.ProfileInfo.FirstName;
                checkOutModel.LastName = UserContext.ProfileInfo.LastName;
                checkOutModel.State = UserContext.ProfileInfo.State;
                checkOutModel.City = UserContext.ProfileInfo.City;
                checkOutModel.ZipCode = UserContext.ProfileInfo.ZipCode;
                checkOutModel.Country = UserContext.ProfileInfo.Country;
                checkOutModel.Address = UserContext.ProfileInfo.Address;
                checkOutModel.Amount = model.RechargeValues.Amount;
                checkOutModel.ServiceFee = model.RechargeValues.ServiceFee;
                checkOutModel.CurrencyCode = model.RechargeValues.CurrencyCode;
                RazaLogger.WriteInfo("Error code is : " + model.FinalResponseMessage);
                switch (model.FinalResponseMessage)
                {
                    case "A":
                        checkOutModel.ErrorMessage = "Invalid CVV Number. " + Environment.NewLine +
                                                     "Invalid Address or Zipcode/Postalcode.";
                        break;
                    case "B":
                        checkOutModel.ErrorMessage = "Invalid CVV Number.";
                        break;
                    case "C":
                        checkOutModel.ErrorMessage = "Invalid Address or Zipcode/Postalcode.";
                        break;
                    default:
                        checkOutModel.ErrorMessage = "Please Review your information.";
                        break;
                }

                if (model.RechargeValues.TransactionType == "PurchaseNewPlan")
                {
                    checkOutModel.ReviewOrderBanner = Url.Content("~/images/purchasing1_latest_banner.jpg");
                }
                else if (model.RechargeValues.TransactionType == "Recharge")
                {
                    checkOutModel.ReviewOrderBanner = Url.Content("~/images/rechargebanner.png");
                }
                else if (model.RechargeValues.TransactionType == "CheckOut")
                {
                    checkOutModel.ReviewOrderBanner = Url.Content("~/images/checkout_chaged.jpg");
                }
                else if (model.RechargeValues.TransactionType == "TopUp")
                {
                    checkOutModel.ReviewOrderBanner = Url.Content("~/images/checkout_chaged.jpg");
                }

                var cdata = CacheManager.Instance.GetFromCountries();
                var cid = cdata.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                if (cid != null)
                {
                    var states = _repository.GetStateList(SafeConvert.ToInt32(cid.Id));
                    checkOutModel.Statelist = new SelectList(states, "id", "name");

                }
                ViewBag.state = checkOutModel.Statelist;
                switch (model.RechargeValues.CardType.ToLower())
                {
                    case "visa":
                        checkOutModel.CardType = "Visa";
                        break;
                    case "mastercard":
                        checkOutModel.CardType = "MasterCard";
                        break;
                    case "amex":
                        checkOutModel.CardType = "Amex";
                        break;
                    case "discover":
                        checkOutModel.CardType = "Discover";
                        break;
                    default:
                        checkOutModel.CardType = string.Empty;
                        break;

                }
                return View(checkOutModel);
            }
            catch (Exception ex)
            {
                //RazaLogger.WriteInfo("error in review order page: " + ex.Message);
                RazaLogger.WriteInfoPartial("error in review order page: " + ex.Message);
                return RedirectToAction("OrderFailed", "Cart");
            }

        }

        [CompressFilter]
        [Authorize]
        [HttpPost]
        [RequiresSSL]
        public ActionResult ReviewOrder(CheckOutModel checkOutModel)
        {
            try
            {
                var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
                if (model != null)
                {
                    if (model.RechargeValues == null)
                    {
                        RazaLogger.WriteInfo("Rechargevalues Model is null in review order.");
                        //return RedirectToAction("Index", "Cart");
                    }
                    RazaLogger.WriteInfo("Review order post");
                    var rechmodel = model.RechargeValues;
                    rechmodel.IsFromReSubmit = true;
                    rechmodel.cvv = checkOutModel.CvvNumber;
                    rechmodel.ExpMonth = checkOutModel.ExpMonth;
                    rechmodel.ExpYear = checkOutModel.ExpYear.Substring(2);
                    rechmodel.ExpDate = rechmodel.ExpMonth + rechmodel.ExpYear;
                    rechmodel.CardNumber = checkOutModel.CardNumber;
                    rechmodel.CardType = checkOutModel.CardType;
                    rechmodel.FirstName = UserContext.ProfileInfo.FirstName;
                    rechmodel.LastName = UserContext.ProfileInfo.LastName;
                    rechmodel.State = UserContext.ProfileInfo.State;
                    rechmodel.City = UserContext.ProfileInfo.City;
                    rechmodel.ZipCode = UserContext.ProfileInfo.ZipCode;
                    rechmodel.Country = UserContext.ProfileInfo.Country;
                    rechmodel.Address = UserContext.ProfileInfo.Address;

                    RazaLogger.WriteInfo("New Cvv Number is:" + checkOutModel.CvvNumber);
                    var res = CardLookUp(rechmodel);
                    RazaLogger.WriteInfo("error message: " + res.Status + "," + res.Errormsg);
                    if (res.Status)
                    {
                        return RedirectToAction("AuthenticateInfo", "Cart");
                    }
                }
                var bi = _repository.GetBillingInfo(UserContext.MemberId);

                checkOutModel.PlanName = model.RechargeValues.PlanName;
                checkOutModel.FirstName = UserContext.ProfileInfo.FirstName;
                checkOutModel.LastName = UserContext.ProfileInfo.LastName;
                checkOutModel.State = UserContext.ProfileInfo.State;
                checkOutModel.City = UserContext.ProfileInfo.City;
                checkOutModel.ZipCode = UserContext.ProfileInfo.ZipCode;
                checkOutModel.Country = UserContext.ProfileInfo.Country;
                checkOutModel.Address = UserContext.ProfileInfo.Address;
                checkOutModel.NameOnCard = UserContext.FullName;
                checkOutModel.Amount = model.RechargeValues.Amount;
                checkOutModel.ServiceFee = model.RechargeValues.ServiceFee;
                checkOutModel.CurrencyCode = model.RechargeValues.CurrencyCode;
                var cdata = CacheManager.Instance.GetFromCountries();
                var cid = cdata.FirstOrDefault(a => a.Name == bi.Country);
                if (cid != null)
                {
                    var states = _repository.GetStateList(SafeConvert.ToInt32(cid.Id));
                    checkOutModel.Statelist = new SelectList(states, "id", "name");

                }
                ViewBag.state = checkOutModel.Statelist;

                return View(checkOutModel);
            }
            catch (Exception ex)
            {
                //RazaLogger.WriteInfo("Error in Resubmit" + ex.Message);
                RazaLogger.WriteInfoPartial("Error in Resubmit ReviewOrder With checkOutModel" + ex.Message);
                return View(new CheckOutModel());
            }
        }


        private CommonStatus CardLookUp(Recharge recharge)
        {
            var responsemodel = new CommonStatus();

            if (recharge.CouponCode == null || recharge.CouponCode.ToLower() == "null")
            {
                recharge.CouponCode = string.Empty;
            }
            if (recharge.ExpDate != null && recharge.ExpDate.Contains("/"))
            {
                var expdate = recharge.ExpDate.Replace("/", "");
                recharge.ExpDate = expdate;
            }
            recharge.IpAddress = Request.ServerVariables["REMOTE_ADDR"];

            string currencyCode = string.Empty;

            switch (recharge.CurrencyCode)
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

            if (Session["RegPinlessNumber"] != null)
            {
                RazaLogger.WriteInfo("Adding number to Model list");
                recharge.PinlessNumbers = Session["RegPinlessNumber"] as List<PinLessSetupNumbers>;
                Session["RegPinlessNumber"] = null;
            }

            string prefix = string.Empty;

            string transmode = string.Empty;


            if (recharge.TransactionType == "PurchaseNewPlan") // existing customer new plan
            {
                prefix = "OS";
                transmode = "S";
            }
            else if (recharge.TransactionType == "Recharge") //// existing customer existing plan
            {
                prefix = "OR";
                transmode = "R";
            }
            else if (recharge.TransactionType == "CheckOut") //// new customer new plan
            {
                prefix = "OA";
                transmode = "S";
            }
            else if (recharge.TransactionType == "TopUp") //// new customer new plan
            {
                prefix = "TP";
                transmode = "S";
            }


            if (recharge.TransactionType == "Recharge" && recharge.IsFromReSubmit == false)
            {
                recharge.OldOrderId = recharge.order_id;
            }

            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

            //if (recharge.TransactionType == "PurchaseNewPlan" || recharge.TransactionType == "CheckOut")

            recharge.order_id = orderId;


            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", ConfigurationManager.AppSettings["CentinelMessageVersion"]);
            ccRequest.add("MsgType", "cmpi_lookup");
            ccRequest.add("MerchantId", ConfigurationManager.AppSettings["CentinelMerchantId"]);
            ccRequest.add("ProcessorId", ConfigurationManager.AppSettings["CentinelProcessorId"]);
            ccRequest.add("TransactionPwd", ConfigurationManager.AppSettings["CentinelTransactionPwd"]);

            if (recharge.PaymentType == "P")
            {
                ccRequest.add("TransactionType", "X"); // Express Checkout
            }
            else if (recharge.PaymentType == "C")
            {
                if (recharge.CardNumber.StartsWith("3") && recharge.IsFromReSubmit == false) // Amex card add 50
                {
                    // EXTRA 50¢ WILL BE CHARGED FOR AMEX CREDIT CARD
                    recharge.Amount += 0.5;
                }

                if (string.IsNullOrEmpty(recharge.ExpMonth))
                {
                    var listOfCards = _repository.Get_top_CreditCard(UserContext.MemberId);

                    var selectedCard = listOfCards.GetCardList.FirstOrDefault(a => a.CreditCardNumber == recharge.CardNumber);

                    if (selectedCard != null)
                    {
                        recharge.ExpMonth = selectedCard.ExpiryMonth;
                        recharge.ExpYear = selectedCard.ExpiryYear;
                    }
                    else
                    {
                        // return Json(new { Message = "Incomplete Card Information", status = false }, JsonRequestBehavior.AllowGet);
                        responsemodel.Status = false;
                        responsemodel.Errormsg = "Incomplete Card Information";
                        return responsemodel;
                    }

                    //call the web service method for validate the card - mohi 
                    if (recharge.CouponCode == null)
                    {
                        recharge.CouponCode = string.Empty;
                    }
                    var res = _repository.ValidateCustomer(UserContext.MemberId, recharge.CardNumber, recharge.CardId,
                        recharge.Amount, transmode, recharge.Countryfrom, recharge.Countryto, recharge.CouponCode);
                    if (!res.Status)
                    {
                        //return if validation  fail.
                        //return Json(new { Message = "Invalid Card Information", status = false }, JsonRequestBehavior.AllowGet);
                        return new CommonStatus()
                        {
                            Status = false,
                            Errormsg = "Invalid Card Information",
                        };
                    }

                }

                ccRequest.add("TransactionType", recharge.PaymentType);
                ccRequest.add("CardNumber", recharge.CardNumber);
                ccRequest.add("CardExpMonth", recharge.ExpMonth);
                ccRequest.add("CardExpYear", DateTime.Now.Year.ToString().Substring(0, 2) + recharge.ExpYear);
            }
            else
            {
                //return Json(new { Message = "Invalid Transactions", status = false }, JsonRequestBehavior.AllowGet);
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = "Invalid Transactions",
                };
            }

            var randomOrderNumber = new Random().Next(10000000, 99999999);
            double amount = recharge.Amount * 100;

            ccRequest.add("OrderNumber", recharge.order_id == null ? orderId : recharge.order_id);// );
            ccRequest.add("OrderDescription", "This is where you put the order description");
            ccRequest.add("Amount", amount.ToString());
            ccRequest.add("CurrencyCode", currencyCode);
            ccRequest.add("UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]);
            ccRequest.add("BrowserHeader", Request.ServerVariables["HTTP_ACCEPT"]);
            ccRequest.add("IPAddress", Request.ServerVariables["REMOTE_ADDR"]);

            if (recharge.PaymentType == "P")
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
                RazaLogger.WriteInfo(string.Format("Error while sending request to centinel server {0}", ex.Message + ex.StackTrace));
                //return Json(new
                //{
                //    Message = "Internal error with payment gateway. Please try later.",
                //    status = false,
                //}, JsonRequestBehavior.AllowGet);
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = "Internal error with payment gateway. Please try later."
                };
            }

            var errorNo = ccResponse.getValue("ErrorNo");
            var errorDesc = ccResponse.getValue("ErrorDesc");
            var enrolled = ccResponse.getValue("Enrolled");
            var payload = ccResponse.getValue("Payload");
            var acsurl = ccResponse.getValue("ACSUrl");
            var transactionId = ccResponse.getValue("TransactionId");

            RazaLogger.WriteInfo("Transaction Id " + transactionId);
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

                string Centinel_RetUrl = ConfigurationManager.AppSettings["Centinel_RetUrl"];
                var cartAuthenticateModel = new CartAuthenticateModel()
                {
                    CentinelPayload = payload,
                    CentinelTermURL = Centinel_RetUrl,
                    Centinel_ACSURL = acsurl,
                    Centinel_TransactionId = transactionId,
                    Centinel_TransactionType = recharge.PaymentType,
                    RechargeValues = recharge,
                    IsCcProcess = true
                };

                Session["CartAuthenticateModel"] = cartAuthenticateModel;


                //return Json(new
                //{
                //    Message = message,
                //    status = status,
                //    CentinelPayload = payload,
                //    Centinel_ACSURL = acsurl,
                //    CentinelTermURL = Centinel_RetUrl,
                //    Centinel_TransactionId = transactionId,

                //}, JsonRequestBehavior.AllowGet);
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

                // return Json(new { Message = message, status = false }, JsonRequestBehavior.AllowGet);
                return new CommonStatus()
                {
                    Status = status,
                    Errormsg = message
                };
            }
        }

        [CompressFilter]
        [RequiresSSL]
        public ActionResult Confirmation()
        {
            return View(new GenericModel());
        }

    }
}
