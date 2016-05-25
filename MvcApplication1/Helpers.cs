using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using MvcApplication1.Models;
using Raza.Model;
using WebGrease;
using System.Web.Routing;

namespace MvcApplication1
{
    using System.Configuration;
    using System.Net;

    public class Helpers
    {
        public static void SendEmail(string senderEmailAddress, string senderEmailName, string toEmailAddress, string subject, string mailbody, bool htmlcontent, string razarecipent = "")
        {
            var fromAddress = new MailAddress(senderEmailAddress, senderEmailName);
            var toAddress = new MailAddress(toEmailAddress);

            var smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            var port = SafeConvert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            var password = ConfigurationManager.AppSettings["EmailPassword"];

            var smtp = new SmtpClient
            {
                Host = smtpServer,
                Port = port,
                Credentials = new NetworkCredential(senderEmailAddress, password),
                EnableSsl = false
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = htmlcontent,
                Body = mailbody
            })
            {
                smtp.Send(message);
            }
        }

        public static List<String> ConvertCountryModelListToCountryList(List<Country> countrylist)
        {
            List<String> result = new List<String>();
            result = countrylist.Select(t => t.Name).ToList();
            //result = (from eachcountry in countrylist select eachcountry.Name).ToList();
            return result;
        }

        public static string MaskAccountNumber(string accountnumber, string maskchar)
        {
            //return accountnumber;
            //173XXXXX40
            string maskstring = "";
            for (int i = 1; i <= 5; i++)
                maskstring += maskchar;
            return accountnumber.Substring(0, 3) + maskstring + accountnumber.Substring(7);
        }

        public static string GetHelpLineNumber(int userCountryId)
        {
            string helplinenumber = string.Empty;

            if (userCountryId != 0)
            {
                switch (userCountryId)
                {
                    case 1:
                        helplinenumber = "1-(877) 463-4233";
                        break;
                    case 2:
                        helplinenumber = "1-(800) 550-3501";
                        break;
                    case 3:
                        helplinenumber = "+44-(800) 520-0329"; ;
                        break;
                    default:
                        helplinenumber = "1-(877) 463-4233"; ;
                        break;
                }
            }
            else
            {
                helplinenumber = "1-(877) 463-4233";
            }
            return helplinenumber;
        }

        public static bool CheckForPinlessNumber(int planid)
        {
            var list = new List<int>
            {
                175,176,177,178,179,180
            };
            if (list.Exists(a => a == planid))
            {
                return true;
            }
            return false;
        }

        public static string CheckMandatoryAutorefill(int planid)
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

