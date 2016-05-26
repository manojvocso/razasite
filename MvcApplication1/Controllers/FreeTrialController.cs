using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.App_Start;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Model;
using Raza.Repository;

namespace MvcApplication1.Controllers
{
    public class FreeTrialController : Controller
    {
        //
        // GET: /FreeTrial/


        [CompressFilter]
        public ActionResult Index()
        {
            var model = new GenericModel();
            //model.CountrybyIp = (int)Session["CountrybyIp"];
            return View(model);
        }



        public ActionResult GetFreeTrial()
        {
            //return View();

            string returnUrl = "http://www.raza.com";
            //return RedirectPermanent(returnUrl);
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.ToString().ToLower().Contains("sajha.com"))
                    return RedirectToAction("Nepal69Promotion", "Promotion");
                else
                    return RedirectToAction("Index", "Account");
            }
            else
                return RedirectToAction("Index", "Account");
        }



        public ActionResult Testing()
        {
            var model = new GenericModel();
            //model.CountrybyIp = (int)Session["CountrybyIp"];
            return View(model);
        }

    }
}
