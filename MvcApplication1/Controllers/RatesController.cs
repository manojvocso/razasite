using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Compression;
using MvcApplication1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using System.IO;
using System.Diagnostics;
using MvcApplication1.App_Start;
using System.Collections;

namespace MvcApplication1.Controllers
{

    [UnRequiresSSL]
    public class RatesController : BaseController
    {
        DataRepository _repository = new DataRepository();
        //
        // GET: /Rates/

        public ActionResult Index()
        {
            return View();
        }




        [CompressFilter]
        public ActionResult SearchRate(int countryfrom = 0, int countryto = 0, string mobileorGlobal = "",
            string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            if (countryfrom == 0)
            {
                //return RedirectToAction("Plans","Rate");
                return RedirectToAction("Plans", "Rate", new { whatever = "abc" });
            }

            var ratemodel = new RateModel();

            if (Request.QueryString["countryfrom"] != "")
            {
                ratemodel.CountryFromQuery = Convert.ToInt16(Request.QueryString["countryfrom"]);
                ratemodel.MobileOrGlobalPlan = Request.QueryString["MobileDiect"];
                ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
                    ? SharedResources.CountriesMap[countryto] + ".jpg"
                    : string.Empty;
            }

            ////******************************************* ADDED BY SABAL ON 10/27/2014 TO ADD DYNAMIC META TAGS *********************************************************
            //var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
            //var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            //ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name +" | Raza";
            //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
            //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
            ////***********************************************************************************************************************************************************


            //******************************************* ADDED BY SABAL ON 11/12/2014 TO ADD DYNAMIC META TAGS *********************************************************

            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag = data.RatePageMetaTags.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);

            foreach (var item in MyMetaTag)
            {
                ViewBag.Title = item.Title;
                ViewBag.PageDescription = item.PageDescription;
                ViewBag.PageKeywords = item.PageKeywords;
            }
            if (string.IsNullOrEmpty(ViewBag.Title))
            {
                var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
                var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

                ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name + " | Raza";
                //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
                ViewBag.PageDescription = "Raza: Call " + countrytodata.Name + " from " + countryfromdata.Name + " at attractive rates! Dedicated calling plans for landlines & mobile phones to help you SAVE money!";
                //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
                ViewBag.PageKeywords = "Calling Cards to " + countrytodata.Name + " from " + countryfromdata.Name + ", Cheap Phone Cards to " + countrytodata.Name;
            }
            //***********************************************************************************************************************************************************


            //return View("SearchRate2", ratemodel);
            return View(ratemodel);
        }




        //[CompressFilter]
        //public ActionResult Afghanistan(int countryfrom = 1)
        //{
        //    int countryto = 1;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}

        //[CompressFilter]
        //public ActionResult India(int countryfrom = 1)
        //{
        //    int countryto = 130;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}

        //[CompressFilter]
        //public ActionResult Nepal(int countryfrom = 1)
        //{
        //    int countryto = 224;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}

        //[CompressFilter]
        //public ActionResult Pakistan(int countryfrom = 1)
        //{
        //    int countryto = 238;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}

        //[CompressFilter]
        //public ActionResult Bangladesh(int countryfrom = 1)
        //{
        //    int countryto = 27;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}

        //[CompressFilter]
        //public ActionResult SriLanka(int countryfrom = 1)
        //{
        //    int countryto = 281;
        //    var ratemodel = new RateModel();
        //    this.Show_MetaTag_Title(countryfrom, countryto);
        //    return View("SearchRate_Generic", ratemodel);
        //}






        [CompressFilter]
        public ActionResult SearchRate_Generic(int countryfrom = 0, int countryto = 130)
        {
            //int countryto = 130;

            string currentPageFileName = new FileInfo(this.Request.Url.LocalPath).Name;
            //Response.Write(currentPageFileName);
            countryto = this.Get_CountryTo_By_MapRoute(currentPageFileName);
            ViewData["CountryToID_Raza"] = countryto.ToString();

            if (countryfrom == 0)
                countryfrom = 1;

            var ratemodel = new RateModel();

            ratemodel.CountryFromQuery = countryfrom;
            ratemodel.MobileOrGlobalPlan = "";
            ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
                ? SharedResources.CountriesMap[countryto] + ".jpg"
                : string.Empty;

            ////******************************************* ADDED BY SABAL ON 10/27/2014 TO ADD DYNAMIC META TAGS *********************************************************
            //var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
            //var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            //ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name +" | Raza";
            //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
            //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
            ////***********************************************************************************************************************************************************


            //******************************************* ADDED BY SABAL ON 11/12/2014 TO ADD DYNAMIC META TAGS *********************************************************

            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag = data.RatePageMetaTags.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);

            foreach (var item in MyMetaTag)
            {
                ViewBag.Title = item.Title;
                ViewBag.PageDescription = item.PageDescription;
                ViewBag.PageKeywords = item.PageKeywords;
            }
            if (string.IsNullOrEmpty(ViewBag.Title))
            {
                var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
                var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

                ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name + " | Raza";
                //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
                ViewBag.PageDescription = "Raza: Call " + countrytodata.Name + " from " + countryfromdata.Name + " at attractive rates! Dedicated calling plans for landlines & mobile phones to help you SAVE money!";
                //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
                ViewBag.PageKeywords = "Calling Cards to " + countrytodata.Name + " from " + countryfromdata.Name + ", Cheap Phone Cards to " + countrytodata.Name;
            }
            //***********************************************************************************************************************************************************


