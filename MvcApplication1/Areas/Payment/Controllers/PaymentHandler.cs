using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raza.Model;
using Raza.Model.PaymentProcessModel;
using Raza.Common;

namespace MvcApplication1.Areas.Payment.Controllers
{
    public class PaymentHandler
    {
        public PayFlow_SaleCC_VeriSign CreditCardPayment(PayPalCheckoutModel info, BillingInfo billinfInfo, string messagetype = "sale")
        {
            var dd = new PayFlow_SaleCC_VeriSign();
            dd.CVV2MATCH = "Y";
            dd.AVSZIP = "Y";
            dd.AVSADDR = "Y";
            //return dd;

            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            PayFlow_SaleCC_VeriSign ccSaleCcVeriSign = new PayFlow_SaleCC_VeriSign();
            string ipaddress = string.Empty;

            ccSaleCcVeriSign.SaleCC(Guid.NewGuid().ToString(), info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate, info.ProcessPaymentInfo.Cvv.ToString(),
                info.ProcessPlanInfo.Amount, info.ProcessPlanInfo.CurrencyCode, billinfInfo.FirstName, billinfInfo.LastName,
                billinfInfo.City, billinfInfo.State, billinfInfo.ZipCode,
                billinfInfo.Country, billinfInfo.Address, billinfInfo.Email, billinfInfo.PhoneNumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2);

            //RazaLogger.WriteInfo(string.Format("parameter in Creditcardpayment is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}", info.RechargeValues.CardNumber,
            //    info.RechargeValues.ExpDate, info.RechargeValues.cvv,
            //    info.RechargeValues.Amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName, info.RechargeValues.LastName,
            //info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode, info.RechargeValues.Country, info.RechargeValues.Address,
            //usercontext.Email, usercontext.ProfileInfo.PhoneNumber, info.RechargeValues.IpAddress, info.RechargeValues, cavv, eciflag, Xid, comment, comment2
            //));

            RazaLogger.WriteInfo(string.Format("ccSale VeriSign {0}", ccSaleCcVeriSign.RESPMSG));

            return ccSaleCcVeriSign;


            //string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "hello";
            //SaleCC_VeriSign ccSaleCcVeriSign = new SaleCC_VeriSign();

            //ccSaleCcVeriSign.SaleCC(Guid.NewGuid().ToString(), "4111111111111111", "0115", "123", 10, "USD", "John", "Deo", "Chicago", "IL", "16606", "ABC", "abc@abc.com", "123213", "127.0.0.1",
            //    cavv, eciflag, Xid, comment, comment2);

            //RazaLogger.WriteInfo(string.Format("ccSale VeriSign {0}",ccSaleCcVeriSign.RESPMSG));

            //return ccSaleCcVeriSign;

            //temp
            //  return CreditCardPayment();


            //HttpContext CurrContext = HttpContext.Current;

            //// ###Items
            //// Items within a transaction.
            //Item item = new Item
            //{
            //    name = info.RechargeValues.TransactionType,
            //    currency = "USD",
            //    price = info.RechargeValues.Amount.ToString(),
            //    quantity = "1",
            //    sku = "sku"
            //};

            //List<Item> itms = new List<Item> { item };
            //ItemList itemList = new ItemList { items = itms };

            //// ###Address
            //// Base Address object used as shipping or billing
            //// address in a payment.
            //Address billingAddress = new Address
            //{
            //    city = info.RechargeValues.City,
            //    country_code = "US",
            //    line1 = info.RechargeValues.Address,
            //    postal_code = info.RechargeValues.ZipCode,
            //    state = "OH"

            //};

            //// ###CreditCard
            //// A resource representing a credit card that can be
            //// used to fund a payment.
            //CreditCard crdtCard = new CreditCard
            //{
            //    billing_address = billingAddress,
            //    cvv2 = info.RechargeValues.cvv,
            //    expire_month = int.Parse(info.RechargeValues.ExpMonth),
            //    expire_year = int.Parse(info.RechargeValues.ExpYear),
            //    first_name = info.RechargeValues.FirstName,
            //    last_name = info.RechargeValues.LastName,
            //    number = info.RechargeValues.CardNumber,
            //    type = info.RechargeValues.CardType.ToLower()

            //};

            //// ###Details
            //// Let's you specify details of a payment amount.
            //Details details = new Details
            //{
            //    subtotal = info.RechargeValues.Amount.ToString(),
            //    tax = SafeConvert.ToString(info.RechargeValues.ServiceFee)

            //};
            ////details.shipping = "1";

            //// ###Amount
            //// Let's you specify a payment amount.
            //Amount amnt = new Amount
            //{
            //    currency = "USD",
            //    total = SafeConvert.ToString(info.RechargeValues.Amount + info.RechargeValues.ServiceFee),
            //    details = details
            //};
            //// Total must be equal to sum of shipping, tax and subtotal.

            //// ###Transaction
            //// A transaction defines the contract of a
            //// payment - what is the payment for and who
            //// is fulfilling it. 
            //Transaction tran = new Transaction
            //{
            //    amount = amnt,
            //    description = info.RechargeValues.TransactionType,
            //    item_list = itemList
            //};


            //// The Payment creation API requires a list of
            //// Transaction; add the created `Transaction`
            //// to a List
            //List<Transaction> transactions = new List<Transaction> { tran };

            //// ###FundingInstrument
            //// A resource representing a Payer's funding instrument.
            //// For direct credit card payments, set the CreditCard
            //// field on this object.
            //FundingInstrument fundInstrument = new FundingInstrument { credit_card = crdtCard };

            //// The Payment creation API requires a list of
            //// FundingInstrument; add the created `FundingInstrument`
            //// to a List
            //List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument> { fundInstrument };

            //// ###Payer
            //// A resource representing a Payer that funds a payment
            //// Use the List of `FundingInstrument` and the Payment Method
            //// as `credit_card`
            //Payer payr = new Payer();
            //payr.funding_instruments = fundingInstrumentList;
            //payr.payment_method = "credit_card";

            //// ###Payment
            //// A Payment Resource; create one using
            //// the above types and intent as `sale` or `authorize`
            //Payment pymnt = new Payment();
            //if (messagetype == "sale")
            //{
            //    pymnt.intent = "sale";
            //}
            //else if (messagetype == "authorize")
            //{
            //    pymnt.intent = "authorize";
            //}

            //pymnt.payer = payr;
            //pymnt.transactions = transactions;

            //try
            //{
            //    // ### Api Context
            //    // Pass in a `APIContext` object to authenticate 
            //    // the call and to send a unique request id 
            //    // (that ensures idempotency). The SDK generates
            //    // a request id if you do not pass one explicitly. 
            //    // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext..


            //    // Create a payment using a valid APIContext
            //    Payment createdPayment = pymnt.Create(Api);

            //    // var paymentResponse = JObject.Parse(createdPayment.ConvertToJson());

            //    //CurrContext.Items.Add("ResponseJson", JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented));

            //    return createdPayment;
            //}
            //catch (PayPal.Exception.PayPalException ex)
            //{
            //    // CurrContext.Items.Add("Error", ex.Message);
            //    RazaLogger.WriteInfo("catch msg in Creditcardpayment=>" + ex.ToString());
            //    throw new Exception(ex.ToString());
            //}

            //return null;

            //    CurrContext.Items.Add("RequestJson", JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented));

        }