            //am is for autorefill mandatory
            //mp is for monthly plans.
            var autorefillmandatoryplan = new List<int>
            {
                163,164,120,121
            };
            var monthlyrefillplan = new List<int>
            {
                176,178,180,175,177,179
            };
            if (autorefillmandatoryplan.Exists(a => a == planid))
            {
                return "am";
            }
            else if (monthlyrefillplan.Exists(a => a == planid))
            {
                return "mp";
            }
            return "nm";
        }

        public static string GetMyAccoutMessage(string rf = "")
        {
            string message = string.Empty;
            switch (rf)
            {
                case "e":
                    message = "You can buy only one premium plan. Please recharge your existing plan.";
                    break;
                case "ai":
                    message = "Your plan is already issued, Please check your order history.";
                    break;
                case "ar":
                    message = "Your plan is already recharged, Please check your order history.";
                    break;
            }

            return message;
        }

        public static List<ExpDateEntity> GetMonthList()
        {
            var list = new List<ExpDateEntity>();
            list.Add(new ExpDateEntity() { Text = "Jan", Value = "01" });
            list.Add(new ExpDateEntity() { Text = "Feb", Value = "02" });
            list.Add(new ExpDateEntity() { Text = "March", Value = "03" });
            list.Add(new ExpDateEntity() { Text = "April", Value = "04" });
            list.Add(new ExpDateEntity() { Text = "May", Value = "05" });
            list.Add(new ExpDateEntity() { Text = "June", Value = "06" });
            list.Add(new ExpDateEntity() { Text = "July", Value = "07" });
            list.Add(new ExpDateEntity() { Text = "Aug", Value = "08" });
            list.Add(new ExpDateEntity() { Text = "Sep", Value = "09" });
            list.Add(new ExpDateEntity() { Text = "Oct", Value = "10" });
            list.Add(new ExpDateEntity() { Text = "Nov", Value = "11" });
            list.Add(new ExpDateEntity() { Text = "Dec", Value = "12" });

            return list;
        }

        public static List<ExpDateEntity> GetYearList()
        {
            var list = new List<ExpDateEntity>();
            var currentyear = DateTime.Now.Year;
            for (var i = 0; i <= 10; i++)
            {
                list.Add(new ExpDateEntity()
                {
                    Text = SafeConvert.ToString(currentyear + i),
                    Value = SafeConvert.ToString(currentyear + i).Remove(0,2),
                });
            }

            return list;
        }















        //****************************** THESE METHODS ARE ADDED ON 01/19/15 TO REDIRECT OLD URLS TO NEW ONES *********************************
        //used from http://www.bloggersworld.com/index.php/redirecting-old-urls-to-new-in-aspnet-mvc/
        class RedirectRule
        {
            public string OldUrlContains;
            public string OldUrlContainsNot;
            public string NewUrl;

            public RedirectRule(string strOldUrlContains, string strOldUrlContainsNot, string strNewUrl)
            {
                OldUrlContains = strOldUrlContains;
                OldUrlContainsNot = strOldUrlContainsNot;
                NewUrl = strNewUrl;
            }
        };


        public class LegacyUrlRoute : RouteBase
        {
            RedirectRule[] RedirectRules =
        {            
            new RedirectRule("/signup.aspx", "", "/Account/LogOn?ReturnUrl=%2faccount%2fMyaccount"),
            new RedirectRule("/faq.aspx", "", "/support#tabs-2"),
            new RedirectRule("/mobile-apps/index.aspx", "", "/account/MobileApp"),
            new RedirectRule("/freetrial/index.aspx", "", "/FreeTrial"),
            new RedirectRule("/phonecards_offer/india_offer.aspx", "", "/Promotion/OneCentPlan"),
            new RedirectRule("/phonecards_offer/bangladesh_signup_offer.aspx", "", "/Promotion/BangladeshPromotion"),
            new RedirectRule("/phonecards_offer/pakistan_signup_offer.aspx", "", "/Promotion/Pakistan49Promotion"),
        };


            public override RouteData GetRouteData(HttpContextBase httpContext)
            {
                const string status = "301 Moved Permanently";
                var request = httpContext.Request;
                var response = httpContext.Response;
                var legacyUrl = request.Url.ToString();
                var newUrl = "";

                foreach (RedirectRule rule in RedirectRules)
                {
                    //if we don't have to check for a string that does not exist in the url
                    if (rule.OldUrlContainsNot.Length == 0)
                    {

                        //this does a case insensitive comparison
                        if (legacyUrl.IndexOf(rule.OldUrlContains, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            newUrl = rule.NewUrl;
                        }
                    }
                    else
                    {
                        //if we don't have to check for a string that does not exist in the url
                        if ((legacyUrl.IndexOf(rule.OldUrlContains, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            //so that it doesn't go in infinite loop since the end part of both urls are same
                            && (!(legacyUrl.IndexOf(rule.OldUrlContainsNot, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)))
                        {
                            newUrl = rule.NewUrl;
                        }
                    }

                    //found anything?
                    if (newUrl.Length > 0)
                    {
                        break;
                    }

                }


                if (newUrl.Length > 0)
                {
                    response.Status = status;
                    response.RedirectLocation = newUrl;
                    response.End();
                }

                return null;
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext,
                        RouteValueDictionary values)
            {
                return null;
            }
        }
        //****************************** END OF CODING FOR URL REDIRECTION ******************************************

    }
}