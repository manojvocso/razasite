using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    using System.Globalization;

    public class OrderHistorySnapshot
    {
        public OrderHistorySnapshot()
        {
            OrderInfos = new List<OrderHistoricPlanInfoSnapshot>();
        }

        private List<string> plansForNotRechargeShowBalance = new List<string>
            {
                "Canada 1cent plan",
                "Canada Country Special",
                "Monthly Pack Canada",
                "1Cent plan",
                "City Talk",
                "Country Special",
                "Mobile Talk",
                "Monthly Pack",
                "Talk City",
                "Talk Mobile"
            };


        private List<string> plansForNotShowBalance = new List<string>
            {
               "Canada Mobile Unlimited $9.99",
                "Canada Unlimited  $14.99",
                "Canada Unlimited $19.99",
                "Canada 50 Cents a Day",
                "Mobile Unlimited $9.99",
                "Unlimited  $14.99",
                "Unlimited $19.99",
                "50 Cents a Day"
            };


        public List<OrderHistoricPlanInfoSnapshot> OrderInfos { get; set; }

        public OrderHistorySnapshot CreateSnapshot(string data)
        {
            var orders = new OrderHistorySnapshot();

            if (data.Split('|').Length > 1)
            {
                string[] allrows = data.Split('|')[1].Split('~');

                foreach (string eachrow in allrows)
                {
                    DateTime transdate;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    if (!DateTime.TryParse(eachrow.Split(',')[3], culture, DateTimeStyles.None, out transdate))
                    {
                        transdate = DateTime.MinValue;
                    }

                    var order = new OrderHistoricPlanInfoSnapshot()
                        {
                            OrderId = eachrow.Split(',')[0],
                            PlanName = eachrow.Split(',')[1],
                            AccountNumber = eachrow.Split(',')[2],
                            TransactionDate = transdate,
                            CurrencyCode = eachrow.Split(',')[4].Split(' ')[0],
                            TransactionAmount = eachrow.Split(',')[4].Split(' ')[1],
                            AllowRecharge = eachrow.Split(',')[5] == "0" ? false : true,
                            AllowCDR = eachrow.Split(',')[6] == "0" ? false : true,
                            AllowPinless = eachrow.Split(',')[7] == "0" ? false : true,
                            AllowQuickkey = eachrow.Split(',')[8] == "0" ? false : true,
                            AutoRefillStatus = eachrow.Split(',')[9],
                            UsesFrom = eachrow.Split(',')[10],
                            ServiceFee = eachrow.Split(',')[11].Split(' ')[0],
                            ShowBalance = true,
                            AllowCallForwading = eachrow.Split(',')[12] == "0" ? false : true,
                            IsActivePlan = eachrow.Split(',')[13] == "A" ? true : false,
                            PlanId = eachrow.Split(',')[14],
                            CountryFrom = SafeConvert.ToInt32(eachrow.Split(',')[15]),
                            CountryTo = SafeConvert.ToInt32(eachrow.Split(',')[16]),
                            AccountBalance = SafeConvert.ToDecimal(eachrow.Split(',')[17])

                        };

                    //if (plansForNotRechargeShowBalance.Any(a => a.ToLower() == order.PlanName.ToLower()))
                    //{
                    //    order.ShowBalance = true;
                    //    order.AllowRecharge = false;
                    //}

                    //if (plansForNotShowBalance.Any(a => a.ToLower() == order.PlanName.ToLower()))
                    //{
                    //    order.ShowBalance = false;
                    //    order.AllowRecharge = false;
                    //}


                    orders.OrderInfos.Add(order);

                }
            }
            return orders;
        }


    }

    public class OrderHistoricPlanInfoSnapshot
    {
        public string OrderId { get; set; }
        public string PlanName { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionAmount { get; set; }
        public bool AllowRecharge { get; set; }
        public bool AllowCDR { get; set; }
        public bool AllowPinless { get; set; }
        public bool AllowQuickkey { get; set; }
        public string AutoRefillStatus { get; set; }
        public bool AllowCallForwading { get; set; }
        public bool IsActivePlan { get; set; }
        public bool ShowBalance { get; set; }

        public string MyAccountBal { get; set; }
        public string ServiceFee { get; set; }

        public string UsesFrom { get; set; }
        public string PlanId { get; set; }
        public int CountryFrom { get; set; }
        public int CountryTo { get; set; }
        public decimal AccountBalance { get; set; }
        
    }
}
