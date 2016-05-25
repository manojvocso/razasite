using System.Web.Mvc;
using System.Web.Security;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Raza.Model;
using Raza.Repository;
using MvcApplication1.App_Start;

namespace MvcApplication1.Controllers
{
    public class FeaturesController : BaseController
    {
        private DataRepository _repository = new DataRepository();

        [CompressFilter]
        [HttpGet]
        [UnRequiresSSL]
        public ActionResult Index()
        {
            var data = new Features
            {
                CountryTo = CacheManager.Instance.GetCountryListTo(),
                CountryFrom = CacheManager.Instance.GetFromCountries()
            };

            if (User.Identity.IsAuthenticated)
            {
                data.Email = UserContext.Email;
            }

            return View(data);
        }

        [HttpPost]
        [UnRequiresSSL]
        public JsonResult RazaRewardSignup(string Email, string Password, string Month, string Date, string Year)
        {


            var context = _repository.Authenticate(Email, Password);
            if (context == null)
            {
                return Json(new { result = "Error: Invalid login information." });
            }
            string dateOfBirth = string.Format("{0}-{1}-{2}", Month, Date, Year);

            var result = _repository.RewardSignup(context.MemberId, dateOfBirth);

            if (result.Status)
            {
                return Json(new { result = "You are registered to raza bonus points program." });

            }
            else
            {
                if (result.Errormsg == "already exists")
                {
                    return Json(new { result = "You are already registered to raza bonus points program." });
                }
              
            }
            return Json(new { result = "Error: Invalid login information." });
        }


    }
}
