using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace Raza.Model
{
    public class RatePlans
    {
        public List<EachRatePlan> Plans { get; set; }

        public RatePlans()
        {
            Plans = new List<EachRatePlan>();
        }


        public static RatePlans Create(string data)
        {
            var plansfound = new RatePlans();
            string[] allrows = data.Split('~');

            if (allrows.Length > 1)
            {
                for (int i = 1; i < allrows.Length && allrows[i].Length > 0; i++)
                {
                    plansfound.Plans.Add(new EachRatePlan
                    {
                        FromToMapping = allrows[i].Split(',')[0],  //CardId
                        CardTypeName = allrows[i].Split(',')[1],
                        PlanId = allrows[i].Split(',')[2],
                        PlanAmount = SafeConvert.ToDecimal(allrows[i].Split(',')[3]),
                        RatePerMin = Convert.ToDecimal(allrows[i].Split(',')[4]),
                        ServiceFee = SafeConvert.ToDecimal(allrows[i].Split(',')[5]),
                        Discount = SafeConvert.ToDecimal(allrows[i].Split(',')[6]),
                        CardType = int.Parse(allrows[i].Split(',')[7]),
                        CurrencyCode = allrows[i].Split(',')[8],
                        TotalMinutes = SafeConvert.ToDecimal(allrows[i].Split(',')[9]),
                        PlanCategoryId = allrows[i].Split(',')[10]
                    });
                }
            }
            return plansfound;
        }
    }

    public class EachRatePlan
    {
        public string FromToMapping { get; set; }
        public string CardTypeName { get; set; }
        public string PlanId { get; set; }
        public decimal PlanAmount { get; set; }
        public decimal RatePerMin { get; set; }
        public string Minute { get; set; }
        public int CardType { get; set; }
        public decimal Discount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal DiscountedRate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TotalMinutes { get; set; }
        public bool IsAutoRefill { get; set; }
        public bool IsEnrollToExtraMinute { get; set; }
        public string PlanCategoryId { get; set; }
    }
}

