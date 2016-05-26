using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class PromotionViewModel
    {
        public PromotionViewModel()
        {
            
        }
        public string CallingFrom { get; set; }
        public string CallingTo { get; set; }
        public List<Country> FromCountryList { get; set; }
        public List<Country> ToCountryList { get; set; } 
    }
}