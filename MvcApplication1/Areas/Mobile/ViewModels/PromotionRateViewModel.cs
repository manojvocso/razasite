using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Areas.Mobile.ViewModels
{
    public class PromotionRateViewModel
    {
        public PromotionRateViewModel()
        {
            FromCountryList = new List<Country>();
            ToCountryList = new List<Country>();
            Denominations = new List<DenominationDropdownEntity>();
        }
        [Required(ErrorMessage = "The callingfrom is required")]
        public int CountryFrom { get; set; }

        [Required(ErrorMessage = "The callingto is required")]
        public int CountryTo { get; set; }
        public List<Country> FromCountryList { get; set; }
        public List<Country> ToCountryList { get; set; }
        public List<DenominationDropdownEntity> Denominations { get; set; }
        public string CustomerType { get; set; }
        public double Denomination { get; set; }
        public string CouponCode { get; set; }
        public bool IsOnlyForNewCustomer { get; set; }
        public bool IsNewCustomer { get; set; }
    }

    public class DenominationDropdownEntity
    {
        public double Denomination { get; set; }
        public int RegularMin { get; set; }
        public int ExtraMin { get; set; }

        public int TotalMin
        {
            get { return RegularMin + ExtraMin; }
        }
        public double RatePerMin { get; set; }
    }
}