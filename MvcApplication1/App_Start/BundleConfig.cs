using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MvcApplication1
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            #region Bundle For Mobile Site

            #region Bundles for Css for Mobile
            bundles.Add(new StyleBundle("~/Content/MobileTheme/css/css")
                .Include("~/Content/MobileTheme/css/bootstrap.min.css")
                .Include("~/Content/MobileTheme/css/font-awesome.min.css") //font awesome icons
                .Include("~/Content/MobileTheme/css/drop_down.css") //side menu
                .Include("~/Content/MobileTheme/css/dropdown.min.css") //semantic dropdown
                .Include("~/Content/MobileTheme/css/transition.min.css") // sematic dropdown transitions
                .Include("~/Content/MobileTheme/css/tooltip_skin_variation.css")  //home slider for promotions
                .Include("~/Content/MobileTheme/css/Raza-Mobile.css") // custom by developers
                .Include("~/Content/MobileTheme/css/flag.min.css") // sprite flags for semantic search dropdown
                .Include("~/Content/MobileTheme/css/flaticon.css") // flat icons
                // .Include("~/Content/MobileTheme/css/jquery-accordion-menu.css") // flat icons
                .Include("~/Content/MobileTheme/css/accormodation.min.css") // faq accordian
                .Include("~/Content/MobileTheme/css/default.css") // calendor datepicker
                .Include("~/Content/MobileTheme/css/default.date.css") // calendor datepicker
                .Include("~/Content/MobileTheme/css/jquery.mmenu.all.css") // faq accordian
                .Include("~/Content/MobileTheme/css/main_css.css") // main site css
                );


            bundles.Add(new StyleBundle("~/Content/home").Include("~/Content/MobileTheme/css/bootstrap.min.css"));

            #endregion

            #region Bundles for Jquery for Mobile

            var jqueryCdnPath = "http://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js";
            bundles.Add(new ScriptBundle("~/bundles/jqueryMobile", jqueryCdnPath).Include(
                "~/Content/MobileTheme/js/jquery-1.11.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/mvcfoolproof.unobtrusive.min.js",
                "~/Scripts/MvcFoolproofJQueryValidation.min.js"));

            //Bundle for Datepicker control
            bundles.Add(new ScriptBundle("~/bundles/datePicker").Include(
                "~/Content/MobileTheme/assets/pickadate/picker.js",
                "~/Content/MobileTheme/assets/pickadate/picker.date.js",
                "~/Content/MobileTheme/assets/pickadate/legacy.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryraza")
                .Include("~/Content/MobileTheme/js/bootstrap.min.js")
                .Include("~/Content/MobileTheme/js/dropdown.min.js")
                .Include("~/Content/MobileTheme/js/transition.min.js")
                .Include("~/Content/MobileTheme/js/jquery.mmenu.min.all.js")
                .Include("~/Content/MobileTheme/js/jquery.bwlaccordion.min.js")
                //.Include("~/Content/MobileTheme/js/jquery.sky.carousel-1.0.2.min.js")
                .Include("~/Content/MobileTheme/js/jquery.payment.js")
                .Include("~/Content/MobileTheme/js/dropdown_scripts.js")
                .Include("~/Content/MobileTheme/js/loadingoverlay.min.js")
                .Include("~/Content/MobileTheme/js/jquery.buttonLoader.min.js")
                );
            
            #endregion

            #endregion

            BundleTable.EnableOptimizations = true;




        }

    }
}

