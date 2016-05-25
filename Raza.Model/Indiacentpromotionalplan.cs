using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class Indiacentpromotionalplanmodel
    {
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public string PlanName { get; set; }
        public string PlanId { get; set; }
        public string ServiceFee { get; set; }
        public string CentPerMinute { get; set; }
        public string TotalCharge { get; set; }
       
        public string TotalMinute { get; set; }
        public string CurrencyCode { get; set; }
        public string RatePerMinSign { get; set; }
        public string CountryCode { get; set; }
  
       
    }
    public class IndiaCentPlanModel
    {
        public List<Indiacentpromotionalplanmodel> IndiaCentPlanRate { get; set; }
    }
}
