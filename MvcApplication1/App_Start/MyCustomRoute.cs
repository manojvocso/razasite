using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication1.App_Start
{
    public class MyCustomRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData mobileRoute = new RouteData(this, new MvcRouteHandler());
            mobileRoute.Values.Add("name", "Default");
            mobileRoute.Values.Add("url", "{controller}/{action}/{id}");
            mobileRoute.Values.Add("controller", "Account");
            mobileRoute.Values.Add("action", "Index");
            mobileRoute.DataTokens.Add("namespaces", new[] { "MvcApplication1.Areas.Mobile.Controllers" });
            mobileRoute.DataTokens["area"] = "Mobile";

            return mobileRoute;



            string url = httpContext.Request.AppRelativeCurrentExecutionFilePath;

            //check null for URL
            const string defaultcontrollername = "Home";
            string[] spliturl = url.Split("//".ToCharArray());
            string controllername = String.Empty;
            string actionname = "Index";
            string area = string.Empty;
            string areaNamespace = "MvcApplication5.Controllers";

            string strUserAgent = httpContext.Request.UserAgent;
            Regex strBrowser = new Regex(@"android.+mobile|blackberry|ip(hone|od)|android.+tab", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((strBrowser.IsMatch(strUserAgent)))
            {
                area = "Mobile";
                areaNamespace = "MvcApplication5.Areas.Mobile.Controllers";
            }


            if (spliturl.Length == 2) //for ~/home.aspx and ~/ 
            {
                if (String.IsNullOrEmpty(spliturl[1])) //TODO:  http://localhost:57282/ not working - to make it working
                {
                    controllername = defaultcontrollername;
                }
                else
                {
                    controllername = spliturl[1];
                    if (controllername.Contains("."))
                    {
                        controllername = controllername.Substring(0, controllername.LastIndexOf("."));
                    }
                }
            }
            else if (spliturl.Length == 3) // For #/home/index.aspx and /home/about
            {
                controllername = spliturl[1];
                actionname = spliturl[2];
                if (actionname.Contains("."))
                {
                    actionname = actionname.Substring(0, actionname.LastIndexOf("."));
                }
            }
            else //final block in final case sned it to Home Controller
            {
                controllername = defaultcontrollername;
                actionname = "Index";
            }




            RouteData rd = new RouteData(this, new MvcRouteHandler());
            rd.Values.Add("name", "Default");
            rd.Values.Add("url", "{controller}/{action}/{id}");
            rd.Values.Add("controller", controllername);
            rd.Values.Add("action", actionname);
            rd.DataTokens.Add("namespaces", new[] { areaNamespace });
            rd.DataTokens["area"] = area;

            return rd;

        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }

        public class RequestSwitcherAttribute : ActionFilterAttribute
        {
            bool _isEnableForMobileRedirect = ConfigurationManager.AppSettings["IsEnableForMobileRedirect"] == "Y";

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var context = filterContext.HttpContext;
                var request = context.Request;


                if (_isEnableForMobileRedirect)
                {
                    if (request.Path.ToLower().Contains("ccverifier") || request.Path.ToLower().StartsWith("/payment"))
                    {
                        Raza.Common.RazaLogger.WriteErrorForMobile("My Custom route cc verifier");
                        return;
                    }


                    string strUserAgent = HttpContext.Current.Request.UserAgent ?? String.Empty;
                    //Regex strBrowser = new Regex(@"android.+mobile|blackberry|ip(hone|od)|android.+tab",
                    //    RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Regex strBrowser = new Regex(@"android.+mobile|blackberry|ip(hone|od|ad)|android.+tab|Android|Tablet|tablet|tab",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    bool isViewSet = (HttpContext.Current.Session["SiteView"] as string ?? String.Empty) == "Y";
                    
                    if(isViewSet)
                        return;

                    if (strBrowser.IsMatch(strUserAgent) && !request.Path.ToLower().StartsWith("/mobile")) 
                    {
                        string url = "/Mobile";
                        HttpContext.Current.Session["SiteView"] = null;
                        filterContext.Result = new RedirectResult(url);
                    }
                    else if (!strBrowser.IsMatch(strUserAgent) && request.Path.ToLower().StartsWith("/mobile"))
                    {
                        string url = "/";
                        HttpContext.Current.Session["SiteView"] = null;
                        filterContext.Result = new RedirectResult(url);

                    }
                    
                }

            }
        }

    }



}
