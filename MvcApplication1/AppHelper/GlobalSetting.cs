using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MvcApplication1.AppHelper
{
    public static class GlobalSetting
    {
        public static readonly string CheckOutSesionKey = "PaypalCheckoutModel";

        public static string ServerName
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerName"];
            }
        }

        private static readonly Dictionary<string, string> CurrencyCodeDict = new Dictionary<string, string>
        {
            {"USD", "$"},
            {"CAD", "$"},
            {"GBP", "£"}
        };

        public static string GetCurrencySign(string currencyCode)
        {
            var sign = CurrencyCodeDict[currencyCode];
            if (string.IsNullOrEmpty(sign))
            {
                sign = "$";
            }
            return sign;
        }

        public static string GetCurrencySign2(int countryId)
        {
            if (countryId == 3)
                return "p/min";

            return "¢/min";
        }
    }

    public static class MessageType
    {
        public const string Error = "alert-danger";
        public const string Warning = "alert-warning";
        public const string Info = "alert-info";
        public const string Success = "alert-success";

    }

    enum Months
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
    
}