using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    public class RechargeController : BaseController
    {
        private DataRepository _repository = new DataRepository();
        //

        [CompressFilter]
        [Authorize]
        [RequiresSSL]
        public ActionResult Index(string orderid, string RechAmount = "", string CouponCode = "",
            bool IsAutoRefill = false)
        {

            if (Session["IsTransactionSuccess"] != null && (bool)Session["IsTransactionSuccess"])
            {
                Session["IsTransactionSuccess"] = false;
                return RedirectToAction("MyAccount", "Account", new { rf = "ar" });
            }
            var plans = _repository.GetCustomerPlanList(UserContext.MemberId);
            var plandata = plans.OrderInfos.FirstOrDefault(a => a.OrderId == orderid);
            var dict = FlagDictonary.GetCurrencycode();
            var rechargeSetUpModel = new RechargeSetup
            {
                ServiceFee = Convert.ToDouble(plandata.ServiceFee),
                CurrencyCode = plandata.CurrencyCode
            };

            if (dict.ContainsKey(plandata.CurrencyCode))
            {
                rechargeSetUpModel.CurrencySign = dict[plandata.CurrencyCode];
            }

            var amount = _repository.RechargeAmountVal(SafeConvert.ToInt32(plandata.PlanId));

            rechargeSetUpModel.AmountRecharge = amount.GetAmountlist;


            //var data = _repository.GetPlanDetails(orderid, UserContext.MemberId);

            ViewBag.order_id = orderid;
            ViewBag.plan_id = plandata.PlanId;
            ViewBag.CouponCode = CouponCode;
            ViewBag.RechAmount = RechAmount;
            rechargeSetUpModel.CountryFrom = plandata.CountryFrom;
            rechargeSetUpModel.CountryTo = plandata.CountryTo;
            rechargeSetUpModel.PlanId = SafeConvert.ToInt32(plandata.PlanId);
            rechargeSetUpModel.RechargeBalance = string.IsNullOrEmpty(SafeConvert.ToString(plandata.AccountBalance))
                ? "0"
                : SafeConvert.ToString(plandata.AccountBalance);
            rechargeSetUpModel.PlanName = plandata.PlanName;

            var fs = new List<State>();
            var result = CacheManager.Instance.GetFromCountries();
            ViewBag.Cont =
                new SelectList(Helpers.ConvertCountryModelListToCountryList(_repository.GetCountryList("From")));

            foreach (var res in result)
            {
                foreach (var dat in _repository.GetStateList(Convert.ToInt16(res.Id)))
                {
                    fs.Add(dat);
                }
            }

            rechargeSetUpModel.StateList = fs;
            rechargeSetUpModel.Statelist = new SelectList(fs, "id", "name");
            ViewBag.state = rechargeSetUpModel.Statelist;

            var info = _repository.GetPassCode(plandata.AccountNumber);
            if (info)
            {
                rechargeSetUpModel.GetCodeString = "No thanks, I prefer dialing without passcode.";
                rechargeSetUpModel.PassCodeString = "Yes, I prefer to setup a passcode for security.";

            }
            else
            {
                rechargeSetUpModel.GetCodeString = "I prefer to use existing passcode.";
                rechargeSetUpModel.PassCodeString = "I prefer to setup a new passcode.";
            }

            rechargeSetUpModel.IsAutoRefill = IsAutoRefill;

            if (plandata.AccountNumber != null)
            {
                rechargeSetUpModel.Pin = plandata.AccountNumber.Remove(3, 5).Insert(3, "XXXXX");
                return View(rechargeSetUpModel);
            }
            else
            {
                return RedirectToAction("Myaccount", "Account");
            }

        }


        [CompressFilter]
        [HttpGet]
        [Authorize]
        [RequiresSSL]
        public ActionResult PurchasePlan()
        {
            if ((bool)Session["IsTransactionSuccess"])
            {
                return RedirectToAction("MyAccount", "Account", new { rf = "ai" });
            }
            var rechargeSetUpModel = new RechargeSetup();
            var shopmodel = new ShoppingCartModel();
            if (Session["cart"] != null)
            {
                shopmodel = Session["Cart"] as ShoppingCartModel;

                rechargeSetUpModel.PlanName = shopmodel.PlanName;
                rechargeSetUpModel.CurrencyCode = shopmodel.CurrencyCode;

                var fs = new List<State>();
                var result = CacheManager.Instance.GetFromCountries();
                ViewBag.Cont =
                    new SelectList(Helpers.ConvertCountryModelListToCountryList(CacheManager.Instance.GetFromCountries()));

                foreach (var res in result)
                {

                    foreach (var dat in _repository.GetStateList(Convert.ToInt16(res.Id)))
                    {

                        fs.Add(dat);

                    }
                }
                var dict = FlagDictonary.GetCurrencycode();


                if (dict.ContainsKey(shopmodel.CurrencyCode))
                {
                    rechargeSetUpModel.CurrencySign = dict[shopmodel.CurrencyCode];
                }
                var amount = _repository.RechargeAmountVal(SafeConvert.ToInt32(shopmodel.PlanId));

                rechargeSetUpModel.AmountRecharge = amount.GetAmountlist;

                rechargeSetUpModel.StateList = fs;
                rechargeSetUpModel.Statelist = new SelectList(fs, "id", "name");
                ViewBag.state = rechargeSetUpModel.Statelist;

                rechargeSetUpModel.GetCodeString = "No thanks, I prefer dialing without passcode.";
                rechargeSetUpModel.PassCodeString = "Yes, I prefer to setup a passcode for security.";

                rechargeSetUpModel.IsMandatoryAutorefill = Helpers.CheckMandatoryAutorefill(SafeConvert.ToInt32(shopmodel.PlanId));

                return View(rechargeSetUpModel);
            }
            return RedirectToAction("SearchRate", "Rate");

        }

        [RequiresSSL]
        public JsonResult GetCreditCard(string order_id)
        {
            var rechargeSetUpModel = new RechargeSetup();

            //string ns = string.Join(currency, rechargeSetUpModel.AmountRecharge.ToList());

            var CreditCard = new GetTopCreditCards();
            CreditCard = _repository.Get_top_CreditCard(UserContext.MemberId);

            var list = new List<GetCard>();
            foreach (var item in CreditCard.GetCardList)
            {
                if (Session["NewCardName"] != null && Session["NewCardNo"] != null &&
                    item.CreditCardNumber == (string)Session["NewCardNo"])
                {
                    list.Add(new GetCard()
                    {
                        CVV = item.CVV,
                        CardAddedDate = item.CardAddedDate,
                        CardStatus = item.CardStatus,
                        CardTypeLogo = item.CardTypeLogo,
                        CreditCardName = (string)Session["NewCardName"],
                        CreditCardNo = item.CreditCardNo,
                        CreditCardNumber = item.CreditCardNumber,
                        CreditCardType = item.CreditCardType,
                        CurrentDate = item.CurrentDate,
                        CurrentDateYear = item.CurrentDateYear,
                        ExpiryDate = item.ExpiryDate,
                        ExpiryMonth = item.ExpiryMonth,
                        ExpiryYear = item.ExpiryYear,
                        ViewExpDate = item.ViewExpDate,
                        MaskCardNumber = item.MaskCardNumber,
                    });
                    
                    Session["NewCardName"] = null;
                    Session["NewCardNo"] = null;
                }
                else
                {
                    list.Add(new GetCard()
                    {
                        CVV = item.CVV,
                        CardAddedDate = item.CardAddedDate,
                        CardStatus = item.CardStatus,
                        CardTypeLogo = item.CardTypeLogo,
                        CreditCardName = item.CreditCardName,
                        CreditCardNo = item.CreditCardNo,
                        CreditCardNumber = item.CreditCardNumber,
                        CreditCardType = item.CreditCardType,
                        CurrentDate = item.CurrentDate,
                        CurrentDateYear = item.CurrentDateYear,
                        ExpiryDate = item.ExpiryDate,
                        ExpiryMonth = item.ExpiryMonth,
                        ExpiryYear = item.ExpiryYear,
                        ViewExpDate = item.ViewExpDate,
                        MaskCardNumber = item.MaskCardNumber,
                    });
                }
            }
            var date = DateTime.Now;
            rechargeSetUpModel.Currentdate = date.ToString("yy/MM");
           // getCard.CurrentDateYear = date.ToString("yyyy/MM");
            rechargeSetUpModel.CardList = list;
            rechargeSetUpModel.Years = Enumerable.Range(DateTime.Now.Year, 11).ToList();

            return Json(rechargeSetUpModel, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SetPassCode(string order_id, string PassCode)
        {
            var data = _repository.GetPlanDetails(order_id, UserContext.MemberId);
            string Pin = data.Pin;
            var res = _repository.SetPassCode(Pin, PassCode);
            if (res)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }


        }


        [RequiresSSL]
        public JsonResult AddCreditCard(string creditCard, string exp_Month, string exp_Year, string cVV,
            string nameOnCard)
        {
            BillingInfo billingInfoModel = _repository.GetBillingInfo(UserContext.MemberId);
            string country = string.Empty;
            string billing_Address = billingInfoModel.Address;
            string city = billingInfoModel.City;
            string state = billingInfoModel.State;
            string zipCode = billingInfoModel.ZipCode;
            string Exp_Year = exp_Year;

            var fromcountrylist = CacheManager.Instance.GetFromCountries();
            if (fromcountrylist.Any())
            {
                var data = fromcountrylist.FirstOrDefault(a => a.Name == billingInfoModel.Country);
                if (data != null) country = data.Id;
            }
            var res = _repository.AddCreditCard(UserContext.MemberId, nameOnCard, creditCard, exp_Month, Exp_Year, cVV,
                country, billing_Address, city, state, zipCode);

            if (res.Status)
            {
                Session["NewCardName"] = nameOnCard;
                Session["NewCardNo"] = creditCard;
                return Json(new { status = true, message = "Credit card added successfully." });

            }
            else
            {
                return Json(new { status = false, message = res.Errormsg });
            }

        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult RechargePin(Recharge recharge)
        {
            //  recharge.ExpMonth = recharge.ExpDate.Split('/')[0];
            // recharge.ExpYear = recharge.ExpYear.Split('/')[1];
            if ((bool) Session["IsTransactionSuccess"])
            {
                return
                       Json(
                           new
                           {
                               status = true,
                               Orderid = string.Empty,
                               statuserror = "Transaction is arleady successfull, Please check your order history."
                           });
            }

            string prefix = "OR";
            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

            if (recharge.ExpDate != null && recharge.ExpDate.Contains("/"))
            {
                var exp = recharge.ExpDate.Replace("/", "");
                recharge.ExpDate = exp;
            }


            //recharge.FirstName = recharge.UserName.Split(' ')[0]; 
            //recharge.LastName = recharge.UserName.Split(' ')[1];
            var value = _repository.GetPlanDetails(recharge.order_id, UserContext.MemberId);
            recharge.Pin = value.Pin;
            double fee = Convert.ToDouble(recharge.ServiceFee);


            //*********************************** Added on 01/06/15 TO GET CORRECT IP ADDRESS OF CUSTOMER *****************************
            // Look for a proxy address first
            string Cust_IP_Address = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address
            if (Cust_IP_Address == null || Cust_IP_Address.ToLower() == "unknown")
                Cust_IP_Address = Request.ServerVariables["REMOTE_ADDR"];
            //**************************************************************************************************************************


            string country = string.Empty;
            var cFromList = CacheManager.Instance.GetFromCountries();
            if (cFromList != null)
            {
                var cdata = cFromList.FirstOrDefault(a => a.Name == UserContext.ProfileInfo.Country);
                if (cdata != null) country = cdata.Id;
            }

            try
            {
                var repository = new DataRepository();

                var data = _repository.GetPlanDetails(recharge.order_id, UserContext.MemberId);

                RechargeInfo rechargeInfo = new RechargeInfo
                {
                    OrderId = orderId,
                    MemberId = UserContext.MemberId,
                    //IpAddress = Request.ServerVariables["REMOTE_ADDR"],
                    IpAddress = Cust_IP_Address,
                    Pin = data.Pin,
                    Amount = recharge.Amount,
                    CardNumber = recharge.CardNumber,
                    City = recharge.City,
                    State = recharge.State,
                    CVV2 = recharge.cvv,
                    Address1 = recharge.Address,
                    Address2 = "Desktop Site Recharge",
                    ZipCode = recharge.ZipCode,
                    Country = recharge.Country,
                    ExpiryDate = recharge.ExpDate,
                    IsCcProcess = false,
                    CoupanCode = string.IsNullOrEmpty(recharge.CouponCode) ? string.Empty : recharge.CouponCode,
                    ServiceFee = recharge.ServiceFee,
                    AVSResponse = string.Empty,
                    CVV2Response = string.Empty,
                    Cavv = string.Empty,
                    EciFlag = string.Empty,
                    PaymentTransactionId = string.Empty,
                    Xid = string.Empty,
                    PaymentType = recharge.PaymentType,
                    UserType = "Old",
                    //Address2 = string.Empty,

                };

                if (rechargeInfo.PaymentType == "P")
                {
                    rechargeInfo.PaymentMethod = "PayPal";
                    rechargeInfo.CardNumber = string.Empty;
                    rechargeInfo.CVV2 = string.Empty;
                    rechargeInfo.ExpiryDate = string.Empty;
                }
                else if (rechargeInfo.PaymentType == "C")
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }

                if (recharge.autoRefill == "T")
                {
                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = Convert.ToDouble(recharge.autoRefillAmount);
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }


                var rechstatus = repository.Recharge_Pin(rechargeInfo);

                if (rechstatus.Rechargestatus == "1")
                {
                    if (recharge.IsPasscodeDial == "T" && recharge.PassCodePin != null)
                    {
                        var res = repository.SetPassCode(data.Pin, recharge.PassCodePin);
                    }
                    recharge.order_id = rechstatus.OrderId;
                    new CreditCartHandler().RechargeConfirmMail(recharge, UserContext.Email);

                    return
                        Json(
                            new
                            {
                                status = true,
                                Orderid = rechstatus.OrderId,
                                statuserror = string.Empty
                            });

                }
                else
                {
                    return
                        Json(
                            new
                            {
                                status = false,
                                Orderid = string.Empty,
                                statuserror = "Transaction is not successfull, Please Try again."
                            });
                }

            }

            catch (Exception ex)
            {
                return
                       Json(
                           new
                           {
                               status = false,
                               Orderid = string.Empty,
                               statuserror = "Transaction is not successfull, Please Try again."
                           });
            }

        }

        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult Regphone()
        {
            var regPhoneModel = new RegPhoneModel();

            if (Session["cart"] != null)
            {

                var shopmodel = new ShoppingCartModel();
                shopmodel = Session["Cart"] as ShoppingCartModel;

                regPhoneModel = new RegPhoneModel
                {
                    PlanName = shopmodel.PlanName,
                    PlanId = shopmodel.PlanId,
                    CurrencyCode = shopmodel.CurrencyCode,
                    Quantity = 1,
                    AutoRefill = shopmodel.AutoRefill,
                    FromToMapping = shopmodel.FromToMapping,
                    CallingFrom = shopmodel.CallingFrom,
                    CallingTo = shopmodel.CallingTo,
                    CountryFrom = shopmodel.CountryFrom,
                    CountryTo = shopmodel.CountryTo,
                    ServiceFee = shopmodel.ServiceFee,
                    Price = shopmodel.Price,
                    Userstatus = shopmodel.Userstatus,
                };
            }

            return View(regPhoneModel);
        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult Regphone(RegPhoneModel regphonemodel)
        {
            Session["RegPinlessNumber"] = null;
            var list = new List<PinLessSetupNumbers>();
            if (!string.IsNullOrEmpty(regphonemodel.PinlessNumberOne))
            {
                list.Add(new PinLessSetupNumbers
                {
                    CountryCode = regphonemodel.CountryCodeOne,
                    PinlessNumber = regphonemodel.PinlessNumberOne
                });
            }

            if (!string.IsNullOrEmpty(regphonemodel.PinlessNumberTwo))
            {
                list.Add(new PinLessSetupNumbers()
                {
                    CountryCode = regphonemodel.CountryCodeTwo,
                    PinlessNumber = regphonemodel.PinlessNumberTwo
                });
            }

            if (!string.IsNullOrEmpty(regphonemodel.PinlessNumberThree))
            {
                list.Add(new PinLessSetupNumbers()
                {
                    CountryCode = regphonemodel.CountryCodeThree,
                    PinlessNumber = regphonemodel.PinlessNumberThree
                });
            }

            if (!string.IsNullOrEmpty(regphonemodel.PinlessNumberFour))
            {
                list.Add(new PinLessSetupNumbers()
                {
                    CountryCode = regphonemodel.CountryCodeFour,
                    PinlessNumber = regphonemodel.PinlessNumberFour
                });
            }

            if (!string.IsNullOrEmpty(regphonemodel.PinlessNumberFive))
            {
                list.Add(new PinLessSetupNumbers()
                {
                    CountryCode = regphonemodel.CountryCodeFive,
                    PinlessNumber = regphonemodel.PinlessNumberFive
                });
            }
            //163 1 CENT PLAN
            //164 CANADA 1 CENT PLAN
            //125 CANADA TALK CITY
            //126 CANADA TALK MOBILE
            //120 TALK CITY
            //121 TALK MOBILE


            //India unlimited plan

            //176 CANADA MOBILE UNLIMITED $9.99
            //178 CANADA UNLIMITED $14.99
            //180 CANADA UNLIMITED $19.99
            //175 MOBILE UNLIMITED $9.99
            //177 UNLIMITED $14.99
            //179 UNLIMITED $19.99
            
            var shopmodel = Session["Cart"] as ShoppingCartModel;
            if (Helpers.CheckForPinlessNumber(SafeConvert.ToInt32(shopmodel.PlanId)) && list.Count()>1)
            {
                return
                    Json(
                        new
                        {
                            status = false,
                            statuserror = "Only one pinlessnumber can add with this plan."
                        });
            }
            
            var res = DoesPhoneNumberExists(list);
            if (res.Status)
            {
                RazaLogger.WriteInfo("RegPhone List count" + list.Count());
                Session["RegPinlessNumber"] = list;
                return
                    Json(
                        new
                        {
                            status = true,
                            statuserror = string.Empty
                        });
            }
            else
            {
                return
                    Json(
                        new
                        {
                            status = false,
                            statuserror = "Same phone number already exists in another account. " +
                                          "Please register a new number or recharge under your existing account."
                        });
            }

            //return RedirectToAction("PurchasePlan", "Recharge");
        }



        public JsonResult GetAllPinlessNumber()
        {
            var numberlist = new PinlessSetupEdit();
            var list = new List<PinLessSetupNumbers>();
            var allplans = _repository.GetCustomerPlanList(UserContext.MemberId);
            if (allplans.OrderInfos.Any())
            {
                foreach (var plan in allplans.OrderInfos)
                {
                    var pinlessnumbers = _repository.GetPinLessNumber(plan.AccountNumber);
                    if (pinlessnumbers.PinlessNumberList.Any())
                    {
                        foreach (var number in pinlessnumbers.PinlessNumberList)
                        {
                            list.Add(new PinLessSetupNumbers
                            {
                                CountryCode = number.CountryCode,
                                PinlessNumber = number.PinlessNumber,
                                UnmaskPinlessNumber = number.UnmaskPinlessNumber,
                                OrderId = plan.OrderId
                            });
                        }
                    }

                }
                numberlist.PinlessNumberList = list;
            }
            if (UserContext.ProfileInfo.PhoneNumber.Contains("-"))
            {
                numberlist.AniNumber = UserContext.ProfileInfo.PhoneNumber.Replace("-", "");
            }
            numberlist.AniNumber = UserContext.ProfileInfo.PhoneNumber;
            return Json(numberlist, JsonRequestBehavior.AllowGet);

        }

        [CompressFilter]
        [HttpGet]
        [RequiresSSL]
        public ActionResult RechargeConfirmation()
        {
            var model = Session["CartAuthenticateModel"] as CartAuthenticateModel;
            Session["CartAuthenticateModel"] = null;
            if (model != null)
            {
                model.RechargeValues.TotalAmount = model.RechargeValues.Amount + model.RechargeValues.ServiceFee;
                if (model.RechargeValues.PaymentType == "C")
                {
                    model.RechargeValues.PaymentMethod = model.RechargeValues.CardType + ", xxxx " +
                                                         model.RechargeValues.CardNumber.GetLast(4);
                    string pin = model.RechargeValues.Pin.Remove(3, 5).Insert(3, "XXXXX");
                    model.RechargeValues.Pin = pin;
                }
                else if (model.RechargeValues.PaymentType == "P")
                {
                    string pin = model.RechargeValues.Pin.Remove(3, 5).Insert(3, "XXXXX");
                    model.RechargeValues.Pin = pin;
                    model.RechargeValues.PaymentMethod = "PayPal";
                }

            }
            return View(model);
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
            model.FinalResponseMessage = "Transaction is not successful, Please try again!!";

            return View(model);
        }

        [HttpPost]
        [RequiresSSL]
        public JsonResult ReedeemPoints(Recharge recharge)
        {

            //  recharge.ExpMonth = recharge.ExpDate.Split('/')[0];
            // recharge.ExpYear = recharge.ExpYear.Split('/')[1];
            var points = _repository.GetRazaReward(UserContext.MemberId);
            if (SafeConvert.ToInt32(points) < 1000)
            {
                return
                     Json(
                         new
                         {
                             status = false,
                             OrderId = string.Empty,
                             statuserror = "You don't have enough points to reedem."
                         });
            }

            var cartauthmodel = new CartAuthenticateModel()
            {
                RechargeValues = recharge
            };



            //recharge.FirstName = recharge.UserName.Split(' ')[0]; 
            //recharge.LastName = recharge.UserName.Split(' ')[1];
            var value = _repository.GetPlanDetails(recharge.order_id, UserContext.MemberId);

            double fee = Convert.ToDouble(recharge.ServiceFee);

            string prefix = "OR";
            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

            try
            {

                var repository = new DataRepository();


                var data = _repository.GetPlanDetails(recharge.order_id, UserContext.MemberId);

                RechargeInfo rechargeInfo = new RechargeInfo
                {
                    OrderId = orderId,
                    MemberId = UserContext.MemberId,
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"],
                    CVV2Response = string.Empty,
                    Xid = string.Empty,
                    AuthTransactionId = string.Empty,
                    AVSResponse = string.Empty,
                    Amount = recharge.Amount,
                    CardNumber = string.Empty,
                    City = recharge.City,
                    State = recharge.State,
                    CVV2 = string.Empty,
                    IsCcProcess = false,
                    CoupanCode = string.Empty,
                    Cavv = string.Empty,
                    EciFlag = string.Empty,
                    PaymentTransactionId = string.Empty,
                    Address1 = recharge.Address,
                    ZipCode = recharge.ZipCode,
                    Country = recharge.Country,
                    Pin = value.Pin,
                    ExpiryDate = string.Empty,
                    Address2 = string.Empty,
                    ServiceFee = recharge.ServiceFee,
                    IsAutoRefill = string.Empty,
                    UserType = "Old"

                };

                rechargeInfo.PaymentMethod = "Redeem";


                var rechstatus = repository.Recharge_Pin(rechargeInfo);

                if (rechstatus.Rechargestatus == "1")
                {
                    recharge.order_id = rechstatus.OrderId;
                    new CreditCartHandler().RechargeConfirmMail(recharge, UserContext.Email);
                    return
                    Json(
                        new
                        {
                            status = true,
                            OrderId = rechstatus.OrderId,
                            datetime = DateTime.Now.ToString()
                        });

                }
                else
                {
                    return
                     Json(
                         new
                         {
                             status = false,
                             OrderId = string.Empty,
                             statuserror = "Transaction is not successfull, Please Try again."
                         });
                }

            }

            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { Message = ex.Message });
            }

        }


        public void RechargeConfirmMail(Recharge model, string email)
        {
            try
            {
                RazaLogger.WriteInfo("Sending Reedem Confirmation mail.");
                string paymentmethod = "Reedem";
                
                int acc = model.Pin.Length;
                string accountnumber = model.Pin.Substring(0, 4) + "XXXX" +
                                       model.Pin.Substring(acc - 3, 2);
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = 0;
                double totalcharge = model.Amount + servicecharge;
                string servername = ConfigurationManager.AppSettings["ServerName"];

                string helplinenumber = string.Empty;
                var usercountryid = new CreditCartHandler().GetUserCountryId(model.Country);
                helplinenumber = Helpers.GetHelpLineNumber(usercountryid);

                string redirectlink = "Account/MyAccount";


                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/email-recharge.html"));

                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--UserName-->", model.UserName);
                mailbody = mailbody.Replace(@"<!--PlanName-->", model.PlanName);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", model.order_id);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(model.Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--DateTime-->", datetime);
                mailbody = mailbody.Replace(@"<!--Currencycode-->", model.CurrencyCode);
                mailbody = mailbody.Replace(@"<!--OldOrderId-->", model.OldOrderId);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    "Points Redeemed.",
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in recharge confirmation mail: " + ex.Message);
            }

        }

        private CommonStatus DoesPhoneNumberExists(List<PinLessSetupNumbers> list)
        {
            foreach (var num in list)
            {
                var res = _repository.DoesPhoneNumberExist(UserContext.MemberId, num.PinlessNumber);
                if (!res.Status)
                {
                    return res;
                }
            }
            return new CommonStatus()
            {
                Status = true
            };

        }


    }
}
