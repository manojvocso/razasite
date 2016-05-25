using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class OpenComplaintModel : BaseRazaViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderId { get; set; }
        public string Email { get; set; }
        public string MemberID { get; set; }
        public string ContactPhone { get; set; }
        public string Access_Number { get; set; }
        public string Destination_Number { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public List<Country> CountryToList { get; set; }
        public List<Country> CountryFromList { get; set; }
        public List<Country> AllCountryList { get; set; }
        public List<PlanInfo> PlansList { get; set; }
        public List<State> StateList { get; set; } 

    }
}