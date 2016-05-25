using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class CallForwarding : BaseRazaViewModel
    {
        public List<Country> ToCountryList { get; set; }
        public  string CountryCode { get; set; }
        public List<GetList800> ListNumber_800 { get; set; }
        public string Destination_Number { get; set; }
        public string Activation_Date { get; set; }
        public string Expiry_Date { get; set; }
        public  string Forwarding_Name { get; set; }
        public string Added_By { get; set; }
        public string Number_800 { get; set; }
        public string Pin { get; set; }
        public string ErrorMsg { get; set; }
        public  string order_id { get; set; }
        public List<GetForwardList> ForwardNumberList { get; set; } 
        public  int SNumber { get; set; }
    }
}