using System.Collections.Generic;
using Raza.Model;

namespace MvcApplication1.Models
{

    public class HomeViewModel : BaseRazaViewModel
    {
        public  List<Country> CountryListFrom { get; set; }
        public  List<Country> CountryListTo { get; set; }
        public string TopupMobileNumber { get; set; }
        public string TopMobileCountry { get; set; }
        public string Operator { get; set; }
        public string Amount { get; set; }
        public string CallingFrom { get; set; }
        public string CallingTo { get; set; }

        public List<Country> TopupCountryList = new List<Country>(); 

        public List<HomePageRateModel> HomepageRates=new List<HomePageRateModel>(); 

    } 
      
    
}