        public PayFlow_AuthCC_VeriSign CreditCardAuthorize(PayPalCheckoutModel info, BillingInfo billingInfo)
        {

            string cavv = string.IsNullOrEmpty(info.CentinelAuthenticationResponse.CentinelCavv) ? string.Empty : info.CentinelAuthenticationResponse.CentinelCavv;
            string eciflag = string.IsNullOrEmpty(info.CentinelAuthenticationResponse.CentinelEciFlag)
                ? string.Empty
                : info.CentinelAuthenticationResponse.CentinelEciFlag;
            string Xid = string.IsNullOrEmpty(info.CentinelAuthenticationResponse.CentinelXid)
                ? string.Empty
                : info.CentinelAuthenticationResponse.CentinelXid;
            string comment2 = "";
            //string comment = "Credit card transaction.";
            string comment = "Online " + info.ProcessPaymentInfo.TransactionType + " CustomerID: " + billingInfo.MemberId;

            PayFlow_AuthCC_VeriSign vreCcVeriSign = new PayFlow_AuthCC_VeriSign();

            var phonenumber = billingInfo.PhoneNumber.Contains("-") ? billingInfo.PhoneNumber.Replace("-", "") : billingInfo.PhoneNumber;

            var amount = info.ProcessPlanInfo.TotalAmount;

            RazaLogger.WriteErrorForMobile(string.Format("parameter in creditcard authorize is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
                info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate2,
                info.ProcessPaymentInfo.Cvv, amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode, billingInfo.Country,
                billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2));

            vreCcVeriSign.AuthCC(Guid.NewGuid().ToString(), info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate2,
                info.ProcessPaymentInfo.Cvv.ToString(), amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode, billingInfo.Country,
                billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2);

            RazaLogger.WriteErrorForMobile(string.Format("AuthCC_VeriSign responseMsg={0},Result={1},AVSADDR={2},AVSZIP={3},CVV2MATCH={4}", vreCcVeriSign.RESPMSG, vreCcVeriSign.RESULT, vreCcVeriSign.AVSADDR, vreCcVeriSign.AVSZIP, vreCcVeriSign.CVV2MATCH));

            return vreCcVeriSign;
        }

