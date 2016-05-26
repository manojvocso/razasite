using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class CountryWithLowestRateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal UsaMobileRate { get; set; }
        public decimal UsaLandlineRate { get; set; }
        public decimal CanadaMobileRate { get; set; }
        public decimal CanadaLandlineRate { get; set; }
        public decimal UkMobileRate { get; set; }
        public decimal UkLandlineRate { get; set; }


    }
}
