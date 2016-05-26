using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    public class TopUpMobileController : BaseController
    {
        private DataRepository _repository = new DataRepository();

        [HttpPost]
        [UnRequiresSSL]
        public ActionResult TopupMobileIndex(HomeViewModel homemodel)
        {
            if (UserContext != null)
            {
                return RedirectToAction("ExistingCustomerTopup", "TopUpMobile",
                    new { topupmobilenumber = homemodel.TopupMobileNumber, topupcountry = homemodel.TopMobileCountry });
            }
            else
            {
                return RedirectToAction("NewCustomerTopUp", "TopUpMobile",
                    new { topupmobilenumber = homemodel.TopupMobileNumber, topupcountry = homemodel.TopMobileCountry });
            }
        }

        // GET: /TopUpMobile/
        [CompressFilter]
        [Authorize]
        [HttpGet]
        [RequiresSSL]
        public ActionResult ExistingCustomerTopup(string topupmobilenumber, string topupcountry)
        {
            if (Session["IsTransactionSuccess"] != null && (bool)Session["IsTransactionSuccess"])
            {
                Session["IsTransactionSuccess"] = false;
                var rec = new Recharge();
                var model = new CartAuthenticateModel();

                rec.StatusMessage1 = "Mobile Top Up already Successfull!!";
                rec.StatusMessage2 = "Thank you for Ordering!";
                model.RechargeValues = rec;
                return View("TopupConfirmation", model);
            }
            Session["IsTransactionSuccess"] = false;

            var topupModel = new TopupModel();

            topupModel.MobileNumber = topupmobilenumber;
            topupModel.Country = SafeConvert.ToInt32(topupcountry);

            var fs = new List<State>();
            var repository = new DataRepository();
            var data = repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

            foreach (var res in data)
            {

                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            topupModel.States = fs;
            topupModel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = topupModel.Statelist;


            return View(topupModel);

        }




        // GET: /TopUpMobile/
        [CompressFilter]
        [Authorize]
        [HttpGet]
        [RequiresSSL]
        public ActionResult ExistingCustomerTopup_v1(string topupmobilenumber, string topupcountry)
        {
            if (Session["IsTransactionSuccess"] != null && (bool)Session["IsTransactionSuccess"])
            {
                Session["IsTransactionSuccess"] = false;
                var rec = new Recharge();
                var model = new CartAuthenticateModel();

                rec.StatusMessage1 = "Mobile Top Up already Successfull!!";
                rec.StatusMessage2 = "Thank you for Ordering!";
                model.RechargeValues = rec;
                return View("TopupConfirmation", model);
            }
            Session["IsTransactionSuccess"] = false;

            var topupModel = new TopupModel();

            topupModel.MobileNumber = topupmobilenumber;
            topupModel.Country = SafeConvert.ToInt32(topupcountry);

            var fs = new List<State>();
            var repository = new DataRepository();
            var data = repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

            foreach (var res in data)
            {

                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            topupModel.States = fs;
            topupModel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = topupModel.Statelist;


            return View(topupModel);

        }



        // GET: /TopUpMobile/
        [CompressFilter]
        [Authorize]
        [HttpGet]
        [RequiresSSL]
        public ActionResult ExistingCustomerTopup_Raza(string topupmobilenumber, string topupcountry)
        {
            if (Session["IsTransactionSuccess"] != null && (bool)Session["IsTransactionSuccess"])
            {
                Session["IsTransactionSuccess"] = false;
                var rec = new Recharge();
                var model = new CartAuthenticateModel();

                rec.StatusMessage1 = "Mobile Top Up already Successfull!!";
                rec.StatusMessage2 = "Thank you for Ordering!";
                model.RechargeValues = rec;
                return View("TopupConfirmation", model);
            }
            Session["IsTransactionSuccess"] = false;

            var topupModel = new TopupModel();

            topupModel.MobileNumber = topupmobilenumber;
            topupModel.Country = SafeConvert.ToInt32(topupcountry);

            var fs = new List<State>();
            var repository = new DataRepository();
            var data = repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

            foreach (var res in data)
            {

                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            topupModel.States = fs;
            topupModel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = topupModel.Statelist;


            return View(topupModel);

        }



        [RequiresSSL]
        public ActionResult NewCustomerTopUp(string topupmobilenumber, string topupcountry)
        {
            if (Session["IsTransactionSuccess"] != null && (bool)Session["IsTransactionSuccess"])
            {
                var rec = new Recharge();
                var model = new CartAuthenticateModel();
                Session["IsTransactionSuccess"] = false;
                rec.StatusMessage1 = "Mobile Top Up Successfull!!";
                rec.StatusMessage2 = "Thank you for Ordering!";
                model.RechargeValues = rec;
                return View("TopupConfirmation", model);
            }
            var topupModel = new TopupModel();
            topupModel.MobileNumber = topupmobilenumber;
            topupModel.Country = SafeConvert.ToInt32(topupcountry);

            var fs = new List<State>();
            var repository = new DataRepository();
            var data = repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

            foreach (var res in data)
            {
                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            topupModel.States = fs;
            topupModel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = topupModel.Statelist;


            return View(topupModel);
        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult Topuplogin(string emailAddress, string password)
        {
            LogOnModel vgn;

            string email = emailAddress.Trim();

            vgn = new LogOnModel { EmailAddress = email, Password = password };

            try
            {
                if (!string.IsNullOrWhiteSpace(emailAddress) && !string.IsNullOrWhiteSpace(password))
                {
                    var usercontext = Authenticate(vgn.EmailAddress, vgn.Password);
                    if (usercontext != null)
                    {
                        //WelcomeRazaMailSend(usercontext);
                        return
                         Json(
                             new
                             {
                                 Islogin = true
                             });
                    }
                    else
                    {
                        return Json(new { Islogin = false, message = "Invalid email and password combination." });
                    }
                }
                else
                {
                    return Json(new { Islogin = false, message = "Invalid email and password combination." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Islogin = false, message = "Invalid email and password combination." });
            }

            return Json(new { Islogin = false });
        }


        [RequiresSSL]
        public JsonResult TopUpSignup(LogOnModel model)
        {
            string ipAddress2 = Request.UserHostAddress;
            int countryFrom = model.CountryFrom;
            string email = model.Email;
            string password = model.Password;
            string phone = model.Phone_Number;
            int countryTo = model.CountryTo;

            UserContext context = new UserContext();

            var res = _repository.QuickSignUp(countryFrom, countryTo, email, password, phone, ipAddress2, ref context);

            var vgn = new LogOnModel { Email = email, Password = password };

            if (res == "1")
            {
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                {
                    var usercontext = Authenticate(vgn.Email, vgn.Password);
                    if (usercontext == null)
                    {
                        vgn.ErrorMsg = "Invalid Email and Password combination.";
                    }
                }
            }
            else
            {
                if (res == "phone number exist")
                {
                    vgn.ErrorMsg = "Phone number already exist, Please try again.";
                }
                else
                {
                    vgn.ErrorMsg = "Email Address already exist, Please try again.";
                }

                return
                     Json(
                         new
                         {
                             status = false,
                             message = vgn.ErrorMsg
                         });
            }

            return
                       Json(
                           new
                           {
                               status = true,
                               message = vgn.ErrorMsg
                           });
        }

        [HttpGet]
        public JsonResult GetMobileOperatorInfo(string mobileNumber, string countryid)
        {
            var countrylist = CacheManager.Instance.GetCountryListTo();

            var countrydata = countrylist.FirstOrDefault(a => a.Id == countryid);

            if (countrydata != null)
            {
                string destnumber = countrydata.CountCode + mobileNumber;

                var operatorinfo = _repository.TopUpPhoneNumberInfo(destnumber);


                return Json(operatorinfo, JsonRequestBehavior.AllowGet);
            }

            return Json(new Mobiletopup(), JsonRequestBehavior.AllowGet);
        }



        public Mobiletopup MobileOperatorInfo(string mobileNumber, string countryid)
        {
            var countrylist = CacheManager.Instance.GetCountryListTo();

            var countrydata = countrylist.FirstOrDefault(a => a.Id == countryid);

            if (countrydata != null)
            {
                string destnumber = countrydata.CountCode + mobileNumber;
                string number = string.Empty;
                if (destnumber.Contains('-'))
                {
                    number = destnumber.Replace("-", "");
                }
                else
                {
                    number = destnumber;
                }
                var operatorinfo = _repository.TopUpPhoneNumberInfo(number);
                operatorinfo.TopUpCountryCode = countrydata.CountCode;

                return operatorinfo;
            }

            return new Mobiletopup();
        }


        [HttpPost]
        [RequiresSSL]
        public JsonResult MobileTopUpRecharge(Mobiletopup topup) // topup recharge for new user..
        {
            RazaLogger.WriteInfoPartial("Calling MobileTopUpRecharge with CustID: " + topup.MemberId);
            if ((bool)Session["IsTransactionSuccess"])
            {
                RazaLogger.WriteInfoPartial("TopUp IsTransactionSuccess not null");
                Session["IsTransactionSuccess"] = false;
                return null;
            }
            try
            {
                var billnginfo = new BillingInfo
                {
                    FirstName = topup.FirstName,
                    LastName = topup.LastName,
                    PhoneNumber = topup.BillngPhoneNumber,
                    Address = topup.Address1,
                    City = topup.City,
                    State = topup.State,
                    ZipCode = topup.ZipCode,
                    Country = topup.Country,
                    //MemberId = UserContext.MemberId
                };

                if (topup.ExpDate != null && topup.ExpDate.Contains("/"))
                {
                    var expdate = topup.ExpDate.Replace("/", "");
                    topup.ExpDate = expdate;
                }

                var response = _repository.UpdateBillingInfo(billnginfo);

                RazaLogger.WriteInfoPartial("Response Of MobileTopUp UpdateBillingInfo: " + response.Status.ToString());

                if (response.Status)
                {
                    topup.MemberId = UserContext.MemberId;



                    RazaLogger.WriteInfoPartial(
   string.Format(
       "Calling topup with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
       "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", topup.NewOrderId ?? string.Empty, topup.MemberId ?? string.Empty, topup.OperatorCode ?? string.Empty, topup.Operator ?? string.Empty, topup.CountryId ?? string.Empty,
       topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber ?? string.Empty, topup.SmsTo ?? string.Empty, topup.CouponCode ?? string.Empty, topup.PaymentMethod ?? string.Empty,
       topup.Isccprocess ? "N" : "Y",
       topup.Pin ?? string.Empty, topup.PurchaseAmount, topup.CardNumber ?? string.Empty, topup.ExpDate ?? string.Empty, topup.Cvv2 ?? string.Empty, topup.PaymentTransactionId ?? string.Empty,
       topup.AvsResponse ?? string.Empty, topup.Cvv2Response ?? string.Empty, topup.Cavv ?? string.Empty, topup.EciFlag ?? string.Empty, topup.Xid ?? string.Empty, topup.Address1 ?? string.Empty, topup.Address2 ?? string.Empty,
       topup.City ?? string.Empty, topup.State ?? string.Empty, topup.ZipCode ?? string.Empty, topup.Country ?? string.Empty, topup.IpAddress ?? string.Empty, topup.PayerId ?? string.Empty));



                    var res = _repository.MobileTopupRecharge(topup);
                    if (res.Status)
                    {
                        var handler = new CreditCartHandler();
                        var recharge = new Recharge()
                        {
                            order_id = topup.NewOrderId
                        };

                        handler.MobileTopUpMail(recharge, UserContext);
                    }
                    return null;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception exception)
            {
                RazaLogger.WriteInfoPartial("TopUp is not successfull due to " + exception.Message + exception.StackTrace);
                return null;
            }

        }


        [HttpPost]
        [RequiresSSL]
        public JsonResult MobileTopUpRechargeExistinguser(Mobiletopup topup) //toprecharge for existing user...
        {
            try
            {
                if ((bool)Session["IsTransactionSuccess"])
                {
                    RazaLogger.WriteInfoPartial("Calling MobileTopUpRecharge with this CustID: " + topup.MemberId);
                    Session["IsTransactionSuccess"] = false;
                    return Json(new { status = true, error = "Transaction already successfull" });
                }
                var billngdata = _repository.GetBillingInfo(UserContext.MemberId);
                string destphonenumber = string.Empty;
                if (topup.TopUpCountryCode.Contains("-"))
                {
                    destphonenumber = topup.TopUpCountryCode.Replace("-", "") + topup.DestinationPhoneNumber;
                }
                else
                {
                    destphonenumber = topup.TopUpCountryCode + topup.DestinationPhoneNumber;
                }

                //********************Added on 01/28/15 to create new order id**************************
                string prefix = "OR";
                string orderId = string.Format("{0}{1}", prefix,
Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));
                //*************************************************************

                var topuprech = new Mobiletopup()
                {
                    //This NewOrderId was added by sabal on 01/28/2015
                    NewOrderId = orderId,
                    MemberId = UserContext.MemberId,
                    OperatorCode = topup.OperatorCode,
                    CountryId = topup.CountryId,
                    SourceAmount = topup.SourceAmount,
                    DestinationAmt = topup.DestinationAmt,
                    SmsTo = topup.DestinationPhoneNumber,
                    PaymentMethod = topup.PaymentMethod,
                    Pin = "",
                    PurchaseAmount = topup.SourceAmount,
                    CardNumber = topup.CardNumber,
                    ExpDate = topup.ExpDate.Replace("/", ""),
                    Cvv2 = topup.Cvv2,
                    //Address1 = billngdata.Address,
                    Address1 = UserContext.ProfileInfo.Address,
                    Address2 = string.Empty,
                    City = UserContext.ProfileInfo.City,
                    State = UserContext.ProfileInfo.State,
                    ZipCode = UserContext.ProfileInfo.ZipCode,
                    Country = UserContext.ProfileInfo.Country,
                    IpAddress = Request.UserHostName,
                    DestinationPhoneNumber = destphonenumber,
                    Operator = topup.Operator,
                    AvsResponse = string.Empty,
                    Xid = string.Empty,
                    CouponCode = string.Empty,
                    Cvv2Response = string.Empty,
                    EciFlag = string.Empty,
                    Isccprocess = false,
                    Cavv = string.Empty,
                    PaymentTransactionId = string.Empty,

                };

                if (topup.ExpDate != null && topup.ExpDate.Contains("/"))
                {
                    var expdate = topup.ExpDate.Replace("/", "");
                    topup.ExpDate = expdate;
                }


                topup.MemberId = UserContext.MemberId;


                RazaLogger.WriteInfoPartial(
string.Format(
"Calling topup with these params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
"{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", topuprech.NewOrderId ?? string.Empty, topuprech.MemberId ?? string.Empty, topuprech.OperatorCode ?? string.Empty, topuprech.Operator ?? string.Empty, topuprech.CountryId ?? string.Empty,
topuprech.SourceAmount, topuprech.DestinationAmt, topuprech.DestinationPhoneNumber ?? string.Empty, topuprech.SmsTo ?? string.Empty, topuprech.CouponCode ?? string.Empty, topuprech.PaymentMethod ?? string.Empty,
topuprech.Isccprocess ? "N" : "Y",
topuprech.Pin ?? string.Empty, topuprech.PurchaseAmount, topuprech.CardNumber ?? string.Empty, topuprech.ExpDate ?? string.Empty, topuprech.Cvv2 ?? string.Empty, topuprech.PaymentTransactionId ?? string.Empty,
topuprech.AvsResponse ?? string.Empty, topuprech.Cvv2Response ?? string.Empty, topuprech.Cavv ?? string.Empty, topuprech.EciFlag ?? string.Empty, topuprech.Xid ?? string.Empty, topuprech.Address1 ?? string.Empty, topuprech.Address2 ?? string.Empty,
topuprech.City ?? string.Empty, topuprech.State ?? string.Empty, topuprech.ZipCode ?? string.Empty, topuprech.Country ?? string.Empty, topuprech.IpAddress ?? string.Empty, topuprech.PayerId ?? string.Empty));



                var res = _repository.MobileTopupRecharge(topuprech);
                if (res.Status)
                {
                    var dict = FlagDictonary.GetCurrencycodebyCountry();
                    var CountryID = topuprech.Country.ToUpper().Replace("U.S.A.", "1").Replace("USA", "1").Replace("CANADA", "2").Replace("U.K.", "3").Replace("UK", "3");

                    var recharge = new Recharge()
                    {
                        order_id = res.OrderId,
                        TopupMobileNumber = topuprech.DestinationPhoneNumber,
                        CardNumber = topuprech.CardNumber,
                        PaymentMethod = topuprech.PaymentMethod,
                        TopupSourceAmount = topuprech.SourceAmount,
                        CurrencyCode = dict[SafeConvert.ToInt32(CountryID)]
                        //CurrencyCode = dict[SafeConvert.ToInt32(topuprech.CountryId)]
                    };
                    var handler = new CreditCartHandler();
                    handler.MobileTopUpMail(recharge, UserContext);
                    RazaLogger.WriteInfoPartial("TopUp MobileTopupRecharge is successfull");
                    return Json(new { status = res.Status, orderid = res.OrderId });
                }
                else
                {
                    RazaLogger.WriteInfoPartial("TopUp MobileTopupRecharge is not successfull due to error: " + res.RechargeError);
                    return Json(new { status = res.Status, error = res.RechargeError });
                }

            }
            catch (Exception exception)
            {
                RazaLogger.WriteInfoPartial("TopUp is not successfull due to error: " + exception.Message + exception.StackTrace);
                return null;
            }

        }


        [RequiresSSL]
        public JsonResult GetCreditCard()
        {
            var rechargeSetUpModel = new RechargeSetup();


            var CreditCard = new GetTopCreditCards();
            CreditCard = _repository.Get_top_CreditCard(UserContext.MemberId);
            rechargeSetUpModel.CardList = CreditCard.GetCardList;
            // rechargeSetUpModel.Years = Enumerable.Range(DateTime.Now.Year, 11).ToList();

            return Json(rechargeSetUpModel, JsonRequestBehavior.AllowGet);
        }


        [RequiresSSL]
        public JsonResult AddCreditCard(string creditCard, string exp_Month, string exp_Year, string cVV, string nameOnCard)
        {
            BillingInfo billingInfoModel = _repository.GetBillingInfo(UserContext.MemberId);
            string country = billingInfoModel.Country;
            string billing_Address = billingInfoModel.Address;
            string city = billingInfoModel.City;
            string state = billingInfoModel.State;
            string zipCode = billingInfoModel.ZipCode;
            string Exp_Year = exp_Year;

            var res = _repository.AddCreditCard(UserContext.MemberId, nameOnCard, creditCard, exp_Month, Exp_Year, cVV,
                country, billing_Address, city, state, zipCode);

            if (res.Status)
            {
                return Json(new { status = true, message = "Your credit card addedd successfully." });

            }
            else
            {
                return Json(new { status = false, message = res.Errormsg });
            }


        }


        public JsonResult GetSeletedOpeartorAmountList_110415_Bak(int countryid, string operatorname)
        {
            var topupoperators = _repository.GetMobile_TopupOperator(countryid);

            var operatordata =
                    topupoperators.OperatorList.Where(a => a.OperatorName == operatorname).ToList();
            var mobileoperators = new List<MobileTopupOperator>();
            foreach (var data in operatordata)
            {
                var opt = new MobileTopupOperator()
                {
                    CountryId = data.CountryId,
                    OperatorName = data.OperatorName,
                    OperatorCode = data.OperatorCode,
                    SourceAmount = data.SourceAmount,
                    DestinationAmount = data.DestinationAmount,
                    Currency = data.Currency,
                    SourceAmountwithSign = "$" + data.SourceAmount
                };
                mobileoperators.Add(opt);
            }
            return Json(mobileoperators, JsonRequestBehavior.AllowGet);

        }




        public JsonResult GetSeletedOpeartorAmountList(int countryid, string operatorname)
        {
            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(UserContext.ProfileInfo.Country))
            {
                //model.RatesFromCountryList = CacheManager.Instance.GetTop3FromCountries()
                //    .Where(a => a.Name == UserContext.ProfileInfo.Country).ToList();

                string CountryFrom = UserContext.ProfileInfo.Country.ToUpper();
                if (CountryFrom == "CANADA")
                    CountryFrom = "2";
                else
                    CountryFrom = "1";

                var topupoperators = _repository.GetMobile_TopupOperator(Convert.ToInt16(CountryFrom),countryid);

                var operatordata =
                        topupoperators.OperatorList.Where(a => a.OperatorName == operatorname).ToList();
                var mobileoperators = new List<MobileTopupOperator>();
                foreach (var data in operatordata)
                {
                    var opt = new MobileTopupOperator()
                    {
                        CountryId = data.CountryId,
                        OperatorName = data.OperatorName,
                        OperatorCode = data.OperatorCode,
                        SourceAmount = data.SourceAmount,
                        DestinationAmount = data.DestinationAmount,
                        Currency = data.Currency,
                        SourceAmountwithSign = "$" + data.SourceAmount
                    };
                    mobileoperators.Add(opt);
                }
                return Json(mobileoperators, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var topupoperators = _repository.GetMobile_TopupOperator(countryid);

                var operatordata =
                        topupoperators.OperatorList.Where(a => a.OperatorName == operatorname).ToList();
                var mobileoperators = new List<MobileTopupOperator>();
                foreach (var data in operatordata)
                {
                    var opt = new MobileTopupOperator()
                    {
                        CountryId = data.CountryId,
                        OperatorName = data.OperatorName,
                        OperatorCode = data.OperatorCode,
                        SourceAmount = data.SourceAmount,
                        DestinationAmount = data.DestinationAmount,
                        Currency = data.Currency,
                        SourceAmountwithSign = "$" + data.SourceAmount
                    };
                    mobileoperators.Add(opt);
                }
                return Json(mobileoperators, JsonRequestBehavior.AllowGet);
            }

        }





        //this method call on page load upload upload operator data and its amount on second tab..
        [HttpGet]
        public JsonResult GetCountryOperators(string mobilenumber, int countryid)
        {
            var topupdata = new Mobiletopup();
            try
            {
                string CountryFrom = "1";
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(UserContext.ProfileInfo.Country))
                {
                    CountryFrom = UserContext.ProfileInfo.Country.ToUpper();
                    if (CountryFrom == "CANADA")
                        CountryFrom = "2";
                    else
                        CountryFrom = "1";
                }
                    
                //var topupoperators = _repository.GetMobile_TopupOperator(countryid);
                var topupoperators = _repository.GetMobile_TopupOperator(Convert.ToInt16(CountryFrom),countryid);


                var operatorinfo = MobileOperatorInfo(mobilenumber, SafeConvert.ToString(countryid));

                // var countryoperatorlist =new List<MobileTopupOperator>();
                //topupdata.IsAutoOperatorfind = !string.IsNullOrEmpty(operatorinfo.TopUpMobileOperator);

                var operatordata =
                    topupoperators.OperatorList.Where(a => a.OperatorName == operatorinfo.TopUpMobileOperator).ToList();
                if (!string.IsNullOrEmpty(operatorinfo.TopUpMobileOperator) || operatordata.Count != 0)
                {
                    topupdata.IsAutoOperatorfind = true;
                }
                else
                {
                    topupdata.IsAutoOperatorfind = false;
                }
                var mobileoperators = new List<MobileTopupOperator>();
                //  var listofCurrSign = FlagDictonary.GetCurrencycode();
                string imagename = string.Empty;
                if (topupdata.IsAutoOperatorfind)
                {
                    string filepath = Server.MapPath(@"/Content/Operator_images.xml");

                    var operatorImages = CacheManager.Instance.GetOpeartorImages(filepath);
                    var operatorimagedata =
                        operatorImages.ListOfOperators.FirstOrDefault(
                            a => a.operatorname == operatorinfo.TopUpMobileOperator);

                    if (operatorimagedata != null)
                    {
                        imagename = "~/images/operator_images/" + operatorimagedata.operatorimage;
                    }
                    else
                    {
                        var allcountryoperators = topupoperators.OperatorList.DistinctBy(a => a.OperatorName).ToList();
                        topupdata.AllCountryOperatorList = allcountryoperators;
                    }
                }
                else
                {
                    var allcountryoperators = topupoperators.OperatorList.DistinctBy(a => a.OperatorName).ToList();
                    topupdata.AllCountryOperatorList = allcountryoperators;
                }
                foreach (var data in operatordata)
                {
                    var opt = new MobileTopupOperator()
                    {
                        CountryId = data.CountryId,
                        OperatorName = data.OperatorName,
                        OperatorCode = data.OperatorCode,
                        SourceAmount = data.SourceAmount,
                        DestinationAmount = data.DestinationAmount,
                        Currency = data.Currency,
                        SourceAmountwithSign = "$" + data.SourceAmount
                    };
                    mobileoperators.Add(opt);
                }


                topupdata.Operatordata = mobileoperators;
                topupdata.Operator = operatorinfo.TopUpMobileOperator;
                topupdata.TopUpCountryCode = operatorinfo.TopUpCountryCode;
                topupdata.TopUpMobileCountry = operatorinfo.Country;
                topupdata.OperatorImage = string.IsNullOrEmpty(imagename) ? string.Empty : Url.Content(imagename);

                return Json(topupdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(topupdata, JsonRequestBehavior.AllowGet);
            }
        }


        // when we select the amount this method call on after..
        public JsonResult GetSelectedOperatorData(string operatorcode, int countryid)
        {
            var topupoperators = CacheManager.Instance.GetCountryOperators(countryid);

            var selectedoperatordata = topupoperators.OperatorList.FirstOrDefault(a => a.OperatorCode == operatorcode);

            return Json(selectedoperatordata, JsonRequestBehavior.AllowGet);
        }


        private void SendTopUpMail(Mobiletopup model)
        {
            try
            {

                string orderId = model.NewOrderId;
                string mobileTopupNumber = model.DestinationPhoneNumber;
                string creditcard = model.CardNumber;
                string topupamount = "$" + SafeConvert.ToString(model.SourceAmount) + "USD";
                string paymentmethod = model.PaymentMethod;
                string taxamount = "0%";
                string email = UserContext.Email;
                string datetime = DateTime.Now.ToString();


                string mailbody =
                                 System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/mobile-topup.html"));

                mailbody = mailbody.Replace(@"<!--datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--OrderNumber-->", orderId);
                mailbody = mailbody.Replace(@"<!--MobileTopupNumber-->", mobileTopupNumber);
                mailbody = mailbody.Replace(@"<!--Creditcard-->", creditcard);
                mailbody = mailbody.Replace(@"<!--TopupAmount-->", topupamount);
                mailbody = mailbody.Replace(@"<!--PaymentMethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Amountwithtax-->", topupamount);
                mailbody = mailbody.Replace(@"<!--Taxamount-->", taxamount);

                mailbody = mailbody.Replace(@"<!--EmailId-->", email);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Rechargeconfim"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {

            }

        }

        [HttpGet]
        public ActionResult TopupConfirmation()
        {
            var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
            Session["CartAuthenticateModel"] = null;
            var status = Session["TopupStatus"] as RechargeStatusModel;
            Session["TopupStatus"] = null;
            if (model.RechargeValues.TopupStatus)
            {
                model.RechargeValues.StatusMessage1 = "Mobile Top Up Successfull!!";
                model.RechargeValues.StatusMessage1 = "Thank you for Ordering!";
            }
            else
            {
                model.RechargeValues.StatusMessage1 = "Mobile Top Up is not Successfull!!";
                model.RechargeValues.StatusMessage1 = "Please Try Again!";
            }
            if (model != null)
            {
                model.RechargeValues.EmailId = UserContext.Email;
                if (status != null) model.RechargeValues.TopupStatus = status.Status;
                int cardlength = model.RechargeValues.CardNumber.Length;
                string maskedcardnumber = model.RechargeValues.CardNumber.Substring(0, 4) + "XXXXXXXX" +
                                       model.RechargeValues.CardNumber.Substring(cardlength - 4, 4);
                model.RechargeValues.CardNumber = maskedcardnumber;
                model.RechargeValues.PaymentMethod = "Credit Card";
            }
            return View(model);

        }

        [HttpGet]
        public ActionResult TopupFailed()
        {
            //var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
            //Session["CartAuthenticateModel"] = null;
            //var status = Session["TopupStatus"] as RechargeStatusModel;
            //Session["TopupStatus"] = null;


            //if (model != null)
            //{
            //    model.RechargeValues.EmailId = UserContext.Email;
            //    if (status != null) model.RechargeValues.TopupStatus = status.Status;
            //}
            //return View(model);           



            Session["IsTransactionSuccess"] = false;
            var topupModel = new TopupModel();

            var repository = new DataRepository();
            var data = repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

            return View(topupModel);

        }

        private int GetUserCountryId()
        {
            if (UserContext != null && UserContext.ProfileInfo != null)
            {
                var countrydata = CacheManager.Instance.GetFromCountries();
                var cid = countrydata.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                return SafeConvert.ToInt32(cid.Id);
            }
            else
            {
                return 1;
            }
        }


        private void WelcomeRazaMailSend(UserContext model)
        {
            string email = model.Email;
            var repository = new DataRepository();
            try
            {
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/welcome_raza.html"));

                string redirectlink = "Account/MyAccount";
                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId();
                if (model != null && model.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }

                var password = repository.GetPassword(model.Email);
                string mailrow = string.Empty;

                mailbody = mailbody.Replace(@"<!--Password-->", password);
                mailbody = mailbody.Replace(@"<!--EmailId-->", model.Email);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["WelcomeRaza"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in sending Signup Mail: " + ex.Message);
            }


        }

    }
}
