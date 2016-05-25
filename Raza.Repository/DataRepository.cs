using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
//using System.Web.Mvc;
using System.Web.Services.Description;
using Raza.Common;
using Raza.Model;
using Raza.Repository.RazaWSClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Raza.Model.Helpers;

namespace Raza.Repository
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;


    public class DataRepository
    {
        public void Signout()
        {
            //UtilCommon.MemberID = string.Empty;
            //UtilCommon.Email = string.Empty;
            //UtilCommon.UserType = string.Empty;
        }

        public UserContext Authenticate(string Email, string Password)
        {
            var response = WSClient.Login(Email, Password);

            var context = new UserContext();

            if (IsValidResponse(response))
            {
                context.MemberId = response.Split('|')[1].Split(',')[0].Split('=')[1];
                context.Email = response.Split('|')[1].Split(',')[1].Split('=')[1];
                context.UserType = response.Split('|')[1].Split(',')[2].Split('=')[1];

                context.IsEmailSubscribed = GetEmailSubscription(context.Email);

                var bi = BillingInfo.CreateProfileInfo(response);

                if (bi != null)
                {
                    context.ProfileInfo = bi;
                }

                return context;
            }
            else if (response.Split('|')[1].Split('=')[1] == "customer blocked")
            {
                context.ServiceResponse = response.Split('|')[1].Split('=')[1];
                return context;
            }
            else
            {
                return null;
            }
        }


        public BillingInfo GetBillingInfo(string CustID)
        {
            var response = WSClient.Get_Billing_Information(CustID);

            if (IsValidResponse(response))
            {
                return BillingInfo.Create(response);
            }
            else
            {
                return new BillingInfo();
            }
        }

        public string QuickSignUp(int countryFrom, int countryTo, string EmailAddress, string Password,
            string Phone_Number, string IP_Address, ref UserContext userContext)
        {
            var response = WSClient.Customer_Quick_SignUp(
                countryFrom, countryTo, EmailAddress, Password, Phone_Number, IP_Address);

            userContext = new UserContext();

            if (IsValidResponse(response))
            {
                userContext.MemberId = response.Split('|')[1].Split(',')[0].Split('=')[1];
                userContext.Email = response.Split('|')[1].Split(',')[1].Split('=')[1];
                userContext.UserType = response.Split('|')[1].Split(',')[2].Split('=')[1];


                return response.Split('|')[0].Split('=')[1];
            }
            else
            {
                if (response.Split('|')[1].Split('=')[1] == "phone number exist")
                {
                    return response.Split('|')[1].Split('=')[1];
                }
                else
                {
                    return response.Split('|')[1].Split('=')[1];

                }

            }

        }

        public string Customer_SignUp(CustomerRegistration reg, ref UserContext userContext)
        {
            var response = WSClient.Customer_Registration(reg.Email, reg.Password, reg.Title, reg.FirstName,
                reg.LastName, reg.PrimaryPhone, reg.AlternatePhone, reg.Address, reg.City, reg.State, reg.ZipCode,
                reg.Country, reg.AboutUs, reg.RefererEmail, reg.IpAddress);

            userContext = new UserContext();

            if (IsValidResponse(response))
            {
                userContext.MemberId = response.Split('|')[1].Split(',')[0].Split('=')[1];
                userContext.Email = response.Split('|')[1].Split(',')[1].Split('=')[1];
                userContext.UserType = response.Split('|')[1].Split(',')[2].Split('=')[1];


                return response.Split('|')[0].Split('=')[1];
            }
            else
            {
                if (response.Split('|')[1].Split('=')[1] == "phone number exist")
                {
                    return response.Split('|')[1].Split('=')[1];
                }
                else
                {
                    return response.Split('|')[1].Split('=')[1];

                }

            }

        }


        public string GetTopUp(int CountryID)
        {
            var response = WSClient.Get_MobileTopUp_Operators(CountryID);
            if (IsValidResponse(response))
            {
                return response;

            }
            else
            {
                return response;
            }

        }


        public GetLowestRate GetLowestRates(int CountryFrom, int CountryTo)
        {
            var response = WSClient.Get_Lowest_Rate(CountryFrom, CountryTo);

            if (IsValidResponse2(response))
            {
                return GetLowestRate.GetRate(response);
            }
            else
            {
                return new GetLowestRate();
            }

        }

        public GetLowestRate NewCustomerLowRate(int CountryFrom, int CountryTo)
        {
            var response = WSClient.Get_NewCustomer_Rate(CountryFrom, CountryTo);

            if (IsValidResponse2(response))
            {
                return GetLowestRate.GetNewCustomerRate(response);
            }
            else
            {
                return new GetLowestRate();
            }


        }
        public string GetRewardPoint(string MemberID)
        {
            var response = WSClient.Get_Reward_Points(MemberID);
            if (IsValidResponse(response))
            {
                return response;
            }
            else
            {
                return response;
            }


        }

        /// <summary>
        /// Add new credit card
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="Name_On_Card"></param>
        /// <param name="CreditCard"></param>
        /// <param name="Exp_Month"></param>
        /// <param name="Exp_Year"></param>
        /// <param name="CVV2"></param>
        /// <param name="Country"></param>
        /// <param name="Billing_Address"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="ZipCode"></param>
        /// <returns></returns>
        public CommonStatus AddCreditCard(string MemberID, string Name_On_Card, string CreditCard, string Exp_Month,
            string Exp_Year, string CVV2, string Country, string Billing_Address, string City, string State,
            string ZipCode)
        {

            RazaLogger.WriteInfo(string.Format("Calling Add newcreditcard : {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", MemberID, Name_On_Card, CreditCard, Exp_Month,
                Exp_Year, CVV2, Country, Billing_Address, City, State, ZipCode));

            var response = WSClient.Add_Credit_Card(
                MemberID,
                Name_On_Card,
                CreditCard,
                Exp_Month,
                Exp_Year,
                CVV2,
                Country,
                Billing_Address,
                City,
                State,
                ZipCode);
            RazaLogger.WriteInfo("response of AddcreditCard is: " + response);
            var statusMsg = new CommonStatus();
            if (IsValidResponse(response))
            {
                statusMsg.Status = true;

            }
            else
            {
                statusMsg.Status = false;
                statusMsg.Errormsg = response.Split('|')[1].Split('=')[1].ToDisplayMessage();
            }
            return statusMsg;
        }


        public List<Country> GetCountryList(string SearchType, string DestinationType = "0")
        {
            var response = WSClient.Get_Country_List(SearchType, DestinationType);
            List<Country> contries = new List<Country>();
            if (IsValidResponse(response))
            {
                string[] str = response.Split('|')[1].Split(',');

                for (int i = 0; i < str.Length; i++)
                {
                    string countrycode = str[i].Split('~')[2];
                    string id = str[i].Split('~')[0];
                    string name = str[i].Split('~')[1];
                    string countcode = countrycode;
                    int type = SearchType.ToLower() == "from" ? 0 : SafeConvert.ToInt32(str[i].Split('~')[3]);
                    string ratetype = "Country";
                    if (type == 1)
                    {
                        ratetype = "City";
                    }
                    else if (type == 2)
                    {
                        ratetype = "Mobile";
                    }

                    contries.Add(new Country(id, name, countrycode, countcode, ratetype));


                }

                return contries;
            }
            else
            {
                response = response.Split('|')[0].Split('=')[1];
                return contries;
            }
        }

        public List<State> GetStateList(int CountryID)
        {
            var response = WSClient.Get_State_List(CountryID);

            var states = new List<State>();

            if (IsValidResponse(response))
            {
                string[] str = response.Split('|')[1].Split(',');

                for (int i = 0; i < str.Length; i++)
                {
                    string id = str[i].Split('~')[1];
                    string name = str[i].Split('~')[0];

                    states.Add(new State { Id = id, Name = name });
                }

                return states;
            }
            else
            {
                response = response.Split('|')[0].Split('=')[1];
                return states;
            }
        }

        public List<Areacode> GetAreacode(string state)
        {
            var response = WSClient.Get_AreaCode_List(state);
            var areacode = new List<Areacode>();

            if (IsValidResponse(response))
            {
                string[] str = response.Split('|')[1].Split(',');

                areacode.AddRange(str.Select(t => t.Split('~')[0]).Select(code => new Areacode { Code = code }));

                return areacode;
            }
            else
            {
                response = response.Split('|')[0].Split('=')[1];
                return areacode;
            }
        }

        public List<Availnumber> GetAvailableOneTouchNumbers(string Pin, int countryId, string state, string areaCode)
        {
            var response = WSClient.Get_Available_OneTouch_Numbers(Pin, countryId, state, areaCode);

            if (IsValidResponse(response))
            {
                string[] str = response.Split('|')[1].Split(',');

                return str.Select(t => t.Split('~')[0]).Select(num => new Availnumber { Number = num }).ToList();
            }
            else
            {
                return new List<Availnumber>();
            }

        }


        public CommonStatus UpdateBillingInfo(BillingInfo billing)
        {
            var response = WSClient.Update_Billing_Information(billing.MemberId, billing.FirstName, billing.LastName, billing.PhoneNumber,
                billing.Address, billing.City, billing.State, billing.ZipCode, billing.Country, billing.UserType, billing.RefererEmail);

            var res = new CommonStatus();
            if (IsValidResponse(response))
            {
                res.Status = true;
            }
            else
            {
                res.Status = false;
                res.Errormsg = response.Split('|')[1].Split('=')[1];
            }
            return res;
        }

        public string GetPassword(string email)
        {
            var response = WSClient.Get_Password(email);

            if (IsValidResponse(response))
            {
                if (response.Split('|').Length > 1)
                {
                    return response.Split('|')[1].Split('=')[1];
                }
                return string.Empty;
                //return response;
            }
            else
            {
                return string.Empty;
            }
        }

        public RatePlans GetRates(Rates Rate)
        {
            var response = WSClient.Get_Rates(Rate.CountryFrom, Rate.CountryTo, Rate.CardName);

            if (IsValidResponse2(response))
            {
                return RatePlans.Create(response);
                //return response;
            }
            else
            {
                return new RatePlans(); // string.Empty;
            }
        }



        public OrderHistorySnapshot GetCustomerPlanList(string CustomerID)
        {
            var response = WSClient.Get_Customer_Plan_List(CustomerID);

            if (IsValidResponse(response))
            {
                return new OrderHistorySnapshot().CreateSnapshot(response);
            }
            else
            {
                return new OrderHistorySnapshot();
            }
        }

        public OrderHistory GetOrderHistory(string CustomerID)
        {
            var response = WSClient.Get_Order_History(CustomerID);

            if (IsValidResponse(response))
            {
                return OrderHistory.Create(response, GetCustomerPlanList(CustomerID));
            }

            return new OrderHistory();

        }

        public CallDetails GetCallHistory(string Pin, string startdate, string enddate)
        {
            var response = WSClient.Get_Call_History(Pin, startdate, enddate);

            if (IsValidResponse(response))
            {
                return CallDetails.Create(response);
            }
            else
            {
                return new CallDetails();
            }
        }

        public bool GetPassCode(string Pin)
        {
            var res = WSClient.Get_PassCode(Pin);
            if (res.Split('|')[1].Split('=')[1] == "")
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool SetPassCode(string Pin, string PassCode)
        {
            RazaLogger.WriteInfo("calling to setpasscode with params : " + Pin + "," + PassCode);
            var data = WSClient.Set_PassCode(Pin, PassCode);
            if (data.Split('=')[1] == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public CommonStatus PinLessSetUpEdit(string AnilName, string MemberID, string RequestedBy, string Pin, string AnilNumber,
            string CoutryCode)
        {
            var response = WSClient.PinLess_SetUp(AnilName, MemberID, RequestedBy, Pin, AnilNumber, CoutryCode);
            var status = new CommonStatus();
            if (IsValidResponse(response))
            {
                status.Status = true;
                status.Errormsg = string.Empty;
                //return response;
            }
            else
            {
                status.Status = false;
                status.Errormsg = response.Split('|')[1].Split('=')[1];
            }
            return status;
        }

        public string DeletePinLessSetUp(string Pin, string AnilNumber, string CoutryCode, string RequestedBy)
        {
            var response = WSClient.Delete_PinLess_SetUp(Pin, AnilNumber, CoutryCode, RequestedBy);

            if (IsValidResponse(response))
            {
                return response;
            }
            else
            {
                return string.Empty;
            }
        }

        public QuickeySetup GetQuickKeyNumbers(string Pin)
        {
            var response = WSClient.Get_Quickeys_Numbers(Pin);


            return Raza.Model.QuickeySetup.Create(response);

        }

        public CommonStatus QuickKeySetUp(string MemID, string Pin, string CoutryCode, string DestNum, string SpecDialNum,
            string NicName)
        {
            var status = new CommonStatus();
            var response = WSClient.Quickeys_SetUp(MemID, Pin, CoutryCode, DestNum, SpecDialNum, NicName);


            if (IsValidResponse(response))
            {
                status.Status = true;
            }
            else
            {
                var error = response.Split('|')[1].Split('=')[1];
                status.Status = false;
                status.Errormsg = error;
            }
            return status;
        }

        public CommonStatus OneTouchSetup(string Pin, string OneTouch_Number, string Destination, string OneTouch_Name,
            string Added_By)
        {
            var response = WSClient.OneTouch_SetUp(Pin, OneTouch_Number, Destination, OneTouch_Name, Added_By);
            var status = new CommonStatus();
            if (IsValidResponse(response))
            {
                status.Status = true;
            }
            else
            {
                var error = response.Split('|')[1].Split('=')[1];
                status.Status = false;
                status.Errormsg = error;
                //throw new ValidationException(error);
            }
            return status;
        }

        public CommonStatus CallForwarding(string Pin, string Number_800, string CountryCode, string Destination_Number, string Activation_Date, string Expiry_Date, string Forwarding_Name, string Added_By)
        {
            var response = WSClient.Call_Forwarding_SetUp(Pin, Number_800, CountryCode, Destination_Number,
                Activation_Date, Expiry_Date, Forwarding_Name, Added_By);
            var status = new CommonStatus();
            if (IsValidResponse(response))
            {
                status.Status = true;
            }
            else
            {
                var error = response.Split('|')[1].Split('=')[1];
                status.Status = false;
                status.Errormsg = error;
                //throw new ValidationException(error);
            }
            return status;


        }

        public GetForwardNumber GetCallForwarding(string Pin)
        {
            var res = WSClient.Get_Call_Forwarding_Numbers(Pin);
            if (IsValidResponse(res))
            {
                return Raza.Model.GetForwardNumber.Create(res);

            }
            else
            {
                var error = res.Split('|')[1].Split('=')[1];


            }
            return Raza.Model.GetForwardNumber.Create(res);

        }

        public bool DeleteForwardNumber(int sNo, string pin, string memberId)
        {
            var res = WSClient.Delete_Call_Forwarding_Numbers(sNo, pin, memberId);
            if (IsValidResponse(res))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public GetNumber800 GetNumber800()
        {
            var res = WSClient.Get_Available_800Numbers();


            return Raza.Model.GetNumber800.Create(res);

        }
        public OneTouchSet GetOneTouchSetup(string Pin)
        {
            var response = WSClient.Get_OneTouch_SetUp(Pin);

            if (IsValidResponse(response))
            {
                return Raza.Model.OneTouchSet.Create(response);
            }
            else
            {
                return new OneTouchSet();
            }




        }

        public PinLessNumbers GetPinLessNumber(string Pin)
        {
            var response = WSClient.Get_PinLess_Numbers(Pin);

            if (IsValidResponse(response))
            {
                return Raza.Model.PinLessNumbers.Create(response);
            }
            else
            {
                var error = response.Split('|')[1].Split('=')[1];

            }

            return Raza.Model.PinLessNumbers.Create(response);

        }

        public bool IsPayerIdExist(string payerid, string consumerstatus, string addressStatus, string country)
        {
            var response = WSClient.Does_PayerID_Exist(payerid, consumerstatus, addressStatus, country);

            if (IsValidResponse(response))
            {
                var res = response.Split('|')[1].Split('=')[1];
                if (res == "N")
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            return true;
        }


        public bool AutoReFilledit(string MemberID, string Pin, double ReFill_Amount, string Card_Number)
        {
            var response = WSClient.Add_AutoReFill(MemberID, Pin, ReFill_Amount, Card_Number);
            if (IsValidResponse(response))
            {

                return true;

            }
            else
            {
                return false;
            }

        }

        public bool AutoRefillRemove(string memberId, string Pin, string ip_Address)
        {
            var res = WSClient.Remove_AutoReFill(memberId, Pin, ip_Address.Replace("::1", "192.168.1.1"));
            if (IsValidResponse(res))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteQuickKeySetUp(string Pin, string DestNum, string SpecDialNum, string ReqBy)
        {
            var response = WSClient.Delete_Quickeys_SetUp(Pin, DestNum, SpecDialNum, ReqBy);

            if (IsValidResponse(response))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteOneTouchSetup(string Pin, string OneTouch_Number, string Deleted_By)
        {
            var response = WSClient.Delete_OneTouch_SetUp(Pin, OneTouch_Number, Deleted_By);

            if (IsValidResponse(response))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReferFriendEmail(string MemberID, string FriendEmail, string ip_addres)
        {
            var res = WSClient.Refer_A_Friend(MemberID, FriendEmail, ip_addres);
            if (res.Split('=')[1] == "1")
            {

                return true;
            }
            else
            {
                return false;
            }


        }
        public CommonStatus RewardSignup(string MemberID, string DateOfBirth)
        {

            var res = WSClient.Reward_SignUp(MemberID, DateOfBirth);
            var status = new CommonStatus();
            if (res.Split('=')[1] == "1")
            {
                status.Status = true;
                // error="already exists"
            }
            else
            {
                status.Status = false;
                status.Errormsg = res.Split('|')[1].Split('=')[1];
            }

            return status;
        }

        public CommonStatus UpdatePassword(string MemberId, string OldPwd, string NewPwd)
        {
            var response = WSClient.Update_Password(MemberId, OldPwd, NewPwd);
            var model = new CommonStatus();
            if (IsValidResponse(response))
            {
                model.Status = true;
                model.Errormsg = string.Empty;
            }
            else
            {
                model.Status = false;
                model.Errormsg = response.Split('|')[1].Split('=')[1];
            }
            return model;
        }

        ///// <summary>
        ///// Recharge Pin for payment gateway
        ///// </summary>
        ///// <param name="MemberId"></param>
        ///// <param name="OldPwd"></param>
        ///// <param name="NewPwd"></param>
        ///// <returns></returns>
        //public string RechargePin(string MemberID,
        //    string Pin,
        //    double Purchase_Amount,
        //    string Card_Number,
        //    string Exp_Date,
        //    string CVV2,
        //    string Address1,
        //    string Address2,
        //    string City,
        //    string State,
        //    string ZipCode,
        //    string Country,
        //    string IP_Address)
        //{
        //    var response = WSClient.Recharge_Pin(MemberID,
        //        Pin,
        //        Purchase_Amount,
        //        Card_Number,
        //        Exp_Date,
        //        CVV2,
        //        Address1,
        //        Address2,
        //        City,
        //        State,
        //        ZipCode,
        //        Country,
        //        IP_Address);

        //    if (IsValidResponse(response))
        //    {
        //        return response;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }


        //}

        public string GetRazaReward(string MemberID)
        {
            var res = WSClient.Get_Reward_Points(MemberID);


            if (IsValidResponse(res))
            {
                var data = res.Split('|')[1].Split(',')[1].Split('=')[1];
                return data;
            }
            else
            {
                var data = res.Split('|')[1].Split('=')[1];
                return data;
            }


        }

        public CommonStatus Feedback(bool israzaCustomer, string feedbackType, string firstName, string lastName, string phoneNumber, string email, string feedBack)
        {
            var res = WSClient.Save_Feedback(israzaCustomer, feedbackType, firstName, lastName, phoneNumber, email,
                feedBack);
            if (IsValidResponse(res))
            {
                return new CommonStatus()
                {
                    Status = true,
                    Errormsg = string.Empty,
                };
            }
            else
            {
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = res.Split('|')[1].Split('=')[1]
                };
            }
        }
        public PlanInfo GetPlanDetails(string orderId, string memberId)
        {
            PlanInfo info = new PlanInfo();
            var result = GetCustomerPlanList(memberId);
            foreach (var setup in result.OrderInfos.Where(x => x.OrderId == orderId))
            {
                info.PlanName = setup.PlanName;
                info.Pin = setup.AccountNumber;
            }
            return info;
        }

        public CommonStatus New_complaint(string MemberID, string ContactPhone, string OrderID, string Access_Number,
            string Destination_Number, int CountryFrom, int CountryTo, string Description, string Notes)
        {
            var response = WSClient.New_Complaint(MemberID, ContactPhone, OrderID, Access_Number, Destination_Number,
                CountryFrom, CountryTo, Description, Notes);
            var status = new CommonStatus();
            if (IsValidResponse(response))
            {
                status.Status = true;
                status.Errormsg = response.Split('|')[1].Split('=')[1];
            }
            else
            {
                status.Status = false;
                status.Errormsg = response.Split('|')[1].Split('=')[1];
            }
            return status;
        }

        public CheckoutStatusModel IssueNewPin(RechargeInfo info)
        {

            RazaLogger.WriteErrorForMobile(
                string.Format(
                    "Calling Issue new Pin with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                    "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", info.OrderId, info.MemberId, info.UserType, info.IsCcProcess ? "N" : "Y", info.CardId,
                    info.Amount, info.ServiceFee, info.CountryFrom, info.CountryTo, info.CoupanCode, info.AniNumber, info.PaymentMethod,
                    "xxxx xxxx xxxx xxxx", info.ExpiryDate, "xxx", info.PaymentTransactionId, info.AVSResponse, info.CVV2Response,
                    info.Cavv, info.EciFlag, info.Xid, info.Address1, info.Address2, info.City, info.State, info.ZipCode,
                    info.Country, info.IpAddress, info.PayerId, info.IsAutoRefill, info.AutoRefillAmount));

            //if (info.PaymentMethod.ToLower() == "free trial")
            //{
            //    RazaLogger.WriteInfo2(
            //       string.Format(
            //           "Calling Issue new Pin with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
            //           "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", info.OrderId, info.MemberId, info.UserType, info.IsCcProcess ? "N" : "Y", info.CardId,
            //           info.Amount, info.ServiceFee, info.CountryFrom, info.CountryTo, info.CoupanCode, info.AniNumber, info.PaymentMethod,
            //           "xxxx-xxx-xxx", "xx-xx", "xxx", info.PaymentTransactionId, info.AVSResponse, info.CVV2Response,
            //           info.Cavv, info.EciFlag, info.Xid, info.Address1, info.Address2, info.City, info.State, info.ZipCode,
            //           info.Country, info.IpAddress, info.PayerId, info.IsAutoRefill, info.AutoRefillAmount));
            //}

            var response = WSClient.Issue_New_Pin(info.OrderId, info.MemberId, info.UserType, info.IsCcProcess ? "N" : "Y",
                info.CardId, info.Amount, info.ServiceFee, info.CountryFrom, info.CountryTo, info.CoupanCode, info.AniNumber,
                info.PaymentMethod, info.CardNumber, info.ExpiryDate, info.CVV2, info.PaymentTransactionId, info.AVSResponse ?? string.Empty,
                info.CVV2Response ?? string.Empty, info.Cavv ?? string.Empty, info.EciFlag ?? string.Empty, info.Xid ?? string.Empty,
                info.Address1, info.Address2, info.City, info.State, info.ZipCode,
                info.Country, info.IpAddress, info.PayerId ?? string.Empty, info.IsAutoRefill, info.AutoRefillAmount);

            RazaLogger.WriteErrorForMobile("Response of IssueNewPin is : " + response);

            var responsemodel = new CheckoutStatusModel();

            if (IsValidResponse(response))
            {
                responsemodel.IssuenewpinStatus = true;
                responsemodel.NewPin = response.Split('|')[2].Split('=')[1];

            }
            else
            {
                responsemodel.IssuenewpinStatus = false;
                responsemodel.NewPin = response.Split('|')[1].Split('=')[1];

                //responsemodel.Errormsg = response.Split('|')[1].Split('=')[1].Split(':')[1].Split('.')[0];
            }
            return responsemodel;
        }

        private bool IsValidResponse(string response)
        {
            return response.Split('|')[0].Split('=')[1] == "1";
        }

        private bool IsValidResponse2(string response)
        {
            return response.Split('~')[0].Split('=')[1] == "1";
        }

        public GetTopCreditCards Get_top_CreditCard(string MemberID)
        {
            var res = WSClient.Get_Top3_Credit_Cards(MemberID);

            if (IsValidResponse2(res))
            {
                return Raza.Model.GetTopCreditCards.Create(res);
            }
            else
            {
                return new GetTopCreditCards();
            }
            return null;
        }

        public TopUpOperator GetMobile_TopupOperator(int countryId)
        {
            var response = WSClient.Get_MobileTopUp_Operators(countryId);
            if (IsValidResponse(response))
            {
                return TopUpOperator.Create(response);
            }
            else
            {
                return new TopUpOperator();
            }
        }

        public TopUpOperator GetMobile_TopupOperator(int countryfrom, int countryto)
        {
            var response = WSClient.Get_MobileTopUp_Operators_v2(countryfrom, countryto);
            if (IsValidResponse(response))
            {
                return TopUpOperator.Create(response);
            }
            else
            {
                return new TopUpOperator();
            }
        }

        public List<TrialCountryInfo> FreeTrial_Country_List()
        {
            try
            {
                var data = WSClient.FreeTrial_Country_List();
                if (IsValidResponse(data))
                {
                    return TrialCountry.Create(data);
                }
            }
            catch (Exception ex)
            {

            }

            return new List<TrialCountryInfo>();
        }

        public string GetPinBalance(string Pin)
        {
            var res = WSClient.Get_Pin_Balance(Pin);
            string data = "";

            if (IsValidResponse(res))
            {
                data = res.Split('|')[1].Split('=')[1];
                return data;

            }
            else
            {
                data = res.Split('|')[1].Split('=')[1];
                return data;
            }
        }

        public List<ReferAFriendModel> GetReferFriendPt(string MemberID)
        {
            var list = new List<ReferAFriendModel>();

            var res = WSClient.Get_Refer_Friend_Points(MemberID);

            if (IsValidResponse(res))
            {
                if (res.Split('|').Count() > 1)
                {
                    string[] allrows = res.Split('|')[1].Split('~');

                    foreach (var allrow in allrows)
                    {
                        var model = new ReferAFriendModel
                            {
                                Email = allrow.Split(',')[0]
                            };

                        int pt;
                        model.Points = int.TryParse(allrow.Split(',')[1], out pt) ? pt : 0;

                        list.Add(model);
                    }
                }

            }


            return list;

        }

        public List<RedeemModel> RedeemPt(string memberid)
        {
            List<RedeemModel> list = new List<RedeemModel>();

            var res = WSClient.Get_Redeemed_Points(memberid);

            if (IsValidResponse(res))
            {
                if (res.Split('|').Count() > 1)
                {
                    string[] allrows = res.Split('|')[1].Split('~');
                    list.AddRange(allrows.Select(allrow => new RedeemModel { RedeemDate = allrow.Split(',')[0], RedeemPoints = SafeConvert.ToInt32(allrow.Split(',')[1]) }));
                }
            }

            return list;
        }

        public RechargeStatusModel Recharge_Pin(RechargeInfo rechargeInfo)
        {
            //RazaLogger.WriteErrorForMobile(
            //    string.Format(
            //        "Calling Recharge Pin with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
            //        "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27}", rechargeInfo.OrderId, rechargeInfo.MemberId,
            //        rechargeInfo.UserType, rechargeInfo.Pin,
            //        rechargeInfo.Amount, rechargeInfo.ServiceFee, rechargeInfo.CoupanCode,
            //        rechargeInfo.PaymentMethod,
            //        rechargeInfo.IsCcProcess ? "N" : "Y",
            //        "**** **** **** ****",
            //        rechargeInfo.ExpiryDate, "***", rechargeInfo.PaymentTransactionId,
            //        rechargeInfo.AVSResponse, rechargeInfo.CVV2Response, rechargeInfo.Cavv,
            //        rechargeInfo.EciFlag, rechargeInfo.Xid, rechargeInfo.Address1, rechargeInfo.Address2,
            //        rechargeInfo.City, rechargeInfo.State, rechargeInfo.ZipCode,
            //        rechargeInfo.Country, rechargeInfo.IpAddress, rechargeInfo.PayerId ?? string.Empty,
            //        rechargeInfo.IsAutoRefill, rechargeInfo.AutoRefillAmount));

            try
            {

                var res = WSClient.Recharge_Pin(rechargeInfo.OrderId, rechargeInfo.MemberId, rechargeInfo.UserType, rechargeInfo.Pin,
                    rechargeInfo.Amount, rechargeInfo.ServiceFee,
                    rechargeInfo.CoupanCode, rechargeInfo.PaymentMethod,
                    rechargeInfo.IsCcProcess ? "N" : "Y",
                    rechargeInfo.CardNumber,
                    rechargeInfo.ExpiryDate,
                    rechargeInfo.CVV2,
                    rechargeInfo.PaymentTransactionId,
                    rechargeInfo.AVSResponse,
                    rechargeInfo.CVV2Response,
                    rechargeInfo.Cavv,
                    rechargeInfo.EciFlag,
                    rechargeInfo.Xid,
                    rechargeInfo.Address1,
                    rechargeInfo.Address2 ?? String.Empty,
                    rechargeInfo.City, rechargeInfo.State, rechargeInfo.ZipCode,
                    rechargeInfo.Country, rechargeInfo.IpAddress, rechargeInfo.PayerId ?? string.Empty,
                    rechargeInfo.IsAutoRefill, rechargeInfo.AutoRefillAmount);
                //RazaLogger.WriteErrorForMobile(res);
                if (IsValidResponse(res))
                {

                    var statusmodel = new RechargeStatusModel
                    {
                        Status = true,
                        Rechargestatus = res.Split('|')[0].Split('=')[1],
                        OrderId = res.Split('|')[1].Split('=')[1]
                    };

                    //if (rechargeInfo.PaymentMethod.ToLower() == "paypal")
                    //RazaLogger.WriteInfoPartial("Successfull Recharge and order id is: " + statusmodel.OrderId);

                    return statusmodel;
                }
                else
                {
                    var statusmodel = new RechargeStatusModel
                    {
                        Rechargestatus = res.Split('|')[0].Split('=')[1],
                        RechargeError = res.Split('|')[1].Split('=')[1]
                    };

                    //if (rechargeInfo.PaymentMethod.ToLower() == "paypal")
                    //    RazaLogger.WriteInfoPartial(string.Format("Recharge Pin Failed :{0}", res));

                    return statusmodel;
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo(ex.Message);
            }
            return new RechargeStatusModel();
        }

        public bool IsCentinelProcess(string memberid, string userType, string ordertype)
        {
            var res = WSClient.IsCentinel(memberid, userType, ordertype);
            if (IsValidResponse(res))
            {
                string[] allrows = res.Split('|')[1].Split('=');
                return allrows[1].ToLower() == "y";
            }
            return false;
        }



        public CcValidationModel CcProcessValidation(string memberid, string userType, string ordertype, string PaymentMethod, string CardNumber, string Country, double TransAmount)
        {
            if (ordertype.ToUpper() == "TOPUP")
                RazaLogger.WriteInfoPartial(string.Format("Calling CcProcessValidation For Mobile TopUp: {0},{1},{2},{3},{4},{5}", memberid, userType, ordertype, PaymentMethod, CardNumber, Country));

            //var res = WSClient.CC_Process_Validation(memberid, userType, ordertype, PaymentMethod, CardNumber, Country);
            var res = WSClient.CC_Process_Validation_New(memberid, userType, ordertype, PaymentMethod, TransAmount, CardNumber, Country);

            if (ordertype.ToUpper() == "TOPUP")
                RazaLogger.WriteInfoPartial("response of ccprocessvalidation: " + res);

            if (IsValidResponse(res))
            {
                var model = new CcProcessValidations();
                var response = model.Create(res);
                return response;
            }
            return new CcValidationModel();
        }

        public bool SaveNewPendingOrder(RechargeInfo info)
        {


            RazaLogger.WriteErrorForMobile(
               string.Format(
                   "Calling SaveNewPendingOrder with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                   "{18},{19},{20},{21},{22},{23},{24},{25},{26}", info.MemberId, info.OrderId, info.CardId, info.Amount,
                   info.CountryFrom, info.CountryTo,
                info.CoupanCode, info.PaymentMethod, "xxxx xxxx xxxx xxxx", info.ExpiryDate, "xxx", info.FirstName, info.LastName, info.EmailAddress,
                info.AniNumber, info.Address1, info.Address2, info.City, info.State, info.ZipCode, info.Country, info.AuthTransactionId,
                info.CentinelTransactionId, info.CentinelPayLoad, info.PayResPayLoad, info.ProcessedBy, info.IpAddress));

            try
            {
                var res = WSClient.SaveNewPendingOrder(info.MemberId, info.OrderId, info.CardId, info.Amount, info.CountryFrom, info.CountryTo,
                    info.CoupanCode, info.PaymentMethod, info.CardNumber, info.ExpiryDate, info.CVV2, info.FirstName, info.LastName, info.EmailAddress,
                    info.AniNumber, info.Address1, info.Address2, info.City, info.State, info.ZipCode, info.Country, info.PayerId, info.AuthTransactionId,
                    info.CentinelTransactionId, info.CentinelPayLoad, info.PayResPayLoad, info.ProcessedBy, info.IpAddress);
                RazaLogger.WriteInfo("Response of SaveNewPendingOrder is :" + res);

                if (IsValidResponse(res))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<RazaRewardPoint> GetRazaRewardPt(string memberID)
        {
            var res = WSClient.Get_Raza_Reward_Points(memberID);

            List<RazaRewardPoint> list = new List<RazaRewardPoint>();
            if (IsValidResponse(res))
            {
                if (res.Split('|').Count() > 1)
                {
                    string[] allrows = res.Split('|')[1].Split('~');

                    foreach (var allrow in allrows)
                    {
                        var model = new RazaRewardPoint()
                        {
                            RewardDate = allrow.Split(',')[0]
                        };

                        int pt;
                        model.Points = int.TryParse(allrow.Split(',')[1], out pt) ? pt : 0;
                        list.Add(model);
                    }
                }

            }
            return list;

        }

        public Mobiletopup TopUpPhoneNumberInfo(string phonenumber)
        {
            RazaLogger.WriteInfo(string.Format("Calling TopUP_PhoneNo_Info with parameter:{0}", phonenumber));
            var response = WSClient.TopUp_PhoneNo_Info(phonenumber);
            RazaLogger.WriteInfo(string.Format("Response of TopUP_PhoneNo_Info :" + response));
            if (IsValidResponse(response))
            {
                var topupModel = new Mobiletopup();

                var res = response.Split('|')[1];
                topupModel.TopUpCountryCode = res.Split(',')[0].Split('=')[1];
                topupModel.TopUpMobileOperator = res.Split(',')[1].Split('=')[1];

                return topupModel;
            }
            else
            {
                return new Mobiletopup();
            }
        }


        public bool ValidateCouponCode(string memberId, string couponCode, int cardId, int countryFrom, int countryTo, double purchaseAmount, string transMode)
        {

            var res = WSClient.Validate_CouponCode(memberId, couponCode, cardId, countryFrom, countryTo, purchaseAmount,
                transMode);
            RazaLogger.WriteInfo(string.Format("Calling validate_CouponCode with parameters {0},{1},{2},{3},{4},{5},{6}", memberId, couponCode, cardId, countryFrom, countryTo, purchaseAmount, transMode));
            if (IsValidResponse(res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public CommonStatus ValidateCustomer(string memberId, string cardNumber, int cardId, double purchaseAmount, string transMode, int Countryfrom, int Countryto, string couponCode = "")
        {
            if (couponCode == null)
            {
                couponCode = "";
            }


            var response = WSClient.Validate_Customer(memberId, cardNumber, cardId, purchaseAmount,
                couponCode, Countryfrom, Countryto, transMode);


            if (IsValidResponse(response))
            {
                return new CommonStatus()
                {
                    Status = true
                };
            }
            else
            {
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = response.Split('|')[1].Split('=')[1].ToDisplayMessage()
                };
            }
        }



        public string GetCustomerLocation(string ipaddress)
        {

            var response = WSClient.Get_Customer_Location(ipaddress);

            if (IsValidResponse(response))
            {
                return response.Split('|')[1].Split('=')[1];
            }
            else
            {
                return "1";
            }
        }

        public RechargeStatusModel MobileTopupRecharge2(Mobiletopup topup)
        {
            RazaLogger.WriteErrorForMobile(
               string.Format(
                   "Calling topup with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                   "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", topup.NewOrderId, topup.MemberId, topup.OperatorCode, topup.Operator, topup.CountryId,
                   topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber, topup.SmsTo, topup.CouponCode, topup.PaymentMethod,
                   topup.Isccprocess ? "N" : "Y",
                   topup.Pin, topup.PurchaseAmount, "**** **** **** ****", topup.ExpDate, "***", topup.PaymentTransactionId,
                   topup.AvsResponse, topup.Cvv2Response, topup.Cavv, topup.EciFlag, topup.Xid, topup.Address1, topup.Address2,
                   topup.City, topup.State, topup.ZipCode, topup.Country, topup.IpAddress, topup.PayerId ?? string.Empty));

            var response = WSClient.Mobile_TopUp_v2(topup.NewOrderId, topup.MemberId, topup.OperatorCode ?? string.Empty, topup.Operator ?? string.Empty, topup.CountryId,
    topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber ?? string.Empty, topup.SmsTo ?? string.Empty, topup.CouponCode ?? string.Empty, topup.PaymentMethod ?? string.Empty,
    topup.Isccprocess ? "N" : "Y",
    topup.Pin, topup.PurchaseAmount, topup.CardNumber ?? string.Empty, topup.ExpDate ?? string.Empty, topup.Cvv2 ?? string.Empty, topup.PaymentTransactionId ?? string.Empty,
    topup.AvsResponse ?? string.Empty, topup.Cvv2Response ?? string.Empty, topup.Cavv ?? string.Empty, topup.EciFlag ?? string.Empty, topup.Xid ?? string.Empty, topup.Address1 ?? string.Empty,
    topup.Address2 ?? string.Empty, topup.City ?? string.Empty, topup.State ?? string.Empty, topup.ZipCode ?? string.Empty, topup.Country ?? string.Empty, topup.IpAddress.Replace("::1", "192.168.1.1") ?? string.Empty, topup.PayerId ?? string.Empty);


            RazaLogger.WriteErrorForMobile("service response is: " + response);
            if (IsValidResponse(response))
            {

                var statusmodel = new RechargeStatusModel
                {
                    Status = true,
                    OrderId = response.Split('|')[1].Split('=')[1]
                };
                return statusmodel;
            }
            else
            {
                var statusmodel = new RechargeStatusModel
                {
                    Rechargestatus = response.Split('|')[0].Split('=')[1],
                    RechargeError = response.Split('|')[1].Split('=')[1]
                };
                return statusmodel;
            }
        }

        public RechargeStatusModel MobileTopupRecharge(Mobiletopup topup)
        {
            RazaLogger.WriteErrorForMobile(
               string.Format(
                   "Calling topup with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                   "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", topup.NewOrderId, topup.MemberId, topup.OperatorCode, topup.Operator, topup.CountryId,
                   topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber, topup.SmsTo, topup.CouponCode, topup.PaymentMethod,
                   topup.Isccprocess ? "N" : "Y",
                   topup.Pin, topup.PurchaseAmount, "**** **** **** ****", topup.ExpDate, "***", topup.PaymentTransactionId,
                   topup.AvsResponse, topup.Cvv2Response, topup.Cavv, topup.EciFlag, topup.Xid, topup.Address1, topup.Address2,
                   topup.City, topup.State, topup.ZipCode, topup.Country, topup.IpAddress, topup.PayerId ?? string.Empty));

            //var response = WSClient.Mobile_TopUp(topup.NewOrderId, topup.MemberId, topup.OperatorCode, topup.Operator, topup.CountryId,
            //    topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber, topup.SmsTo, topup.CouponCode, topup.PaymentMethod,
            //    topup.Isccprocess ? "N" : "Y",
            //    topup.Pin, topup.PurchaseAmount, topup.CardNumber, topup.ExpDate, topup.Cvv2, topup.PaymentTransactionId,
            //    topup.AvsResponse, topup.Cvv2Response, topup.Cavv, topup.EciFlag, topup.Xid, topup.Address1,
            //    topup.Address2, topup.City, topup.State, topup.ZipCode, topup.Country, topup.IpAddress, topup.PayerId ?? string.Empty);


            var response = WSClient.Mobile_TopUp(topup.NewOrderId, topup.MemberId, topup.OperatorCode ?? string.Empty, topup.Operator ?? string.Empty, topup.CountryId,
    topup.SourceAmount, topup.DestinationAmt, topup.DestinationPhoneNumber ?? string.Empty, topup.SmsTo ?? string.Empty, topup.CouponCode ?? string.Empty, topup.PaymentMethod ?? string.Empty,
    topup.Isccprocess ? "N" : "Y",
    topup.Pin, topup.PurchaseAmount, topup.CardNumber ?? string.Empty, topup.ExpDate ?? string.Empty, topup.Cvv2 ?? string.Empty, topup.PaymentTransactionId ?? string.Empty,
    topup.AvsResponse ?? string.Empty, topup.Cvv2Response ?? string.Empty, topup.Cavv ?? string.Empty, topup.EciFlag ?? string.Empty, topup.Xid ?? string.Empty, topup.Address1 ?? string.Empty,
    topup.Address2 ?? string.Empty, topup.City ?? string.Empty, topup.State ?? string.Empty, topup.ZipCode ?? string.Empty, topup.Country ?? string.Empty, topup.IpAddress.Replace("::1", "192.168.1.1") ?? string.Empty, topup.PayerId ?? string.Empty);


            RazaLogger.WriteErrorForMobile("service response is: " + response);
            if (IsValidResponse(response))
            {

                var statusmodel = new RechargeStatusModel
                {
                    Status = true,
                    OrderId = response.Split('|')[1].Split('=')[1]
                };
                return statusmodel;
            }
            else
            {
                var statusmodel = new RechargeStatusModel
                {
                    Rechargestatus = response.Split('|')[0].Split('=')[1],
                    RechargeError = response.Split('|')[1].Split('=')[1]
                };
                return statusmodel;
            }
        }

        public HomePageRate HomePageCountryRateList(int countryfrom)
        {
            var response = WSClient.HomePage_Country_RateList(countryfrom);
            if (IsValidResponse(response))
            {
                return HomePageRate.CreateModel(response);
            }
            else
            {
                return new HomePageRate();
            }
        }
        public CommonStatus AgentSignUp(string FirstName, string LastName, string Email, string PhoneNumber, string Address, string City, string ZipCode, string Message, string State, string Country)
        {
            var res = WSClient.Agent_SignUp(FirstName, LastName, PhoneNumber, Email, Address, City, State, ZipCode, Country, Message);
            if (IsValidResponse(res))
            {
                return new CommonStatus()
                {
                    Status = true,
                    // Errormsg = res.Split('|')[1].Split('=')[1]
                };

            }
            else
            {
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = res.Split('|')[1].Split('=')[1]
                };
            }


        }
        public bool SubsEmail(string email, string mode)
        {
            var res = WSClient.Email_Subscription(email, mode);

            if (IsValidResponse(res))
            {
                return true;
            }
            return false;
        }

        public List<EmailModel> GetEmails(string MemberId)
        {
            List<EmailModel> response = new List<EmailModel>();
            try
            {

                using (var connection = new SqlConnection(ConfigurationManager.AppSettings["conn"]))
                {

                    connection.Open();

                    var command =
                        new SqlCommand(
                            string.Format(
                                "SELECT Id,Subject,Body, b.UpdatedDate FROM emails a "
                                + "INNER JOIN EmailMembers b ON a.Id  = b.EmailId"
                                + " WHERE b.MemberId = '{0}' ORDER by b.UpdatedDate Desc",
                                MemberId)) { Connection = connection };

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = new EmailModel
                                {
                                    EmailId = SafeConvert.ToInt32(reader["Id"]),
                                    Subject = reader["Subject"].ToString(),
                                    Body = reader["Body"].ToString(),
                                    UpdatedTimestamp = Convert.ToDateTime(reader["UpdatedDate"])
                                };
                            response.Add(r);
                        }
                        reader.Close();

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }


        public bool DeleteMail(string MemberId, int EmailId)
        {
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.AppSettings["conn"]))
                {
                    connection.Open();

                    var command =
                        new SqlCommand(
                            string.Format(
                                "DELETE from EmailMembers WHERE EmailId = {0} AND MemberId = '{1}'", EmailId, MemberId))
                        {
                            Connection = connection
                        };

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool GetEmailSubscription(string Email)
        {
            var res = WSClient.Get_Email_Subscription(Email);

            if (IsValidResponse(res))
            {
                return true;
            }
            return false;

            //try
            //{
            //    using (var connection = new SqlConnection(ConfigurationManager.AppSettings["conn"]))
            //    {
            //        connection.Open();

            //        var command =
            //            new SqlCommand(
            //                string.Format("SELECT IsEmailSubscribed from EmailMembers WHERE Email = '{0}'", Email));

            //        command.Connection = connection;
            //        return (bool)command.ExecuteScalar();
            //    }
            //}
            //catch (Exception)
            //{

            //}

            //return false;
        }

        public bool DeleteEmailSubscription(string Email)
        {

            return SubsEmail(Email, "0");
            //try
            //{
            //    using (var connection = new SqlConnection(ConfigurationManager.AppSettings["conn"]))
            //    {
            //        connection.Open();

            //        var command = new SqlCommand(string.Format(
            //                           "Update dbo.EmailSubscription SET IsEmailSubscribed = 0 WHERE Email = '{0}'", Email)) { Connection = connection };

            //        command.ExecuteNonQuery();
            //        return true;
            //    }
            //}
            //catch (Exception)
            //{

            //}

            //return false;
        }


        public List<LocalAccessNumber> GetLocalAccessNumber(string state, string countryId, string phoneNumber, string searchType)
        {

            RazaLogger.WriteInfo(string.Format("Calling GetLocal AccessNumber: {0},{1},{2},{3}", countryId, state, phoneNumber, searchType));
            var response = WSClient.Get_LocalAccess_Numbers(countryId, state, phoneNumber, searchType);
            //RazaLogger.WriteInfo("response of GetLocalAccessNumber is: ", response);
            if (IsValidResponse(response))
            {
                return CreateLocalAccessNumberModel.Create(response);
            }
            else
            {
                return new List<LocalAccessNumber>();

            }

        }
        public GetRechargeAmount RechargeAmountVal(int planId)
        {
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
            List<int> list = new List<int>()
            {
                163,
                164,
                125,
                126,
                120,
                121,
                176,
                178,
                180,
                175,
                177,
                179
            };
            if (list.Exists(ele => ele == planId))
            {
                var amntlist = new List<GetAmount>();
                double amnt = 0;
                switch (planId)
                {
                    case 175:
                        {
                            amnt = 9.99;
                            break;
                        }
                    case 176:
                        {
                            amnt = 9.99;
                            break;
                        }
                    case 177:
                        {
                            amnt = 14.99;
                            break;
                        }
                    case 178:
                        {
                            amnt = 14.99;
                            break;
                        }
                    case 179:
                        {
                            amnt = 19.99;
                            break;
                        }
                    case 180:
                        {
                            amnt = 19.99;
                            break;
                        }
                    case 163:
                        {
                            amnt = 15;
                            break;
                        }
                    case 164:
                        {
                            amnt = 15;
                            break;
                        }
                    case 121:
                        {
                            amnt = 10;
                            break;
                        }
                    case 120:
                        {
                            amnt = 10;
                            break;
                        }
                    case 125:
                        {
                            amnt = 10;
                            break;
                        }
                    case 126:
                        {
                            amnt = 10;
                            break;
                        }
                }
                amntlist.Add(new GetAmount()
                {
                    RechAmount = amnt
                });
                return new GetRechargeAmount()
                {
                    GetAmountlist = amntlist
                };
            }
            //var res = WSClient.Get_Recharge_Amount_List(planId);
            //if (IsValidResponse(res))
            //{
            //    return Raza.Model.GetRechargeAmount.Create(res);
            //}
            //else
            //{
            //    return new GetRechargeAmount();
            //}


            if (planId == 71 || planId == 72)//MONTHLY PACK','MONTHLY PACK CANADA
                return Raza.Model.GetRechargeAmount.Create("status=1|amount=29.99,59.99,99.99");
            else if (planId == 102)//UK DIRECT DIAL
                return Raza.Model.GetRechargeAmount.Create("status=1|amount=5,10,25,50,100");
            else if (planId == 90 || planId == 91 || planId == 125 || planId == 126)
                return Raza.Model.GetRechargeAmount.Create("status=1|amount=10");
            else
                return Raza.Model.GetRechargeAmount.Create("status=1|amount=10,20,50,90,100,150,200,250");

            return null;




        }

        public List<LocalAccessNumber> GetAccessNumber(string state, string countryId, string phoneNumber)
        {
            List<LocalAccessNumber> list = new List<LocalAccessNumber>();

            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.AppSettings["conn"]))
                {
                    connection.Open();

                    var command = new SqlCommand("dbo.GetLocalAccessNumber", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    if (phoneNumber.Length >= 3)
                    {
                        command.Parameters.AddWithValue("@AreaCode", phoneNumber.Substring(0, 3));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AreaCode", DBNull.Value);
                    }

                    if (phoneNumber.Length >= 6)
                    {
                        command.Parameters.AddWithValue("@NextThree", phoneNumber.Substring(3, 3));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@NextThree", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    command.Parameters.AddWithValue("@State", state);
                    int i = 0;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (countryId == "1" || countryId == "2") //USA or canada
                            {
                                var localAccessNumber = new LocalAccessNumber
                                {
                                    Ani = SafeConvert.ToString(reader["Ani"]),
                                    NonAni = SafeConvert.ToString(reader["NonAni"]),
                                    AreaCode = SafeConvert.ToString(reader["AreaCode"]),
                                    State = SafeConvert.ToString(reader["State"]),
                                    NextThree = SafeConvert.ToString(reader["NextThree"]),
                                    City = SafeConvert.ToString(reader["City"]),
                                    SNo = (++i).ToString(),
                                    CountryId = SafeConvert.ToString(reader["CountryId"])
                                };
                                list.Add(localAccessNumber);
                            }
                            else
                            {
                                var localAccessNumber = new LocalAccessNumber
                                {
                                    City = SafeConvert.ToString(reader["City"]),
                                    SNo = (++i).ToString(),
                                    CountryId = SafeConvert.ToString(reader["CountryId"]),
                                    PhoneNo = SafeConvert.ToString(reader["PhoneNo"])
                                };
                                list.Add(localAccessNumber);
                            }

                        }
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteError(ex.ToString(), "Repository->GetAccessNumber");
            }

            return list;
        }

        public CommonStatus DoesPhoneNumberExist(string memberid, string phonenumber)
        {
            var res = WSClient.Does_PhoneNumber_Exist(memberid, phonenumber);
            if (IsValidResponse(res))
            {
                return new CommonStatus()
                {
                    Status = true
                };
            }
            else
            {
                return new CommonStatus()
                {
                    Status = false,
                    Errormsg = res.Split('|')[1].Split('=')[1]
                };
            }
        }

        private Raza_FrontEnd WSClient
        {
            get
            {
                var client = new Raza_FrontEnd
                {
                    Raza_FrontEnd_AuthHeaderValue = new Raza_FrontEnd_AuthHeader()
                    {
                        AuthClientID = "RAZA",
                        AuthUsername = "XXXXXXX",
                        AuthPassword = "XXXXXXX"
                    }
                };

                return client;
            }
        }

        public List<MobileTopupOperator> Getalloperator()
        {

            var list = GetCountryList("to");
            var OperatorList = new List<MobileTopupOperator>();
            foreach (var data in list)
            {
                var response = GetMobile_TopupOperator(SafeConvert.ToInt32(data.Id));
                OperatorList.AddRange(response.OperatorList);
            }
            return OperatorList;
        }

        public bool ValidateCustomerPlan(string memberId, string pin)
        {
            var res = WSClient.Verify_Customer_Plan(memberId, pin);
            if (IsValidResponse(res))
            {
                return true;
            }
            return false;
        }

        public List<CountryWithLowestRateModel> GetCountryWithLowestRates()
        {
            var res = WSClient.Get_AllCountry_With_LowestRate();
            if (IsValidResponse2(res))
            {
                string[] str = res.Split('~');
                var list = new List<CountryWithLowestRateModel>();
                for (int i = 1; i < str.Length - 1; i++)
                {
                    //string countrycode = str[i].Split('~')[2];
                    //string id = str[i].Split('~')[0];
                    //string name = str[i].Split('~')[1];
                    //string countcode = countrycode;
                    list.Add(new CountryWithLowestRateModel()
                    {
                        Id = SafeConvert.ToInt32(str[i].Split(',')[0]),
                        Name = str[i].Split(',')[1],
                        Code = str[i].Split(',')[2],
                        UsaLandlineRate = string.IsNullOrEmpty(str[i].Split(',')[3]) ? 0 : Convert.ToDecimal(str[i].Split(',')[3]),
                        UsaMobileRate = string.IsNullOrEmpty(str[i].Split(',')[4]) ? 0 : Convert.ToDecimal(str[i].Split(',')[4]),
                        CanadaLandlineRate = string.IsNullOrEmpty(str[i].Split(',')[5]) ? 0 : Convert.ToDecimal(str[i].Split(',')[5]),
                        CanadaMobileRate = string.IsNullOrEmpty(str[i].Split(',')[6]) ? 0 : Convert.ToDecimal(str[i].Split(',')[6]),
                        UkLandlineRate = string.IsNullOrEmpty(str[i].Split(',')[7]) ? 0 : Convert.ToDecimal(str[i].Split(',')[7]),
                        UkMobileRate = string.IsNullOrEmpty(str[i].Split(',')[8]) ? 0 : Convert.ToDecimal(str[i].Split(',')[8])
                    });
                }

                return list;
            }
            return new List<CountryWithLowestRateModel>();

            //return res;  //1,Afghanistan,93,12.9,13.8,14.3,17,10.8,10.2
        }

        public CommonStatus DeleteCreditCard(string memberId, string creditCard)
        {
            var res = WSClient.Delete_Credit_Card(memberId, creditCard);
            if (IsValidResponse(res))
            {
                return new CommonStatus()
                {
                    Status = true
                };
            }
            else
            {
                return new CommonStatus()
                {
                    Errormsg = res.Split('|')[1].Split('=')[1]
                };
            }
        }

    }


}

