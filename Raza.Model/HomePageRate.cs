using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
   public class HomePageRate
    {
       public HomePageRate()
       {
              Homepagerates=new List<HomePageRateModel>();
       }
       public List<HomePageRateModel> Homepagerates { get; set; }

       public static HomePageRate CreateModel(string data)
       {
           try
           {


               var homepagerate = new HomePageRate();

               if (data.Split('|').Length > 1)
               {
                   string[] allrows = data.Split('|')[1].Split(',');
                   if (allrows.Length > 0)
                   {
                       foreach (string eachrow in allrows)
                       {
                          homepagerate.Homepagerates.Add(new HomePageRateModel()
                           {
                               CountryId = SafeConvert.ToInt32(eachrow.Split('~')[0]),
                               CountryName = eachrow.Split('~')[1],
                               LowestRate = Convert.ToDouble(eachrow.Split('~')[2])

                           });
                       }
                   }
               }
               return homepagerate;
           }
           catch (Exception ex)
           {
            return   new HomePageRate();
           }
           
       }
    }

    public class HomePageRateModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public double LowestRate { get; set; }
        public string CurrencySign { get; set; }
        public string FlagLogo { get; set; }
        public int CountryFrom { get; set; }
    }


    
}
