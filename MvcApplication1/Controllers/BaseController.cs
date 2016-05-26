using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using MvcApplication1.Models;
using Raza.Repository;
using MvcApplication1.AppHelper;


namespace MvcApplication1.Controllers
{
    using System.Web.Security;

    using Raza.Model;


    public class BaseController : Controller
    {

        private RazaPrincipal _userContext;

        protected virtual RazaPrincipal UserContext
        {
            get { return _userContext; }
        }

        protected virtual UserContext Authenticate(string Email, string Password, bool rememberMe = false)
        {
            var repository = new DataRepository();

            var context = repository.Authenticate(Email, Password);
            // Session["Isnewuser"] = false;
            if (HttpContext.Session != null)
            {
                if (context != null && context.ServiceResponse != "customer blocked")
                {
                    // var data = repository.GetCustomerPlanList(context.MemberId);
                    if (context.UserType.ToLower() != "new")
                    {
                        HttpContext.Session["TryusFree"] = "notvalid";
                    }
                    else
                    {
                        HttpContext.Session["TryusFree"] = "valid";
                    }

                    //updating user default country to its billing info country

                    //********* COMMENTED AS PER MOHINDAR ON 10/21/2015 *************************
                    //HttpContext.Session["CountrybyIp"] = ControllerHelper.GetUserCountryId(context.ProfileInfo.Country).Id;
                }

                else
                {
                    HttpContext.Session["TryusFree"] = "valid";
                }
            }
            if (context == null)
                return null;

            UpdateUserContext(Email, context, rememberMe);

            //********* ADDED AS PER MOHINDAR ON 10/21/2015 *************************
            HttpContext.Session["CountrybyIp"] = ControllerHelper.GetUserCountryId(context.ProfileInfo.Country).Id;

            return context;
        }

        protected void UpdateUserContext(string Email, Raza.Model.UserContext context, bool rememberMe = false)
        {
            context.ProfileInfo.UserType = context.UserType;
            var serializeModel = new CustomPrincipalSerializeModel
            {
                MemberId = context.MemberId,
                FirstName = context.FirstName,
                LastName = context.LastName,
                IsEmailSubscribed = context.IsEmailSubscribed,
                Email = context.Email,
                UserType = context.UserType,
                ProfileInfo = context.ProfileInfo,
            };

            var serializer = new JavaScriptSerializer();

            string userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                1,
                Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(rememberMe ? 43200 : 20),
                rememberMe,
                userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                RedirectToAction("LogOn", "Account");
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.User != null)
            {
                _userContext = requestContext.HttpContext.User as RazaPrincipal;

            }
            if (requestContext.HttpContext.Session != null)
            {
                if (requestContext.HttpContext.Session["CountrybyIp"] == null)
                {
                    string ipaddress = requestContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    var country = GetLocationInfo(ipaddress);
                    if (country == 0)
                    {
                        country = 1;
                    }
                    string heplNumber = "1-(877) 463-4233";
                    requestContext.HttpContext.Session["CountrybyIp"] = country;
                    switch (country)
                    {
                        case 2:
                            heplNumber = "1-(800) 550-3501";
                            break;
                        case 3:
                            heplNumber = "+44-(800) 520-0329";
                            break;
                    }

                    requestContext.HttpContext.Session["HelpNumber"] = heplNumber;

                }

                if (requestContext.HttpContext.User != null)
                {
                    if (!requestContext.HttpContext.User.Identity.IsAuthenticated)
                    {
                        requestContext.HttpContext.Session["TryusFree"] = "valid";
                    }
                }
                else
                {
                    requestContext.HttpContext.Session["TryusFree"] = "valid";
                }
            }

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*string sApplicationPath = Server.MapPath("~");*/

            /*  LoadIpCountryTable(sApplicationPath + @"\IpAddress2\apnic.latest");
            LoadIpCountryTable(sApplicationPath + @"\IpAddress2\arin.latest");
            LoadIpCountryTable(sApplicationPath + @"\IpAddress2\lacnic.latest");
            LoadIpCountryTable(sApplicationPath + @"\IpAddress2\ripencc.latest");*/

            //if (Session["userContext"] != null)
            //{
            //    UserContext = Session["userContext"] as UserContext;
            //}
            //else
            //{
            //    UserContext = new UserContext();
            //}

            //   HttpSessionStateBase session = filterContext.HttpContext.Session;

            //if ((string.IsNullOrWhiteSpace(UserContext.MemberId) && !session.IsNewSession) || (session.IsNewSession))
            //{
            //    UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
            //    string loginUrl = urlHelper.Content("~/Account/LogOut");

            //    FormsAuthentication.SignOut();
            //  //  filterContext.HttpContext.Response.Redirect(loginUrl, true);
            //}

            string VisitorsIPAddr = string.Empty;
            //Users IP Address.                
            /*   if (filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR "] != null)
            {
                //To get the IP address of the machine and not the proxy
                VisitorsIPAddr = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            else if (filterContext.HttpContext.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = filterContext.HttpContext.Request.UserHostAddress;
            }*/


            if (filterContext.HttpContext.User != null)
            {
                _userContext = filterContext.HttpContext.User as RazaPrincipal;
            }

            /*   string VisitorsCountry = Ip2Country.GetCountry(VisitorsIPAddr);*/
            if (filterContext.HttpContext.User.Identity.IsAuthenticated
                && string.IsNullOrWhiteSpace(UserContext.MemberId))
            {
                FormsAuthentication.SignOut();
            }

            //   base.OnActionExecuting(filterContext);
        }

        /* public void LoadIpCountryTable(string name)
         {
             FileStream s = new FileStream(name, FileMode.Open);
             StreamReader sr = new StreamReader(s);
             Ip2Country.Load(sr);
             sr.Close();
             s.Close();
         }*/

        protected void DeleteCartSession()
        {
            Session[GlobalSetting.CheckOutSesionKey] = null;
        }


        private int GetLocationInfo(string ipaddress)
        {
            return 1;
            var repository = new DataRepository();
            try
            {
                //  var ipaddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];

                //string ipaddress = " 173.209.149.122";
                int usercountry = 1;
                if (ipaddress != "127.0.0.1")
                {
                    // IPAddress i = IPAddress.Parse(ipaddress);
                    string ip = ipaddress.ToString();

                    var response = repository.GetCustomerLocation(ip);

                    usercountry = SafeConvert.ToInt32(response);
                }


                return usercountry;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
                       {
                           Data = data,
                           ContentType = contentType,
                           ContentEncoding = contentEncoding,
                           JsonRequestBehavior = behavior
                       };

        }
    }
}