        public PayFlow_CaptureCC_VeriSign CreditCardCapture(PayPalCheckoutModel info, BillingInfo billingInfo, string OrigId)
        {

            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            PayFlow_CaptureCC_VeriSign vreCcVeriSign = new PayFlow_CaptureCC_VeriSign();

            var phonenumber = billingInfo.PhoneNumber.Contains("-") ? billingInfo.PhoneNumber.Replace("-", "") : billingInfo.PhoneNumber;

            var amount = info.ProcessPlanInfo.TotalAmount;

            RazaLogger.WriteErrorForMobile(string.Format("parameter in creditcard Capture is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}",
                OrigId, info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate2,
                info.ProcessPaymentInfo.Cvv, amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode,
                billingInfo.Country, billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2));

            vreCcVeriSign.CaptureCC(Guid.NewGuid().ToString(), OrigId, info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate2,
                info.ProcessPaymentInfo.Cvv.ToString(), amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode,
                billingInfo.Country, billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2);


            RazaLogger.WriteInfo(string.Format("CaptureCC_VeriSign ResponseMsg{0}, Result{1}", vreCcVeriSign.RESPMSG, vreCcVeriSign.RESULT));

            return vreCcVeriSign;
        }

        public PayFlow_VoidCC_VeriSign CreditCardVoid(PayPalCheckoutModel info, BillingInfo billingInfo, string OrigId)
        {

            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            PayFlow_VoidCC_VeriSign vreCcVeriSign = new PayFlow_VoidCC_VeriSign();

            string phonenumber = billingInfo.PhoneNumber.Contains("-") ? billingInfo.PhoneNumber.Replace("-", "") : billingInfo.PhoneNumber;
            var amount = info.ProcessPlanInfo.Amount;

            RazaLogger.WriteInfo(string.Format("parameter in creditcard Void is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}",
                OrigId, info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate,
                info.ProcessPaymentInfo.Cvv, amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode,
                billingInfo.Country, billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2));

            vreCcVeriSign.VoidCC(Guid.NewGuid().ToString(), OrigId, info.ProcessPaymentInfo.CardNumber, info.ProcessPaymentInfo.ExpDate,
                info.ProcessPaymentInfo.Cvv.ToString(), amount, info.ProcessPlanInfo.CurrencyCode, billingInfo.FirstName,
                billingInfo.LastName, billingInfo.City, billingInfo.State, billingInfo.ZipCode,
                billingInfo.Country, billingInfo.Address, billingInfo.Email, phonenumber, info.ProcessPaymentInfo.IpAddress,
                cavv, eciflag, Xid, comment, comment2);


            RazaLogger.WriteInfo(string.Format("VoidCC_VeriSign ResponseMsg{0}, Result{1}", vreCcVeriSign.RESPMSG, vreCcVeriSign.RESULT));

            return vreCcVeriSign;
        }


    }
}