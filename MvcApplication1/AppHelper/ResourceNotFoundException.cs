using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Raza.Common;

namespace MvcApplication1.AppHelper
{
    public class ResourceNotFoundException : Exception
    {

    }

    public class AuthSessionExpiredException : Exception
    {

    }

    public class HandleResourceNotFoundAttribute : FilterAttribute, IExceptionFilter
    {
        //THIS CODE WAS COMMENTED ON 02/08/2016 TO FIX THE ERRROR ON EVENT LOG
        //public void OnException(ExceptionContext filterContext)
        //{
        //    Controller controller = filterContext.Controller as Controller;
        //    if (controller == null || filterContext.ExceptionHandled)
        //        return;

        //    Exception exception = filterContext.Exception;
        //    if (exception == null)
        //        return;

        //    // Action method exceptions will be wrapped in a
        //    // TargetInvocationException since they're invoked using 
        //    // reflection, so we have to unwrap it.
        //    if (exception is TargetInvocationException)
        //        exception = exception.InnerException;

        //    // If this is not a ResourceNotFoundException error, ignore it.
        //    string viewName = "_Error404";
        //    if ((exception is AuthSessionExpiredException))
        //    {
        //        viewName = "_SessionExpiredError";
        //    }

        //    //RazaLogger.WriteException("----------------------Exception Start----------------------");
        //    //RazaLogger.WriteException("Request Url: " + filterContext.HttpContext.Request.Url);
        //    //RazaLogger.WriteException("Message: " + exception.Message);
        //    //RazaLogger.WriteException("Inner Exception: " + exception.InnerException);
        //    //RazaLogger.WriteException("----------------------Exception End----------------------");

        //    filterContext.Result = new ViewResult()
        //    {
        //        TempData = controller.TempData,
        //        ViewName = viewName
        //    };

        //    filterContext.ExceptionHandled = true;
        //    filterContext.HttpContext.Response.Clear();
        //    filterContext.HttpContext.Response.StatusCode = 404;
        //}




        public void OnException(ExceptionContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            if (controller == null || filterContext.ExceptionHandled)
                return;

            Exception exception = filterContext.Exception;
            if (exception == null)
                return;

            // Action method exceptions will be wrapped in a
            // TargetInvocationException since they're invoked using 
            // reflection, so we have to unwrap it.
            if (exception is TargetInvocationException)
                exception = exception.InnerException;

            // If this is not a ResourceNotFoundException error, ignore it.
            string viewName = "_Error404";
            if ((exception is AuthSessionExpiredException))
            {
                viewName = "_SessionExpiredError";
            }

            filterContext.Result = new ViewResult()
            {
                TempData = controller.TempData,
                ViewName = viewName
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 404;

        }








        //public void OnException(ExceptionContext filterContext)
        //{
        //    Controller controller = filterContext.Controller as Controller;
        //    if (controller == null || filterContext.ExceptionHandled)
        //        return;

        //    Exception exception = filterContext.Exception;
        //    if (exception == null)
        //        return;

        //    // Action method exceptions will be wrapped in a
        //    // TargetInvocationException since they're invoked using 
        //    // reflection, so we have to unwrap it.
        //    if (exception is TargetInvocationException)
        //        exception = exception.InnerException;

        //    // If this is not a ResourceNotFoundException error, ignore it.
        //    if (!(exception is ResourceNotFoundException))
        //        return;


        //    filterContext.Result = new ViewResult()
        //    {
        //        TempData = controller.TempData,
        //        ViewName = "_Error404"
        //    };

        //    filterContext.ExceptionHandled = true;
        //    filterContext.HttpContext.Response.Clear();
        //    filterContext.HttpContext.Response.StatusCode = 404;
        //}
    }

}