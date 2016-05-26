using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class MyAccountModel : BaseRazaViewModel
    {
        public MyAccountModel()
        {
            MyOrders = new List<EachOrderSnapshot>();
            Email = new EmailModel();
            ReferEmails = new List<ReferAFriendModel>();
        }

        public List<EachOrderSnapshot> MyOrders { get; set; }

        public string RedirectFrom { get; set; }
        public string Point { get; set; }
        public int RedeemPoint { get; set; }
        public BillingInfoModel ProfileInformation { get; set; }
        public string State { get; set; }
        public string Balance { get; set; }
        public string CurrencySign { get; set; }

        public List<ReferAFriendModel> ReferEmails { get; set; }
        public List<State> StateList { get; set; }
        public SelectList Statelist { get; set; }
        public EmailModel Email { get; set; }
        public List<PointListModel> ReedemPointsList { get; set; } 
        public string Message { get; set; }
    }

    public class PointListModel
    {
        public string ReedemPoints { get; set; }
        public decimal PointValue { get; set; }
    }

    public class EachOrderSnapshot : BaseRazaViewModel
    {
        public string OrderId { get; set; }

        public string PlanName { get; set; }

        public string PlanId { get; set; }

        public int CardId { get; set; }
        public string MyAccountBal { get; set; }

        public string AccountNumber { get; set; }
        public string ServiceFee { get; set; }
        public string EncapsuleAccountNumber { get; set; }
       
        public bool AllowCallForwarding { get; set; }

        public bool AllowCallDetails { get; set; }

        public bool AllowOneTouchSetup { get; set; }

        public string AllowAutoRefill { get; set; }

        public bool ShowBalance { get; set; }

        public bool AllowRecharge { get; set; }

        public bool AllowPinlessSetup { get; set; }

        public bool AllowQuickkeySetup { get; set; }

        public bool IsActivePlan { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CurrencyCode { get; set; }

        public int CountryFrom { get; set; }

        public int CountryTo { get; set; }

        public string PlanType { get; set; }

    }

    public class LogOnModel : BaseRazaViewModel
    {
        public LogOnModel()
        {
            list = new List<Country>();
        }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool? RememberMe { get; set; }
        public string UserName { get; set; }

        public string Error { get; set; }
        public string Cookie { get; set; }

        public List<Country> list { get; set; }
        public List<Country> listTo { get; set; }
        public int CountryTo { get; set; }
        public int CountryFrom { get; set; }
        public string Email { get; set; }

        public string Phone_Number { get; set; }
        public string ErrorMsg { get; set; }

    }
    public class ForgotPasswordModel : BaseRazaViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }



    public class BillingInfoModel : BaseRazaViewModel
    {
        public string MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public List<State> States { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string RefererEmail { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }
        public SelectList Statelist { get; set; }

        //public string PhoneNo { get; set; }

        public string OldPwd { get; set; }
        public string NewPwd { get; set; }

        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }

        }

        public List<Country> Countries { get; set; }
    }
    public class GetBillingInfoJsonModel 
    {
        public string MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public List<State> States { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string RefererEmail { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }
        public SelectList Statelist { get; set; }

        //public string PhoneNo { get; set; }

        public string OldPwd { get; set; }
        public string NewPwd { get; set; }

        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }

        }

        public List<Country> Countries { get; set; }
    }

    public class OrderHistoricModel : EachOrderSnapshot
    {
        public string CurrencyCode { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionType { get; set; }
    }

    public class OrderHistoryModel : BaseRazaViewModel
    {
        public List<OrderHistoricModel> Orders { get; set; }
    }
    public class PinlessSetupsModel
    {
        public List<PinlessSetupModel> AllSetup { get; set; }
    }

    public class PinlessSetupModel : BaseRazaViewModel
    {
        public string PlanName { get; set; }
        public string OriginalPriceCurrencyCode { get; set; }
        public string OriginalPrice { get; set; }
        public string AccessNumber { get; set; }
        public string CustomerSerialNumber { get; set; }
        public string PinNumber { get; set; }
        public string OrderId { get; set; }
    }

    public class QuickeysSetupModel : BaseRazaViewModel
    {
        public string PlanName { get; set; }
        public string OriginalPrice { get; set; }

        public string OrderId { get; set; }

        public string Pin { get; set; }


    }
    public class QuickeysSetupsModel : BaseRazaViewModel
    {
        public List<QuickeysSetupModel> AllSetup { get; set; }
    }
    public class CallDetailsModel : BaseRazaViewModel
    {
        public List<EachCallModel> AllCalls { get; set; }
    }
    public class EachCallModel : BaseRazaViewModel
    {
        public DateTime CallDate { get; set; }
        public string SourceNumber { get; set; }
        public string DestinationNumber { get; set; }
        public string DestinationCity { get; set; }
        public string CallDuration { get; set; }
        public string CallAmount { get; set; }

        //public string CardName { get; set; }
        //public string PinNumber { get; set; }
        //public string CallDate { get; set; }
        //public string CallDuration { get; set; }
    }

    public class PlanName
    {

        public string Plan { get; set; }

    }
    public class RechargeSetup : BaseRazaViewModel
    {
        public string PlanName { get; set; }
        public string Price { get; set; }
        public double ServiceFee { get; set; }
        public string order_id { get; set; }
        public string GetCodeString { get; set; }
        public string PassCodeString { get; set; }
        public string Message { get; set; }
        public string Pin { get; set; }
        public List<PlanName> PlanList { get; set; }
        public string MemberID { get; set; }
        public string Name_On_Card { get; set; }
        public string Card_Number { get; set; }
        public string Exp_Month { get; set; }
        public string Exp_Year { get; set; }
        public string CVV2 { get; set; }
        public string Country { get; set; }
        public string Billing_Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<State> StateList { get; set; }
        public List<GetCard> CardList { get; set; }
        public List<GetAmount> AmountRecharge { get; set; }
        public string RechargeBalance { get; set; }
        public double autoRefillAmount { get; set; }
        public SelectList Statelist { get; set; }
        public string autoRefill { get; set; }
        public List<Country> Countries { get; set; }
        public string cvv { get; set; }
        public string FirstName { get; set; }
        public List<int> Years { get; set; }
        public bool IsAutoRefill { get; set; }
        public string CurrencySign { get; set; }
        public List<string> CurrencyCodeWithAmount { get; set; }
        public string Curr { get; set; }
        public string LastName { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string Address { get; set; }
        public int PlanId { get; set; }
        public string Currentdate { get; set; }
        public string Email { get; set; }
        public List<string> NewAmount{ get; set; }
        public string IsMandatoryAutorefill { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + "" + LastName;
            }

        }

        public double Amount { get; set; }
        public string CardNumber { get; set; }
        public string CurrencyCode { get; set; }


        public string PaymentMethod { get; set; }
        public string CoupanCode { get; set; }
        public string IpAddress { get; set; }
        public string ExpDate { get; set; }

        public string CardType { get; set; }


        public string AniNumber { get; set; }
    }

}


enum MonthsInCallDetail
{
    Janurary = 1,
    Feburary = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
   
}


