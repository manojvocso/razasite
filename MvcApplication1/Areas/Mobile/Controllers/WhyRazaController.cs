using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.App_Start;
using MvcApplication1.Compression;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    [UnRequiresSSL]
    [CompressFilter]
    public class WhyRazaController : Controller
    {
        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            return View();
        }

    }
}
