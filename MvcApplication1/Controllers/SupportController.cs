
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    [UnRequiresSSL]
    public class SupportController : BaseController
    {
        private DataRepository _repository = new DataRepository();
        //
        // GET: /Support/

        [CompressFilter]
        [HttpGet]
        public ActionResult index()
        {

            var complaintModel = new OpenComplaintModel();

            if (User.Identity.IsAuthenticated)
            {

                complaintModel.MemberID = UserContext.MemberId;
                complaintModel.FirstName = UserContext.FirstName;
                complaintModel.LastName = UserContext.LastName;
                complaintModel.Email = UserContext.Email;
            }

            complaintModel.CountryFromList = CacheManager.Instance.GetFromCountries().OrderBy(x => Convert.ToInt32(x.Id)).ToList();
            var list = new List<Country>();


            //complaintModel.AllCountryList = complaintModel.CountryFromList.Concat(complaintModel.CountryToList).ToList();


            var PlansList = new List<PlanInfo>();

            if (User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(UserContext.MemberId))
                {

                    var result = _repository.GetCustomerPlanList(UserContext.MemberId);

                    foreach (var info in result.OrderInfos)
                    {
                        PlansList.Add(
                            new PlanInfo()
                            {
                                PlanName = info.PlanName,
                                Pin = info.AccountNumber,
                                OrderId = info.OrderId
                            });

                    }
                    complaintModel.PlansList = PlansList;
                }

            }

            return View(complaintModel);

        }

        [HttpGet]
        public JsonResult GetStates(string id)
        {
            var res = _repository.GetStateList(SafeConvert.ToInt32(id));

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        /* [HttpPost]
         public ActionResult index(OpenComplaintModel model)
         {
             if (!User.Identity.IsAuthenticated)
             {
                 FormsAuthentication.RedirectToLoginPage();
             }

             var complaintModel = new OpenComplaintModel();

             BillingInfo billInf = _repository.GetBillingInfo(UserContext.MemberId);
             complaintModel.MemberID = billInf.MemberId;
             complaintModel.FirstName = billInf.FirstName;
             complaintModel.LastName = billInf.LastName;
             complaintModel.Email = billInf.Email;
             var fromCountries = _repository.GetCountryList("From");
             var toCountries = _repository.GetCountryList("To");
             complaintModel.CountryFromList = fromCountries;
             complaintModel.CountryToList = toCountries;
             complaintModel.AllCountryList = fromCountries.Concat(toCountries).ToList();
             var fs = new List<State>();
             var repository = new DataRepository();
             var data = repository.GetCountryList("from");
             ViewBag.Cont =
                 new SelectList(Helpers.ConvertCountryModelListToCountryList(repository.GetCountryList("From")));

             foreach (var res in data)
             {

                 foreach (var dat in repository.GetStateList(Convert.ToInt16(res.Id)))
                 {

                     fs.Add(dat);

                 }
             }


             complaintModel.StateList = fs;




             var PlansList = new List<PlanInfo>();

             if (!String.IsNullOrEmpty(UserContext.MemberId))
             {

                 var result = _repository.GetCustomerPlanList(UserContext.MemberId);

                 foreach (var info in result.OrderInfos)
                 {
                     PlansList.Add(
                         new PlanInfo()
                         {
                             PlanName = info.PlanName,
                             Pin = info.AccountNumber,
                             OrderId = info.OrderId
                         });

                 }
                 complaintModel.PlansList = PlansList;
             }
             //else
             //{
             //    //FormsAuthentication.RedirectToLoginPage();
             //    //return RedirectToAction("Login");
             //    //Response.Redirect("Login");
             //}

             return View(complaintModel);
         }*/


        [HttpPost]
        [Authorize]
        public JsonResult ComplaintSubmit(OpenComplaintModel model)
        {
            string MemberID = UserContext.MemberId;
            string ContactPhone = model.ContactPhone;
            string order_id = model.OrderId;
            string access_number = model.ContactPhone;
            string Destination_Number = model.Destination_Number;
            int countryfrom = model.CountryFrom;
            int countryto = model.CountryTo;
            string desc = model.Description;
            string notes = model.Notes;

            var response = _repository.New_complaint(MemberID, ContactPhone, order_id,
                                                        access_number, Destination_Number,
                                                        countryfrom, countryto,
                                                        desc, notes);
            if (response.Status)
            {
                SendCompalintMail(response.Errormsg);
                return
                    Json(
                        new
                        {
                            status = true,
                            message = response.Errormsg
                        });

            }

            return Json(new
            {
                status = false,
                message = response.Errormsg
            });

        }

        //[HttpPost]
        //public ActionResult index(int countryto, int countryfrom, string Destination_Number, string notes, string access_number, string order_id, string desc)
        //{
        //    OpenComplaintModel complaintModel=new OpenComplaintModel();
        //    complaintModel.MemberID = UserContext.MemberId;
        //    string response = _repository.New_complaint(complaintModel.MemberID, complaintModel.ContactPhone, order_id,
        //                                              access_number, Destination_Number,
        //                                              countryfrom, countryto,
        //                                              desc, notes);
        //    if (response != "")
        //    {
        //        ViewBag.compid = response;
        //        return Json(new { success = true });

        //    }
        //    return Json(new {success=false});
        //}
        [Authorize]
        public JsonResult GetComplaintData()
        {
            var model = new OpenComplaintModel
            {
                CountryFromList = CacheManager.Instance.GetFromCountries().OrderBy(x => Convert.ToInt32(x.Id)).Take(3).ToList(),
                CountryToList = CacheManager.Instance.GetCountryListTo()
            };
            var PlansList = new List<PlanInfo>();
            var fill = _repository.GetCustomerPlanList(UserContext.MemberId);
            foreach (var info in fill.OrderInfos)
            {
                PlansList.Add(
                    new PlanInfo() { PlanName = info.PlanName, Pin = info.AccountNumber, OrderId = info.OrderId });

            }
            model.PlansList = PlansList;

            BillingInfo billInf = _repository.GetBillingInfo(UserContext.MemberId);
            model.MemberID = billInf.MemberId;
            model.FirstName = billInf.FirstName;
            model.LastName = billInf.LastName;
            model.Email = billInf.Email;
            model.ContactPhone = billInf.PhoneNumber;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetLocalAccessNumber(string phoneNumber, string countryId, string state)
        {
            string statecode;

            if (state == "undefined" || state == "")
            {
                statecode = null;
            }
            else
            {
                statecode = state;

            }
            string searchType = "A";
            if (phoneNumber.Length == 0)
            {
                searchType = "S";
            }
            try
            {
                var data = _repository.GetLocalAccessNumber(statecode, countryId, phoneNumber, searchType);
                if (data.Count == 0)
                {
                    List<LocalAccessNumber> list = new List<LocalAccessNumber>();
                    list.Add(
                        new LocalAccessNumber()
                            {

                                AccessNumber = "1-877-777-3971",
                                City = "Any City",

                                SNo = "1. (U.S.A.)",
                                State = "Any State"
                            });

                    list.Add(
                      new LocalAccessNumber()
                      {

                          AccessNumber = "1-877-777-1674",
                          City = "Any City",

                          SNo = "2. (CANADA)",
                          State = "Any State"
                      });
                    list.Add(
                     new LocalAccessNumber()
                     {

                         AccessNumber = "1-866-397-2880",
                         City = "Any City",

                         SNo = "3.",
                         State = "HAWAII"
                     });
                    list.Add(
                     new LocalAccessNumber()
                     {

                         AccessNumber = "1-866-397-2880",
                         City = "Any City",

                         SNo = "4.",
                         State = "PUERTO RICO"
                     });
                    list.Add(
                     new LocalAccessNumber()
                     {

                         AccessNumber = "1-866-397-2880",
                         City = "Any City",

                         SNo = "5.",
                         State = "US VIRGIN ISLANDS"
                     });

                    data = list;
                }


                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ValidationError = true, ValidationMessage = ex.Message });
            }

        }

        public JsonResult CustomerFeedback(SaveFeedback model)
        {
            bool isRazaCust = model.IsRazaCustomer;
            string feedType = model.FeedbackType;
            string firstname = model.FirstName;
            string lastname = model.LastName;
            string email = model.EmailAddress;
            string feedBack = model.Feedback;
            string phone = model.PhoneNumber;
            var result = _repository.Feedback(isRazaCust, feedType, firstname, lastname, phone, email, feedBack);

            if (result.Status)
            {
                SendFeedbackMail(model);
                SendFeedbacktoRaza(model);
                return Json(new { message = "Your feedback has been submitted." });
            }
            else
            {
                return Json(new { message = result.Errormsg });
            }

        }

        private void SendFeedbackMail(SaveFeedback model)
        {
            string email = model.EmailAddress;
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = string.Empty;
            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                redirectlink = "Account/MyAccount";
            }
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

        private void SendFeedbacktoRaza(SaveFeedback model)
        {
            //string razarecipent = "azim@raza.com,sabal@raza.com";
            string razarecipent = "qc@raza.com,sabal@raza.com";
            string[] allrows = razarecipent.Split(',');
            string servername = ConfigurationManager.AppSettings["ServerName"];
            string redirectlink = string.Empty;

            if (UserContext != null && UserContext.UserType.ToLower() == "old")
            {
                redirectlink = "Account/MyAccount";
            }
            string userfeedback = "<strong>Dear Raza</strong> <br/><br/>" + model.Feedback + "<br/><br/>" + model.FirstName + " " + model.LastName +
                                  "<br/>" + model.EmailAddress + "<br/>" + model.PhoneNumber;
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

        private void SendCompalintMail(string complaintNumber)
        {
            RazaLogger.WriteInfo("Sending complaint mail to user and complaint id is:" + complaintNumber);
            string email = UserContext.Email;
            string servername = ConfigurationManager.AppSettings["ServerName"];
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


    }


}
