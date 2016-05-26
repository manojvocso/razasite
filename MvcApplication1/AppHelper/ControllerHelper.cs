using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;
using AutoMapper;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Models;
using Raza.Model.PaymentProcessModel;

namespace MvcApplication1.AppHelper
{
    public static class ControllerHelper
    {
        private static readonly DataRepository _dataRepository;
        static ControllerHelper()
        {
            _dataRepository = new DataRepository();
        }

        public static string GetIpAddressofUser(HttpRequestBase request)
        {
            string Cust_IP_Address = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address
            if (Cust_IP_Address == null || Cust_IP_Address.ToLower() == "unknown")
                Cust_IP_Address = request.ServerVariables["REMOTE_ADDR"];

            if (Cust_IP_Address == "::1")
            {
                Cust_IP_Address = "127.0.0.1";
            }

            return Cust_IP_Address;

        }

        public static Country GetUserCountryId(string country)
        {

            var countries = CacheManager.Instance.GetTop3FromCountries();
            var usercountryinfo = countries.FirstOrDefault(a => a.Name == country);
            if (usercountryinfo == null)
            {
                usercountryinfo = countries.FirstOrDefault(a => a.Id == country);
                if (usercountryinfo == null)
                {
                    usercountryinfo = new Country()
                    {
                        Id = "1",
                        Name = "U.S.A",
                        CountCode = "1"
                    };
                }
            }
            return usercountryinfo;
        }

        public static List<Country> GetCountryToWithCountryCodeInId()
        {
            var toCountries = CacheManager.Instance.GetCountryListTo();
            return toCountries.Select(item => new Country()
            {
                Id = item.CountryCode,
                Name = item.Name
            }).ToList();
        }

        public static Country GetcountryInfoByCountryCode(string ccode)
        {
            var toCountries = CacheManager.Instance.GetCountryListTo();
            return toCountries.FirstOrDefault(a => a.CountCode == ccode) ?? new Country();

        }

        public static Country GetcountryInfoByCountryId(string cId)
        {
            var toCountries = CacheManager.Instance.GetAllCountryTo();
            return toCountries.FirstOrDefault(a => a.Id == cId) ?? new Country();

        }

        public static Country GetFromCountryInfoByCountryId(string cId)
        {
            var toCountries = CacheManager.Instance.GetFromCountries();
            return toCountries.FirstOrDefault(a => a.Id == cId) ?? new Country();
        }


        public static List<GetCard> GetUserExistingCreditCard(string memberid)
        {
            return Mapper.Map<List<GetCard>>(_dataRepository.Get_top_CreditCard(memberid).GetCardList);
        }

        public static GetCard GetCreditCardByCardId(string memberId, int cardId)
        {
            var creditCardsData = _dataRepository.Get_top_CreditCard(memberId);
            return creditCardsData.GetCardList.FirstOrDefault(a => a.CreditCardId == cardId);
        }

        #region Cryptography Methods

        private static readonly string key = "786534872543";//"jdsg432387#";

        public static string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    return String.Empty;
                }

                byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };
                var encryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(encryptKey, iv),
                    CryptoStreamMode.Write);
                cStream.Write(inputByte, 0, inputByte.Length);
                cStream.FlushFinalBlock();
                var encText = Convert.ToBase64String(mStream.ToArray());
                return encText.Replace('+', '-').Replace('/', '_').Replace('=', ',');
                //return HttpUtility.UrlEncode(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new ResourceNotFoundException();
            }
        }

        public static string Decrypt(string encryptedText)
        {
            //string key = "jdsg432387#";
            try
            {
                string encText = encryptedText.Replace('-', '+').Replace('_', '/').Replace(',', '=');

                byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };

                var decryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                var inputByte = Convert.FromBase64String(encText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(decryptKey, iv), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw new ResourceNotFoundException();
            }
        }

        #endregion

        public static string MaskedCreditCard(string ccNumber)
        {
            //return Regex.Replace(ccNumber.Substring(0, ccNumber.Length - 4), "\d", "*") &
            // ccNumber.Substring(ccNumber.Length - 4);
            if (string.IsNullOrEmpty(ccNumber))
            {
                return String.Empty;
            }
            return ccNumber.Remove(4, 8).Insert(4, "-XXXX-XXXX-");

        }

        public static List<double> GetAutoRefillOptions()
        {
            var options = new List<double>() { 10, 20, 50, 90 };
            return options;
        }

        public static string CreateOrderId(string transactionType)
        {
            string prefix = string.Empty;

            string transmode = string.Empty;


            if (transactionType == "PurchaseNewPlan") // existing customer new plan
            {
                prefix = "OS";
                transmode = "S";
            }
            else if (transactionType == "Recharge") //// existing customer existing plan
            {
                prefix = "OR";
                transmode = "R";
            }
            else if (transactionType == "CheckOut") //// new customer new plan
            {
                prefix = "OA";
                transmode = "S";
            }
            else if (transactionType == "TopUp") //// new customer new plan
            {
                prefix = "TP";
                transmode = "S";
            }

            string orderId = string.Format("{0}{1}", prefix,
                Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10));

            return orderId;

        }

        public static string GetTransactionMode(string transactionType)
        {
            string transmode = string.Empty;

            if (transactionType == TransactionType.PurchaseNewPlan.ToString()) // existing customer new plan
            {
                transmode = "S";
            }
            else if (transactionType == TransactionType.Recharge.ToString()) //// existing customer existing plan
            {
                transmode = "R";
            }
            else if (transactionType == TransactionType.CheckOut.ToString()) //// new customer new plan
            {
                transmode = "A";
            }
            else if (transactionType == TransactionType.TopUp.ToString()) //// new customer new plan
            {
                transmode = "T";
            }

            return transmode;

        }

        public static double CalculateServiceFee(double serviceFee, double amount)
        {
            return amount * serviceFee / 100;
        }

        public static bool ValidateCustomerPlanPin(string memberId, string pin)
        {
            return _dataRepository.ValidateCustomerPlan(memberId, pin);
        }

        public static OrderHistoricPlanInfoSnapshot GetUserPremiumPlan(string memberId)
        {
            var data = _dataRepository.GetCustomerPlanList(memberId);
            if (data == null)
                return null;

            var planInfo = data.OrderInfos.FirstOrDefault(a => a.PlanId == "161" || a.PlanId == "162" || a.PlanId == "102");
            return planInfo;
        }

        /// <summary>
        /// If user is purchasing a new plan and he already have Premium plan enforce the customer to recharge.
        /// </summary>
        /// <param name="paypalCheckOutModel"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static bool IsEnforceToRecharge(PayPalCheckoutModel paypalCheckOutModel, string memberId)
        {
            var premiumPlanIds = new List<int> { 161, 162, 102 };

            if ((paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.PurchaseNewPlan.ToString() || paypalCheckOutModel.ProcessPaymentInfo.TransactionType == TransactionType.CheckOut.ToString()) &&
    GetUserPremiumPlan(memberId) != null && premiumPlanIds.Exists(a => a == paypalCheckOutModel.ProcessPlanInfo.CardId))
            {
                return true;
            }
            return false;

        }

        public static bool IsValidForReedemPoints(RazaPrincipal usercontext)
        {
            var points = _dataRepository.GetRazaReward(usercontext.MemberId);
            int pointLimit = 1000;
            var userCountry = GetUserCountryId(usercontext.ProfileInfo.Country);
            if (userCountry.Id == "2")
            {
                pointLimit = 1100;
            }
            if (SafeConvert.ToInt32(points) >= pointLimit)
            {
                return true;
            }
            return false;
        }

        public static List<SelectListItem> GetReedemPointOptions(RazaPrincipal usercontext, decimal serviceFee)
        {
            var points = SafeConvert.ToInt32(_dataRepository.GetRazaReward(usercontext.MemberId));
            //var userCountry = GetUserCountryId(usercontext.ProfileInfo.Country);
            int pointLimit = 1000;
            pointLimit += SafeConvert.ToInt32(pointLimit * serviceFee / 100);

            if (points >= pointLimit)
            {
                var list = new List<SelectListItem>();

                var calcPoint = pointLimit * 1;
                list.Add(new SelectListItem()
                {
                    Value = "10",
                    Text = "$10 (" + calcPoint + " points)"
                });

                if (points >= pointLimit * 2)
                {
                    calcPoint = pointLimit * 2;
                    list.Add(new SelectListItem()
                    {
                        Value = "20",
                        Text = "$20 (" + calcPoint + " points)"
                    });
                }

                if (points >= pointLimit * 5)
                {
                    calcPoint = pointLimit * 5;
                    list.Add(new SelectListItem()
                    {
                        Value = "50",
                        Text = "$50 (" + calcPoint + " points)"
                    });
                }

                if (points >= pointLimit * 9)
                {
                    calcPoint = pointLimit * 9;
                    list.Add(new SelectListItem()
                    {
                        Value = "90",
                        Text = "$90 (" + calcPoint + " points)"
                    });
                }
                return list;
            }
            return null;
        }

        public static bool IsValidCouponCode(RazaPrincipal usercontext, PayPalCheckoutModel payPalCheckoutModel)
        {
            if (string.IsNullOrEmpty(payPalCheckoutModel.ProcessPaymentInfo.CouponCode))
                return true;

            string transMode = GetTransactionMode(payPalCheckoutModel.ProcessPaymentInfo.TransactionType);
            var isValid = _dataRepository.ValidateCouponCode(usercontext.MemberId,
                payPalCheckoutModel.ProcessPaymentInfo.CouponCode, payPalCheckoutModel.ProcessPlanInfo.CardId,
                payPalCheckoutModel.ProcessPlanInfo.CountryFrom, payPalCheckoutModel.ProcessPlanInfo.CountryTo,
                payPalCheckoutModel.ProcessPlanInfo.Amount, transMode);
            return isValid;
        }

        public static List<Country> GetLocalAccessFromCountryList()
        {
            var list = new List<Country>()
            {
                new Country("1", "USA", "", "", ""),
                new Country("2", "CANADA", "", "", ""),
                new Country("3", "UK", "", "", ""),
                new Country("9", "BELGIUM", "", "", ""),
                new Country("10", "BULGARIA", "", "", ""),
                new Country("27", "CRECH REPUBLIC", "", "", ""),
                new Country("11", "DENMARK", "", "", ""),
                new Country("12", "FRANCE", "", "", ""),
                new Country("13", "GERMANY", "", "", ""),
                new Country("44", "HUNGARY", "", "", ""),
                new Country("17", "ITALY", "", "", ""),
                new Country("50", "LITHUANIA", "", "", ""),
                new Country("20", "NEW ZEALAND", "", "", ""),
                new Country("21", "POLAND", "", "", ""),
                new Country("57", "FOR OTHER COUNTRIES", "", "", "")
            };
            return list;
        }

        public static List<LocalAccessNumber> GetLocalAccessNumbers(LocalAccessNumberViewModel model)
        {
            string searchType = "A";
            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                searchType = "S";
                model.PhoneNumber = String.Empty;
            }

            var data = _dataRepository.GetLocalAccessNumber(model.AccessState, model.AccessCountry,
                model.PhoneNumber, searchType);
            if (data.Count == 0)
            {
                #region Default Local AccessNumbers
                List<LocalAccessNumber> list = new List<LocalAccessNumber>
                    {
                        new LocalAccessNumber()
                        {
                            AccessNumber = "1-877-777-3971 (U.S.A.)",
                            City = "Any City",
                            SNo = "1. (U.S.A.)",
                            State = "Any State"
                        },
                        new LocalAccessNumber()
                        {
                            AccessNumber = "1-877-777-1674 (CANADA)",
                            City = "Any City",
                            SNo = "2. (CANADA)",
                            State = "Any State"
                        },
                        new LocalAccessNumber()
                        {
                            AccessNumber = "1-866-397-2880",
                            City = "Any City",
                            SNo = "3.",
                            State = "HAWAII"
                        },
                        new LocalAccessNumber()
                        {
                            AccessNumber = "1-866-397-2880",
                            City = "Any City",
                            SNo = "4.",
                            State = "PUERTO RICO"
                        },
                        new LocalAccessNumber()
                        {
                            AccessNumber = "1-866-397-2880",
                            City = "Any City",
                            SNo = "5.",
                            State = "US VIRGIN ISLANDS"
                        }
                    };

                data = list;

                #endregion

            }

            return data;
        }

        public static OrderHistoricPlanInfoSnapshot GetSinglePlanInfo(string pin, string memberId)
        {
            var data = _dataRepository.GetCustomerPlanList(memberId);
            if (data == null)
                return null;

            var planInfo = data.OrderInfos.FirstOrDefault(a => a.AccountNumber == pin);
            return planInfo;
        }

        public static string ToFourDigitYear(this string year)
        {
            var prefix = DateTime.Now.Year.ToString().Substring(0, 2);
            return prefix + year;
        }

        public static string ToTwoDigitYear(this string year)
        {
            return year.Remove(0, 2);
        }

        public static bool IsValidForRegisterPinlessNumbers(string transactionType, string planPin)
        {
            var notValidList = new List<int>() { 17, 177, 179, 176, 178, 180 };
            if (transactionType == TransactionType.CheckOut.ToString() ||
                transactionType == TransactionType.PurchaseNewPlan.ToString())
            {
                if (!notValidList.Exists(a => a == SafeConvert.ToInt32(planPin)))
                {
                    return true;
                }
            }

            return false;
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("id"))
            {
                var val = filterContext.ActionParameters["id"];
                filterContext.ActionParameters["id"] = ControllerHelper.Decrypt(val.ToString());
            }

            //if (HttpContext.Current.Request.QueryString.Get("plan") != null)
            //{
            //    string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("plan");
            //    string decrptedString = ControllerHelper.Decrypt(encryptedQueryString.ToString());
            //    string[] paramsArrs = decrptedString.Split('?');

            //    for (int i = 0; i < paramsArrs.Length; i++)
            //    {
            //        string[] paramArr = paramsArrs[i].Split('=');
            //        decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
            //    }
            //}
            //for (int i = 0; i < decryptedParameters.Count; i++)
            //{
            //    filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
            //}
            base.OnActionExecuting(filterContext);

        }


    }




}
