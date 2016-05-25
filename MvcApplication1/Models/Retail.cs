using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;


namespace MvcApplication1.Models
{
    public class Retail:BaseRazaViewModel
    {
        public List<Country> CountryFrom { get; set; } 
        public int CallingFrom { get; set; }
        public int CallingTo { get; set; }
    }

    public class GenericModel : BaseRazaViewModel
    {
    }
}