using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class Country
    {
        public  string Id { get; set; }
        public  string Name { get; set; }
        public string CountryCode { get; set; }
        public string CountryFlag { get; set; }
        public string CountCode { get; set; }
        public string RateType { get; set; }
        public string ImageClass { get; set; }
      

        public string CountryType
        {
            get
            {
                if (Name.ToLower().Contains("mobile"))
                {
                    return "Mobile";
                }
                else if (Name.ToLower().Contains("mobile"))
                {
                    return "Mobile";
                }
                else
                {
                    return "Country";
                }
            }
        }

        public Country()
        {
            
        }

        public Country(string id, string name,string countrycode, string countcode, string ratetype )
        {
            Id = id;
            Name = name;
            ImageClass = "big_" + id;
            CountryCode = "011" +countrycode;
            CountCode = countcode;
            RateType = ratetype;
        }
           
        
    }
}
