using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.Controllers;
using Raza.Model;
using Raza.Repository;

namespace MvcApplication1.Models
{
   
    public class Features: BaseRazaViewModel
    {
        public List<Country> CountryTo { get; set; }
        public List<Country> CountryFrom { get; set; }
        public string Email { get; set; }

        public bool Status { get; set; }
        public string Errormsg { get; set; }

    }
}