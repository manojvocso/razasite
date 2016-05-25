using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Areas.Mobile.ViewModels;
using MvcApplication1.Controllers;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.Controllers
{

    public class UiControlsController : BaseController
    {
        [ChildActionOnly]
        [OutputCache(Duration = 86400)]
        public ActionResult CountryToSemanticSearchControl()
        {
            var data = CacheManager.Instance.GetAllCountryTo();
            return PartialView("CountryToSemanticSearchControl", data);
        }

        [ChildActionOnly]
        public ActionResult HomeSearchBox()
        {
            var model = new UiHomeSearchBoxViewModel();
            try
            {
                //var data = CacheManager.Instance.GetAllCountriesWithLowestRates() ??
                //           new List<CountryWithLowestRateModel>();
                //model.CallingToCountriesWithLowestRate = data;
                model.CallingToCountries = CacheManager.Instance.GetCountryListTo();
            }
            catch (Exception ex)
            {
                return null;
            }

            model.CallingFromCountries = CacheManager.Instance.GetFromCountries();
            
            return PartialView("HomeSearchBox", model);


        }

        [HttpGet]
        public ActionResult InsertUpdateCreditCard(string creditCardId = "")
        {
            var model = new UpdateCreditCardViewModel();
            return PartialView("InsertUpdateCreditCard", model);
        }

        [HttpPost]
        public ActionResult InsertUpdateCreditCard(UpdateCreditCardViewModel model)
        {
            return View("InsertUpdateCreditCard", model);
        }


        [OutputCache(Duration = 86400)]
        [ChildActionOnly]
        public ActionResult MobileFooter()
        {
            return PartialView("_MobileFooter");
        }

        [ChildActionOnly]
        public ActionResult LeftSideMenu()
        {
            return PartialView("_LeftSideMenu");
        }

        public ActionResult HomePromotionalPlansSlider()
        {
            return PartialView("_HomePromotionalPlansSlider");
        }
    }


}
