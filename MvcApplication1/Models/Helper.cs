using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Raza.Model;
using System.Globalization;

namespace MvcApplication1.Models
{
    public class Helper : BaseRazaViewModel
    {
        public static OrderHistoryModel ConvertOrderHistoryToModel(OrderHistory orderHistory)
        {
            var result = new OrderHistoryModel { Orders = new List<OrderHistoricModel>() };

            foreach (OrderHistoric order in orderHistory.Orders)
            {
                var or = new OrderHistoricModel()
                {
                    OrderId = order.OrderId,
                    PlanId = order.PlanId,
                    PlanName = order.PlanName,
                    AccountNumber = order.AccountNumber,
                    CurrencyCode = order.CurrencyCode,
                    TransactionAmount = order.TransactionAmount,
                    TransactionDate = Convert.ToDateTime(String.Format("{0:G}", order.TransactionDate)),
                    AllowCallDetails = order.AllowCDR,
                    AllowOneTouchSetup = true, // allow one touch setup
                    AllowAutoRefill = order.AutoRefillStatus,
                    AllowRecharge = order.AllowRecharge,
                    AllowPinlessSetup = order.AllowPinless,
                    AllowQuickkeySetup = order.AllowQuickkey,
                    TransactionType = order.TransactionType,
                    MyAccountBal = order.MyAccountBal,
                    ServiceFee = order.ServiceFee,
                    IsActivePlan = order.IsActivePlan,
                    ShowBalance = order.ShowBalance,
                   
                };
                result.Orders.Add(or);
            }
            return result;
        }


        public static PinlessSetupsModel ConvertPinlessSetupsToModel(PinlessSetups setup)
        {
            var result = new PinlessSetupsModel();
            result.AllSetup = new List<PinlessSetupModel>();
            foreach (PinlessSetup eachsetup in setup.Setups)
            {
                var or = new PinlessSetupModel()
                {
                    AccessNumber = eachsetup.CardType,
                    CustomerSerialNumber = eachsetup.CustomerSrNoPin,
                    OriginalPrice = eachsetup.OrigPrice,
                    OrderId = eachsetup.OrderId,
                    PinNumber = eachsetup.Pin,
                    PlanName = eachsetup.PlanName
                };
                result.AllSetup.Add(or);
            }
            return result;
        }

        public static QuickeysSetupsModel ConvertQuickeysSetupsToModel(QuickeysSetups setup)
        {
            var result = new QuickeysSetupsModel { AllSetup = new List<QuickeysSetupModel>() };
            foreach (EachQuickeysSetup eachsetup in setup.Setups)
            {
                var or = new QuickeysSetupModel()
                {
                    PlanName = eachsetup.PlanName,
                    OriginalPrice = eachsetup.OriginalPrice,
                    Pin = eachsetup.PinNumber
                };

                result.AllSetup.Add(or);
            }
            return result;
        }

        public static CallDetailsModel ConvertCallDetailsToModel(CallDetails calldetail, DateTime sortdate)
        {
            var result = new CallDetailsModel { AllCalls = new List<EachCallModel>() };
            IEnumerable<EachCall> query;

            var data = calldetail.AllCalls.Where(a => a.CallDate.Month == sortdate.Month).ToList();

            foreach (EachCall eachcall in data)
            {
                var or = new EachCallModel()
                {
                    CallDate = Convert.ToDateTime(String.Format("{0:G}", eachcall.CallDate)),
                    SourceNumber = eachcall.SourceNumber,
                    DestinationNumber = eachcall.DestinationNumber,
                    DestinationCity = eachcall.DestinationCity,
                    CallDuration = eachcall.CallDuration,
                    CallAmount = eachcall.CallAmount
                };

                result.AllCalls.Add(or);
            }

            return result;

            //if (startdate != DateTime.MinValue && enddate != DateTime.MinValue)
            //{
            //    query = from i in calldetail.AllCalls
            //            where
            //              Convert.ToDateTime(i.CallDate).Month > startdate.Month
            //                && Convert.ToDateTime(i.CallDate).Month < enddate.Month
            //            select i;

                
            //}
            //else
            //{
            //    query = from i in calldetail.AllCalls
            //            select i;
            //}
            //string format = "M/d/yyyy";
            //foreach (EachCall eachcall in query)
            //{
            //    var or = new EachCallModel()
            //    {
            //        CallDate =  Convert.ToDateTime(String.Format("{0:G}", eachcall.CallDate)),  
            //        SourceNumber = eachcall.SourceNumber,
            //        DestinationNumber = eachcall.DestinationNumber,
            //        DestinationCity = eachcall.DestinationCity,
            //        CallDuration = eachcall.CallDuration,
            //        CallAmount = eachcall.CallAmount
            //    };

            //    result.AllCalls.Add(or);
            //}

            //return result;
        }


    }
}