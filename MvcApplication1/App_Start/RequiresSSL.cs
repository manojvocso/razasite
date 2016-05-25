﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.App_Start
{
    public class RequiresSSL : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if we're secure or not and if we're on the local box
            if (!req.IsSecureConnection && !req.IsLocal)
            {
                var builder = new UriBuilder(req.Url)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443
                };
                res.Redirect(builder.Uri.ToString());
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class UnRequiresSSL : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if we're secure or not and if we're on the local box
            if (req.IsSecureConnection && !req.IsLocal)
            {
                var builder = new UriBuilder(req.Url)
                {
                    Scheme = Uri.UriSchemeHttp,
                    Port = 80
                };
                res.Redirect(builder.Uri.ToString());
            }
            base.OnActionExecuting(filterContext);
        }
    }
}