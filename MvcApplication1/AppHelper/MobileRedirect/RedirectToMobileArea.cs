using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.AppHelper.MobileRedirect
{
    public class RedirectToMobileAreaAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Controller controller = filterContext.Controller as Controller;
            //code here for redirect on the base of first request and for mobile device...
            if (filterContext.RequestContext.HttpContext.Session != null && filterContext.RequestContext.HttpContext.Session.IsNewSession)
            {
              //  filterContext.Result = new RedirectResult("/Mobile/Account/Index");
            }

        }
    }
}