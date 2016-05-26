using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    [UnRequiresSSL]
    public class WhyRazaController : BaseController
    {
        private DataRepository _repository = new DataRepository();
        //
        // GET: /WhyRaza/

        [CompressFilter]
        public ActionResult Index()
        {
            WhyRaza raza = new WhyRaza();
            if (User.Identity.IsAuthenticated)
            {
                 raza.Email = UserContext.Email;
            }
            return View(raza);
        }

        public JsonResult ReferFreind(WhyRaza raza)

        {
            string MemberID = UserContext.MemberId;
            string ip_address = Request.UserHostAddress;
            var Mails = new List<string>();
            var data = new List<string>();

            var model = new WhyRaza();
            if (raza.mail1 != null && raza.mail1 != "undefined")
            {
                Mails.Add(raza.mail1);
            }
            if (raza.mail2 != null && raza.mail2 != "undefined")
            {
                Mails.Add(raza.mail2);    
            }
            if (raza.mail3 != null && raza.mail3 != "undefined")
            {
                Mails.Add(raza.mail3);
            }
            if (raza.mail4 != null && raza.mail4 != "undefined")
            {
                Mails.Add(raza.mail4);
            }
            if (raza.mail5 != null && raza.mail5 != "undefined")
            {
                Mails.Add(raza.mail5);
            }

            var succcessreferlist = new List<string>();
            model.ReferFriendName = Mails;
            foreach (var friendmail in Mails)
            {
                
                if (friendmail != "undefined")
                {
                  
                    var result = _repository.ReferFriendEmail(MemberID, friendmail, ip_address);
                    if (result)
                    {
                    
                        data.Add("Your Mail Successfully Sent to:" + friendmail);
                        succcessreferlist.Add(friendmail);
                        SendEmailstofriend(friendmail);
                    }
                    else
                    {
                        data.Add("These are Already Our Customer(s):" + friendmail);
                    }

                    model.Mail = data;

                }
            }
            model.Msg = "Thank You for referring your friends !!";
            SendEmailsRefree(succcessreferlist);

                return Json(model, JsonRequestBehavior.AllowGet);
            
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

        private void SendEmailsRefree(List<string> list )
        {
          
            if (!UserContext.IsEmailSubscribed) return;

            string email = UserContext.Email;
            string redirectlink = "Account/MyAccount";
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


            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/refer-fried-referee.html"));

                string servername = ConfigurationManager.AppSettings["ServerName"];
                string refermails = string.Empty;
                int i = 1;
                foreach (var mail in list)
                {
                    refermails = refermails + "<strong>" + i + "</strong>  <strong>" + mail + "</strong><br />";
                    i++;
                }

                mailbody = mailbody.Replace(@"<!--Refermails-->", refermails);
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);


                mailbody = mailbody.Replace(@"<!--EmailId-->", UserContext.Email);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Refer-friend-refree"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending mail to Refer a friend Refree: " + ex.Message);
            }


        }

        private void SendEmailstofriend(string referemail)
        {

            string email = referemail;
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
            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/refer-a-friend.html"));
               
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);


                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Refer-friends"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending mail to Refer a friend: " + ex.Message);
            }


        }

    }
}
