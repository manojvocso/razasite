using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using System.Diagnostics;
using WebGrease;
using System.Collections;
using System.Net.Mail;
using MvcApplication1.Compression;
using MvcApplication1.App_Start;



namespace MvcApplication1.Controllers
{
    [Intercept]
    public class AccountController : BaseController
    {

        private DataRepository _repository = new DataRepository();

        // GET: /Account/
        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult Index()
        {
            Session["IsTransactionSuccess"] = false;
            var model1 = new HomeViewModel();

            model1.CountryListTo = CacheManager.Instance.GetCountryListTo();

            var data = GettopupCountryList();
            //model1.TopupCountryList = data;
            model1.TopupCountryList = data.Where(a => (a.Id != "314" && a.Id != "1")).ToList();

            var res = HomePageCountryRateList();
            model1.HomepageRates = res.Homepagerates.ToList();

            //model1.ListOfTop3FromCountries = null;
            // model1.ListOfToCountries = null;
            // model1.ListOfFromCountries = null;
            //  model1.CountryListTo = null;

            return View(model1);
        }








        //[HttpGet]
        //public ActionResult ReadLogFile()
        //{
        //    return View(new ReadLogFileModel());
        //}

        //[HttpPost]
        //public ActionResult ReadLogFile(ReadLogFileModel model)
        //{
        //    string path = Server.MapPath(model.FileDate + "\\raza.log");
        //    string p = path.Replace("Account", "logs");
        //    using (var rd = new StreamReader(p))
        //    {
        //        int size = 0;
        //        string log = string.Empty;
        //        while (!rd.EndOfStream)
        //        {
        //            log += rd.ReadLine();
        //            log += System.Environment.NewLine;
        //        }
        //        model.LogData = log;

        //    };

        //    return View(model);
        //}








        [HttpPost]
        [UnRequiresSSL]
        public ActionResult CountryChange(int countries)
        {
            string returnUrl = string.Empty;

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
            {
                if ((Request.UrlReferrer.LocalPath == "/Rate/searchrate" || Request.UrlReferrer.LocalPath == "/Rate/SearchRate")
                    && Request.UrlReferrer.Query.Length > 0)
                {
                    var countryto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["countryto"];
                    returnUrl = Url.Action("SearchRate", "Rate", new { countryfrom = countries, countryto = countryto });

                }
                else
                {
                    returnUrl = Request.UrlReferrer.ToString();
                }

            }


            Session["CountrybyIp"] = countries;
            string heplNumber = "1-(877) 463-4233";
            if (countries == 2)
            {
                heplNumber = "1-(800) 550-3501";
            }
            else if (countries == 3)
            {
                heplNumber = "44-(800) 520-0329";
            }

            Session["HelpNumber"] = heplNumber;
            return RedirectPermanent(returnUrl);
            //return RedirectToAction("Index", "Account");
        }




        private HomePageRate HomePageCountryRateList()
        {
            int countryfrom = 1;
            if (Session["CountrybyIp"] != null)
            {
                countryfrom = SafeConvert.ToInt32(Session["CountrybyIp"]);
            }

            string sign = "¢";
            if (countryfrom == 3)
            {
                sign = "p";
            }
            var response = _repository.HomePageCountryRateList(countryfrom);

            var dict = FlagDictonary.GetHomePageCountryFlag();

            foreach (var var in response.Homepagerates)
            {
                var.CurrencySign = sign;
                var.FlagLogo = dict.ContainsKey(var.CountryId) ? dict[var.CountryId] : string.Empty;
                var.LowestRate = (var.LowestRate * 10) / 10;
                var.CountryFrom = countryfrom;
                if (var.CountryId == 309)
                {
                    var.CountryName = "U.A.E.";
                }
                else if (var.CountryId == 312)
                {
                    var.CountryName = "U.K.";
                }
            }

            return response;
        }

        private List<Country> GettopupCountryList()
        {
            var countryto = CacheManager.Instance.GetCountryListTo();
            var flagDict = FlagDictonary.CreateDictinary();

            foreach (var country in countryto)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'
                string flagname = string.Empty;
                if (flagDict.ContainsKey(Convert.ToInt32(country.Id)))
                {
                    flagname = flagDict[Convert.ToInt32(country.Id)];
                }
                else
                {
                    flagname = "/images/flag.jpg";
                }
                country.CountryFlag = flagname;
            }

            return countryto;
        }


        public ActionResult Forgotpassword()
        {
            return View();
        }

        [CompressFilter]
        [Authorize]
        [RequiresSSL]
        public ActionResult MyAccount(string rf = "")
        {
            var model = new MyAccountModel();
            Session["IsTransactionSuccess"] = false;
            model.Message = Helpers.GetMyAccoutMessage(rf);
            var res = _repository.GetRazaReward(UserContext.MemberId);

            model.Point = res;

            var email = GetEmails(UserContext.MemberId).FirstOrDefault();

            if (email != null)
            {
                model.Email.Subject = email.ShortSubject;
            }
            else
            {
                model.Email = new EmailModel();
            }

            var result = _repository.GetReferFriendPt(UserContext.MemberId);



            model.ReferEmails = result;


            model.ProfileInformation = new BillingInfoModel()
            {
                FirstName = UserContext.ProfileInfo.FirstName,
                LastName = UserContext.ProfileInfo.LastName,
                Address = UserContext.ProfileInfo.Address,
                City = UserContext.ProfileInfo.City,
                Country = UserContext.ProfileInfo.Country,
                ZipCode = UserContext.ProfileInfo.ZipCode,
                PhoneNumber = UserContext.ProfileInfo.PhoneNumber,
                Email = UserContext.ProfileInfo.Email,
                State = UserContext.ProfileInfo.State
            };//getBillingInfo();

            Session["ProfileInfo"] = model.ProfileInformation;

            OrderHistorySnapshot orderHistory = GetCustomerPlanList();


            foreach (var order in orderHistory.OrderInfos)
            {
                model.MyOrders.Add(
                    new EachOrderSnapshot
                    {
                        OrderId = order.OrderId,
                        PlanName = order.PlanName,
                        AllowCallDetails = order.AllowCDR,
                        AccountNumber = order.AccountNumber,
                        PlanType = Helpers.CheckMandatoryAutorefill(SafeConvert.ToInt32(order.PlanId)),
                        CurrencyCode = order.CurrencyCode,
                        AllowCallForwarding = order.AllowCallForwading,
                        AllowOneTouchSetup = true,
                        AllowPinlessSetup = order.AllowPinless,
                        AllowQuickkeySetup = order.AllowQuickkey,
                        AllowRecharge = order.AllowRecharge,
                        AllowAutoRefill = order.AutoRefillStatus,
                        IsActivePlan = order.IsActivePlan,
                        TransactionDate = order.TransactionDate,
                        ShowBalance = order.ShowBalance,
                        ServiceFee = order.ServiceFee,
                        CardId = SafeConvert.ToInt32(order.PlanId),
                        MyAccountBal = order.AccountBalance.ToString(),//_repository.GetPinBalance(order.AccountNumber),
                        CountryFrom = order.CountryFrom,
                        CountryTo = order.CountryTo

                    }
                        );

            }

            //GetCustomerPlanDetail(model, orderHistory);

            model.MyOrders.OrderByDescending(a => a.TransactionDate);

            return View(model);

        }

        [CompressFilter]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult RedeemRewardNow()
        {
            var points = _repository.GetRazaReward(UserContext.MemberId);
            if (SafeConvert.ToInt32(points) < 1000)
            {
               return RedirectToAction("RazaReward", "Account");
            }
            var model = new MyAccountModel();
            OrderHistorySnapshot orderHistory = GetCustomerPlanList();
            foreach (var order in orderHistory.OrderInfos)
            {
                model.MyOrders.Add(
                    new EachOrderSnapshot
                    {
                        OrderId = order.OrderId,
                        PlanName = order.PlanName,
                        AllowCallDetails = order.AllowCDR,
                        AccountNumber = order.AccountNumber,
                        EncapsuleAccountNumber = order.AccountNumber.Remove(3, 4).Insert(3, "XXXX"),
                        CurrencyCode = order.CurrencyCode,
                        AllowRecharge = order.AllowRecharge,
                        IsActivePlan = order.IsActivePlan,
                        ShowBalance = order.ShowBalance,
                        ServiceFee = order.ServiceFee,
                        CardId = SafeConvert.ToInt32(order.PlanId),
                        MyAccountBal = order.AccountBalance.ToString(),
                        //_repository.GetPinBalance(order.AccountNumber),
                        CountryFrom = order.CountryFrom,
                        CountryTo = order.CountryTo
                    }
                    );

            }

            if (model.MyOrders.Any(x => (x.AllowRecharge == true && x.IsActivePlan == true)))
            {


                var fs = new List<State>();
                var result = CacheManager.Instance.GetFromCountries().OrderBy(x => Convert.ToInt32(x.Id)).Take(3).ToList();
                ViewBag.Cont = new SelectList(Helpers.ConvertCountryModelListToCountryList(_repository.GetCountryList("From").OrderBy(x => Convert.ToInt32(x.Id)).Take(3).ToList()));

                foreach (var res in result)
                {
                    foreach (var dat in _repository.GetStateList(Convert.ToInt16(res.Id)))
                    {
                        fs.Add(dat);
                    }
                }

                model.StateList = fs;
                model.Statelist = new SelectList(fs, "id", "name");
                ViewBag.state = model.Statelist;
                
                model.RedeemPoint = SafeConvert.ToInt32(points);
                var data = CacheManager.Instance.GetFromCountries().FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                int country = SafeConvert.ToInt32(data.Id);
                //GetCustomerPlanDetail(model, orderHistory);
                var list = GetListforReedemPoint(model.RedeemPoint, country);
                model.CurrencySign = country == 3 ? "£" : "$";

                model.ReedemPointsList = list;
                return View(model);
            }
            else
            {
               // var points = _repository.GetRazaReward(UserContext.MemberId);
                TempData["points"] = SafeConvert.ToInt32(points);
                return RedirectToAction("GlobalRedeemReward", "Account");
            }

        }

        private List<PointListModel> GetListforReedemPoint(int points, int country)
        {
            var list = new List<PointListModel>();



            if (points >= 1100 && points < 2200)
            {
                list.Add(new PointListModel()
                {
                    ReedemPoints = "1100",
                    PointValue = country == 3 ? 5 : 10,
                });
            }
            else if (points >= 2200 && points < 5500)
            {
                list.Add(new PointListModel()
                {
                    ReedemPoints = "1100",
                    PointValue = country == 3 ? 5 : 10,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "2200",
                    PointValue = country == 3 ? 10 : 20,
                });
            }
            else if (points >= 5500 && points < 9900)
            {
                list.Add(new PointListModel()
                {
                    ReedemPoints = "1100",
                    PointValue = country == 3 ? 5 : 10,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "2200",
                    PointValue = country == 3 ? 10 : 20,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "5500",
                    PointValue = country == 3 ? 25 : 50,
                });
            }
            else if (points >= 5500)
            {
                list.Add(new PointListModel()
                {
                    ReedemPoints = "1100",
                    PointValue = country == 3 ? 5 : 10,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "2200",
                    PointValue = country == 3 ? 10 : 20,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "5500",
                    PointValue = country == 3 ? 15 : 50,
                });
                list.Add(new PointListModel()
                {
                    ReedemPoints = "9900",
                    PointValue = country == 3 ? 50 : 90,
                });
            }


            return list;
        }

