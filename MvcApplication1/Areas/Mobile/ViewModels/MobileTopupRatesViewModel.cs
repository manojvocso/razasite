using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class MobileTopupRatesViewModel : BaseMobileViewModel
    {
        public List<Country> RatesFromCountryList { get; set; }
        public List<MobileTopupOperator> DenominationList { get; set; }

        [Required(ErrorMessage = "The country is a required field")]
        public int RateFromCountry { get; set; }
        public string RateFromCountryName { get; set; }

        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string CarrierImage { get; set; }
        public string Carrier { get; set; }

        [Required(ErrorMessage = "Please select a amount")]
        public double SourceAmount { get; set; }
        public string OperatorCode { get; set; }
        public double DestinationAmount { get; set; }

    }
}