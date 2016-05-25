using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MvcApplication1.AppHelper;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;
using System.Configuration;
using MvcApplication1.App_Start;
using Raza.Common;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    [CompressFilter]
    public class SupportController : BaseController
    {
        private readonly DataRepository _dataRepository;

        public SupportController()
        {
            _dataRepository = new DataRepository();
        }

        #region Private methods

        private List<SelectListItem> BindOpenComplaintModelPlanList()
        {
            var fill = _dataRepository.GetCustomerPlanList(UserContext.MemberId);
            return fill.OrderInfos.Select(info => new SelectListItem()
            {
                Value = info.OrderId,
                Text = info.PlanName
            }).DistinctBy(a => a.Text).ToList();
        }

        #endregion



        [HttpGet]
        [RequiresSSL]
        public ActionResult Feedback()
        {
            var model = new FeedbackViewModel();
            return View(model);
        }

        [HttpPost]
        [RequiresSSL]
        public ActionResult Feedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isRazaCustomer = User.Identity.IsAuthenticated;
                var result = _dataRepository.Feedback(isRazaCustomer, model.FeedbackType, model.FirstName,
                    model.LastName, model.PhoneNumber, model.Email, model.Feedback);
                ModelState.Clear();
                if (result.Status)
                {
                    //sent feedback mail to the user.... and to the raza..
                    SendFeedbackMail(model);
                    SendFeedbacktoRaza(model);

                    model = new FeedbackViewModel()
                    {
                        Message = "Your feedback has been submitted.",
                        MessageType = MessageType.Success
                    };

                }
                else
                {

                    model = new FeedbackViewModel()
                    {
                        Message = result.Errormsg,
                        MessageType = MessageType.Error
                    };
                }
            }
            return View(model);
        }

        [HttpGet]
        [MobileAuthorize]
        [RequiresSSL]
        public ActionResult OpenComplaint()
        {
            var model = new OpenComplaintViewModel
            {
                UserPlanList = BindOpenComplaintModelPlanList(),
                FirstName = UserContext.ProfileInfo.FirstName,
                LastName = UserContext.ProfileInfo.LastName,
                PhoneNumber = UserContext.ProfileInfo.PhoneNumber,
                EmailAddress = UserContext.ProfileInfo.Email,
                CountryFromList = CacheManager.Instance.GetTop3FromCountries(),
                CountryToList = CacheManager.Instance.GetCountryListTo()
            };
            return View(model);
        }


        [HttpPost]
        [MobileAuthorize]
        [RequiresSSL]
        public ActionResult OpenComplaint(OpenComplaintViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = _dataRepository.New_complaint(UserContext.MemberId, model.PhoneNumber, model.PlanOrderId,
                    model.PhoneNumber, model.DestinationNumber, SafeConvert.ToInt32(model.CallingFromCountry),
                    SafeConvert.ToInt32(model.CallingToCountry),
                    model.Description, model.Comment);

                ModelState.Clear();
                if (res.Status)
                {
                    model = new OpenComplaintViewModel()
                    {
                        FirstName = UserContext.ProfileInfo.FirstName,
                        LastName = UserContext.ProfileInfo.LastName,
                        PhoneNumber = UserContext.ProfileInfo.PhoneNumber,
                        EmailAddress = UserContext.ProfileInfo.Email,
                        MessageType = MessageType.Success,
                        Message = res.Errormsg
                    };
                    SendCompalintMail(res.Errormsg);
                }
                else
                {
                    model.MessageType = MessageType.Error;
                    model.Message = res.Errormsg;
                }
            }
            model.UserPlanList = BindOpenComplaintModelPlanList();
            model.CountryToList = CacheManager.Instance.GetAllCountryTo();
            model.CountryFromList = CacheManager.Instance.GetTop3FromCountries();

            return View(model);
        }

        [UnRequiresSSL]
        public ActionResult ContactUs()
        {
            return View();
        }

        [UnRequiresSSL]
        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult TermsAndCondition()
        {
            return View();
        }

        [UnRequiresSSL]
        [HttpGet]
        [MobileAuthorize]
        public ActionResult LocalAccessNumber()
        {
            var model = new LocalAccessNumberViewModel()
            {
                CountryFromList = ControllerHelper.GetLocalAccessFromCountryList()
            };
            return View(model);
        }

        [UnRequiresSSL]
        [HttpPost]
        [MobileAuthorize]
        public ActionResult LocalAccessNumber(LocalAccessNumberViewModel model)
        {
            if (ModelState.IsValid && (model.AccessCountry == "1" || model.AccessCountry == "2"))
            {
                model.LocalAccessNumbers = ControllerHelper.GetLocalAccessNumbers(model);
            }

            model.CountryFromList = ControllerHelper.GetLocalAccessFromCountryList();
            if (!string.IsNullOrEmpty(model.AccessCountry))
            {
                model.StateList = _dataRepository.GetStateList(SafeConvert.ToInt32(model.AccessCountry));
            }

            return View(model);
        }

        [UnRequiresSSL]
        public ActionResult MobileTopTermsAndConditions()
        {
            return View();
        }

        [UnRequiresSSL]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [UnRequiresSSL]
        public ActionResult BillingAndPayment()
        {
            return View();
        }

        [UnRequiresSSL]
        public ActionResult Security()
        {
            return View();
        }


        private void SendCompalintMail(string complaintNumber)
        {
            RazaLogger.WriteInfo("Sending complaint mail to user and complaint id is:" + complaintNumber);
            string email = UserContext.Email;
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = "Account/MyAccount";
            string helplinenumber;
            var usercountryInfo = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
            if (UserContext != null && UserContext.ProfileInfo != null)
            {
                helplinenumber = Helpers.GetHelpLineNumber(SafeConvert.ToInt32(usercountryInfo.Id));
            }
            else
            {
                helplinenumber = (string)Session["HelpNumber"];
            }
            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/open_complaint.html"));

                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--complaintNumber-->", complaintNumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["OpenComplaint"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in Sending Complaint Mail: " + ex.Message);
            }
        }

        private void SendFeedbackMail(FeedbackViewModel model)
        {
            string email = model.Email;
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = "Account/MyAccount";

            string helplinenumber;
            if (UserContext != null && UserContext.ProfileInfo != null)
            {
                var usercountryinfo = ControllerHelper.GetUserCountryId(UserContext.ProfileInfo.Country);
                helplinenumber = Helpers.GetHelpLineNumber(SafeConvert.ToInt32(usercountryinfo.Id));
            }
            else
            {
                helplinenumber = (string)Session["HelpNumber"];
            }
            try
            {
                string mailbody =
                    System.IO.File.ReadAllText(Server.MapPath(@"/Email-Temp/feedback.html"));
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Feedback"],
                    mailbody,
                    true);

            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in sending feedback mail to user: " + ex.Message);
            }

        }

        private void SendFeedbacktoRaza(FeedbackViewModel model)
        {
            string razarecipent = "qc@raza.com,sabal@raza.com";
            string[] allrows = razarecipent.Split(',');
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = string.Empty;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                redirectlink = "Account/MyAccount";
            }
            string userfeedback = "<strong>Dear Raza</strong> <br/><br/>" + model.Feedback + "<br/><br/>" + model.FirstName + " " + model.LastName +
                                  "<br/>" + model.Email + "<br/>" + model.PhoneNumber;
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
                        model.FeedbackType,
                        mailbody,
                        true, razarecipent);
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending feedbacktoraza mail to user: " + ex.Message);
            }

        }

    }
}
