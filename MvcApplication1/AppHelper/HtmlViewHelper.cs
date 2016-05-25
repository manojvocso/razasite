using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Raza.Model;

namespace MvcApplication1.AppHelper
{
    public static class HtmlViewHelper
    {
        public static IHtmlString CurrencySign(this HtmlHelper helper, string currencyCode)
        {
            var currencySign = GlobalSetting.GetCurrencySign(currencyCode);
            return new HtmlString(currencySign);
        }

        public static IHtmlString CurrencySign2(this HtmlHelper helper, int countryId)
        {
            var currencySign = GlobalSetting.GetCurrencySign2(countryId);
            return new HtmlString(currencySign);
        }

        public static IHtmlString CurrencySign3(this HtmlHelper helper, int countryId)
        {
            var currencySign = "$";
            if (countryId == 3)
            {
                currencySign = "£";
            }
            return new HtmlString(currencySign);
        }

        public static IHtmlString MaskedPlanPin(this HtmlHelper helper, string pin)
        {
            var maskedPin = Helpers.MaskAccountNumber(pin, "X");
            return new MvcHtmlString(maskedPin);
        }

        public static MvcHtmlString CurrentMonthText(this HtmlHelper helper)
        {
            var month = (Months)DateTime.Now.Month;
            return new MvcHtmlString(month.ToString());
        }

        public static MvcHtmlString PastOneMonthText(this HtmlHelper helper)
        {
            string pastMonth = String.Empty;
            if (DateTime.Now.Month == 1)
            {
                pastMonth = "December";
            }
            else if (DateTime.Now.Month == 2)
            {
                pastMonth = ((MonthsInCallDetail)System.DateTime.Now.Month - 1).ToString();
            }
            else
            {
                pastMonth = ((MonthsInCallDetail)DateTime.Now.Month - 1).ToString();
            }


            return new MvcHtmlString(pastMonth);
        }

        public static MvcHtmlString PastTwoMonthText(this HtmlHelper helper)
        {
            string pastMonth = String.Empty;
            if (DateTime.Now.Month == 1)
            {
                pastMonth = "November";
            }
            else if (DateTime.Now.Month == 2)
            {
                pastMonth = "December";
            }
            else
            {

                pastMonth = ((MonthsInCallDetail)DateTime.Now.Month - 2).ToString();
            }


            return new MvcHtmlString(pastMonth);
        }

        public static MvcHtmlString MaskedPinlessNumbers(this HtmlHelper helper, string pinlessNumber)
        {
            return new MvcHtmlString(Convert.ToInt64(pinlessNumber).ToString("(000)-000-0000"));
        }

        public static string EncodedParameterValue(this HtmlHelper htmlHelper, string id)
        {
            return ControllerHelper.Encrypt(id);
        }

        public static MvcHtmlString MessageBox(this HtmlHelper helper, string message, string messageType)
        {
            var messageBox = new StringBuilder();
            messageBox.Append("<div class=\"alert ");
            messageBox.Append(messageType);
            messageBox.Append("\">");
            messageBox.Append("<a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">×</a>");

            switch (messageType)
            {
                case MessageType.Error:
                    messageBox.Append("<strong>Error! </strong>");
                    break;
                case MessageType.Success:
                    messageBox.Append("<strong>Success! </strong>");
                    break;
                case MessageType.Warning:
                    messageBox.Append("<strong>Warning! </strong>");
                    break;
                case MessageType.Info:
                    messageBox.Append("<strong>Info! </strong>");
                    break;
            }

            messageBox.Append(message);
            messageBox.Append("</div>");
            return new MvcHtmlString(messageBox.ToString());
        }

        public static MvcHtmlString CardStatus(this HtmlHelper helper, string expMonth, string expYear)
        {
            if (SafeConvert.ToInt32(expYear) < DateTime.Now.Year)
            {
                return new MvcHtmlString("<h3 class=\" text-success card_titile\">Active</h3>");
            }
            else if (SafeConvert.ToInt32(expYear) == DateTime.Now.Year &&
                     SafeConvert.ToInt32(expMonth) <= DateTime.Now.Month)
            {
                return new MvcHtmlString("<h3 class=\" text-success card_titile\">Active</h3>");
            }
            else
            {
                return new MvcHtmlString("<h3 class=\" text-warning card_titile\">Expired</h3>");
            }
        }
    }
}