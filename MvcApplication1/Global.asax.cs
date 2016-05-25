using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using MvcApplication1.Models;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using System.Web.Optimization;
using System.Data;
using MvcApplication1.App_Start;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Controllers;
using Raza.Model.Helpers;


namespace MvcApplication1
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           //Register all AutoMapper Mappings
            AutoMapperConfig.RegisterMappings();
            ModelBinders.Binders.Add(typeof (UpdateCreditCardViewModel), new CustomModelBinder());
            ModelBinders.Binders.Add(typeof(CartPaymentInfoViewModel), new CustomModelBinder());

            System.Diagnostics.Trace.Listeners.Add(new ConsoleTraceListener());


            #region LoadBanner for SearchRatePage
            //Load maps
            string filepath = Server.MapPath(@"/Content/CountriesBanner.xml");

            DataSet ds = new DataSet();
            ds.ReadXml(filepath);
            SharedResources.CountriesMap = new Dictionary<int, string>();


            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (!SharedResources.CountriesMap.ContainsKey(Convert.ToInt32(dr[0])))
                    {
                        SharedResources.CountriesMap.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                    }
                    else
                    {
                        SharedResources.CountriesMap[Convert.ToInt32(dr[0])] = dr[1].ToString();
                    }
                }
            }

            #endregion

            #region Loading Message Factory

            filepath = Server.MapPath(@"/Content/AppMessages.xml");
            DataSet ds1 = new DataSet();
            ds1.ReadXml(filepath);
            MessageFactory.InitilizeFactory(ds1);

            #endregion

        }



        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }



        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                CustomPrincipalSerializeModel serializeModel =
                    serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);


                RazaPrincipal newUser = new RazaPrincipal(authTicket.Name)
                {
                    MemberId = serializeModel.MemberId,
                    Email = serializeModel.Email,
                    IsEmailSubscribed = serializeModel.IsEmailSubscribed,
                    Pin = serializeModel.Pin,
                    UserType = serializeModel.UserType
                };

                if (serializeModel.ProfileInfo == null)
                {
                    var repositry = new DataRepository();
                    var bi = repositry.GetBillingInfo(serializeModel.MemberId);
                    newUser.ProfileInfo = bi;
                }
                else
                {
                    newUser.ProfileInfo = serializeModel.ProfileInfo;
                }

                HttpContext.Current.User = newUser;
            }

        }

    }


    public static class SharedResources
    {
        public static System.Collections.Generic.Dictionary<int, string> CountriesMap { get; set; }
    }

}