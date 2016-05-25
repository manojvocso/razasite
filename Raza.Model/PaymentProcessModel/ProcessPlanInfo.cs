using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Raza.Model.PaymentProcessModel
{
    public class ProcessPlanInfo
    {
        public ProcessPlanInfo()
        {
            PinlessNumbers = new List<string>();
        }
        public string Pin { get; set; }
        //public string OldOrderId { get; set; }
        public string OrderId { get; set; }
        public int CardId { get; set; }
        public string CurrencyCode { get; set; }

        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }

        public string PlanName { get; set; }

        public bool IsAutoRefill { get; set; }
        public double AutoRefillAmount { get; set; }

        public double Amount { get; set; }
        public bool IsTryUsFree { get; set; }

        public double TotalAmount
        {
            get { return Amount + ServiceFee; }
        }

        public double ServiceFee { get; set; }

        public MobileTopupInfo MobileTopupInfo { get; set; }

        public List<string> PinlessNumbers { get; set; }

    }

    public class MobileTopupInfo
    {
        public string TopupMobileNumber { get; set; }
        public string TopupCountry { get; set; }
        public string TopupOperatorCode { get; set; }
        public string TopupOperatorName { get; set; }
        public double TopupSourceAmount { get; set; }
        public double TopupDestAmount { get; set; }
        public bool TopupStatus { get; set; }
        public string TopUpCountryCode { get; set; }
        public int TopupFromCountry { get; set; }
        public TopupType TopupType { get; set; }
    }
}