            //return View("SearchRate2", ratemodel);
            //return View("SearchRate", ratemodel);
            return View(ratemodel);
        }




        [CompressFilter]
        public ActionResult SearchRate_Amvrin(int countryfrom = 0, int countryto = 130)
        {
            //int countryto = 130;

            string From_And_To = "";
            string ExactURL = Request.Url.AbsoluteUri;
            if (ExactURL.ToUpper().IndexOf("/RATE/") > -1 && ExactURL.ToUpper().IndexOf("?FROM-") > -1)
            {
                From_And_To = ExactURL.ToUpper().Substring(ExactURL.ToUpper().IndexOf("?FROM-") + 1);
            }

            string currentPageFileName = new FileInfo(this.Request.Url.LocalPath).Name;
            //Response.Write(currentPageFileName);

            string From_To = this.Get_CountryFrom_And_To_Amvrin(From_And_To);

            countryfrom = Convert.ToInt16(From_To.Substring(0, 1));
            countryto = Convert.ToInt16(From_To.Substring(2));


            ViewData["CountryToID_Raza"] = countryto.ToString();

            var ratemodel = new RateModel();

            ratemodel.CountryFromQuery = countryfrom;
            ratemodel.MobileOrGlobalPlan = "";
            ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
                ? SharedResources.CountriesMap[countryto] + ".jpg"
                : string.Empty;

            ////******************************************* ADDED BY SABAL ON 10/27/2014 TO ADD DYNAMIC META TAGS *********************************************************
            //var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
            //var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            //ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name +" | Raza";
            //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
            //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
            ////***********************************************************************************************************************************************************


            //******************************************* ADDED BY SABAL ON 11/12/2014 TO ADD DYNAMIC META TAGS *********************************************************

            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag = data.RatePageMetaTags.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);

            foreach (var item in MyMetaTag)
            {
                ViewBag.Title = item.Title;
                ViewBag.PageDescription = item.PageDescription;
                ViewBag.PageKeywords = item.PageKeywords;
            }
            if (string.IsNullOrEmpty(ViewBag.Title))
            {
                var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
                var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

                ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name + " | Raza";
                //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
                ViewBag.PageDescription = "Raza: Call " + countrytodata.Name + " from " + countryfromdata.Name + " at attractive rates! Dedicated calling plans for landlines & mobile phones to help you SAVE money!";
                //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
                ViewBag.PageKeywords = "Calling Cards to " + countrytodata.Name + " from " + countryfromdata.Name + ", Cheap Phone Cards to " + countrytodata.Name;
            }
            //***********************************************************************************************************************************************************


            //return View("SearchRate2", ratemodel);
            //return View("SearchRate", ratemodel);
            return View(ratemodel);
        }




        private int Get_CountryTo_By_MapRoute(string PageName)
        {
            //******************************************* ADDED BY SABAL ON 11/22/2014 TO ADD RETURN COUNTRYID BASED UPON COUNTRY NAME *********************************************************
            if (PageName.ToUpper() == "INDIA" || PageName.ToUpper() == "INDIA-PHONE-CARD")
                return 130;
            else if (PageName.ToUpper() == "PAKISTAN" || PageName.ToUpper() == "PAKISTAN-PHONE-CARD")
                return 238;
            else if (PageName.ToUpper() == "NEPAL" || PageName.ToUpper() == "NEPAL-PHONE-CARD")
                return 224;
            else if (PageName.ToUpper() == "BANGLADESH" || PageName.ToUpper() == "BANGLADESH-PHONE-CARD")
                return 27;
            else if (PageName.ToUpper() == "SRILANKA" || PageName.ToUpper() == "SRILANKA-PHONE-CARD")
                return 281;
            else if (PageName.ToUpper() == "AFGHANISTAN" || PageName.ToUpper() == "AFGHANISTAN-PHONE-CARD")
                return 1;
            else if (PageName.ToUpper() == "GHANA" || PageName.ToUpper() == "GHANA-PHONE-CARD")
                return 110;
            else if (PageName.ToUpper() == "NIGERIA" || PageName.ToUpper() == "NIGERIA-PHONE-CARD")
                return 232;
            else if (PageName.ToUpper() == "CANADA" || PageName.ToUpper() == "CANADA-PHONE-CARD")
                return 57;
            else if (PageName.ToUpper() == "EGYPT" || PageName.ToUpper() == "EGYPT-PHONE-CARD")
                return 87;
            else if (PageName.ToUpper() == "JORDAN" || PageName.ToUpper() == "JORDAN-PHONE-CARD")
                return 178;
            else if (PageName.ToUpper() == "KENYA" || PageName.ToUpper() == "KENYA-PHONE-CARD")
                return 180;
            else if (PageName.ToUpper() == "PHILIPPINES" || PageName.ToUpper() == "PHILIPPINES-PHONE-CARD")
                return 248;
            else if (PageName.ToUpper() == "SAUDI ARABIA" || PageName.ToUpper() == "SAUDI ARABIA-PHONE-CARD")
                return 265;
            else if (PageName.ToUpper() == "UAE" || PageName.ToUpper() == "UAE-PHONE-CARD")
                return 309;
            else if (PageName.ToUpper() == "UK" || PageName.ToUpper() == "UK-PHONE-CARD")
                return 312;
            else
                return 314;
            //***********************************************************************************************************************************************************
        }




        private string Get_CountryFrom_And_To_Amvrin(string From_And_To)
        {
            //******************************************* ADDED BY SABAL ON 01/05/2015 TO ADD RETURN COUNTRYID BASED UPON AMVRIN RECOMENDATION *********************************************************
            string CountryFrom = "1";
            From_And_To = Server.UrlDecode(From_And_To);
            if(From_And_To.ToUpper().StartsWith("FROM-CANADA-TO-"))
                CountryFrom = "2";
            else if (From_And_To.ToUpper().StartsWith("FROM-UK-TO-"))
                CountryFrom = "3";

            if (From_And_To.ToUpper() == "FROM-USA-TO-AFGHANISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-AFGHANISTAN" || From_And_To.ToUpper() == "FROM-UK-TO-AFGHANISTAN") return CountryFrom + "*1";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ALASKA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ALASKA" || From_And_To.ToUpper() == "FROM-UK-TO-ALASKA") return CountryFrom + "*387";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ALBANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ALBANIA" || From_And_To.ToUpper() == "FROM-UK-TO-ALBANIA") return CountryFrom + "*2";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ALGERIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ALGERIA" || From_And_To.ToUpper() == "FROM-UK-TO-ALGERIA") return CountryFrom + "*3";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-AMERICAN-SAMOA" || From_And_To.ToUpper() == "FROM-CANADA-TO-AMERICAN-SAMOA" || From_And_To.ToUpper() == "FROM-UK-TO-AMERICAN-SAMOA") return CountryFrom + "*4";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ANDORRA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ANDORRA" || From_And_To.ToUpper() == "FROM-UK-TO-ANDORRA") return CountryFrom + "*5";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ANGOLA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ANGOLA" || From_And_To.ToUpper() == "FROM-UK-TO-ANGOLA") return CountryFrom + "*6";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ANGUILLA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ANGUILLA" || From_And_To.ToUpper() == "FROM-UK-TO-ANGUILLA") return CountryFrom + "*7";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ANTARCTICA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ANTARCTICA" || From_And_To.ToUpper() == "FROM-UK-TO-ANTARCTICA") return CountryFrom + "*8";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ANTIGUA-BARBUDA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ANTIGUA-BARBUDA" || From_And_To.ToUpper() == "FROM-UK-TO--ANTIGUA-BARBUDA") return CountryFrom + "*9";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ARGENTINA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ARGENTINA" || From_And_To.ToUpper() == "FROM-UK-TO-ARGENTINA") return CountryFrom + "*10";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ARMENIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ARMENIA" || From_And_To.ToUpper() == "FROM-UK-TO-ARMENIA") return CountryFrom + "*17";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ARUBA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ARUBA" || From_And_To.ToUpper() == "FROM-UK-TO-ARUBA") return CountryFrom + "*18";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ASCENSION-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-ASCENSION-ISLAND" || From_And_To.ToUpper() == "FROM-UK-TO-ASCENSION-ISLAND") return CountryFrom + "*498";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-AUSTRALIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-AUSTRALIA" || From_And_To.ToUpper() == "FROM-UK-TO-AUSTRALIA") return CountryFrom + "*19";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-AUSTRIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-AUSTRIA" || From_And_To.ToUpper() == "FROM-UK-TO-AUSTRIA") return CountryFrom + "*22";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-AZERBAIJAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-AZERBAIJAN" || From_And_To.ToUpper() == "FROM-UK-TO-AZERBAIJAN") return CountryFrom + "*24";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BAHAMAS" || From_And_To.ToUpper() == "FROM-CANADA-TO-BAHAMAS" || From_And_To.ToUpper() == "FROM-UK-TO-BAHAMAS") return CountryFrom + "*25";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BAHRAIN" || From_And_To.ToUpper() == "FROM-CANADA-TO-BAHRAIN" || From_And_To.ToUpper() == "FROM-UK-TO-BAHRAIN") return CountryFrom + "*26";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BANGLADESH" || From_And_To.ToUpper() == "FROM-CANADA-TO-BANGLADESH" || From_And_To.ToUpper() == "FROM-UK-TO-BANGLADESH") return CountryFrom + "*27";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BARBADOS" || From_And_To.ToUpper() == "FROM-CANADA-TO-BARBADOS" || From_And_To.ToUpper() == "FROM-UK-TO-BARBADOS") return CountryFrom + "*33";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BELARUS" || From_And_To.ToUpper() == "FROM-CANADA-TO-BELARUS" || From_And_To.ToUpper() == "FROM-UK-TO-BELARUS") return CountryFrom + "*34";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BELGIUM" || From_And_To.ToUpper() == "FROM-CANADA-TO-BELGIUM" || From_And_To.ToUpper() == "FROM-UK-TO-BELGIUM") return CountryFrom + "*35";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BELIZE" || From_And_To.ToUpper() == "FROM-CANADA-TO-BELIZE" || From_And_To.ToUpper() == "FROM-UK-TO-BELIZE") return CountryFrom + "*36";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BENIN" || From_And_To.ToUpper() == "FROM-CANADA-TO-BENIN" || From_And_To.ToUpper() == "FROM-UK-TO-BENIN") return CountryFrom + "*37";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BERMUDA" || From_And_To.ToUpper() == "FROM-CANADA-TO-BERMUDA" || From_And_To.ToUpper() == "FROM-UK-TO-BERMUDA") return CountryFrom + "*38";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BHUTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-BHUTAN" || From_And_To.ToUpper() == "FROM-UK-TO-BHUTAN") return CountryFrom + "*39";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BOLIVIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-BOLIVIA" || From_And_To.ToUpper() == "FROM-UK-TO-BOLIVIA") return CountryFrom + "*40";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BOSNIA-AND-HERZEGOVINA" || From_And_To.ToUpper() == "FROM-CANADA-TO-BOSNIA-AND-HERZEGOVINA" || From_And_To.ToUpper() == "FROM-UK-TO-BOSNIA-AND-HERZEGOVINA") return CountryFrom + "*41";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BOTSWANA" || From_And_To.ToUpper() == "FROM-CANADA-TO-BOTSWANA" || From_And_To.ToUpper() == "FROM-UK-TO-BOTSWANA") return CountryFrom + "*42";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BRAZIL" || From_And_To.ToUpper() == "FROM-CANADA-TO-BRAZIL" || From_And_To.ToUpper() == "FROM-UK-TO-BRAZIL") return CountryFrom + "*43";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BRITISH-VIRGIN-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-BRITISH-VIRGIN-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-BRITISH-VIRGIN-ISLANDS") return CountryFrom + "*47";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BRUNEI" || From_And_To.ToUpper() == "FROM-CANADA-TO-BRUNEI" || From_And_To.ToUpper() == "FROM-UK-TO-BRUNEI") return CountryFrom + "*48";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BULGARIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-BULGARIA" || From_And_To.ToUpper() == "FROM-UK-TO-BULGARIA") return CountryFrom + "*49";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-BURUNDI" || From_And_To.ToUpper() == "FROM-CANADA-TO-BURUNDI" || From_And_To.ToUpper() == "FROM-UK-TO-BURUNDI") return CountryFrom + "*54";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CAMBODIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CAMBODIA" || From_And_To.ToUpper() == "FROM-UK-TO-CAMBODIA") return CountryFrom + "*55";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CAMEROON" || From_And_To.ToUpper() == "FROM-CANADA-TO-CAMEROON" || From_And_To.ToUpper() == "FROM-UK-TO-CAMEROON") return CountryFrom + "*56";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CANADA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CANADA" || From_And_To.ToUpper() == "FROM-UK-TO-CANADA") return CountryFrom + "*57";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CAPE-VERDE-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-CAPE-VERDE-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-VERDE-ISLANDS") return CountryFrom + "*58";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CAYMAN-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-CAYMAN-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-CAYMAN-ISLANDS") return CountryFrom + "*59";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CENTRAL-AFRICAN-REP" || From_And_To.ToUpper() == "FROM-CANADA-TO-CENTRAL-AFRICAN-REP" || From_And_To.ToUpper() == "FROM-UK-TO-CENTRAL-AFRICAN-REP") return CountryFrom + "*60";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CHAD-REPUBLIC" || From_And_To.ToUpper() == "FROM-CANADA-TO-CHAD-REPUBLIC" || From_And_To.ToUpper() == "FROM-UK-TO-CHAD-REPUBLIC") return CountryFrom + "*62";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CHILE" || From_And_To.ToUpper() == "FROM-CANADA-TO-CHILE" || From_And_To.ToUpper() == "FROM-UK-TO-CHILE") return CountryFrom + "*63";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CHINA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CHINA" || From_And_To.ToUpper() == "FROM-UK-TO-CHINA") return CountryFrom + "*64";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CHRISTMAS-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-CHRISTMAS-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-CHRISTMAS-ISLANDS") return CountryFrom + "*404";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-COCOS-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-COCOS-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-COCOS-ISLANDS") return CountryFrom + "*345";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-COLOMBIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-COLOMBIA" || From_And_To.ToUpper() == "FROM-UK-TO-COLOMBIA") return CountryFrom + "*67";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-COMOROS-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-COMOROS-ISLAND" || From_And_To.ToUpper() == "FROM-UK-TO-COMOROS-ISLAND") return CountryFrom + "*68";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CONGO" || From_And_To.ToUpper() == "FROM-CANADA-TO-CONGO" || From_And_To.ToUpper() == "FROM-UK-TO-CONGO") return CountryFrom + "*70";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-COOK-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-COOK-ISLANDS" || From_And_To.ToUpper() == "FROM-UK-TO-COOK-ISLANDS") return CountryFrom + "*71";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-COSTA-RICA" || From_And_To.ToUpper() == "FROM-CANADA-TO-COSTA-RICA" || From_And_To.ToUpper() == "FROM-UK-TO-COSTA-RICA") return CountryFrom + "*72";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CROATIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CROATIA" || From_And_To.ToUpper() == "FROM-UK-TO-CROATIA") return CountryFrom + "*73";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CUBA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CUBA" || From_And_To.ToUpper() == "FROM-UK-TO-CUBA") return CountryFrom + "*74";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CYPRUS" || From_And_To.ToUpper() == "FROM-CANADA-TO-CYPRUS" || From_And_To.ToUpper() == "FROM-UK-TO-CYPRUS") return CountryFrom + "*75";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-CZECH-REPUBLIC" || From_And_To.ToUpper() == "FROM-CANADA-TO-CZECH-REPUBLIC" || From_And_To.ToUpper() == "FROM-UK-TO-CZECH-REPUBLIC") return CountryFrom + "*381";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DEM.-REP.-OF-CONGO" || From_And_To.ToUpper() == "FROM-CANADA-TO-DEM.-REP.-OF-CONGO" || From_And_To.ToUpper() == "FROM-UK-TO-DEM.-REP.-OF-CONGO") return CountryFrom + "*327";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DENMARK" || From_And_To.ToUpper() == "FROM-CANADA-TO-DENMARK" || From_And_To.ToUpper() == "FROM-UK-TO-DENMARK") return CountryFrom + "*80";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DIEGO-GARCIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-DIEGO-GARCIA" || From_And_To.ToUpper() == "FROM-UK-TO-DIEGO-GARCIA") return CountryFrom + "*81";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DJIBOUTI" || From_And_To.ToUpper() == "FROM-CANADA-TO-DJIBOUTI" || From_And_To.ToUpper() == "FROM-UK-TO-DJIBOUTI") return CountryFrom + "*82";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DOMINICA" || From_And_To.ToUpper() == "FROM-CANADA-TO-DOMINICA" || From_And_To.ToUpper() == "FROM-UK-TO-DOMINICA") return CountryFrom + "*83";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-DOMINICAN-REPUBLIC" || From_And_To.ToUpper() == "FROM-CANADA-TO-DOMINICAN-REPUBLIC" || From_And_To.ToUpper() == "FROM-UK-TO-DOMINICAN-REPUBLIC") return CountryFrom + "*84";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ECUADOR" || From_And_To.ToUpper() == "FROM-CANADA-TO-ECUADOR" || From_And_To.ToUpper() == "FROM-UK-TO-ECUADOR") return CountryFrom + "*85";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-EGYPT" || From_And_To.ToUpper() == "FROM-CANADA-TO-EGYPT" || From_And_To.ToUpper() == "FROM-UK-TO-EGYPT") return CountryFrom + "*87";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-EL-SALVADOR" || From_And_To.ToUpper() == "FROM-CANADA-TO-EL-SALVADOR") return CountryFrom + "*88";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-EQUATORIAL-GUINEA" || From_And_To.ToUpper() == "FROM-CANADA-TO-EQUATORIAL-GUINEA") return CountryFrom + "*552";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ERITREA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ERITREA") return CountryFrom + "*89";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ESTONIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ESTONIA") return CountryFrom + "*90";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ETHIOPIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ETHIOPIA") return CountryFrom + "*91";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FALKLAND-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-FALKLAND-ISLANDS") return CountryFrom + "*93";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FAROE-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-FAROE-ISLANDS") return CountryFrom + "*92";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FIJI-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-FIJI-ISLANDS") return CountryFrom + "*382";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FINLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-FINLAND") return CountryFrom + "*95";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FRANCE" || From_And_To.ToUpper() == "FROM-CANADA-TO-FRANCE") return CountryFrom + "*96";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FRENCH-ANTILLES" || From_And_To.ToUpper() == "FROM-CANADA-TO-FRENCH-ANTILLES") return CountryFrom + "*99";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FRENCH-GUIANA" || From_And_To.ToUpper() == "FROM-CANADA-TO-FRENCH-GUIANA") return CountryFrom + "*100";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-FRENCH-POLYNESIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-FRENCH-POLYNESIA") return CountryFrom + "*101";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GABON" || From_And_To.ToUpper() == "FROM-CANADA-TO-GABON") return CountryFrom + "*102";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GAMBIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GAMBIA") return CountryFrom + "*103";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GEORGIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GEORGIA") return CountryFrom + "*104";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GERMANY" || From_And_To.ToUpper() == "FROM-CANADA-TO-GERMANY") return CountryFrom + "*105";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GHANA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GHANA" || From_And_To.ToUpper() == "FROM-UK-TO-GHANA") return CountryFrom + "*110";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GIBRALTAR" || From_And_To.ToUpper() == "FROM-CANADA-TO-GIBRALTAR") return CountryFrom + "*111";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GREECE" || From_And_To.ToUpper() == "FROM-CANADA-TO-GREECE") return CountryFrom + "*113";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GREENLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-GREENLAND") return CountryFrom + "*115";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GRENADA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GRENADA") return CountryFrom + "*116";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUADELOUPE" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUADELOUPE") return CountryFrom + "*117";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUAM" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUAM") return CountryFrom + "*118";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUANTANAMO-BAY" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUANTANAMO-BAY") return CountryFrom + "*119";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUATEMALA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUATEMALA") return CountryFrom + "*120";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUINEA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUINEA") return CountryFrom + "*122";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-GUYANA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GUYANA") return CountryFrom + "*123";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-HAITI" || From_And_To.ToUpper() == "FROM-CANADA-TO-HAITI") return CountryFrom + "*124";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-HAWAII" || From_And_To.ToUpper() == "FROM-CANADA-TO-HAWAII") return CountryFrom + "*411";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-HONDURAS" || From_And_To.ToUpper() == "FROM-CANADA-TO-HONDURAS") return CountryFrom + "*125";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-HONG-KONG" || From_And_To.ToUpper() == "FROM-CANADA-TO-HONG-KONG") return CountryFrom + "*126";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-HUNGARY" || From_And_To.ToUpper() == "FROM-CANADA-TO-HUNGARY") return CountryFrom + "*128";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ICELAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-ICELAND") return CountryFrom + "*129";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-INDIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-INDIA" || From_And_To.ToUpper() == "FROM-UK-TO-INDIA") return CountryFrom + "*130";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-INDONESIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-INDONESIA") return CountryFrom + "*166";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-IRAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-IRAN") return CountryFrom + "*167";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-IRAQ" || From_And_To.ToUpper() == "FROM-CANADA-TO-IRAQ") return CountryFrom + "*169";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-IRELAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-IRELAND") return CountryFrom + "*170";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ISRAEL" || From_And_To.ToUpper() == "FROM-CANADA-TO-ISRAEL") return CountryFrom + "*171";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ITALY" || From_And_To.ToUpper() == "FROM-CANADA-TO-ITALY") return CountryFrom + "*172";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-IVORY-COAST" || From_And_To.ToUpper() == "FROM-CANADA-TO-IVORY-COAST") return CountryFrom + "*175";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-JAMAICA" || From_And_To.ToUpper() == "FROM-CANADA-TO-JAMAICA") return CountryFrom + "*176";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-JAPAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-JAPAN") return CountryFrom + "*177";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-JORDAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-JORDAN") return CountryFrom + "*178";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KAZAKHSTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-KAZAKHSTAN") return CountryFrom + "*179";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KENYA" || From_And_To.ToUpper() == "FROM-CANADA-TO-KENYA") return CountryFrom + "*180";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KIRIBATI" || From_And_To.ToUpper() == "FROM-CANADA-TO-KIRIBATI") return CountryFrom + "*414";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KOSOVO" || From_And_To.ToUpper() == "FROM-CANADA-TO-KOSOVO") return CountryFrom + "*670";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KUWAIT" || From_And_To.ToUpper() == "FROM-CANADA-TO-KUWAIT") return CountryFrom + "*185";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-KYRGYZSTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-KYRGYZSTAN") return CountryFrom + "*186";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LAOS" || From_And_To.ToUpper() == "FROM-CANADA-TO-LAOS") return CountryFrom + "*187";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LATVIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-LATVIA") return CountryFrom + "*188";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LEBANON" || From_And_To.ToUpper() == "FROM-CANADA-TO-LEBANON") return CountryFrom + "*189";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LESOTHO" || From_And_To.ToUpper() == "FROM-CANADA-TO-LESOTHO") return CountryFrom + "*190";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LIBERIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-LIBERIA") return CountryFrom + "*191";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LIBYA" || From_And_To.ToUpper() == "FROM-CANADA-TO-LIBYA") return CountryFrom + "*192";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LIECHTENSTEIN" || From_And_To.ToUpper() == "FROM-CANADA-TO-LIECHTENSTEIN") return CountryFrom + "*193";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LITHUANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-LITHUANIA") return CountryFrom + "*194";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-LUXEMBOURG" || From_And_To.ToUpper() == "FROM-CANADA-TO-LUXEMBOURG") return CountryFrom + "*195";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MACAO" || From_And_To.ToUpper() == "FROM-CANADA-TO-MACAO") return CountryFrom + "*196";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MACEDONIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MACEDONIA") return CountryFrom + "*197";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MADAGASCAR" || From_And_To.ToUpper() == "FROM-CANADA-TO-MADAGASCAR") return CountryFrom + "*198";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MALAWI" || From_And_To.ToUpper() == "FROM-CANADA-TO-MALAWI") return CountryFrom + "*199";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MALAYSIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MALAYSIA") return CountryFrom + "*200";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MALDIVES" || From_And_To.ToUpper() == "FROM-CANADA-TO-MALDIVES") return CountryFrom + "*203";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MALI-REPUBLIC" || From_And_To.ToUpper() == "FROM-CANADA-TO-MALI-REPUBLIC") return CountryFrom + "*204";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MALTA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MALTA") return CountryFrom + "*205";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MARSHALL-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-MARSHALL-ISLANDS") return CountryFrom + "*206";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MAURITANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MAURITANIA") return CountryFrom + "*207";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MAURITIUS" || From_And_To.ToUpper() == "FROM-CANADA-TO-MAURITIUS") return CountryFrom + "*378";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MAYOTTE-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-MAYOTTE-ISLAND") return CountryFrom + "*597";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MEXICO" || From_And_To.ToUpper() == "FROM-CANADA-TO-MEXICO") return CountryFrom + "*209";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MICRONESIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MICRONESIA") return CountryFrom + "*214";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MOLDOVA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MOLDOVA") return CountryFrom + "*215";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MONACO" || From_And_To.ToUpper() == "FROM-CANADA-TO-MONACO") return CountryFrom + "*216";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MONGOLIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-MONGOLIA") return CountryFrom + "*217";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MONTENEGRO" || From_And_To.ToUpper() == "FROM-CANADA-TO-MONTENEGRO") return CountryFrom + "*667";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MONTSERRAT" || From_And_To.ToUpper() == "FROM-CANADA-TO-MONTSERRAT") return CountryFrom + "*218";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MOROCCO" || From_And_To.ToUpper() == "FROM-CANADA-TO-MOROCCO") return CountryFrom + "*219";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MOZAMBIQUE" || From_And_To.ToUpper() == "FROM-CANADA-TO-MOZAMBIQUE") return CountryFrom + "*220";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-MYANMAR" || From_And_To.ToUpper() == "FROM-CANADA-TO-MYANMAR") return CountryFrom + "*221";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NAMIBIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NAMIBIA") return CountryFrom + "*222";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NAURU" || From_And_To.ToUpper() == "FROM-CANADA-TO-NAURU") return CountryFrom + "*223";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NEPAL" || From_And_To.ToUpper() == "FROM-CANADA-TO-NEPAL" || From_And_To.ToUpper() == "FROM-UK-TO-NEPAL") return CountryFrom + "*224";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NETHERLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-NETHERLANDS") return CountryFrom + "*227";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NETHERLANDS-ANTILLES" || From_And_To.ToUpper() == "FROM-CANADA-TO-NETHERLANDS-ANTILLES") return CountryFrom + "*226";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NEW-CALEDONIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NEW-CALEDONIA") return CountryFrom + "*228";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NEW-ZEALAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-NEW-ZEALAND") return CountryFrom + "*229";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NICARAGUA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NICARAGUA") return CountryFrom + "*230";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NIGER" || From_And_To.ToUpper() == "FROM-CANADA-TO-NIGER") return CountryFrom + "*231";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NIGERIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NIGERIA") return CountryFrom + "*232";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NIUE-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-NIUE-ISLAND") return CountryFrom + "*235";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NORFOLK-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-NORFOLK-ISLAND") return CountryFrom + "*427";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NORTH-KOREA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NORTH-KOREA") return CountryFrom + "*428";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-NORWAY" || From_And_To.ToUpper() == "FROM-CANADA-TO-NORWAY") return CountryFrom + "*236";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-OMAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-OMAN") return CountryFrom + "*237";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PAKISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-PAKISTAN" || From_And_To.ToUpper() == "FROM-UK-TO-PAKISTAN") return CountryFrom + "*238";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PALAU" || From_And_To.ToUpper() == "FROM-CANADA-TO-PALAU") return CountryFrom + "*242";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PALESTINE" || From_And_To.ToUpper() == "FROM-CANADA-TO-PALESTINE") return CountryFrom + "*385";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PANAMA" || From_And_To.ToUpper() == "FROM-CANADA-TO-PANAMA") return CountryFrom + "*243";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PAPUA-NEW-GUINEA" || From_And_To.ToUpper() == "FROM-CANADA-TO-PAPUA-NEW-GUINEA") return CountryFrom + "*244";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PARAGUAY" || From_And_To.ToUpper() == "FROM-CANADA-TO-PARAGUAY") return CountryFrom + "*245";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PERU" || From_And_To.ToUpper() == "FROM-CANADA-TO-PERU") return CountryFrom + "*246";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PHILIPPINES" || From_And_To.ToUpper() == "FROM-CANADA-TO-PHILIPPINES") return CountryFrom + "*248";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-POLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-POLAND") return CountryFrom + "*249";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PORTUGAL" || From_And_To.ToUpper() == "FROM-CANADA-TO-PORTUGAL") return CountryFrom + "*251";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-PUERTO-RICO" || From_And_To.ToUpper() == "FROM-CANADA-TO-PUERTO-RICO") return CountryFrom + "*252";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-QATAR" || From_And_To.ToUpper() == "FROM-CANADA-TO-QATAR") return CountryFrom + "*253";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-REUNION-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-REUNION-ISLAND") return CountryFrom + "*255";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ROMANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ROMANIA") return CountryFrom + "*256";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-RUSSIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-RUSSIA") return CountryFrom + "*258";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-RWANDA" || From_And_To.ToUpper() == "FROM-CANADA-TO-RWANDA") return CountryFrom + "*260";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SAIPAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAIPAN") return CountryFrom + "*250";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SAN-MARINO" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAN-MARINO") return CountryFrom + "*261";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SAO-TOME" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAO-TOME") return CountryFrom + "*627";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SAUDI-ARABIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAUDI-ARABIA") return CountryFrom + "*265";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SENEGAL" || From_And_To.ToUpper() == "FROM-CANADA-TO-SENEGAL") return CountryFrom + "*266";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SERBIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SERBIA") return CountryFrom + "*267";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SEYCHELLES-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-SEYCHELLES-ISLAND") return CountryFrom + "*268";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SIERRA-LEONE" || From_And_To.ToUpper() == "FROM-CANADA-TO-SIERRA-LEONE") return CountryFrom + "*269";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SINGAPORE" || From_And_To.ToUpper() == "FROM-CANADA-TO-SINGAPORE") return CountryFrom + "*270";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SLOVAKIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SLOVAKIA") return CountryFrom + "*271";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SLOVENIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SLOVENIA") return CountryFrom + "*272";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SOLOMON-ISLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-SOLOMON-ISLAND") return CountryFrom + "*273";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SOMALIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SOMALIA") return CountryFrom + "*274";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SOUTH-AFRICA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SOUTH-AFRICA") return CountryFrom + "*275";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SOUTH-KOREA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SOUTH-KOREA") return CountryFrom + "*277";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SPAIN" || From_And_To.ToUpper() == "FROM-CANADA-TO-SPAIN") return CountryFrom + "*278";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SRI-LANKA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SRI-LANKA") return CountryFrom + "*281";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-HELENA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-HELENA") return CountryFrom + "*284";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-KITTS-&-NEVIS" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-KITTS-&-NEVIS") return CountryFrom + "*285";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-LUCIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-LUCIA") return CountryFrom + "*286";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-MARTIN" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-MARTIN") return CountryFrom + "*287";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-PIERRE-&-MEQUELON" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-PIERRE-&-MEQUELON") return CountryFrom + "*288";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ST.-VINCENT" || From_And_To.ToUpper() == "FROM-CANADA-TO-ST.-VINCENT") return CountryFrom + "*379";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SUDAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-SUDAN") return CountryFrom + "*289";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SURINAME" || From_And_To.ToUpper() == "FROM-CANADA-TO-SURINAME") return CountryFrom + "*290";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SWAZILAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-SWAZILAND") return CountryFrom + "*291";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SWEDEN" || From_And_To.ToUpper() == "FROM-CANADA-TO-SWEDEN") return CountryFrom + "*292";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SWITZERLAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-SWITZERLAND") return CountryFrom + "*294";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-SYRIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SYRIA") return CountryFrom + "*295";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TAIWAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-TAIWAN") return CountryFrom + "*296";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TAJIKISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-TAJIKISTAN") return CountryFrom + "*297";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TANZANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-TANZANIA") return CountryFrom + "*679";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-THAILAND" || From_And_To.ToUpper() == "FROM-CANADA-TO-THAILAND") return CountryFrom + "*299";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TOGO" || From_And_To.ToUpper() == "FROM-CANADA-TO-TOGO") return CountryFrom + "*301";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TONGA" || From_And_To.ToUpper() == "FROM-CANADA-TO-TONGA") return CountryFrom + "*302";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TRINIDAD-&-TOBAGO" || From_And_To.ToUpper() == "FROM-CANADA-TO-TRINIDAD-&-TOBAGO") return CountryFrom + "*303";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TUNISIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-TUNISIA") return CountryFrom + "*304";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TURKEY" || From_And_To.ToUpper() == "FROM-CANADA-TO-TURKEY") return CountryFrom + "*305";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TURKMENISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-TURKMENISTAN") return CountryFrom + "*307";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-TUVALU" || From_And_To.ToUpper() == "FROM-CANADA-TO-TUVALU") return CountryFrom + "*308";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-UGANDA" || From_And_To.ToUpper() == "FROM-CANADA-TO-UGANDA") return CountryFrom + "*315";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-UKRAINE" || From_And_To.ToUpper() == "FROM-CANADA-TO-UKRAINE") return CountryFrom + "*316";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-UNITED-ARAB-EMIRATES" || From_And_To.ToUpper() == "FROM-USA-TO-UAE" || From_And_To.ToUpper() == "FROM-CANADA-TO-UNITED-ARAB-EMIRATES" || From_And_To.ToUpper() == "FROM-CANADA-TO-UAE") return CountryFrom + "*309";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-UNITED-KINGDOM" || From_And_To.ToUpper() == "FROM-USA-TO-UK" || From_And_To.ToUpper() == "FROM-CANADA-TO-UNITED-KINGDOM" || From_And_To.ToUpper() == "FROM-CANADA-TO-UK") return CountryFrom + "*312";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-URUGUAY" || From_And_To.ToUpper() == "FROM-CANADA-TO-URUGUAY") return CountryFrom + "*317";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-US-VIRGIN-ISLANDS" || From_And_To.ToUpper() == "FROM-CANADA-TO-US-VIRGIN-ISLANDS") return CountryFrom + "*313";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-USA" || From_And_To.ToUpper() == "FROM-CANADA-TO-USA") return CountryFrom + "*314";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-UZBEKISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-UZBEKISTAN") return CountryFrom + "*318";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-VANUATU" || From_And_To.ToUpper() == "FROM-CANADA-TO-VANUATU") return CountryFrom + "*319";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-VATICAN-CITY" || From_And_To.ToUpper() == "FROM-CANADA-TO-VATICAN-CITY") return CountryFrom + "*320";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-VENEZUELA" || From_And_To.ToUpper() == "FROM-CANADA-TO-VENEZUELA") return CountryFrom + "*321";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-VIETNAM" || From_And_To.ToUpper() == "FROM-CANADA-TO-VIETNAM") return CountryFrom + "*323";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-WALLIS-&-FUTUNA" || From_And_To.ToUpper() == "FROM-CANADA-TO-WALLIS-&-FUTUNA") return CountryFrom + "*663";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-WEST-SAMOA" || From_And_To.ToUpper() == "FROM-CANADA-TO-WEST-SAMOA") return CountryFrom + "*324";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-YEMEN" || From_And_To.ToUpper() == "FROM-CANADA-TO-YEMEN") return CountryFrom + "*325";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-YUGOSLAVIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-YUGOSLAVIA") return CountryFrom + "*326";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ZAMBIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ZAMBIA") return CountryFrom + "*328";
            else if (From_And_To.ToUpper() == "FROM-USA-TO-ZIMBABWE" || From_And_To.ToUpper() == "FROM-CANADA-TO-ZIMBABWE") return CountryFrom + "*448";
            else
                return CountryFrom + "*314";


            //if (From_And_To.ToUpper() == "FROM-USA-TO-INDIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-INDIA")
            //    return CountryFrom+"*130";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-ALBANIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-ALBANIA")
            //    return CountryFrom + "*2";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-PAKISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-PAKISTAN")
            //    return CountryFrom + "*238";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-NEPAL" || From_And_To.ToUpper() == "FROM-CANADA-TO-NEPAL")
            //    return CountryFrom + "*224";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-BANGLADESH" || From_And_To.ToUpper() == "FROM-CANADA-TO-BANGLADESH")
            //    return CountryFrom + "*27";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-SRI LANKA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SRI LANKA" || From_And_To.ToUpper() == "FROM-USA-TO-SRI-LANKA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SRI-LANKA")
            //    return CountryFrom + "*281";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-AFGHANISTAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-AFGHANISTAN")
            //    return CountryFrom + "*1";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-GHANA" || From_And_To.ToUpper() == "FROM-CANADA-TO-GHANA")
            //    return CountryFrom + "*110";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-NIGERIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-NIGERIA")
            //    return CountryFrom + "*232";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-CANADA" || From_And_To.ToUpper() == "FROM-CANADA-TO-CANADA")
            //    return CountryFrom + "*57";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-EGYPT" || From_And_To.ToUpper() == "FROM-CANADA-TO-EGYPT")
            //    return CountryFrom + "*87";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-JORDAN" || From_And_To.ToUpper() == "FROM-CANADA-TO-JORDAN")
            //    return CountryFrom + "*178";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-KENYA" || From_And_To.ToUpper() == "FROM-CANADA-TO-KENYA")
            //    return CountryFrom + "*180";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-PHILIPPINES" || From_And_To.ToUpper() == "FROM-CANADA-TO-PHILIPPINES")
            //    return CountryFrom + "*248";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-SAUDI ARABIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAUDI ARABIA" || From_And_To.ToUpper() == "FROM-USA-TO-SAUDI-ARABIA" || From_And_To.ToUpper() == "FROM-CANADA-TO-SAUDI-ARABIA")
            //    return CountryFrom + "*265";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-UAE" || From_And_To.ToUpper() == "FROM-CANADA-TO-UAE")
            //    return CountryFrom + "*309";
            //else if (From_And_To.ToUpper() == "FROM-USA-TO-UK" || From_And_To.ToUpper() == "FROM-CANADA-TO-UK")
            //    return CountryFrom + "*312";
            //else
            //    return CountryFrom + "*314";
            //***********************************************************************************************************************************************************
        }




        private void Show_MetaTag_Title(int countryfrom, int countryto)
        {
            //******************************************* ADDED BY SABAL ON 11/12/2014 TO ADD DYNAMIC META TAGS *********************************************************
            var ratemodel = new RateModel();



            ratemodel.CountryFromQuery = countryfrom;
            ratemodel.MobileOrGlobalPlan = "";
            ratemodel.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto)
                ? SharedResources.CountriesMap[countryto] + ".jpg"
                : string.Empty;



            string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
            var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

            var MyMetaTag = data.RatePageMetaTags.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);

            foreach (var item in MyMetaTag)
            {
                ViewBag.Title = item.Title;
                ViewBag.PageDescription = item.PageDescription;
                ViewBag.PageKeywords = item.PageKeywords;
            }
            if (string.IsNullOrEmpty(ViewBag.Title))
            {
                var countryfromdata = ratemodel.ListOfFromCountries.FirstOrDefault(a => a.Id == countryfrom.ToString());
                var countrytodata = ratemodel.ListOfToCountries.FirstOrDefault(a => a.Id == countryto.ToString());

                ViewBag.Title = "Call " + countrytodata.Name + " From " + countryfromdata.Name + " | Cheap Calls to " + countrytodata.Name + " | Raza";
                //ViewBag.PageDescription = "Raza: Make cheap calls to any city in " + countrytodata.Name + " from " + countryfromdata.Name + ". Dedicated plans for landlines & mobile phones to help you save money";
                ViewBag.PageDescription = "Raza: Call " + countrytodata.Name + " from " + countryfromdata.Name + " at attractive rates! Dedicated calling plans for landlines & mobile phones to help you SAVE money!";
                //ViewBag.PageKeywords = "Calling " + countrytodata.Name + " From " + countryfromdata.Name + ", Cheap Calls to " + countrytodata.Name + " From " + countryfromdata.Name + "";
                ViewBag.PageKeywords = "Calling Cards to " + countrytodata.Name + " from " + countryfromdata.Name + ", Cheap Phone Cards to " + countrytodata.Name;
            }
            //***********************************************************************************************************************************************************
        }





        public JsonResult GetCountryID_By_CountryName(string PageName)
        {
            #region TO CHECK FOR COUNTRY NAME AND ASSIGN COUNTRYID

            int CountryID = 314;
            if (PageName.ToUpper() == "INDIA" || PageName.ToUpper() == "INDIA-PHONE-CARD")
                CountryID = 130;
            else if (PageName.ToUpper() == "PAKISTAN" || PageName.ToUpper() == "PAKISTAN-PHONE-CARD")
                CountryID = 238;
            else if (PageName.ToUpper() == "NEPAL" || PageName.ToUpper() == "NEPAL-PHONE-CARD")
                CountryID = 224;
            else if (PageName.ToUpper() == "BANGLADESH" || PageName.ToUpper() == "BANGLADESH-PHONE-CARD")
                CountryID = 27;
            else if (PageName.ToUpper() == "SRILANKA" || PageName.ToUpper() == "SRILANKA-PHONE-CARD")
                CountryID = 281;
            else if (PageName.ToUpper() == "AFGHANISTAN" || PageName.ToUpper() == "AFGHANISTAN-PHONE-CARD")
                CountryID = 1;
            else if (PageName.ToUpper() == "GHANA" || PageName.ToUpper() == "GHANA-PHONE-CARD")
                CountryID = 110;
            else if (PageName.ToUpper() == "NIGERIA" || PageName.ToUpper() == "NIGERIA-PHONE-CARD")
                CountryID = 231;
            else if (PageName.ToUpper() == "CANADA" || PageName.ToUpper() == "CANADA-PHONE-CARD")
                CountryID = 57;
            else if (PageName.ToUpper() == "EGYPT" || PageName.ToUpper() == "EGYPT-PHONE-CARD")
                CountryID = 87;
            else if (PageName.ToUpper() == "JORDAN" || PageName.ToUpper() == "JORDAN-PHONE-CARD")
                CountryID = 178;
            else if (PageName.ToUpper() == "KENYA" || PageName.ToUpper() == "KENYA-PHONE-CARD")
                CountryID = 180;
            else if (PageName.ToUpper() == "PHILIPPINES" || PageName.ToUpper() == "PHILIPPINES-PHONE-CARD")
                CountryID = 248;
            else if (PageName.ToUpper() == "SAUDI ARABIA" || PageName.ToUpper() == "SAUDI ARABIA-PHONE-CARD")
                CountryID = 265;
            else if (PageName.ToUpper() == "UAE" || PageName.ToUpper() == "UAE-PHONE-CARD")
                CountryID = 309;
            else if (PageName.ToUpper() == "UK" || PageName.ToUpper() == "UK-PHONE-CARD")
                CountryID = 312;
            else
                CountryID = 314;

            //var rate = new Rates()
            //{
            //    CardName = "",//"HOT DIRECT",
            //    CountryFrom = SafeConvert.ToInt32(countryfrom),
            //    CountryTo = SafeConvert.ToInt32(countryto)
            //};

            //return Json(CountryID.ToString(), JsonRequestBehavior.AllowGet);
            //return Json(rate, JsonRequestBehavior.AllowGet);

            return
        Json(
            new
            {
                ok = true,
                CountryToID = CountryID.ToString()
            });

            #endregion
        }




        //public JsonResult GetRatePage_MetaTag(int countryfrom, int countryto)
        //{
        //    string filepath = Server.MapPath(@"/Content/RatePageMetaTag_New.xml");
        //    var data = SerializationUtility<RatePageMetaTag>.DeserializeObject(System.IO.File.ReadAllText(filepath));

        //    var MyMetaTag =
        //        data.RatePageMetaTags.FirstOrDefault(
        //            a => a.CountryFrom == countryfrom && a.CountryTo == countryto);


        //    return Json(MyMetaTag, JsonRequestBehavior.AllowGet);
        //}



        public JsonResult SearchMethod(int countryfrom, int countryto, string mobileorGlobal, string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            var ratemodel = SearchForRateJson(countryfrom, countryto, mobileorGlobal, cardTypeName, callForwarding,
                globalcall);


            return Json(ratemodel, JsonRequestBehavior.AllowGet);
        }

        public RateViewModelJson SearchForRateJson(int countryfrom, int countryto, string mobileorGlobal, string cardTypeName = "", bool callForwarding = false, bool globalcall = false)
        {
            // var rateModel = new RateModel();

            var ratemodelJson = new RateViewModelJson();
            var repository = new DataRepository();

            var toCountries = CacheManager.Instance.GetAllCountryTo();

            ratemodelJson.CountryToList = toCountries;

            //  ratemodelJson.CountryFrom = rateModel.ListOfFromCountries.FirstOrDefault(t => t.Id == countryfrom.ToString());
            //  ratemodelJson.CountryTo = rateModel.ListOfToCountries.FirstOrDefault(t => t.Id == countryto.ToString());

            var countrydata = toCountries.FirstOrDefault(a => a.Id == countryto.ToString());

            if (countrydata != null)
            {
                var countrycode = countrydata.CountCode;
                ratemodelJson.CountryCode = countrycode;
                // fill the list for country mobile list.
                var countrymobilelist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Mobile").ToList();

                //add country mobile to list 
                var mobilecitylist = countrymobilelist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }).ToList();

                var countrycitylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "City").ToList();

                //add country city to list
                mobilecitylist.AddRange(countrycitylist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }));

                var onlycountrylist = toCountries.Where(a => a.CountCode == countrycode && a.RateType == "Country").ToList();
                mobilecitylist.AddRange(onlycountrylist.Select(country => new Country()
                {
                    CountCode = country.CountCode,
                    CountryCode = country.CountryCode,
                    Id = country.Id,
                    Name = country.Name,
                }));

                ratemodelJson.MobileCityList = mobilecitylist.OrderBy(a => a.Name).ToList();
                ratemodelJson.CountryCityList = countrycitylist;
                ratemodelJson.CountryMobileList = countrymobilelist;

                //search rates
                Rates rate = new Rates()
                {
                    CardName = string.Empty,
                    CountryFrom = countryfrom,
                    CountryTo = countryto
                };

                //code ends here for India unlimited Plans

                // RatePlans rateplan = repository.GetRates(rate); //comment
                RatePlans rateplan = CacheManager.Instance.GetRatesFromCache(rate);

                var currencydict = FlagDictonary.GetCurrencycode();
                var ratepermindict = FlagDictonary.RatePerMinSign();

                if (ratepermindict.ContainsKey(countryfrom))
                {
                    ratemodelJson.RateperMinSign = ratepermindict[countryfrom];
                }
                else
                {
                    ratemodelJson.RateperMinSign = "¢/min";

                }

                switch (cardTypeName)
                {
                    case "Mobile":
                        var mobileratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

                        foreach (EachRatePlan erp in mobileratecards)
                        {
                            ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "Mobile";
                        break;
                    case "City":
                        var directratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "3");  // city direct
                        foreach (EachRatePlan erp in directratecards)
                        {
                            ratemodelJson.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "City";
                        break;
                    case "Country":
                        var onetouchdialratecards = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  //onetouch dial
                        foreach (EachRatePlan erp in onetouchdialratecards)
                        {
                            ratemodelJson.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }
                        ratemodelJson.SearchType = "Country";
                        break;
                    default:
                        var mobileratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "2"); //mobile direct

                        foreach (EachRatePlan erp in mobileratecards1)
                        {
                            ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        var directratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "3"); // city direct
                        foreach (EachRatePlan erp in directratecards1)
                        {
                            ratemodelJson.CityDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        var onetouchdialratecards1 = rateplan.Plans.Where(t => t.PlanCategoryId == "1");  // onetouch dial
                        foreach (EachRatePlan erp in onetouchdialratecards1)
                        {
                            ratemodelJson.OneTouchDialRateCards.Add(createPlanModel(callForwarding, erp, currencydict, countryfrom, countryto));
                        }

                        break;
                }

                var tocountrydata = toCountries.SingleOrDefault(a => a.Id == countryto.ToString());
                if (tocountrydata != null)
                {
                    if (mobileorGlobal == "Mobile")
                    {

                        ratemodelJson.SearchType = "Mobile";

                    }
                    else if (mobileorGlobal != "Mobile")
                    {
                        ratemodelJson.SearchType = tocountrydata.RateType;
                    }
                    if (tocountrydata.RateType == "Country")
                    {
                        var mobiledata =
                            toCountries.Where(a => a.RateType == "Mobile")
                                .FirstOrDefault(a => a.Name.StartsWith(tocountrydata.Name));

                        if (mobiledata != null)
                        {
                            Rates rt = new Rates()
                            {
                                CardName = string.Empty,
                                CountryFrom = countryfrom,
                                CountryTo = SafeConvert.ToInt32(mobiledata.Id),
                            };

                            //RatePlans dataplan = repository.GetRates(rt); //comment
                            RatePlans dataplan = CacheManager.Instance.GetRatesFromCache(rt);
                            var mobileratecards1 = dataplan.Plans.Where(t => t.PlanCategoryId == "2"); // mobile direct

                            foreach (EachRatePlan erp in mobileratecards1)
                            {
                                ratemodelJson.MobileDirectRateCards.Add(createPlanModel(callForwarding, erp, currencydict,
                                    countryfrom, SafeConvert.ToInt32(mobiledata.Id)));
                            }

                        }

                    }
                    if (tocountrydata.RateType == "Country" || tocountrydata.RateType == "Mobile")
                    {
                        if (!ratemodelJson.MobileDirectRateCards.Any())
                        {
                            ratemodelJson.MobileDirectRateCards = ratemodelJson.OneTouchDialRateCards;
                        }
                    }
                    else if (tocountrydata.RateType == "City" && !ratemodelJson.CityDirectRateCards.Any())
                    {
                        ratemodelJson.CityDirectRateCards = ratemodelJson.OneTouchDialRateCards;
                    }
                }
            }

            ratemodelJson.CountryToList = new List<Country>();

            ratemodelJson.CountryBannerPath = SharedResources.CountriesMap.ContainsKey(countryto) ? SharedResources.CountriesMap[countryto] : string.Empty;
            return ratemodelJson;
        }

        private static PlanModel createPlanModel(bool callForwarding, EachRatePlan erp, IDictionary<string, string> currencydict, int countryfrom, int countryto)
        {
            return new PlanModel
            {
                CardType = (PlanType)erp.CardType,
                CardTypeName = erp.CardTypeName,
                FromToMapping = erp.FromToMapping,
                RatePerMin = callForwarding ? erp.RatePerMin + 2 : erp.RatePerMin,//todo: add 2 cents/per min for call forwarding
                PlanAmount = erp.PlanAmount,
                PlanId = erp.PlanId,
                Discount = erp.Discount,
                ServiceFee = erp.ServiceFee,
                CurrencyCode = erp.CurrencyCode,
                CurrencySign = currencydict[erp.CurrencyCode],
                TotalMinutes = erp.TotalMinutes,
                IsAutoRefill = false,
                CountryFrom = countryfrom,
                CountryTo = countryto

            };
        }

        private List<Unlimitedindiapromotionalplan> UnlimitedPromotionalPlan(int countryfrom, int countryto)
        {
            string filePathName = Server.MapPath(@"/Content/UnlimitedIndiaPlan.xml");
            var dataresult = SerializationUtility<UnlimitedIndiaPlanModel>.DeserializeObject(System.IO.File.ReadAllText(filePathName));
            var result = new List<Unlimitedindiapromotionalplan>();

            var unlimitedPromotonPlan = dataresult.UnlimitedIndiaPlans;

            foreach (var res in unlimitedPromotonPlan)
            {
                result.Add(new Unlimitedindiapromotionalplan()
                {
                    ServiceFee = res.ServiceFee,
                    CountryFrom = countryfrom,
                    CountryTo = countryto,
                    CentPerMinute = res.CentPerMinute,
                    TotalMinute = res.TotalMinute,
                });

            }
            return result;


        }


        private List<Indiacentpromotionalplanmodel> PromotionalPlan(int countryfrom, int countryto)
        {
            string filepath = Server.MapPath(@"/Content/IndiacentPlan.xml");
            var data = SerializationUtility<IndiaCentPlanModel>.DeserializeObject(System.IO.File.ReadAllText(filepath));
            var result = new List<Indiacentpromotionalplanmodel>();
            // var PromotionalCentPlan = data.IndiaCentPlanRate.Where(a => a.CountryFrom == countryfrom && a.CountryTo == countryto);
            var PromotionalCentPlan = data.IndiaCentPlanRate;
            var dict = FlagDictonary.GetCurrencycodebyCountry();
            var currencycode = dict[countryfrom];

            var dict2 = FlagDictonary.RatePerMinSign();
            var sign = dict2[countryfrom];

            var dict3 = FlagDictonary.GetCurrencycode();
            var currencysign = dict3[currencycode];

            foreach (var res in PromotionalCentPlan)
            {
                result.Add(new Indiacentpromotionalplanmodel()
                {
                    CountryFrom = countryfrom,
                    CountryTo = countryto,
                    ServiceFee = res.ServiceFee,
                    TotalCharge = res.TotalCharge,
                    CentPerMinute = res.CentPerMinute,
                    TotalMinute = res.TotalMinute,
                    CurrencyCode = res.CurrencyCode,
                    CountryCode = res.CountryCode,
                    RatePerMinSign = sign
                });

            }
            return result;


        }


        public ActionResult AddAutoRefill(string MemberID, string Pin, double ReFill_Amount, string Card_Number)
        {
            //var data = _repository.AddAu(MemberID, Pin, ReFill_Amount, Card_Number);
            return null;
        }

        [CompressFilter]
        public ActionResult Plans()
        {
            var plans = new PlanRate();
            return View(plans);
        }

        public ActionResult AdditionalPlans()
        {
            return View(new GenericModel());
        }

        public ActionResult IndiaOneCent()
        {
            return View(new GenericModel());
        }

        public ActionResult IndiaUnlimited()
        {
            return View(new GenericModel());
        }


        [HttpPost]
        public ActionResult BuyAdditionalPlans(ShoppingCartModel model)
        {
            //For India 1 Cent plan
            //163 1 CENT PLAN
            //164 CANADA 1 CENT PLAN
            //125 CANADA TALK CITY
            //126 CANADA TALK MOBILE
            //120 TALK CITY
            //121 TALK MOBILE


            //India unlimited plan

            //176 CANADA MOBILE UNLIMITED $9.99
            //178 CANADA UNLIMITED $14.99
            //180 CANADA UNLIMITED $19.99
            //175 MOBILE UNLIMITED $9.99
            //177 UNLIMITED $14.99
            //179 UNLIMITED $19.99
            if (model.CountryFrom == 0)
            {
                model.CountryFrom = (int)Session["CountrybyIp"];
            }
            if (model.CountryFrom == 1)
            {
                switch (model.PlanId)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "175";
                            model.PlanName = "MOBILE UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(9.99);
                            model.ServiceFee = Convert.ToDecimal(3.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "177";
                            model.PlanName = "UNLIMITED $14.99";
                            model.Price = Convert.ToDecimal(14.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "179";
                            model.PlanName = "UNLIMITED $19.99";
                            model.Price = Convert.ToDecimal(19.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "163";
                            model.PlanName = "1 Cent Plan";
                            model.Price = Convert.ToDecimal(15);
                            model.ServiceFee = Convert.ToDecimal(4.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "121";
                            model.PlanName = "Talk Mobile";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "120";
                            model.PlanName = "Talk City";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            model.CountryTo = 130;
                            break;
                        }
                }
                model.CurrencyCode = "USD";

            }
            else if (model.CountryFrom == 2)
            {
                switch (model.PlanId)
                {
                    case "IU9.99":
                        {
                            model.PlanId = "176";
                            model.PlanName = "CANADA MOBILE UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(9.99);
                            model.ServiceFee = Convert.ToDecimal(3.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU14.99":
                        {
                            model.PlanId = "178";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(14.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IU19.99":
                        {
                            model.PlanId = "180";
                            model.PlanName = "CANADA UNLIMITED $9.99";
                            model.Price = Convert.ToDecimal(19.99);
                            model.ServiceFee = Convert.ToDecimal(7.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO1":
                        {
                            model.PlanId = "164";
                            model.PlanName = "Canada 1 Cent Plan";
                            model.Price = Convert.ToDecimal(15);
                            model.ServiceFee = Convert.ToDecimal(4.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO2":
                        {
                            model.PlanId = "126";
                            model.PlanName = "Canada Talk Mobile";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                    case "IO3":
                        {
                            model.PlanId = "125";
                            model.PlanName = "Canada Talk City";
                            model.Price = Convert.ToDecimal(10);
                            model.ServiceFee = Convert.ToDecimal(2.99);
                            model.IsAutoRefill = true;
                            model.CouponCode = string.Empty;
                            break;
                        }
                }
                model.CurrencyCode = "CAD";
            }
            model.CountryTo = 130;
            var countryfromdata = CacheManager.Instance.GetFromCountries();
            var callingfrom = countryfromdata.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryFrom));

            var countrytodata = CacheManager.Instance.GetCountryListTo();
            var callingto = countrytodata.FirstOrDefault(a => a.Id == SafeConvert.ToString(model.CountryTo));


            model.FromToMapping = SafeConvert.ToString(model.PlanId);

            model.CallingFrom = callingfrom != null ? callingfrom.Name : string.Empty;
            model.CallingTo = callingto != null ? callingto.Name : string.Empty;

            Session["Cart"] = null;
            Session["Cart"] = model;
            return RedirectToAction("Index", "Cart");

        }


    }


    //public class JsonNetResult : JsonResult
    //{
    //    public override void ExecuteResult(ControllerContext context)
    //    {
    //        if (context == null)
    //            throw new ArgumentNullException("context");

    //        var response = context.HttpContext.Response;

    //        response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

    //        if (ContentEncoding != null)
    //            response.ContentEncoding = ContentEncoding;

    //        if (Data == null)
    //            return;

    //        // If you need special handling, you can call another form of SerializeObject below
    //        var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);
    //        response.Write(serializedObject);
    //    }


    //}

}
