using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication1.AppHelper
{
    public class MobileAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"customAuthRedirect", filterContext.RouteData.Values["customAuthRedirect"]},
                        {"controller", "Account"},
                        {"action", "Logon"},
                        {"area", "Mobile"},
                        {"ReturnUrl", HttpContext.Current.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl)}
                    });
            }
        }
    }
}