        [CompressFilter]
        [HttpGet]
        [UnRequiresSSL]
        public ActionResult GlobalRedeemReward()
        {
            var model = new MyAccountModel();
            model.RedeemPoint = SafeConvert.ToInt32(TempData["points"]);
            var fs = new List<State>();
            var result = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();
            ViewBag.Cont = new SelectList(Helpers.ConvertCountryModelListToCountryList(_repository.GetCountryList("From").OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList()));

            foreach (var res in result)
            {
                foreach (var dat in _repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            model.StateList = fs;
            model.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = model.Statelist;
            return View(model);
        }
        //[HttpGet]
        //[Authorize]
        //public ActionResult GetRedeemPoint()
        //     {
        //         var model = new List<EachOrderSnapshot>();



        //         return Json(model,JsonRequestBehavior.AllowGet);
        //     }

        [CompressFilter]
        [HttpGet]
        [Authorize]
        [RequiresSSL]
        public ActionResult BillingInfo()
        {
            var result = new BillingInfoModel();
            var fs = new List<State>();
            var repository = new DataRepository();
            var data = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();
            ViewBag.Cont =
                  new SelectList(Helpers.ConvertCountryModelListToCountryList(CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList()));

            foreach (var res in data)
            {

                foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                {

                    fs.Add(dat);

                }
            }

            result.Country = UserContext.ProfileInfo.Country;

            //var response= data.FirstOrDefault(a => a.Name == result.Country);
            //result.States = _repository.GetStateList(SafeConvert.ToInt32(response.Id));

            result.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = result.Statelist;

            ViewBag.FetchDataFromUrl = Url.Action("GetBillingInfo", "Account", null, HttpContext.Request.Url.Scheme);

            return View(result);
        }


        
        public JsonResult GetBillingInfo()
        {
            var data = getBillingInfo();
            var billingInfoModel = new GetBillingInfoJsonModel()
            {
                Address = data.Address,
                City = data.City,
                Countries = data.Countries,
                Country = data.Country,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                State = data.State,
                Statelist = data.Statelist,
                MemberId = data.MemberId,
                ZipCode = data.ZipCode,
                PhoneNumber = data.PhoneNumber,
                States = data.States
            };

            return Json(billingInfoModel, JsonRequestBehavior.AllowGet);
        }

        private BillingInfoModel getBillingInfo()
        {
            var repository = new DataRepository();

            BillingInfo billInf = repository.GetBillingInfo(UserContext.MemberId);
            Session["Countrybelongs"] = billInf.Country;

            var billingInfoModel = new BillingInfoModel
                {
                    Address = billInf.Address,
                    City = billInf.City,
                    Country = billInf.Country,
                    FirstName = billInf.FirstName,
                    LastName = billInf.LastName,
                    State = billInf.State,
                    ZipCode = billInf.ZipCode,
                    Email = billInf.Email,
                    MemberId = billInf.MemberId,
                    PhoneNumber = billInf.PhoneNumber
                };

            return billingInfoModel;
        }


        [HttpPost]
        [RequiresSSL]
        public JsonResult
            BillingInfo(BillingInfoModel vgn)
        {
            try
            {
                var repository = new DataRepository();

                var billInf = new BillingInfo();
                var billInfMod = vgn;
                billInf.Address = billInfMod.Address;
                billInf.City = billInfMod.City;
                billInf.Country = billInfMod.Country;
                billInf.FirstName = billInfMod.FirstName;
                billInf.LastName = billInfMod.LastName;
                billInf.State = string.IsNullOrEmpty(billInfMod.State) ? string.Empty : billInfMod.State;
                billInf.ZipCode = billInfMod.ZipCode;
                billInf.Email = billInfMod.Email;
                billInf.PhoneNumber = billInfMod.PhoneNumber;
                billInf.MemberId = UserContext.MemberId;
                billInf.UserType = UserContext.UserType;
                billInf.RefererEmail = string.Empty;

                if (!string.IsNullOrWhiteSpace(vgn.OldPwd) && !string.IsNullOrWhiteSpace(vgn.NewPwd) && vgn.NewPwd.Length > 5)
                {
                    billInf.OldPwd = vgn.OldPwd;
                    billInf.NewPwd = vgn.NewPwd;
                    var response = repository.UpdatePassword(UserContext.MemberId, billInf.OldPwd, billInf.NewPwd);
                    if (!response.Status)
                    {
                        return
                            Json(
                                new
                                {
                                    status = true,
                                    message = response.Errormsg
                                });
                    }
                }
                var respose = repository.UpdateBillingInfo(billInf);

                if (respose.Status)
                {
                    var bi = repository.GetBillingInfo(UserContext.MemberId);

                    UserContext.ProfileInfo = bi;

                    Raza.Model.UserContext usercont = new UserContext();
                    usercont.ProfileInfo = bi;
                    usercont.MemberId = bi.MemberId;
                    usercont.IsEmailSubscribed = UserContext.IsEmailSubscribed;
                    usercont.Email = bi.Email;
                    usercont.UserType = UserContext.UserType;


                    base.UpdateUserContext(UserContext.Email, usercont);

                    Session["usercontext"] = UserContext;


                    return
                        Json(
                            new
                            {
                                status = true,
                                redirectUrl = Url.Action("BillingInfo", "Account"),
                                isRedirect = true,
                                message = "Updated Successfully!"
                            });
                }
                else
                {
                    return
                        Json(
                            new
                            {
                                status = false,
                                redirectUrl = Url.Action("BillingInfo", "Account"),
                                isRedirect = true,
                                message = respose.Errormsg
                            });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { Message = "An error occured. please try again" });
            }

            return null;

        }

        private OrderHistorySnapshot GetCustomerPlanList()
        {
            return new DataRepository().GetCustomerPlanList(UserContext.MemberId);

        }

        [CompressFilter]
        [Authorize]
        [RequiresSSL]
        public ActionResult OrderHistory()
        {
            OrderHistoryModel orderHistoryModel = Helper.ConvertOrderHistoryToModel(GetOrderHistory());
            ViewBag.currentdate = DateTime.Now;
            return View(orderHistoryModel);
        }

        private OrderHistory GetOrderHistory()
        {
            //return new DataRepository().GetOrderHistory(UserContext.MemberId);
            var getorder = _repository.GetOrderHistory(UserContext.MemberId);

            var orderhistorymodel = new OrderHistory();

            foreach (var order in getorder.Orders)
            {

                orderhistorymodel.Orders.Add(new OrderHistoric()
                {
                    // <orderid>,<planname>,<pinnumber>,<transaction_date>,<currency> <amount>,<transaction_type>

                    OrderId = order.OrderId,
                    PlanId = order.PlanId,
                    PlanName = order.PlanName,
                    AccountNumber = order.AccountNumber,
                    TransactionDate = order.TransactionDate,
                    CurrencyCode = order.CurrencyCode,
                    TransactionAmount = order.TransactionAmount,
                    TransactionType = order.TransactionType,
                    AllowCDR = order.AllowCDR,
                    AllowPinless = order.AllowPinless,
                    AllowQuickkey = order.AllowQuickkey,
                    AllowRecharge = order.AllowRecharge,
                    AutoRefillStatus = order.AutoRefillStatus,
                    ServiceFee = order.ServiceFee,
                    IsActivePlan = order.IsActivePlan,
                    ShowBalance = order.ShowBalance,
                    MyAccountBal = _repository.GetPinBalance(order.AccountNumber),


                });
            }

            return orderhistorymodel;
        }

        [CompressFilter]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult CallDetailReport(string order_id, string date)
        {
            DateTime Date = Convert.ToDateTime(date);

            var repository = new DataRepository();

            string pinnumber = string.Empty;
            string planName = string.Empty;
            //ViewBag.Pin = Pin;
            OrderHistorySnapshot orderHistory = GetCustomerPlanList();
            var plandata = orderHistory.OrderInfos.FirstOrDefault(a => a.OrderId == order_id);


            if (plandata != null)
            {
                pinnumber = plandata.AccountNumber;
                planName = plandata.PlanName;
            }


            ViewBag.pinNumber = pinnumber.Remove(3, 5).Insert(3, "XXXXX");

            ViewBag.PlanName = planName;

            var dt = Convert.ToDateTime(date);

            var startdate = new DateTime(dt.Year, dt.Month, 1);
            var enddate = startdate.AddMonths(1).AddDays(-1);

            var data = repository.GetCallHistory(pinnumber, startdate.ToShortDateString(), enddate.ToShortDateString());


            ViewBag.CurrentMonth = (MonthsInCallDetail)System.DateTime.Now.Month;
            //           var res = (MonthsInCallDetail)System.DateTime.Now.Month;

            if (DateTime.Now.Month == 1)
            {
                ViewBag.PastOneMonth = "December";
                ViewBag.PastSecondMonth = "November";
            }
            else if (DateTime.Now.Month == 2)
            {
                ViewBag.PastOneMonth = (MonthsInCallDetail)System.DateTime.Now.Month - 1;
                ViewBag.PastSecondMonth = "December";
            }
            else
            {
                ViewBag.PastOneMonth = (MonthsInCallDetail)DateTime.Now.Month - 1;
                ViewBag.PastSecondMonth = (MonthsInCallDetail)DateTime.Now.Month - 2;
            }


            //DateTime startdate = Convert.ToDateTime(Date.AddMonths(-1));
            //DateTime enddate = Convert.ToDateTime(Date.AddMonths(1));

            ViewBag.orderid = order_id;
            CallDetailsModel callDetailsModel = Helper.ConvertCallDetailsToModel(data, Convert.ToDateTime(date));
            //CallDetailsModel callDetailsModel = Helper.ConvertCallDetailsToModel(data, DateTime.MinValue, DateTime.MinValue);

            return View(callDetailsModel);
        }


        [CompressFilter]
        [ActionName("CallDetailReportSearch")]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult CallDetailReport(string order_id, DateTime date)
        {
            var repository = new DataRepository();

            ViewBag.CurrentMonth = (MonthsInCallDetail)DateTime.Now.Month;
            if (DateTime.Now.Month == 1)
            {
                ViewBag.PastOneMonth = "December";
                ViewBag.PastSecondMonth = "November";
            }
            else if (DateTime.Now.Month == 2)
            {
                ViewBag.PastOneMonth = (MonthsInCallDetail)System.DateTime.Now.Month - 1;
                ViewBag.PastSecondMonth = "December";
            }
            else
            {
                ViewBag.PastOneMonth = (MonthsInCallDetail)DateTime.Now.Month - 1;
                ViewBag.PastSecondMonth = (MonthsInCallDetail)DateTime.Now.Month - 2;
            }

            //  var orderHistory = repository.GetOrderHistory(UserContext.MemberId);
            OrderHistorySnapshot orderHistory = GetCustomerPlanList();
            string pinnumber = string.Empty;
            string planName = string.Empty;
            var plandata = orderHistory.OrderInfos.FirstOrDefault(a => a.OrderId == order_id);


            if (plandata != null)
            {
                pinnumber = plandata.AccountNumber;
                planName = plandata.PlanName;
            }

            var startdate = new DateTime(date.Year, date.Month, 1);
            var enddate = startdate.AddMonths(1).AddDays(-1);

            ViewBag.pinNumber = pinnumber.Remove(3, 5).Insert(3, "XXXXX");
            ViewBag.PlanName = planName;
            var data = repository.GetCallHistory(pinnumber, startdate.ToShortDateString(), enddate.ToShortDateString());
            ViewBag.orderid = order_id;

            // DateTime startdate = Convert.ToDateTime(date.AddMonths(-1));
            // DateTime enddate = Convert.ToDateTime(date.AddMonths(1));

            CallDetailsModel callDetailsModel = Helper.ConvertCallDetailsToModel(data, Convert.ToDateTime(date));

            return View("CallDetailReport", callDetailsModel);
        }


        [CompressFilter]
        [HttpGet]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult PinLessSetupEdit(string order_id)
        {
            var pinlesssetupedit = new PinlessSetupEdit();
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            var plandata = result.OrderInfos.FirstOrDefault(a => a.OrderId == order_id);
            //pinlesssetupedit.OrderID = order_id;

            ViewBag.orderid = order_id;

            ViewBag.Plan_id = plandata.PlanId;
            // var fromCountries = _repository.GetCountryList("From");
            //pinlesssetupedit.CountryFromList = fromCountries;

            //var info = _repository.GetPlanDetails(pinlesssetupedit.OrderID);
            //pinlesssetupedit.Pin = info.Pin;

            //var pinlessnumbers = new PinLessNumbers();
            //pinlessnumbers = _repository.GetPinLessNumber(pinlesssetupedit.Pin);
            //pinlesssetupedit.PinlessNumberList = pinlessnumbers.PinlessNumberList;

            return View(pinlesssetupedit);

        }


        [Authorize]
        [UnRequiresSSL]
        public JsonResult GetPinlessResult(string order_id)
        {
            var pinlesssetupedit = new PinlessSetupEdit();

            pinlesssetupedit.OrderID = order_id;

            var fromCountries = CacheManager.Instance.GetFromCountries();
            pinlesssetupedit.CountryFromList = fromCountries;

            var info = _repository.GetPlanDetails(pinlesssetupedit.OrderID, UserContext.MemberId);
            pinlesssetupedit.Pin = info.Pin;

            var pinlessnumbers = new PinLessNumbers();
            pinlessnumbers = _repository.GetPinLessNumber(pinlesssetupedit.Pin);
            pinlesssetupedit.PinlessNumberList = pinlessnumbers.PinlessNumberList;

            return Json(pinlesssetupedit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("PinLessSetupEdit")]
        [Authorize]
        public JsonResult PinLessSetupEdit(PinlessSetupEdit pinlesssetupedit)
        {
            PlanInfo info = _repository.GetPlanDetails(pinlesssetupedit.OrderID, UserContext.MemberId);
            pinlesssetupedit.Pin = info.Pin;

            string str = pinlesssetupedit.PinNumber;

            string MemberID = UserContext.MemberId;
            string AniName = UserContext.FullName;

            string RequestedBy = "";
            string Pin = pinlesssetupedit.Pin;
            string AniNumber = str;
            string CoutryCode = pinlesssetupedit.CoutryCode;
            var fromCountries = CacheManager.Instance.GetFromCountries();
            pinlesssetupedit.CountryFromList = fromCountries;

            if (CoutryCode == "1" || CoutryCode == "2")
                CoutryCode = "1";
            else if (CoutryCode == "3")
                CoutryCode = "44";

            var cdata = CacheManager.Instance.GetFromCountries().FirstOrDefault(a => a.Id == CoutryCode);
            CoutryCode = cdata.CountCode;

            var response = _repository.PinLessSetUpEdit(AniName, MemberID, RequestedBy, Pin, AniNumber, CoutryCode);

            if (response.Status)
            {
                return Json(new { message = "Phone Number(s) Successfully Added!!" });
            }
            else
            {
                // access number can not be used for pinless setup
                // if that phone number already exist, then you will get response message as "already exist"
                // and if aninumber is not 10 digit number then you will receive err msg as "invalid ani number"
                string errormsg = string.Empty;
                if (response.Errormsg == "access number can not be used for pinless setup")
                {
                    errormsg = "Error:Raza.Com Access Numbers Can Not Be Used as PinLess Phone Number !!! ";
                }
                else if (response.Errormsg == "already exist")
                {
                    errormsg = "This number is already registered on a different plan.";
                }
                else if (response.Errormsg == "invalid ani number")
                {
                    errormsg = "Invalid Ani number, please try with a valid number. ";
                }
                else
                {
                    errormsg = "Unable to process your request, please try again.";
                }
                return Json(new { message = errormsg });
            }


        }


        [Authorize]
        [HttpPost]
        [UnRequiresSSL]
        public JsonResult DeletePinLessSetup(string pn, string cd, string oid)
        {
            var pinlesssetupedit = new PinlessSetupEdit();

            PlanInfo info = _repository.GetPlanDetails(oid, UserContext.MemberId);
            pinlesssetupedit.Pin = info.Pin;

            string RequestedBy = UserContext.FirstName;
            string Pin = pinlesssetupedit.Pin;
            string AniNumber = Regex.Replace(pn, @"[^\d]", string.Empty);
            //string AniNumber = pn;
            string CountryCode = cd;
            var fromCountries = CacheManager.Instance.GetFromCountries();
            pinlesssetupedit.CountryFromList = fromCountries;
            pinlesssetupedit.OrderID = oid;
            var response = _repository.DeletePinLessSetUp(Pin, AniNumber, CountryCode, RequestedBy);

            if (response != null)
            {
                return Json(new { message = "Phone Number(s) Successfully deleted.", status = true });
            }

            return Json(new { message = "Operation failed.", status = false });
        }

        [CompressFilter]
        [HttpGet]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult QuickKeysSetupEdit(string order_id)
        {
            //string currencycode, string RecBal, string servicefee, int plan_id, int countryfrom, int countryto
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            var plandata = result.OrderInfos.FirstOrDefault(a => a.OrderId == order_id);

            var edit = new QuickeysSetupEdit();
            edit.RechargeBalance = plandata.MyAccountBal;
            edit.OrderId = order_id;
            edit.ServiceFee = edit.ServiceFee;
            edit.CurrencyCode = plandata.CurrencyCode;
            edit.CountryTo = plandata.CountryFrom;
            edit.CountryFrom = plandata.CountryTo;
            edit.PlanId = SafeConvert.ToInt32(plandata.PlanId);
            var amount = _repository.RechargeAmountVal(SafeConvert.ToInt32(plandata.PlanId));
            edit.AmountRecharge = amount.GetAmountlist;
            BillingInfo bi = _repository.GetBillingInfo(UserContext.MemberId);
            edit.Email = bi.Email;
            return View(edit);
        }

        [UnRequiresSSL]
        public JsonResult GetQuickData(string order_id)
        {
            var edit = new Models.QuickeysSetupEdit();
            var countryList = CacheManager.Instance.GetFromCountries();
            edit.CountryList = new SelectList(countryList, "Id", "Name");

            edit.Tocountry = CacheManager.Instance.GetCountryListTo();


            Session["CountryFrom"] = edit.CountryList;


            Session["CountryTo"] = edit.ToCountryList;

            edit.returndate = DateTime.Now;
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            foreach (var Fill in result.OrderInfos.Where(x => x.OrderId == order_id))
            {


                edit.PinNumber = Fill.AccountNumber.Remove(3, 5).Insert(3, "XXXXX");

                edit.Pin = Fill.AccountNumber;

                edit.CurrencyCode = Fill.CurrencyCode;

                if (Fill.OrderId == order_id)
                {

                    edit.planname = Fill.PlanName;

                    edit.OrderId = order_id;
                    edit.CurrencyCode = Fill.CurrencyCode;
                    break;
                }

            }

            BillingInfo bi = _repository.GetBillingInfo(UserContext.MemberId);
            edit.Email = bi.Email;

            var setUp = new QuickeySetup();
            setUp = _repository.GetQuickKeyNumbers(edit.Pin);
            edit.QuickeyEdit = setUp.quickeyList;
            return Json(edit, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ActionName("QuickkeysSetupEdit")]
        [Authorize]
        [UnRequiresSSL]
        public JsonResult QuickkeysSetupEdit(QuickeysSetupEdit model)
        {
            model.MemberID = UserContext.MemberId;



            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            foreach (var Fill in result.OrderInfos.Where(x => x.OrderId == model.OrderId))
            {

                if (Fill.OrderId == model.OrderId)
                {
                    model.OrderId = Fill.OrderId;
                    model.CurrencyCode = Fill.CurrencyCode;
                    break;
                }

            }


            model.list = (List<Country>)Session["List"];
            model.Email = UserContext.Email;
            string MemID = model.MemberID;
            string Pin = model.Pin;
            string CountryCode = model.CountryCode;
            string DestinationNumber = model.DestinationNumber;
            string SpeedDialNumber = model.SpeedDialNumber;
            string NickName = model.NickName;
            var data = _repository.QuickKeySetUp(MemID, Pin, CountryCode, DestinationNumber, SpeedDialNumber, NickName);
            if (data.Status)
            {
                return Json(new { message = "Quickey(s) Number Successfully Added!" });
            }
            else
            {
                return Json(new { message = data.Errormsg });
            }

        }

        [Authorize]
        [UnRequiresSSL]
        public JsonResult DeleteQuickeys(string destNum, string spdial, string pin)
        {

            //string pin = "4495215980";
            //string destNum =desno;
            string specDialNum = spdial;
            string reqBy = UserContext.FirstName;
            bool response = _repository.DeleteQuickKeySetUp(pin, destNum, specDialNum, reqBy);
            if (response == true)
            {
                return Json(new { Status = "Deleted" });
            }
            else
            {
                return Json(new { Status = "Error..!!" });
            }
        }

        [UnRequiresSSL]
        public JsonResult GetOneTouchNumber(string orderid)
        {
            string Pin = string.Empty;
            string PlanName = string.Empty;
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            foreach (var oneTouch in result.OrderInfos.Where(a => a.OrderId == orderid))
            {
                Pin = oneTouch.AccountNumber;
                PlanName = oneTouch.PlanName;
            }
            var setUp = new OneTouchSet();
            setUp = _repository.GetOneTouchSetup(Pin);
            var list = new List<OneTouchSetups>();
            if (setUp.oneTouchList.Any())
            {
                foreach (var item in setUp.oneTouchList)
                {
                    list.Add(new OneTouchSetups()
                    {
                        Destination = item.Destination,
                        OneTouch_Name = item.OneTouch_Name,
                        OneTouch_Number = item.OneTouch_Number,
                        PlanName = PlanName
                    });
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [CompressFilter]
        [HttpGet]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult OnetouchSetup(string order_id)
        {
            var edit = new OnetouchSetupEdit { PinNumber = order_id };
            //var countryList = _repository.GetCountryList("from").OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();
            var countryList = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();
            edit.CountryList = new SelectList(countryList, "Id", "Name");
            Session["CountryFrom"] = edit.CountryList;
            countryList = CacheManager.Instance.GetCountryListTo();
            edit.ToCountryList = new SelectList(countryList, "CountryCode", "Name");
            Session["CountryTo"] = edit.ToCountryList;
            edit.OrderId = order_id;
            //UserContext.PinNumber = id;
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            foreach (var oneTouch in result.OrderInfos.Where(a => a.OrderId == order_id))
            {
                edit.Plan_Name = oneTouch.PlanName;
                edit.Pin = oneTouch.AccountNumber;
            }
            var setUp = new OneTouchSet();
            setUp = _repository.GetOneTouchSetup(edit.Pin);
            edit.List = setUp.oneTouchList;

            return View(edit);
        }

        [HttpGet]
        public JsonResult GetStates(string id)
        {
            var res = _repository.GetStateList(SafeConvert.ToInt32(id));

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAreas(string id)
        {
            var res = _repository.GetAreacode(id);

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [UnRequiresSSL]
        public JsonResult GetAvailableNumbers(string pin, int countryId, string state, string areaCode)
        {
            string Pin = pin;

            var res = _repository.GetAvailableOneTouchNumbers(Pin, countryId, state, areaCode);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [UnRequiresSSL]
        public JsonResult OnetouchSetup(OnetouchSetupEdit model)
        {
            try
            {


                var result = _repository.GetCustomerPlanList(UserContext.MemberId);

                foreach (var oneTouch in result.OrderInfos.Where(a => a.OrderId == model.OrderId))
                {

                    model.Plan_Name = oneTouch.PlanName;
                    model.Pin = oneTouch.AccountNumber;

                }

                string Pin = model.Pin;

                model.OneTouch_Number = model.AvailableNumber.Replace("-", string.Empty);
                string OneTouch_Number = model.OneTouch_Number;

                string Destination = model.CountryCode + string.Empty + model.Destination;

                string OneTouch_Name = model.OneTouch_Name;

                string Added_By = string.Empty;

                string planname = model.Plan_Name;

                var data = _repository.OneTouchSetup(Pin, OneTouch_Number, Destination, OneTouch_Name, Added_By);
                if (data.Status)
                {
                    //OneTouchSet setUp = this._repository.GetOneTouchSetup(model.Pin);
                    //model.List = setUp.oneTouchList;
                    //OnetouchMailsend(setUp, planname);

                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false, message = data.Errormsg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }



        [Authorize]
        [UnRequiresSSL]
        public ActionResult DeleteOneTouchSetup(string num, string pin)
        {
            string deletedBy = UserContext.FirstName;
            bool response = _repository.DeleteOneTouchSetup(pin, num, deletedBy);
            if (response)
            {
                return Json(new { Status = true });
            }
            else
            {
                return Json(new { Status = false });

            }
        }


        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult LogOn()
        {

            var viewmodel = new LogOnModel { list = CacheManager.Instance.GetFromCountries() };

            HttpCookie existingCookie = Request.Cookies["userName"];
            if (existingCookie != null)
            {
                viewmodel.UserName = existingCookie.Value;
            }
            string returnUrl = string.Empty;
            if (Request.QueryString["returnUrl"] != null)
            {
                returnUrl = Request.QueryString["returnUrl"];
            }


            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                // returnUrl = Request.UrlReferrer.ToString(); //Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
                returnUrl = Request.UrlReferrer.PathAndQuery;


            //********** Added on 08/13/15 TO ADD COUNTRYTO WITH RETURN URL
            if (string.IsNullOrEmpty(returnUrl) == false)
            {
                if (returnUrl.ToLower().IndexOf("mothersdayexistingcustomer") > -1 || returnUrl.ToLower().IndexOf("valentinesdayexistingcustomer") > -1)
                {
                    //returnUrl = Server.UrlDecode(returnUrl);
                    returnUrl = returnUrl + "&countryto=" + Request.QueryString["countryto"];
                }

                if (returnUrl.ToLower().IndexOf("august14existingcustomer") > -1)
                    returnUrl = "/Promotion/August14ExistingCustomer?countryfrom=1&countryto=238";
                else if (returnUrl.ToLower().IndexOf("august15existingcustomer") > -1)
                    returnUrl = "/Promotion/August15ExistingCustomer?countryfrom=1&countryto=130";
                else if (returnUrl.ToLower().IndexOf("rakhiexistingcustomer") > -1)
                    returnUrl = "/Promotion/RakhiExistingCustomer?countryfrom=1&countryto=130";
            }

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }

            return View("LogOn", viewmodel);
        }


        [UnRequiresSSL]
        public ActionResult LogOff()
        {
            var repository = new DataRepository();
            repository.Signout();
            FormsAuthentication.SignOut();
            Session["Cart"] = null;
            Session["TryusFree"] = null;
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult LogOn(string EmailAddress, string Password, string retUrl)
        {

            if (Session["ErrorMessage"] != null)
            {
                return View();
            }
            return LogOnWithAction(EmailAddress, Password, "MyAccount", retUrl);
        }



        [HttpPost]
        public ActionResult LogOnWithAction(string EmailAddress, string Password, string Action, string retUrl)
        {
            var vgn = new LogOnModel();
            ViewBag.ReturnURL = retUrl;
            string decodedUrl = "";
            if (!string.IsNullOrEmpty(retUrl))
                decodedUrl = Server.UrlDecode(retUrl);

            string Email = EmailAddress.Trim();

            vgn = new LogOnModel { EmailAddress = Email, Password = Password };

            try
            {

                if (!string.IsNullOrWhiteSpace(EmailAddress) && !string.IsNullOrWhiteSpace(Password))
                {
                    var usercontext = Authenticate(vgn.EmailAddress, vgn.Password);
                    if (usercontext != null && usercontext.ServiceResponse != "customer blocked")
                    {
                        if (!string.IsNullOrEmpty(decodedUrl))
                        {
                            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

                            if (decodedUrl.StartsWith("/"))
                                decodedUrl = decodedUrl.Remove(0, 1);
                            if (decodedUrl.ToLower().Contains("cart/checkout") && usercontext.UserType.ToLower() == "old")
                            {
                                var planlist = _repository.GetCustomerPlanList(usercontext.MemberId);
                                var resp = planlist.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162" || a.PlanId == "102");
                                ShoppingCartModel shopmodel = Session["Cart"] as ShoppingCartModel ??
                                                              new ShoppingCartModel();
                                if (resp != null && (shopmodel.FromToMapping == "161" || shopmodel.FromToMapping == "162" || shopmodel.FromToMapping == "102"))
                                {
                                    return RedirectToAction("MyAccount", "Account", new { rf = "e" });
                                }
                                return RedirectToAction("Regphone", "Recharge");
                            }
                            return Redirect(baseUrl + "/" + decodedUrl.Replace('_', '#'));
                        }
                        switch (Action)
                        {
                            case "MyAccount":
                                return RedirectToAction("MyAccount", "Account");
                            case "BillingInfo":
                                return RedirectToAction("BillingInfo", "Account");
                            case "OrderHistory":
                                return RedirectToAction("OrderHistory", "Account");
                            case "RewardPoint":
                                return RedirectToAction("RazaReward", "Account");
                            case "ReferFriend":
                                return RedirectToAction("ReferFriend", "Account");
                            default:
                                //return Redirect(Request.UrlReferrer.ToString());
                                return RedirectToAction("MyAccount", "Account");
                        }
                    }
                    else if (usercontext != null && usercontext.ServiceResponse == "customer blocked")
                    {
                        vgn.Error = "Customer blocked";
                        //string message = vgn.Error;
                        Session["ErrorMessage"] = vgn.Error;
                        return View("LogOn", vgn);
                    }
                    else
                    {
                        vgn.Error = "Invalid email or password.";
                    }
                }
                else
                {
                    vgn.Error = "Email and Password cannot be blank.";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                vgn.Error = "Invalid email or password combination.";
            }

            return View("LogOn", vgn);
        }




        [CompressFilter]
        [Authorize]
        [HttpGet]
        [RequiresSSL]
        public ActionResult AutoRefill(string order_id)
        {
            var refill = new AutoRefil();
            ViewBag.order_id = order_id;

            return View(refill);

        }

        //public ActionResult UpdateCreditCard(int creditCardId)
        //{
        //    var creditCards = _repository.Get_top_CreditCard(UserContext.MemberId);
        //    var data = creditCards.GetCardList.FirstOrDefault(a => a.CreditCardId == creditCardId);


        //    return View("Partials/UpdateCreditCard");
        //}

        public ActionResult EditCreditCard(int creditCardId)
        {
            var card = _repository.Get_top_CreditCard(UserContext.MemberId);
            
            var data = card.GetCardList.FirstOrDefault(a => a.CreditCardId == creditCardId);

            var model = new EditCreditCardModel()
            {
                CreditCardId = data.CreditCardId,
                CreditCardName = data.CreditCardName,
                CreditCardNumber = data.CreditCardNumber.Remove(4, 8).Insert(4, "-XXXXXXXX-"),
                ExpiryMonth = data.ExpiryMonth,
                ExpiryYear = data.ExpiryYear,
                CreditCardType = data.CreditCardType,
                ExpMonthList = Helpers.GetMonthList(),
                ExpYearList = Helpers.GetYearList()
            };
            return PartialView("Partials/UpdateCreditCard", model);
        }

        [HttpPost]
        public ActionResult UpdateCreditCard(EditCreditCardModel model)
        {
            var data =
                _repository.Get_top_CreditCard(UserContext.MemberId)
                    .GetCardList.FirstOrDefault(a => a.CreditCardId == model.CreditCardId);
            var cdata =
                CacheManager.Instance.GetFromCountries().FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);

            var res = _repository.AddCreditCard(UserContext.MemberId, model.CreditCardName, data.CreditCardNumber,
                model.ExpiryMonth, model.ExpiryYear, model.CVV, cdata.Id, UserContext.ProfileInfo.Address,
                UserContext.ProfileInfo.City, UserContext.ProfileInfo.State, UserContext.ProfileInfo.ZipCode);

            if (res.Status)
            {
                return Json(new {status = true, message = "Credit Card successfully updated"});
            }
            else
            {
                return Json(new {status = false, message = res.Errormsg});
            }
            
        }


        [RequiresSSL]
        public JsonResult GetAutoRefilldata(string order_id)
        {
            var refill = new AutoRefil { Pin = order_id };

           // BillingInfo bi = _repository.GetBillingInfo(UserContext.MemberId);
            refill.Email = UserContext.Email;

            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            var data = result.OrderInfos.FirstOrDefault(a => a.OrderId == order_id);
            if (data != null)
            {
                refill.Planname = data.PlanName;
                refill.Pin = data.AccountNumber;
                if (data.AutoRefillStatus == "A" &&
                    Helpers.CheckMandatoryAutorefill(Convert.ToInt32(data.PlanId)) != "nm")
                {
                    refill.IsAutorefillEnroll = "M";
                }
                else if (data.AllowRecharge && data.AutoRefillStatus == "A")
                {
                        refill.IsAutorefillEnroll = "A";
                }
                else
                {
                    refill.IsAutorefillEnroll = "U";
                }
            }



            refill.Years = Enumerable.Range(DateTime.Now.Year, 11).ToList();


            var card = new GetTopCreditCards();
            card = _repository.Get_top_CreditCard(UserContext.MemberId);

            ViewBag.Pin = refill.Pin;
            refill.CardList = card.GetCardList;
            foreach (var info in refill.CardList)
            {
                string cardno = info.CreditCardNumber.Remove(4, 8).Insert(4, "-XXXXXXXX-");
                info.MaskCardNumber = cardno + ", " + info.ViewExpDate;
                info.CreditCardNumber = string.Empty;
            }
            return Json(refill, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult AddCreditCard(AutoRefil model)
        {
            string name = model.NameOnCard;
            string cardNumber = model.CreditCardNo;
            string expMonth = model.Exp_Month;
            string expYear = model.Exp_Year.Remove(0, 2);
            string cvv2 = model.CVV2;
            string billingAddress = UserContext.ProfileInfo.Address;
            string city = UserContext.ProfileInfo.City;
            string zipCode = UserContext.ProfileInfo.ZipCode;
            string state = UserContext.ProfileInfo.State;
            var data = CacheManager.Instance.GetFromCountries();
            string country = data != null
                ? SafeConvert.ToString(data.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country).Id)
                : string.Empty;

            //model.Planname = (string)Session["planname"];
            model.StateList = (List<State>)Session["statelist"];
            var res = _repository.AddCreditCard(UserContext.MemberId, name, cardNumber, expMonth, expYear, cvv2,
                country, billingAddress, city, state, zipCode);
            if (res.Status)
            {
                return Json(new { status = true, message = "Your credit card information is updated." });
            }
            else
            {
                return Json(new {status = false, message = res.Errormsg});
            }

        }

        [Authorize]
        [HttpPost]
        [RequiresSSL]
        public JsonResult AutoReFill(AutoRefil model)
        {
            model.MemberID = UserContext.MemberId;

            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            foreach (var autoReFill in result.OrderInfos.Where(a => a.OrderId == model.order_id))
            {
                model.Pin = autoReFill.AccountNumber;
                model.Planname = autoReFill.PlanName;
            }

            string pin = model.Pin;
            string memberId = model.MemberID;

            Double reFillAmount = model.ReFill_Amount;

            var card = _repository.Get_top_CreditCard(UserContext.MemberId);

            var data = card.GetCardList.FirstOrDefault(a => a.CreditCardId == SafeConvert.ToInt32(model.CreditCardNo));


            string cardNumber = data.CreditCardNumber;
            var cardlist = _repository.Get_top_CreditCard(UserContext.MemberId);
            string cardtype = cardlist.GetCardList.FirstOrDefault(a => a.CreditCardNumber == cardNumber).CreditCardName;
            model.CardType = cardtype;

            bool Data = _repository.AutoReFilledit(memberId, pin, reFillAmount, cardNumber);
            try
            {
                if (Data)
                {
                    AutorefillConfirmMail(model);
                    return Json(new { status = "The autorefill on your plan would be activated within 24 hours." });

                }
                else
                {
                    AutorefillUnsuccesMail(model);
                    return Json(new { status = "inputs are not valid." });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ValidationError = true, ValidationMessage = ex.Message });
            }


        }

        private void AutorefillConfirmMail(AutoRefil model)
        {
            if (!UserContext.IsEmailSubscribed) return;

            string email = UserContext.Email;

            try
            {
                int cardlength = model.CreditCardNo.Length;
                string paymentmethod = model.CardType + ",...." +
                                       model.CreditCardNo.Substring(cardlength - 4, 4);
                int acc = model.Pin.Length;
                string accountnumber = model.Pin.Substring(0, 5) + "XXXXX" +
                                       model.Pin.Substring(acc - 3, 2);
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = 0;
                double totalcharge = model.ReFill_Amount + servicecharge;

                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId();
                var currData = FlagDictonary.GetCurrencycodebyCountry();
                var currencycode = currData[usercountryid];
                if (UserContext != null && UserContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }
                string redirectlink = string.Empty;
                if (UserContext != null && UserContext.UserType.ToLower() == "old")
                {
                    redirectlink = "Account/MyAccount";
                }


                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/auto-refill-enrollment.html"));
                string servername = ConfigurationManager.AppSettings["ServerName"];
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--PlanName-->", model.Planname);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", model.order_id);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(model.ReFill_Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--CurrencyCode-->", currencycode);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Autorefillconfim"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending Autorefill confirmation mail: " + ex.Message);
            }


        }

        private void AutorefillUnsuccesMail(AutoRefil model)
        {
            if (!UserContext.IsEmailSubscribed) return;

            string email = UserContext.Email;

            try
            {
                int cardlength = model.CreditCardNo.Length;
                string paymentmethod = model.CardType + ",...." +
                                       model.CreditCardNo.Substring(cardlength - 4, 4);
                int acc = model.Pin.Length;
                string accountnumber = model.Pin.Substring(0, 4) + "XXXX" +
                                       model.Pin.Substring(acc - 3, 2);
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = 0;
                double totalcharge = model.ReFill_Amount + servicecharge;

                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId();
                if (UserContext != null && UserContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }
                string redirectlink = string.Empty;
                redirectlink = "Account/MyAccount";



                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/auto-refill_failed.html"));
                string servername = ConfigurationManager.AppSettings["ServerName"];
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--PlanName-->", model.Planname);
                mailbody = mailbody.Replace(@"<!--CardNumber-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--UserName-->", UserContext.FullName);
                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);



                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["AutorefillUnsuccess"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {

            }


        }

        private void AutorefillDeact()
        {
            if (!UserContext.IsEmailSubscribed) return;

            string email = UserContext.Email;
            try
            {
                string datetime = Convert.ToString(DateTime.Now);
                string username = UserContext.FullName;
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId();
                if (UserContext != null && UserContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }
                string redirectlink = string.Empty;
                if (UserContext != null && UserContext.UserType.ToLower() == "old")
                {
                    redirectlink = "Account/MyAccount";
                }

                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/auto-refill.html"));

                mailbody = mailbody.Replace(@"<!--UserName-->", username);
                mailbody = mailbody.Replace(@"<!--DateTime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);



                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Autorefillunsubscibe"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {

            }


        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult RemoveAutoRefill(AutoRefil model)
        {
            string memberId = UserContext.MemberId;

            var result = _repository.GetCustomerPlanList(UserContext.MemberId);
            string planid=string.Empty;
            foreach (var autoReFill in result.OrderInfos.Where(a => a.OrderId == model.order_id))
            {
                model.Pin = autoReFill.AccountNumber;
                planid = autoReFill.PlanId;
            }
            var chkdata = Helpers.CheckMandatoryAutorefill(Convert.ToInt32(planid));
            if (chkdata != "nm")
            {
                return Json(new { status = "Autorefill manadatory plan can not deactivate autorefill." });
            }
            string pin = model.Pin;
            string ip_Address = Request.UserHostAddress;
            bool data = _repository.AutoRefillRemove(memberId, pin, ip_Address);
            try
            {
                if (data)
                {
                    //AutorefillDeact();
                    return Json(new { status = "The autorefill on your plan would be deactivated within 24 hours." });

                }
                else
                {
                    return Json(new { status = "Inputs are not valid." });
                    //return Json(new { status = memberId+","+pin+","+ip_Address });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ValidationError = true, ValidationMessage = ex.Message });
                //return Json(new { status = ex.Message });
            }

        }






        [HttpPost]
        [RequiresSSL]
        public JsonResult Forgotpassword(string email)
        {
            try
            {
                var repository = new DataRepository();
                if (email != null)
                {
                    var password = repository.GetPassword(email);
                    string helplinenumber = string.Empty;
                    if (Session["HelpNumber"] != null)
                    {
                        helplinenumber = (string)Session["HelpNumber"];
                    }
                    else
                    {
                        helplinenumber = "1-(877) 463-4233";
                    }
                    string redirectlink = string.Empty;
                    if (UserContext != null && UserContext.UserType.ToLower() == "old")
                    {
                        redirectlink = "Account/MyAccount";
                    }

                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        try
                        {
                            string servername = ConfigurationManager.AppSettings["ServerName"];
                            string mailbody =
                                System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/forgot-password.html"));
                            mailbody = mailbody.Replace(@"<!--username-->", email);
                            mailbody = mailbody.Replace(@"<!--password-->", password);
                            mailbody = mailbody.Replace(@"<!--EmailId-->", email);
                            mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                            mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                            mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);

                            Helpers.SendEmail(
                                ConfigurationManager.AppSettings["senderemailaddress"],
                                ConfigurationManager.AppSettings["sendername"],
                                email,
                                ConfigurationManager.AppSettings["forgotpasswordsubject"],
                                mailbody,
                                true);

                            return Json(new
                            {
                                status = "Your password sent successfully."

                            });
                        }
                        catch (Exception ex)
                        {
                            return Json(new
                            {
                                status = ex.Message
                            });

                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = "Email Address not registered!"

                        });

                    }
                }
                else
                {
                    return Json(new
                    {
                        ValidationError = true,
                        ValidationMessage = "Invalid Data"
                    });

                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new
                {
                    ValidationError = true,
                    ValidationMessage = ex.Message
                });

            }
            return null;

        }

        [HttpPost]
        [UnRequiresSSL]
        public JsonResult MailAllOnetouchNumber(string orderid)
        {
            var details = _repository.GetCustomerPlanList(UserContext.MemberId);
            var plandata = details.OrderInfos.FirstOrDefault(a => a.OrderId == orderid);

            if (plandata != null)
            {
                var setuplist = _repository.GetOneTouchSetup(plandata.AccountNumber);

                OnetouchMailsend(setuplist, plandata.PlanName, orderid);
                return Json(new { status = true });
            }

            return Json(new { status = false });

        }


        private void OnetouchMailsend(OneTouchSet setup, string planname, string orderid)
        {
            if (!UserContext.IsEmailSubscribed) return;

            string email = UserContext.Email;
            string servername = ConfigurationManager.AppSettings["ServerName"];


            string redirectlink = "Account/MyAccount";
            string helplinenumber = string.Empty;
            var userCountryId = GetUserCountryId();
            if (UserContext != null && UserContext.ProfileInfo != null)
            {
                helplinenumber = Helpers.GetHelpLineNumber(userCountryId);
            }
            else
            {
                helplinenumber = (string)Session["HelpNumber"];
            }

            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/one-touch-dial.html"));

                string mailrow = string.Empty;

                int count = 0;
                foreach (var var in setup.oneTouchList)
                {
                    count++;
                    string temp = "<tr style='font-size:14px;' bgcolor='f3f3f3' height='40' >" +
                                  "<td align='center'  style=' border-left:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9; border-right:1px solid #c9c9c9; ' >" + count + " </td>" +
                                  "<td align='center'  style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9; border-left:1px solid #fff; '>" + planname + " </td>" +
                                  "<td align='center'  style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9;  border-left:1px solid #fff; '> <span>" + var.OneTouch_Number + "</span> </td>" +
                                  "<td align='center'  style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9; border-left:1px solid #fff; '> " + var.OneTouch_Name + " </td>" +
                                  "<td align='center'  style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9;  border-left:1px solid #fff; '> " + var.Destination + " </td>" +
                                  "</tr>";

                    mailrow = mailrow + temp;
                }

                mailbody = mailbody.Replace(@"<!--Onetouchrows-->", mailrow);
                mailbody = mailbody.Replace(@"<!--OrderId-->", orderid);
                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["onetouchsubject"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in OnetouchMail send:" + ex.Message);
            }


        }



        private void QuickkeyMailsend(QuickeySetup setup, string orderid)
        {
            string email = UserContext.Email;
            var repository = new DataRepository();
            try
            {
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/quickey-setup.html"));
                //mailbody = mailbody.Replace(@"<!--Destnumber-->", destnumber);
                //mailbody = mailbody.Replace(@"<!--Contactname-->", contactname);
                //mailbody = mailbody.Replace(@"<!--Quickkeys-->", quickkeys);
                string redirectlink = "Account/MyAccount";
                string helplinenumber = string.Empty;
                var userCountryId = GetUserCountryId();
                if (UserContext != null && UserContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(userCountryId);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }

                string mailrow = string.Empty;

                foreach (var var in setup.quickeyList)
                {
                    string temp = "<tr style='font-size:14px;' bgcolor='f3f3f3' height='40' >" +
                              "<td align='center' style=' border-left:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9; border-right:1px solid #c9c9c9; padding:0 0 0 10px;' > " + var.Destination + " </td>" +
                              "<td align='center' style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9; border-left:1px solid #fff;  padding:0 0 0 10px;'> " + var.NickName + "</td>" +
                              "<td align='center'  style=' border-right:1px solid #c9c9c9; border-top:1px solid #fff; border-bottom:1px solid #c9c9c9;  border-left:1px solid #fff; padding:0 0 0 10px'> " + var.SpeedDialNumber + "</td>" +
                              "</tr>";

                    mailrow = mailrow + temp;
                }

                mailbody = mailbody.Replace(@"<!--Quickkeys-->", mailrow);
                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--OrderId-->", orderid);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Quickkeysubject"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {

            }


        }

        [HttpPost]
        [Authorize]
        [UnRequiresSSL]
        public JsonResult MailQuickKeys(string orderId)
        {
            try
            {

                PlanInfo info = _repository.GetPlanDetails(orderId, UserContext.MemberId);
                string pin = info.Pin;

                var setUpList = _repository.GetQuickKeyNumbers(pin);
                QuickkeyMailsend(setUpList, orderId);
                return
                    Json(
                        new
                        {
                            status = true,
                        });
            }
            catch (Exception ex)
            {
                Json(
                        new
                        {
                            status = false,
                        });

            }
            return null;
        }


        public ActionResult SiteMap()
        {
            return View();

        }

        [UnRequiresSSL]
        public ActionResult FreeTrial()
        {
            return View();


            //var model = new GenericModel();
            ////model.CountrybyIp = (int)Session["CountrybyIp"];
            //return View(model);

        }

        
        public ActionResult GetThreeCountryFromList()
        {
            var res = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).Take(3).ToList();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCountryToCountryFromList()
        {
            var res = CacheManager.Instance.GetFromCountries().OrderBy(x => SafeConvert.ToInt32(x.Id)).ToList();


            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountryFromListFlag()
        {
            var res = CacheManager.Instance.GetFromCountries();

            IDictionary<int, string> flagdict = new Dictionary<int, string>();
            flagdict.Add(1, "/images/us.png");
            flagdict.Add(2, "/images/uk.png");
            flagdict.Add(3, "/images/canada.png");
            flagdict.Add(44, "/images/canada.png");

            foreach (var country in res)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'

                string flagname = flagdict[Convert.ToInt32(country.Id)];
                country.CountryFlag = "<a href=''><img src=" + flagname + " alt=''>" + country.Name + "</a>";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult GetCountryToList()
        {
            var res = CacheManager.Instance.GetCountryListTo();

            //var res = CacheManager.Instance.GetCountryListTo();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountryListParm(string searchtype, string dest)
        {
            var res = _repository.GetCountryList(searchtype, dest);
            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCountryListFull()
        {
            var list = CacheManager.Instance.GetAllCountryTo();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CountryToListwithFlag()
        {
            var res = CacheManager.Instance.GetAllCountryTo();
            var flagDict = FlagDictonary.FlagDictonaryforRate();

            foreach (var country in res)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'
                string flagname = string.Empty;
                if (flagDict.ContainsKey(country.Id))
                {
                    flagname = flagDict[country.Id];
                }
                else
                {
                    flagname = "/images/flag.jpg";
                }
                country.CountryFlag = string.Format("<img src={0} alt='{1}'>  {1} ", flagname, country.Name);
            }
            var r = res.Where(a => a.Id == "130").ToList();

            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCountryToListFlag()
        {

            var res = CacheManager.Instance.GetCountryListTo();
            var flagDict = FlagDictonary.CreateDictinary();

            foreach (var country in res)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'
                string flagname = string.Empty;
                if (flagDict.ContainsKey(Convert.ToInt32(country.Id)))
                {
                    flagname = flagDict[Convert.ToInt32(country.Id)];
                }
                else
                {
                    flagname = "/images/flag.jpg";
                }
                //country.CountryFlag = string.Format("<img src={0} alt='{1}'>  {1} (+{2})", flagname, country.Name, country.CountCode);
                country.CountryFlag = string.Format("<img src={0} alt='{1}'>  {1} ", flagname, country.Name);

            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCountryToListFlag2()
        {

            var res = CacheManager.Instance.GetCountryListTo();
            var flagDict = FlagDictonary.CreateDictinary();

            foreach (var country in res)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'
                string flagname = "";
                if (flagDict.ContainsKey(Convert.ToInt32(country.Id)))
                {
                    flagname = flagDict[Convert.ToInt32(country.Id)];
                }
                else
                {
                    flagname = "/images/flag.jpg";
                }
                country.CountryFlag = "<img src=" + flagname + " alt=''>" + country.Name;
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetTopCountrywithflag()
        {

            var res = CacheManager.Instance.GetCountryListTo();
            var flagDict = FlagDictonary.CreateDictinary();

            foreach (var country in res)
            {
                // string i = '<a href=""><img src="images/af-icon.png" alt="">Afghanistan (+93)</a>'
                string flagname = "";
                if (flagDict.ContainsKey(Convert.ToInt32(country.Id)))
                {
                    flagname = flagDict[Convert.ToInt32(country.Id)];
                }
                else
                {
                    flagname = "/images/flag.jpg";
                }
                country.CountryFlag = string.Format("<img src={0} alt='{1}'>  {1} (+{2})", flagname, country.Name, country.CountCode);
                string newname = "<img src=" + flagname + " /> ";
                country.Name = newname;
                //country.CountryFlag = flagname;
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [UnRequiresSSL]
        public JsonResult TopUpInfo(string mobilenumber, string moboperator, string amount)
        {
            return
                        Json(
                            new
                            {
                                redirectUrl = Url.Action("TopUp", "Account", new { mobile_number = mobilenumber, mob_operator = moboperator, _amount = amount }),
                                isRedirect = true,
                            });
        }


        [HttpGet]
        public ActionResult TopUp(string mobile_number, string mob_operator, string _amount)
        {
            var topupmodel = new TopupModel();
            topupmodel.MobileNumber = mobile_number;
            //  topupmodel.MobileOperator = mob_operator;
            topupmodel.TopupAmount = _amount;

            var fs = new List<State>();
            var data = _repository.GetCountryList("from");
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(_repository.GetCountryList("From")));

            foreach (var res in data)
            {

                foreach (var dat in _repository.GetStateList(Convert.ToInt16(res.Id)))
                {

                    fs.Add(dat);

                }
            }


            topupmodel.States = fs;
            topupmodel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = topupmodel.Statelist;

            return View(topupmodel);
        }

        [HttpPost]
        public ActionResult TopUp()
        {

            return View();

        }

        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult Faq()
        {
            var model = new Faq();
            return View(model);

        }

        //[HttpGet]
        //public ActionResult GetFreeTrialCountryList()
        //{
        //    var model = new FooterModel();
        //    model.TrailList = _repository.FreeTrial_Country_List();

        //    return View()
        //    //return Json(res, JsonRequestBehavior.AllowGet);
        //}

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


        //[CompressFilter]
        [ActionName("QuicksignUp")]
        public ActionResult LogOn(LogOnModel model)
        {
            string ipAddress2 = Request.UserHostAddress;
            int countryFrom = model.CountryFrom;
            if (countryFrom == 0)
            {
                countryFrom = 1;
            };
            int countryTo = model.CountryTo;
            if (countryTo == 0)
            {
                countryTo = 1;
            };

            string email = model.Email;
            string password = model.Password;
            string phone = model.Phone_Number;
            //int countryTo = model.CountryTo;

            UserContext context = new UserContext();

            var res = _repository.QuickSignUp(countryFrom, countryTo, email, password, phone, ipAddress2, ref context);

            var vgn = new LogOnModel { Email = email, Password = password };

            if (res == "1")
            {

                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                {
                    var usercontext = Authenticate(vgn.Email, vgn.Password);
                    WelcomeRazaMailSend(usercontext);
                    if (usercontext == null)
                    {
                        vgn.ErrorMsg = "Invalid Email and Password combination.";
                        return View("LogOn", vgn);
                    }
                    Session["Isnewuser"] = true;
                }
            }
            else if (res == "email address already exists")
            {
                string rsp = "Please try again.";
                string rst = "Email Address already exist.";
                vgn.ErrorMsg = "Please try again.Email Address already exist.";
                return View("LogOn", vgn);
            }

            else if (res == "invalid email or password")
            {
                vgn.ErrorMsg = "Invalid email or password, Please try again.";
                return View("LogOn", vgn);
            }
            else
            {
                if (res == "phone number exist")
                {
                    vgn.ErrorMsg = "Phone number already exist, Please try again.";
                }


                return View("LogOn", vgn);
            }

            return RedirectToAction(
                           "SearchRate", "Rate", new { countryfrom = countryFrom, countryto = countryTo });
        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult ChechoutLogon(BillingInfoModel model)
        {
            string ipAddress2 = Request.UserHostAddress;
            int CountryFrom = 0;
            string Email = model.Email;
            string password = model.NewPwd;
            string phone = model.PhoneNumber;
            int CountryTo = 0;
            if (UserContext != null)
            {
                var billInf = new BillingInfo();
                var billInfMod = model;
                billInf.Address = billInfMod.Address;
                billInf.City = billInfMod.City;
                billInf.Country = billInfMod.Country;
                billInf.FirstName = billInfMod.FirstName;
                billInf.LastName = billInfMod.LastName;
                billInf.State = string.IsNullOrEmpty(billInfMod.State) ? string.Empty : billInfMod.State;
                billInf.ZipCode = billInfMod.ZipCode;
                billInf.Email = billInfMod.Email;
                billInf.PhoneNumber = billInfMod.PhoneNumber;
                billInf.MemberId = UserContext.MemberId;
                billInf.RefererEmail = string.IsNullOrEmpty(billInfMod.RefererEmail)
                    ? string.Empty
                    : billInfMod.RefererEmail;
                billInf.UserType = UserContext.UserType;






                var res = _repository.UpdateBillingInfo(billInf);
                if (res.Status)
                {
                    var bi = _repository.GetBillingInfo(UserContext.MemberId);

                    UserContext.ProfileInfo = bi;

                    Raza.Model.UserContext usercont = new UserContext();

                    usercont.ProfileInfo = bi;
                    usercont.MemberId = bi.MemberId;
                    usercont.IsEmailSubscribed = UserContext.IsEmailSubscribed;
                    usercont.Email = bi.Email;
                    usercont.UserType = UserContext.UserType;

                    base.UpdateUserContext(UserContext.Email, usercont);


                    return
                        Json(
                            new
                            {
                                status = true,
                                message = string.Empty
                            });

                }
                else
                {
                    return
              Json(
                  new
                  {
                      status = false,
                      message = res.Errormsg
                  });
                }

            }
            else
            {
                UserContext context = new UserContext();
                //var res = _repository.QuickSignUp(CountryFrom, CountryTo, Email, password, phone, ipAddress2, ref context);
                // var res = "1";
                CustomerRegistration regmodel = new CustomerRegistration()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.NewPwd,
                    ZipCode = model.ZipCode,
                    City = model.City,
                    State = string.IsNullOrEmpty(model.State) ? string.Empty : model.State,
                    Address = model.Address,
                    AboutUs = string.Empty,
                    AlternatePhone = string.Empty,
                    PrimaryPhone = model.PhoneNumber,
                    Country = model.Country,
                    Title = string.Empty,
                    RefererEmail = string.IsNullOrEmpty(model.RefererEmail) ? string.Empty : model.RefererEmail,
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"]

                };

                var res = _repository.Customer_SignUp(regmodel, ref context);
                var vgn = new LogOnModel { Email = Email, Password = password };
                if (res == "1")
                {

                    if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(password))
                    {
                        var usercontext = Authenticate(vgn.Email, vgn.Password);
                        if (usercontext != null)
                        {
                            WelcomeRazaMailSend(usercontext);
                            //   var bi = _repository.GetBillingInfo(usercontext.MemberId);

                            //    usercontext.ProfileInfo = bi;
                            //UserContext.FullName = string.Format("{0} {1}", bi.FirstName, bi.LastName);
                            //   Session["usercontext"] = usercontext;

                            return
                                Json(
                                    new
                                    {
                                        status = true,
                                        message = string.Empty
                                    });




                        }

                        return
                            Json(
                                new
                                {
                                    status = true,
                                    message = "Signup Succesfull, Please Login Again."
                                });

                    }

                }
                else
                {
                    return
                          Json(
                              new
                              {
                                  status = false,
                                  message = res
                              });
                }


            }
            return
                          Json(
                              new
                              {
                                  status = false,
                                  message = "Logon failed!. Please Try again."
                              });
        }



        [HttpGet]
        [UnRequiresSSL]
        public JsonResult GetLowestRate(string CallingFrom, string CallingTo)
        {
            if (string.IsNullOrWhiteSpace(CallingFrom) || string.IsNullOrWhiteSpace(CallingTo) || CallingFrom == "undefined" || CallingTo == "undefined")
            {
                return null;
            }
            var rates = new LowestRates();
            int countryfrom = SafeConvert.ToInt32(CallingFrom);
            int countryto = SafeConvert.ToInt32(CallingTo);
            if (countryto != 0)
            {


                var data = CacheManager.Instance.GetLowestRatesCache(countryfrom, countryto);



                var firstOrDefault = data.Rate.FirstOrDefault(a => a.RateFor == "LandLine");

                if (firstOrDefault != null)
                    rates.LandLineRate = SafeConvert.ToString(firstOrDefault.LowestRate);
                var getRateList = data.Rate.FirstOrDefault(a => a.RateFor == "Mobile");
                string sign;
                if (getRateList != null)
                {
                    rates.MobileRate = SafeConvert.ToString(getRateList.LowestRate);
                }
                else
                {
                    rates.MobileRate = SafeConvert.ToString(0);
                }

                sign = " ¢/min";
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


        [CompressFilter]
        [HttpGet]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult CallForwarding(string order_id)
        {
            var model = new CallForwarding();
            ViewBag.order_id = order_id;
            return View(model);

        }

        [HttpGet]
        [UnRequiresSSL]
        public JsonResult GetCallForward(string order_id)
        {
            var model = new CallForwarding();
            model.ToCountryList = CacheManager.Instance.GetCountryListTo();
            var res = _repository.GetNumber800();
            model.ListNumber_800 = res.GetNumberList;
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            foreach (var autoReFill in result.OrderInfos.Where(a => a.OrderId == order_id))
            {
                model.Pin = autoReFill.AccountNumber;
            }
            string Pin = model.Pin;
            var data = _repository.GetCallForwarding(Pin);
            model.ForwardNumberList = data.GetNumberList;

            return Json(model, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [UnRequiresSSL]
        public JsonResult AddCallForward(CallForwarding model)
        {
            string Number_800 = model.Number_800;
            string CountryCode = model.CountryCode;
            string Destination_Number = model.Destination_Number;
            string Activation_Date = model.Activation_Date;
            string Expiry_Date = model.Expiry_Date;
            string Forwarding_Name = model.Forwarding_Name;
            string Added_By = "";
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            foreach (var autoReFill in result.OrderInfos.Where(a => a.OrderId == model.order_id))
            {
                model.Pin = autoReFill.AccountNumber;
            }
            string Pin = model.Pin;
            var res = _repository.CallForwarding(Pin, Number_800, CountryCode, Destination_Number, Activation_Date, Expiry_Date, Forwarding_Name, Added_By);
            if (res.Status)
            {
                return Json(new { status = "Your Call Forwarding setup is added!" });

            }
            else
            {
                return Json(new {status = res.Errormsg});
            }


        }

        [HttpPost]
        [UnRequiresSSL]
        public JsonResult DeleteCallForward(CallForwarding model)
        {
            string memberId = UserContext.MemberId;
            int sNo = model.SNumber;
            var result = _repository.GetCustomerPlanList(UserContext.MemberId);

            foreach (var autoReFill in result.OrderInfos.Where(a => a.OrderId == model.order_id))
            {
                model.Pin = autoReFill.AccountNumber;
            }
            string pin = model.Pin;
            var data = _repository.DeleteForwardNumber(sNo, pin, memberId);
            if (data)
            {
                return Json(new { Message = "Your call forwarding number deleted successfully!" });

            }
            else
            {
                return Json(new { Message = "operation failed!" });
            }

        }

        [UnRequiresSSL]
        public JsonResult NewCustomerLowestRate(string CallingFrom, string CallingTo)
        {
            if (string.IsNullOrWhiteSpace(CallingFrom) || string.IsNullOrWhiteSpace(CallingTo) || CallingFrom == "undefined" || CallingTo == "undefined")
            {
                return null;
            }

            int countryfrom = SafeConvert.ToInt32(CallingFrom);
            int countryto = SafeConvert.ToInt32(CallingTo);

            /*var data = CacheManager.Instance.GetLowestRatesCache(countryfrom, countryto);*/
            var data = _repository.NewCustomerLowRate(countryfrom, countryto);

            var rates = new LowestRates();

            var firstOrDefault = data.Rate.FirstOrDefault(a => a.RateFor == "LandLine");

            if (firstOrDefault != null)
                rates.LandLineRate = SafeConvert.ToString(firstOrDefault.LowestRate);
            var getRateList = data.Rate.FirstOrDefault(a => a.RateFor == "Mobile");
            if (getRateList != null)
                rates.MobileRate = SafeConvert.ToString(getRateList.LowestRate);

            return Json(rates, JsonRequestBehavior.AllowGet);


        }

        [UnRequiresSSL]
        public ActionResult ReferFriend()
        {
            var model = new ReferFriend();
            var billinginfo = _repository.GetBillingInfo(UserContext.MemberId);
            model.Country = billinginfo.Country;
            var result = _repository.GetReferFriendPt(UserContext.MemberId);
            model.ReferPoint = result.Count == 0 ? "0" : result.Sum(a => a.Points).ToString();

            model.ReferFriends = result;

            string memberid = UserContext.MemberId;

            var data = _repository.RedeemPt(memberid);

            model.RedeemPt = data.Count == 0 ? "0" : data.Sum(a => a.RedeemPoints).ToString();
            // model.ProfileInformation = UserContext.ProfileInfo;
            model.Point = _repository.GetRazaReward(UserContext.MemberId);
            model.PointToshow = SafeConvert.ToInt32(model.Point);
            var rewardpoints = _repository.GetRazaRewardPt(UserContext.MemberId);
            model.RewardPoints = rewardpoints;

            return View(model);

        }

        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult RedeemPoint()
        {
            var model = new ReferFriend();
            var billinginfo = _repository.GetBillingInfo(UserContext.MemberId);
            model.Country = billinginfo.Country;
            var result = _repository.RedeemPt(UserContext.MemberId);
            model.RedeemPt = result.Count == 0 ? "0" : result.Sum(a => a.RedeemPoints).ToString();
            model.RedeemPoints = result;

            var data = _repository.GetReferFriendPt(UserContext.MemberId);

            model.ReferPoint = result.Count == 0 ? "0" : data.Sum(a => a.Points).ToString();

            //      model.ProfileInformation = Session["ProfileInfo"] as BillingInfoModel;

            model.Point = _repository.GetRazaReward(UserContext.MemberId);
            model.PointToshow = SafeConvert.ToInt32(model.Point);
            var rewardpoints = _repository.GetRazaRewardPt(UserContext.MemberId);
            model.RewardPoints = rewardpoints;

            return View(model);
        }

        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult RazaReward()
        {
            var model = new ReferFriend();
            var billinginfo = _repository.GetBillingInfo(UserContext.MemberId);
            model.Country = billinginfo.Country;

            var res = _repository.GetRazaReward(UserContext.MemberId);
            model.Point = res;
            model.PointToshow = SafeConvert.ToInt32(model.Point);
            var rewardpoints = _repository.GetRazaRewardPt(UserContext.MemberId);
            model.RewardPoints = rewardpoints;

            var data = _repository.RedeemPt(UserContext.MemberId);
            model.RedeemPt = data.Count == 0 ? "0" : data.Sum(a => a.RedeemPoints).ToString();
            model.RedeemPoints = data;

            var result = _repository.GetReferFriendPt(UserContext.MemberId);
            model.ReferPoint = result.Count == 0 ? "0" : result.Sum(a => a.Points).ToString();
            model.ReferFriends = result;




            //model.ProfileInformation = Session["ProfileInfo"] as BillingInfoModel;

            return View(model);
        }

        private List<EmailModel> GetEmails(string memberId)
        {
            return _repository.GetEmails(memberId);
        }

        [CompressFilter]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult DeleteMail(int? EmailId)
        {
            _repository.DeleteMail(UserContext.MemberId, EmailId.Value);
            var model = this.GetMails(EmailId);
            return this.View("RazaMailer", model);
        }

        [CompressFilter]
        [Authorize]
        [UnRequiresSSL]
        public ActionResult RazaMailer(int? EmailId)
        {
            var model = this.GetMails(EmailId);

            return View(model);
        }

        [CompressFilter]
        public ActionResult UnsubscribeEmail(string EmailId)
        {
            var model = new UnsubscribeEmailViewModel
                {
                    EmailId = EmailId,
                    Status = false
                };

            // check if you have valid memberID from db
            return View(model);
        }

        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult UnsubscribeMemberFromEmails(string EmailId)
        {
            var model = new UnsubscribeEmailViewModel
            {
                EmailId = EmailId,
                Status = this._repository.DeleteEmailSubscription(EmailId)
            };

            return this.View("UnsubscribeEmail", model);
        }

        private EmailDetailViewModel GetMails(int? EmailId)
        {
            if (EmailId == null)
            {
                EmailId = 0;
            }

            var result = this.GetEmails(UserContext.MemberId);

            var model = new EmailDetailViewModel
                {
                    Emails = new List<EmailModel>(),
                    FullName = (this.Session["ProfileInfo"] as BillingInfoModel).UserName
                };

            foreach (var emailModel in result)
            {

                model.Emails.Add((emailModel));

                if (EmailId == emailModel.EmailId)
                {
                    model.SelectedEmail = emailModel;
                }
            }

            if (EmailId == 0)
            {
                model.SelectedEmail = model.Emails.FirstOrDefault();
            }
            return model;
        }

        [CompressFilter]
        public ActionResult Map()
        {
            int countryfromId = 1;

            var mapmodel = new MapModel();
            if (Session["CountrybyIp"] != null)
            {
                countryfromId = SafeConvert.ToInt32(Session["CountrybyIp"]);
                mapmodel.MapCountry = countryfromId;
            }
            var countryto = 0;


            var list = new List<LowestRates>();

            string landline = string.Empty;
            string mobile = string.Empty;

            var data = _repository.GetLowestRates(countryfromId, SafeConvert.ToInt32(countryto));

            foreach (var country in data.Rate)
            {
                var landlinerate = data.Rate.FirstOrDefault(a => a.RateFor == "LandLine" && a.SubCountryId == country.SubCountryId);

                var mobilerate = data.Rate.FirstOrDefault(a => a.RateFor == "Mobile" && a.SubCountryId == country.SubCountryId);

                if (mobilerate != null)
                {
                    mobile = SafeConvert.ToString(mobilerate.LowestRate);
                }

                if (landlinerate != null) landline = SafeConvert.ToString(landlinerate.LowestRate);
                list.Add(new LowestRates()
                {
                    CountrytoId = SafeConvert.ToInt32(country.LowestCallId),
                    LandLineRate = landline + "¢",
                    MobileRate = mobile + "¢"
                });

            }

            mapmodel.MapRateList = list;


            return View(mapmodel);
        }

        public void CountryFlagDict()
        {
            IDictionary<int, string> flagdict = new Dictionary<int, string>();
            flagdict.Add(1, "~/images/us.png");
            flagdict.Add(1, "~/images/uk.png");
            flagdict.Add(1, "~/images/canada.png");
        }

        protected void GetClientIp()
        {


        }


        public class LocationInfo
        {
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public string CountryName { get; set; }
            public string CountryCode { get; set; }
            public string Name { get; set; }
        }




        private Dictionary<string, LocationInfo> cachedIps = new Dictionary<string, LocationInfo>();

        public LocationInfo GetLocationInfo(string ipParam)
        {
            LocationInfo result = null;
            IPAddress i = IPAddress.Parse(ipParam);
            string ip = i.ToString();
            if (!cachedIps.ContainsKey(ip))
            {
                string r;
                using (var w = new WebClient())
                {
                    r = w.DownloadString(String.Format("http://api.hostip.info/?ip={0}&position=true", ip));
                }

                /*
             string r =
                @"<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>
<HostipLookupResultSet version=""1.0.0"" xmlns=""http://www.hostip.info/api"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.hostip.info/api/hostip-1.0.0.xsd"">
 <gml:description>This is the Hostip Lookup Service</gml:description>
 <gml:name>hostip</gml:name>
 <gml:boundedBy>
    <gml:Null>inapplicable</gml:Null>
 </gml:boundedBy>
 <gml:featureMember>
    <Hostip>
     <gml:name>Sugar Grove, IL</gml:name>
     <countryName>UNITED STATES</countryName>
     <countryAbbrev>US</countryAbbrev>
     <!-- Co-ordinates are available as lng,lat -->
     <ipLocation>
        <gml:PointProperty>
         <gml:Point srsName=""http://www.opengis.net/gml/srs/epsg.xml#4326"">
            <gml:coordinates>-88.4588,41.7696</gml:coordinates>
         </gml:Point>
        </gml:PointProperty>
     </ipLocation>
    </Hostip>
 </gml:featureMember>
</HostipLookupResultSet>";
             */

                var xmlResponse = XDocument.Parse(r);
                var gml = (XNamespace)"http://www.opengis.net/gml";
                var ns = (XNamespace)"http://www.hostip.info/api";

                try
                {
                    result = (from x in xmlResponse.Descendants(ns + "Hostip")
                              select new LocationInfo
                              {
                                  CountryCode = x.Element(ns + "countryAbbrev").Value,
                                  CountryName = x.Element(ns + "countryName").Value,
                                  Latitude = float.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[0]),
                                  Longitude = float.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[1]),
                                  Name = x.Element(gml + "name").Value
                              }).SingleOrDefault();
                }
                catch (NullReferenceException)
                {
                    //Looks like we didn't get what we expected.
                }
                if (result != null)
                {
                    cachedIps.Add(ip, result);
                }
            }
            else
            {
                result = cachedIps[ip];
            }
            return result;
        }

        [UnRequiresSSL]
        public ActionResult CustomerPage()
        {
            return View(new GenericModel());
        }

        [UnRequiresSSL]
        public ActionResult NewCustomer()
        {
            return View(new GenericModel());

        }

        [UnRequiresSSL]
        public JsonResult GetNewCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/NewCustomerPlan.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.TotalMinute =
                    SafeConvert.ToString(SafeConvert.ToInt32(item.RegularMin) + SafeConvert.ToInt32(item.ExtraMin));
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
                item.RateperSign = sign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }

        [UnRequiresSSL]
        public JsonResult GetExistCustomerPromotionRate(int countryfrom, int countryto, string planname)
        {
            string filepath = Server.MapPath(@"/Content/NewCustomerPlan.xml");
            var data = SerializationUtility<NewCustomerPromotionPlan>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var promotionrates =
                data.NewCustomerPlans.Where(
                    a => a.CountryFrom == countryfrom && a.CountryTo == countryto && a.PlanName == planname);

            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var item in promotionrates)
            {
                item.CurrencyCode = currencycode;
                item.CurrencySign = currencysign;
            }


            return Json(promotionrates, JsonRequestBehavior.AllowGet);

        }

        [CompressFilter]
        [Authorize]
        public ActionResult ExistCustomer()
        {
            var res = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (res.OrderInfos != null)
            {
                var premiumplan =
                    res.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162" || a.PlanId == "102");

                if (premiumplan != null)
                {
                    ViewBag.OrderId = premiumplan.OrderId;
                    ViewBag.BalncAmount = _repository.GetPinBalance(premiumplan.AccountNumber);
                    ViewBag.CurrencyCode = premiumplan.CurrencyCode;
                }
                else
                {
                    return RedirectToAction("NewCustomer", "Account");
                }
            }
            else
            {
                return RedirectToAction("NewCustomer", "Account");
            }

            return View(new GenericModel());
        }

        public ActionResult RetailAgent()
        {
            var retail = new Retail();
            retail.CountryFrom = CacheManager.Instance.GetFromCountries();
            return View(retail);
        }


        public ActionResult RetailAgent_Current_Working_Page()
        {
            var retail = new Retail();
            retail.CountryFrom = CacheManager.Instance.GetFromCountries();
            return View(retail);
        }


        [HttpPost]
        [UnRequiresSSL]
        public JsonResult Agent_SignUp(AgentSignUp model)
        {
            string FirstName = model.FirstName;
            string LastName = model.Lastname;
            string Email = model.Email;
            string PhoneNumber = model.Phonenumber;
            string Address = string.IsNullOrEmpty(model.Address) ? string.Empty : model.Address;
            string City = string.IsNullOrEmpty(model.City) ? string.Empty : model.City;
            string ZipCode = string.IsNullOrEmpty(model.Zipcode) ? string.Empty : model.Zipcode;
            string Message = string.IsNullOrEmpty(model.Message) ? string.Empty : model.Message;
            string State = string.IsNullOrEmpty(model.State) ? string.Empty : model.State;

            string Country = string.Empty;
            if (!string.IsNullOrEmpty(model.Country))
            {
                var fromcountrylist = CacheManager.Instance.GetFromCountries();
                var cdata = fromcountrylist.FirstOrDefault(a => a.Name == model.Country);
                if (cdata != null) Country = cdata.Id;
            }


            var response = _repository.AgentSignUp(FirstName, LastName, Email, PhoneNumber, Address, City, ZipCode, Message, State, Country);
            if (response.Status)
            {
                SendAgentSignuptoRaza(model);
                string ResponseMsg = string.Empty;
                if (model.CallApiFor == "A")
                {
                    ResponseMsg = "Thank you for your interest to join our Raza Agent Program. " + Environment.NewLine +
                                  "One of our agent specialists will contact you through phone or e-mail within 24-48 business hours.";
                }
                else
                {
                    ResponseMsg = "Thank you for your interest to join our Raza Web Affiliate Program." +
                                  Environment.NewLine +
                                  " One of our agent specialists will contact you through phone or e-mail within 24-48 business hours.";
                }
                return Json(new { message = ResponseMsg });
            }
            else
            {
                return Json(new { message = response.Errormsg });
            }

        }

        private void SendAgentSignuptoRaza(AgentSignUp model)
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
            string subject = "Sub: Raza Agent program signup Information.";
            if (model.CallApiFor.ToLower() == "w")
            {
                subject = "Sub: Raza Web Affiliate program signup Information.";
            }

            string userfeedback = "<strong>Dear Raza, <strong><br/> <br/> " + subject + "<br/><br/><br/> Name :" + model.FirstName +
                                  " " +
                                  model.Lastname + "<br/><br/>"
                                  + "Email: " + model.Email +
                                  "<br/><br/>Phone Number: " + model.Phonenumber;

            if (!string.IsNullOrEmpty(model.Address))
            {
                userfeedback += "<br/><br/> Address: " + model.Address;
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                userfeedback += "<br/><br/>" + "         " + model.City;
            }
            if (!string.IsNullOrEmpty(model.State))
            {
                userfeedback += ", " + model.State;
            }
            if (!string.IsNullOrEmpty(model.Zipcode))
            {
                userfeedback += ", " + model.Zipcode;
            }
            if (!string.IsNullOrEmpty(model.Country))
            {
                userfeedback += "         " + model.Country;
            }
            if (!string.IsNullOrEmpty(model.Message))
            {
                userfeedback += "<br/><br/> Message: " + model.Message;
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

        [CompressFilter]
        public ActionResult Content()
        {
            var res = new GenericModel();
            return View(res);
        }

        [CompressFilter]
        [UnRequiresSSL]
        public ActionResult MobileApp()
        {
            var res = new GenericModel();
            return View(res);
        }

        [UnRequiresSSL]
        public JsonResult SubscribeMail(string Email)
        {

            var mode = "1";
            var res = _repository.SubsEmail(Email, mode);
            if (res)
            {
                SendSubcriptionMail(Email);
                var data = "Your email address is subscribed!";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = "Email address is not valid!";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

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

        private void SendSubcriptionMail(string mailid)
        {
            string email = mailid;
            try
            {


                string redirectlink = string.Empty;
                if (UserContext != null && UserContext.UserType.ToLower() == "old")
                {
                    redirectlink = "Account/MyAccount";
                }
                string helplinenumber = string.Empty;
                var userCountryId = GetUserCountryId();
                if (UserContext != null && UserContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(userCountryId);
                }
                else
                {
                    helplinenumber = (string)Session["HelpNumber"];
                }
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/email_subscription.html"));

                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Subscribemail"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {

            }
        }



        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }


        public ActionResult GlobalCallSetup(int CallingFrom, int CallingTo)
        {
            return RedirectToAction("SearchRate", "Rate", new { countryfrom = CallingFrom, countryto = CallingTo });
        }

        public ActionResult SwitchView()
        {

            Session["SiteView"] = "Y";
            return RedirectToAction("Index", "Account", new { area = "Mobile" });
        }
    }






}