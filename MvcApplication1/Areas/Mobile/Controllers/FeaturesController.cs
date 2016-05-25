using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.App_Start;
using MvcApplication1.Compression;
using MvcApplication1.Controllers;

namespace MvcApplication1.Areas.Mobile.Controllers
{
    public class FeaturesController : BaseController
    {
        [UnRequiresSSL]
        [CompressFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
