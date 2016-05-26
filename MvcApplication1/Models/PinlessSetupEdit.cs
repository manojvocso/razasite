using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class PinlessSetupEdit : BaseRazaViewModel
    {
        public string AniName { get; set; }
        public string OrderID { get; set; }
        public string CustomerId { get; set; }
        public string RequestedBy { get; set; }
        public string Pin { get; set; }
        public string AniNumber { get; set; }
        public string CoutryCode { get; set; }
        //public string txtAreaCodeFrom1 { get; set; }
        //public string txtPhoneFrom1 { get; set; }
        //public string txtPhoneFrom2 { get; set; }
        public string PinNumber { get; set; }
        public List<Country> CountryFromList { get; set; }
        public List<PinLessSetupNumbers> PinlessNumberList { get; set; }

        //public List<PinLessNumbers> PinlessNumbersList { get; set; }
    }

    //public class PinLessNumbers
    //{
    //    public string PinlessNumber1 { get; set; }
    //    public string PinlessNumber2 { get; set; }
    //    public string PinlessNumber3 { get; set; }

    //}

}