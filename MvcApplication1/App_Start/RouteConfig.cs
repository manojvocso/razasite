using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcApplication1.App_Start;

namespace MvcApplication1
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region MOST IMPORTANT ROUTE MAPING

            routes.MapRoute("IndiaRate",
"India", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("IndiaRate1",
"India-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            //            routes.MapRoute("PakistanRate",
            //"Pakistan", new { controller = "Rates", action = "Pakistan", id = UrlParameter.Optional });

            routes.MapRoute("PakistanRate",
"Pakistan", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("PakistanRate1",
"Pakistan-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("GhanaRate",
"Ghana", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("GhanaRate1",
"Ghana-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("AfghanistanRate",
"Afghanistan", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("AfghanistanRate1",
"Afghanistan-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("NepalRate",
"Nepal", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("NepalRate1",
"Nepal-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("BangladeshRate",
"Bangladesh", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("BangladeshRate1",
"Bangladesh-Phone-Card", new { controller = "Rates", action = "SearchRate_Generic" });

            routes.MapRoute("SelfieContestPage",
"Selfie", new { controller = "Promotion", action = "SelfieContest" }, namespaces: new[] { "MvcApplication1.Controllers" });

            routes.MapRoute("AmvrinReRoute",
"Rate/search-calling-card", new { controller = "Rates", action = "SearchRate_Amvrin" });

            //// Show a 404 error page for anything else.
            //routes.MapRoute("Error", "{*url}",
            //    new { controller = "Error", action = "404" }
            //);


            #endregion

            routes.Add(new Helpers.LegacyUrlRoute());

           // routes.Add(new MyCustomRoute());
            routes.MapRoute(
                "Default",
                // Route name
                "{controller}/{action}/{id}",
                // URL with parameters
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "MvcApplication1.Controllers" } //namespace
                );
        }

    }
}