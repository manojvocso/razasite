using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class QuickSignUp : BaseRazaViewModel
    {
        public int CountryTo { get; set; }
        public int CountryFrom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone_Number { get; set; }
        public string Error { get; set; }
    }

}
