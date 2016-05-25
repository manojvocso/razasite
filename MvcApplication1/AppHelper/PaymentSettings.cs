using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.AppHelper
{
    public static class PaymentSettings
    {
        static PaymentSettings()
        {
            ExpMonthsList = new List<string>();
            ExpYearsList = new List<string>();
            CardTypeList = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                ExpMonthsList.Add(i.ToString("00"));
            }
            for (int i = DateTime.Now.Year; i < DateTime.Now.AddYears(10).Year; i++)
            {
                ExpYearsList.Add(Convert.ToString(i));
            }
            CardTypeList = Enum.GetNames(typeof(CardType)).ToList();
        }
        public enum PaymentType
        {
            CreditCard,
            PayPal
        }

        public enum CardType
        {
            Visa,
            MasterCard,
            Amex,
            Discover
        }

        public static List<string> ExpMonthsList { get; set; }
        public static List<string> ExpYearsList { get; set; }
        public static List<string> CardTypeList { get; set; }


        public static string GetCardType(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "");
            if (Regex.Match(cardNumber, @"^4\d{15}$").Success)
                return PaymentSettings.CardType.Visa.ToString();
            else if (Regex.Match(cardNumber, @"^5[1-5]\d{14}$").Success)
                return PaymentSettings.CardType.MasterCard.ToString();
            else if (Regex.Match(cardNumber, @"^3[47]\d{13}$").Success)
                return PaymentSettings.CardType.Amex.ToString();
            else if (Regex.Match(cardNumber, @"^6(?:011\d\d|5\d{4}|4[4-9]\d{3}|22(?:1(?:2[6-9]|[3-9]\d)|[2-8]\d\d|9(?:[01]\d|2[0-5])))\d{10}$").Success)
                return PaymentSettings.CardType.Discover.ToString();

            return string.Empty;
        }

        public static string GetOrderType(string transactionType)
        {
            if (transactionType == TransactionType.Recharge.ToString())
            {
                return TransactionType.Recharge.ToString();
            }
            else if (transactionType == TransactionType.PurchaseNewPlan.ToString())
            {
                return "NewPin";
            }
            else if (transactionType == TransactionType.CheckOut.ToString())
            {
                return "NewPin";
            }
            else if (transactionType == TransactionType.TopUp.ToString())
            {
                return "Topup";
            }
            return String.Empty;
        }

    }

    public enum TransactionType
    {
        Recharge,
        PurchaseNewPlan,
        CheckOut,
        TopUp,
        SaveOrder
    }


}