using System.Collections.Generic;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class OnetouchSetupEdit : BaseRazaViewModel
    {
        public int Serial_No { get; set; }

        public string CountryCode { get; set; }
        public string Pin { get; set; }
        public string OrderId { get; set; }
        public string PinNumber { get; set; }
        public string Destination { get; set; }
        public string AvailableNumber { get; set; }
        public string DestinationNumber
        {
            get
            {
                return  CountryCode + "" + Destination ;
            }
            set
            {
                value = DestinationNumber;
            }
        }
        public SelectList ToCountryList { get; set; }
        public SelectList CountryList { get; set; }
        public string Plan_Name { get; set; }
        public string Availno { get; set; }
        public string OneTouch_Name { get; set; }
        public string OneTouch_Number { get; set; }
        public string Added_By { get; set; }
        public string Session { get; set; }
        public string ListOfRecords { get; set; }
        public List<OneTouchSetups> List { get; set; }

    }
}