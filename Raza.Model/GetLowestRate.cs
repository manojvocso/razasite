using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
   public class GetLowestRate
    {
        public List<GetRateList> Rate { get; set; }
        public List<GetRateList> MobileRate { get; set; } 

        public string PlanNameLandline { get; set; }
        public string PlanIdLandline { get; set; }
        public string LowestRateLandline { get; set; }
        public string LowestCallIdLandline { get; set; }
        public string RateForLandline { get; set; }

        public string PlanNameMobile { get; set; }
        public string PlanIdMobile { get; set; }
        public string LowestRateMobile { get; set; }
        public string LowestCallIdMobile { get; set; }
        public string RateForMobile { get; set; }

        public static GetLowestRate GetRate(string data)
        {
            var lowestRate = new GetLowestRate();
            string[] allrows = data.Split('~');
            if (allrows.Length > 1)
            {
                var list=new List<GetRateList>();
                for (int i = 1; i < allrows.Length && allrows[i].Length > 0; i++)
                {
                    list.Add(new GetRateList
                    {
                        PlanId = allrows[i].Split(',')[0],
                        PlanName = allrows[i].Split(',')[1],
                        LowestRate = Convert.ToDouble(allrows[i].Split(',')[2]),
                        LowestCallId = allrows[i].Split(',')[3],
                        RateFor = allrows[i].Split(',')[4],
                        SubCountryId = allrows[i].Split(',')[5]
                    });
                }

                lowestRate.Rate = list;
            }

            return lowestRate;

        }

        public static GetLowestRate GetNewCustomerRate(string data)
        {
            var lowestRate = new GetLowestRate();
            string[] allrows = data.Split('~');
            if (allrows.Length > 1)
            {
                var list = new List<GetRateList>();
                for (int i = 1; i < allrows.Length && allrows[i].Length > 0; i++)
                {
                    list.Add(new GetRateList
                    {
                        PlanId = allrows[i].Split(',')[0],
                        PlanName = allrows[i].Split(',')[1],
                        LowestRate = Convert.ToDouble(allrows[i].Split(',')[2]),
                        LowestCallId = allrows[i].Split(',')[3],
                        RateFor = allrows[i].Split(',')[4],
                        
                    });
                }

                lowestRate.Rate = list;
            }

            return lowestRate;

        }

    }

    public class GetRateList
    {
        public string PlanName { get; set; }
        public string PlanId { get; set; }
        public double LowestRate { get; set; }
        public string LowestCallId { get; set; }
        public string RateFor { get; set; }
        public string SubCountryId { get; set; }
    }


    public class LowestRates
    {
        public string LandLineRate { get; set; }
        public string MobileRate { get; set; }
        public int CountrytoId { get; set; }
        public string RateperSign { get; set; }
        

    }


}
