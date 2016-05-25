using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    using System.Globalization;

    public class OrderHistory
    {
        public OrderHistory()
        {
            Orders = new List<OrderHistoric>();
        }

        public List<OrderHistoric> Orders { get; set; }


        public static OrderHistory Create(string data, OrderHistorySnapshot orderhistorysnapshots)
        {
            var orders = new OrderHistory();

            if (data.Split('|').Length > 1)
            {

                string[] allrows = data.Split('|')[1].Split('~');
                foreach (string eachrow in allrows)
                {
                    //OrderID,Plan_ID,Plan_Name,Pin,OrderDate,Price,TransType,allow_recharge,
                    //allow_Cdr,allow_pinless,allow_quickey,autorefill_status,allow_callforwarding,pinstatus

                    string planid = eachrow.Split(',')[1];
                    string plan = eachrow.Split(',')[2];
                    string accountNo = eachrow.Split(',')[3];
                    string orderid = eachrow.Split(',')[0];

                    //OrderHistoricPlanInfoSnapshot ops = orderhistorysnapshots.OrderInfos.FirstOrDefault(x => x.PlanName.ToLower() == plan.ToLower() && x.AccountNumber.Equals(accountNo));
                    var ops = orderhistorysnapshots.OrderInfos.FirstOrDefault(x => x.AccountNumber.Equals(accountNo));

                    DateTime transdate;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    if (!DateTime.TryParse(eachrow.Split(',')[4], culture, DateTimeStyles.None, out transdate))
                    {
                        transdate = DateTime.MinValue;
                    }

                    //orders.Orders.Add(new OrderHistoric()
                    //{
                    //    // <orderid>,<planname>,<pinnumber>,<transaction_date>,<currency> <amount>,<transaction_type>
                    //    OrderId = ops == null ? string.Empty : ops.OrderId,
                    //    PlanName = eachrow.Split(',')[1],
                    //    AccountNumber = eachrow.Split(',')[2],
                    //    TransactionDate = transdate,
                    //    CurrencyCode = eachrow.Split(',')[4].Split(' ')[0],
                    //    TransactionAmount = eachrow.Split(',')[4].Split(' ')[1],
                    //    TransactionType = eachrow.Split(',')[5],
                    //    AllowCDR = ops != null && ops.AllowCDR,
                    //    AllowPinless = ops != null && ops.AllowPinless,
                    //    AllowQuickkey = ops != null && ops.AllowQuickkey,
                    //    AllowRecharge = ops != null && ops.AllowRecharge,
                    //    AutoRefillStatus = ops != null ? ops.AutoRefillStatus : string.Empty,
                    //});
                    //OrderID,Plan_ID,Plan_Name,Pin,OrderDate,Price,TransType,allow_recharge,
                    //allow_Cdr,allow_pinless,allow_quickey,autorefill_status,allow_callforwarding,pinstatus
                    //|MR0396A1B14B,130,MOBILE DIRECT,1655034241,2/22/2014 3:13:04 PM,USD 10.00,Recharge,1,1,1,0,N,0,A
                    orders.Orders.Add(new OrderHistoric()
                    {
                        OrderId = ops == null ? string.Empty : ops.OrderId,
                        PlanId = eachrow.Split(',')[1],
                        PlanName = eachrow.Split(',')[2],
                        AccountNumber = eachrow.Split(',')[3],
                        TransactionDate = transdate,
                        CurrencyCode = eachrow.Split(',')[5].Split(' ')[0],
                        TransactionAmount = eachrow.Split(',')[5].Split(' ')[1],
                        TransactionType = eachrow.Split(',')[6],
                        AllowRecharge = eachrow.Split(',')[7] == "0" ? false : true,
                        AllowCDR = eachrow.Split(',')[8] == "0" ? false : true,
                        AllowPinless = eachrow.Split(',')[9] == "0" ? false : true,
                        AllowQuickkey = eachrow.Split(',')[10] == "0" ? false : true,
                        AutoRefillStatus = eachrow.Split(',')[11],
                        AllowCallForwading = eachrow.Split(',')[12] == "0" ? false : true,
                        IsActivePlan = eachrow.Split(',')[13] == "A" ? true : false,
                        ShowBalance = ops != null && ops.ShowBalance,
                        ServiceFee = ops == null ? string.Empty : ops.ServiceFee

                    });
                    
                }
                

            }
            return orders;
        }


    }
    public class OrderHistoric : OrderHistoricPlanInfoSnapshot
    {
        public string TransactionType { get; set; }

    }


}

