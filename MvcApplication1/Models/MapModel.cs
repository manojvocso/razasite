using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;

namespace MvcApplication1.Models
{
    public class MapModel : BaseRazaViewModel
    {
        public List<LowestRates> MapRateList=new List<LowestRates>();
        public int MapCountry
        {
            get;
            set;
        }

    }
}