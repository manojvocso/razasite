using System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Collections.Specialized;
//using CardinalCommerce;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml;
using MvcApplication1.Models;
//using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using PayPal;
using PayPal.Api.Payments;
using PayPal.OpenIdConnect;
using PayPal.Payments.Transactions;
using Raza.Common;
using Raza.Model;
using Raza.Repository;
using Address = PayPal.Api.Payments.Address;
using Authorization = System.Net.Authorization;

namespace MvcApplication1.Controllers
{
    public enum PaymentMethod
    {
        credit_card,
        paypal
    }

    public static class Configuration
    {
        public static Dictionary<string, string> GetConfiguration()
        {
            Dictionary<string, string> configurationMap = new Dictionary<string, string>();
            configurationMap.Add("mode", "sandbox");
            return configurationMap;
        }
    }

    public class CreditCartHandler
    {
        //        private CardinalCommerce.CentinelRequest request;

        //        void SendRequest()
        //        {

        //        }

        private static Dictionary<string, string> GetConfig()
        {
            var configMap = new Dictionary<string, string>();

            // Endpoints are varied depending on whether sandbox OR live is chosen for mode
            configMap.Add("mode", "sandbox");

            // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value
            // configMap.Add("connectionTimeout", "360000");
            // configMap.Add("requestRetries", "1");
            return configMap;
        }

        private string AccessToken
        {
            get
            {

                string accessToken = new OAuthTokenCredential(
                    "EBWKjlELKMYqRNQ6sYvFo64FtaRLRR5BdHEESmha49TM",
                    "EO422dn3gQLgDbuwqTjzrFgFtaRLRR5BdHEESmha49TM",
                    GetConfig()).GetAccessToken();
                return accessToken;
            }
        }

        private APIContext Api
        {
            get
            {
                APIContext context = new APIContext(AccessToken);
                context.Config = Configuration.GetConfiguration();
                return context;
            }
        }

        //public Payment CreditCardPayment()
        //{
        //    HttpContext CurrContext = HttpContext.Current;

        //    // ###Items
        //    // Items within a transaction.
        //    Item item = new Item();
        //    item.name = "Item Name";
        //    item.currency = "USD";
        //    item.price = "1";
        //    item.quantity = "5";
        //    item.sku = "sku";

        //    List<Item> itms = new List<Item>();
        //    itms.Add(item);
        //    ItemList itemList = new ItemList();
        //    itemList.items = itms;

        //    // ###Address
        //    // Base Address object used as shipping or billing
        //    // address in a payment.
        //    Address billingAddress = new Address();
        //    billingAddress.city = "Johnstown";
        //    billingAddress.country_code = "US";
        //    billingAddress.line1 = "52 N Main ST";
        //    billingAddress.postal_code = "43210";
        //    billingAddress.state = "OH";

        //    // ###CreditCard
        //    // A resource representing a credit card that can be
        //    // used to fund a payment.
        //    CreditCard crdtCard = new CreditCard();
        //    crdtCard.billing_address = billingAddress;
        //    crdtCard.cvv2 = "874";
        //    crdtCard.expire_month = 11;
        //    crdtCard.expire_year = 2018;
        //    crdtCard.first_name = "Joe";
        //    crdtCard.last_name = "Shopper";
        //    crdtCard.number = "4417119669820331";
        //    crdtCard.type = "visa";

        //    // ###Details
        //    // Let's you specify details of a payment amount.
        //    Details details = new Details();
        //    details.shipping = "1";
        //    details.subtotal = "5";
        //    details.tax = "1";

        //    // ###Amount
        //    // Let's you specify a payment amount.
        //    Amount amnt = new Amount();
        //    amnt.currency = "USD";
        //    // Total must be equal to sum of shipping, tax and subtotal.
        //    amnt.total = "7";
        //    amnt.details = details;

        //    // ###Transaction
        //    // A transaction defines the contract of a
        //    // payment - what is the payment for and who
        //    // is fulfilling it. 
        //    Transaction tran = new Transaction();
        //    tran.amount = amnt;
        //    tran.description = "This is the payment transaction description.";
        //    tran.item_list = itemList;

        //    // The Payment creation API requires a list of
        //    // Transaction; add the created `Transaction`
        //    // to a List
        //    List<Transaction> transactions = new List<Transaction>();
        //    transactions.Add(tran);

        //    // ###FundingInstrument
        //    // A resource representing a Payer's funding instrument.
        //    // For direct credit card payments, set the CreditCard
        //    // field on this object.
        //    FundingInstrument fundInstrument = new FundingInstrument();
        //    fundInstrument.credit_card = crdtCard;

        //    // The Payment creation API requires a list of
        //    // FundingInstrument; add the created `FundingInstrument`
        //    // to a List
        //    List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
        //    fundingInstrumentList.Add(fundInstrument);

        //    // ###Payer
        //    // A resource representing a Payer that funds a payment
        //    // Use the List of `FundingInstrument` and the Payment Method
        //    // as `credit_card`
        //    Payer payr = new Payer();
        //    payr.funding_instruments = fundingInstrumentList;
        //    payr.payment_method = "credit_card";

        //    // ###Payment
        //    // A Payment Resource; create one using
        //    // the above types and intent as `sale` or `authorize`
        //    Payment pymnt = new Payment();
        //    pymnt.intent = "sale";
        //    pymnt.payer = payr;
        //    pymnt.transactions = transactions;


        //    // Create a payment using a valid APIContext
        //    Payment createdPayment = pymnt.Create(Api);

        //    return createdPayment;
        //}

        public SaleCC_VeriSign CreditCardPayment(CartAuthenticateModel info, RazaPrincipal usercontext, string messagetype = "sale")
        {
            var dd = new SaleCC_VeriSign();
            dd.CVV2MATCH = "Y";
            dd.AVSZIP = "Y";
            dd.AVSADDR = "Y";
           //return dd;

            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            SaleCC_VeriSign ccSaleCcVeriSign = new SaleCC_VeriSign();
            string ipaddress = string.Empty;
            
            ccSaleCcVeriSign.SaleCC(Guid.NewGuid().ToString(), info.RechargeValues.CardNumber, info.RechargeValues.ExpDate, info.RechargeValues.cvv, 
                info.RechargeValues.Amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName, info.RechargeValues.LastName, 
                info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode,
                info.RechargeValues.Country,info.RechargeValues.Address, usercontext.Email, usercontext.ProfileInfo.PhoneNumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2);
            
            RazaLogger.WriteInfo(string.Format("parameter in Creditcardpayment is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}",info.RechargeValues.CardNumber,
                info.RechargeValues.ExpDate,info.RechargeValues.cvv,
                info.RechargeValues.Amount,info.RechargeValues.CurrencyCode,info.RechargeValues.FirstName,info.RechargeValues.LastName,
            info.RechargeValues.City,info.RechargeValues.State,info.RechargeValues.ZipCode,info.RechargeValues.Country,info.RechargeValues.Address,
            usercontext.Email,usercontext.ProfileInfo.PhoneNumber,info.RechargeValues.IpAddress,info.RechargeValues,cavv,eciflag,Xid,comment,comment2
            ));
            
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


        public AuthCC_VeriSign CreditCardAuthorize(CartAuthenticateModel info, RazaPrincipal usercontext)
        {

            string cavv = string.IsNullOrEmpty(info.CentinelAuthenticateResponse.Centinel_CAVV)? string.Empty : info.CentinelAuthenticateResponse.Centinel_CAVV;
            string eciflag = string.IsNullOrEmpty(info.CentinelAuthenticateResponse.CentinelEciFlag)
                ? string.Empty
                : info.CentinelAuthenticateResponse.CentinelEciFlag;
            string Xid = string.IsNullOrEmpty(info.CentinelAuthenticateResponse.Centinel_XID)
                ? string.Empty
                : info.CentinelAuthenticateResponse.Centinel_XID;
            string comment2 = ""; 
            //string comment = "Credit card transaction.";
            string comment = "Online " + info.RechargeValues.TransactionType + " CustomerID: " + usercontext.MemberId;

            AuthCC_VeriSign vreCcVeriSign = new AuthCC_VeriSign();

            string phonenumber = string.Empty;
            if (usercontext.ProfileInfo.PhoneNumber.Contains("-"))
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber.Replace("-", "");
            }
            else
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber;
            }

            var amount = info.RechargeValues.Amount + info.RechargeValues.ServiceFee;

            RazaLogger.WriteInfo(string.Format("parameter in creditcard authorize is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}",
                info.RechargeValues.CardNumber, info.RechargeValues.ExpDate, info.RechargeValues.cvv,
                amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName, info.RechargeValues.LastName,
                info.RechargeValues.City, info.RechargeValues.ZipCode,
                info.RechargeValues.Country, info.RechargeValues.Address, usercontext.Email, phonenumber,
                info.RechargeValues.IpAddress,
                info.CentinelAuthenticateResponse.Centinel_CAVV, info.CentinelAuthenticateResponse.CentinelEciFlag, info.CentinelAuthenticateResponse.Centinel_XID, comment, comment2));

            vreCcVeriSign.AuthCC(Guid.NewGuid().ToString(), info.RechargeValues.CardNumber, info.RechargeValues.ExpDate,
                info.RechargeValues.cvv, amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName,
                info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode,
                info.RechargeValues.Country,info.RechargeValues.Address, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2);

            
            //RazaLogger.WriteInfo(string.Format("AuthCC_VeriSign responseMsg={0},Result={1},AVSADDR={2},AVSZIP={3},CVV2MATCH={4}", vreCcVeriSign.RESPMSG,vreCcVeriSign.RESULT,vreCcVeriSign.AVSADDR,vreCcVeriSign.AVSZIP,vreCcVeriSign.CVV2MATCH));
            RazaLogger.WriteInfo2(string.Format("AuthCC_VeriSign responseMsg={0},Result={1},AVSADDR={2},AVSZIP={3},CVV2MATCH={4}", vreCcVeriSign.RESPMSG, vreCcVeriSign.RESULT, vreCcVeriSign.AVSADDR, vreCcVeriSign.AVSZIP, vreCcVeriSign.CVV2MATCH));

            return vreCcVeriSign;

            //var response = CreateAuthorization(Api);
            ////  var response = CreditCardPayment(info, "authorize");
            //RazaLogger.WriteInfo("credit card authorize response is=> " + response.state);
            //// return response.transactions[0].related_resources[0].authorization.state == "authorized";
            //return response.state == "authorized";
        }


        public CaptureCC_VeriSign CreditCardCapture(CartAuthenticateModel info, RazaPrincipal usercontext,string OrigId)
        {
            
            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            CaptureCC_VeriSign vreCcVeriSign = new CaptureCC_VeriSign();

            string phonenumber = string.Empty;
            if (usercontext.ProfileInfo.PhoneNumber.Contains("-"))
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber.Replace("-", "");
            }
            else
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber;
            }

            var amount = info.RechargeValues.Amount + info.RechargeValues.ServiceFee;

            RazaLogger.WriteInfo(string.Format("parameter in creditcard Capture is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}", OrigId, info.RechargeValues.CardNumber, info.RechargeValues.ExpDate, info.RechargeValues.cvv,
                amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName, info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.ZipCode,
                info.RechargeValues.Country, info.RechargeValues.Address, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2));

            vreCcVeriSign.CaptureCC(Guid.NewGuid().ToString(), OrigId, info.RechargeValues.CardNumber, info.RechargeValues.ExpDate,
                info.RechargeValues.cvv, amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName,
                info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode,
                info.RechargeValues.Country, info.RechargeValues.Address, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2);

            
            RazaLogger.WriteInfo(string.Format("CaptureCC_VeriSign ResponseMsg{0}, Result{1}", vreCcVeriSign.RESPMSG,vreCcVeriSign.RESULT));

            return vreCcVeriSign;
        }

        public VoidCC_VeriSign CreditCardVoid(CartAuthenticateModel info, RazaPrincipal usercontext,string OrigId)
        {

            string cavv = "", eciflag = "", Xid = "", comment2 = "", comment = "Credit card transaction.";
            VoidCC_VeriSign vreCcVeriSign = new VoidCC_VeriSign();

            string phonenumber = string.Empty;
            if (usercontext.ProfileInfo.PhoneNumber.Contains("-"))
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber.Replace("-", "");
            }
            else
            {
                phonenumber = usercontext.ProfileInfo.PhoneNumber;
            }
            var amount = info.RechargeValues.Amount + info.RechargeValues.ServiceFee;

            vreCcVeriSign.VoidCC(Guid.NewGuid().ToString(),OrigId, info.RechargeValues.CardNumber, info.RechargeValues.ExpDate,
                info.RechargeValues.cvv, amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName,
                info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode,
                info.RechargeValues.Country, info.RechargeValues.Address, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2);

            RazaLogger.WriteInfo(string.Format("parameter in creditcard Void is:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",OrigId, info.RechargeValues.CardNumber, info.RechargeValues.ExpDate, info.RechargeValues.cvv,
                info.RechargeValues.Amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName, info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.ZipCode,
                info.RechargeValues.Country, info.RechargeValues.Address, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
                cavv, eciflag, Xid, comment, comment2));
            RazaLogger.WriteInfo(string.Format("VoidCC_VeriSign ResponseMsg{0}, Result{1}", vreCcVeriSign.RESPMSG,vreCcVeriSign.RESULT));

            return vreCcVeriSign;
        }


        //// Create an authorized payment
        //public static PayPal.Api.Payments.Authorization CreateAuthorization(APIContext apiContext)
        //{
        //    // ###Address
        //    // Base Address object used as shipping or billing
        //    // address in a payment.
        //    Address billingAddress = new Address();
        //    billingAddress.city = "Johnstown";
        //    billingAddress.country_code = "US";
        //    billingAddress.line1 = "52 N Main ST";
        //    billingAddress.postal_code = "43210";
        //    billingAddress.state = "OH";

        //    // ###CreditCard
        //    // A resource representing a credit card that can be
        //    // used to fund a payment.
        //    CreditCard crdtCard = new CreditCard();
        //    crdtCard.billing_address = billingAddress;
        //    crdtCard.cvv2 = "874";
        //    crdtCard.expire_month = 11;
        //    crdtCard.expire_year = 2018;
        //    crdtCard.first_name = "Joe";
        //    crdtCard.last_name = "Shopper";
        //    crdtCard.number = "4417119669820331";
        //    crdtCard.type = "visa";

        //    // ###Details
        //    // Let's you specify details of a payment amount.
        //    Details details = new Details();
        //    details.shipping = "0.03";
        //    details.subtotal = "107.41";
        //    details.tax = "0.03";

        //    // ###Amount
        //    // Let's you specify a payment amount.
        //    Amount amnt = new Amount();
        //    amnt.currency = "USD";
        //    // Total must be equal to sum of shipping, tax and subtotal.
        //    amnt.total = "107.47";
        //    amnt.details = details;

        //    // ###Transaction
        //    // A transaction defines the contract of a
        //    // payment - what is the payment for and who
        //    // is fulfilling it. Transaction is created with
        //    // a `Payee` and `Amount` types
        //    Transaction tran = new Transaction();
        //    tran.amount = amnt;
        //    tran.description = "This is the payment transaction description.";

        //    // The Payment creation API requires a list of
        //    // Transaction; add the created `Transaction`
        //    // to a List
        //    List<Transaction> transactions = new List<Transaction>();
        //    transactions.Add(tran);

        //    // ###FundingInstrument
        //    // A resource representing a Payeer's funding instrument.
        //    // Use a Payer ID (A unique identifier of the payer generated
        //    // and provided by the facilitator. This is required when
        //    // creating or using a tokenized funding instrument)
        //    // and the `CreditCardDetails`
        //    FundingInstrument fundInstrument = new FundingInstrument();
        //    fundInstrument.credit_card = crdtCard;

        //    // The Payment creation API requires a list of
        //    // FundingInstrument; add the created `FundingInstrument`
        //    // to a List
        //    List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
        //    fundingInstrumentList.Add(fundInstrument);

        //    // ###Payer
        //    // A resource representing a Payer that funds a payment
        //    // Use the List of `FundingInstrument` and the Payment Method
        //    // as 'credit_card'
        //    Payer payr = new Payer();
        //    payr.funding_instruments = fundingInstrumentList;
        //    payr.payment_method = "credit_card";

        //    // ###Payment
        //    // A Payment Resource; create one using
        //    // the above types and intent as `sale`
        //    Payment pymnt = new Payment();
        //    pymnt.intent = "authorize";
        //    pymnt.payer = payr;
        //    pymnt.transactions = transactions;

        //    // Create a payment by posting to the APIService
        //    // using a valid APIContext
        //    Payment createdPayment = pymnt.Create(apiContext);

        //    return createdPayment.transactions[0].related_resources[0].authorization;
        //}


        public void RechargePin(CartAuthenticateModel model, RazaPrincipal userContext)
        {
            if ((bool) HttpContext.Current.Session["IsTransactionSuccess"])
            {
                return;
            }
            DataRepository repository = new DataRepository();
            var recharge = model.RechargeValues;

            recharge.FirstName = userContext.FirstName;
            recharge.LastName = userContext.LastName;
            recharge.MemberID = userContext.MemberId;

            string country = string.Empty;
            var cFromList = CacheManager.Instance.GetFromCountries();
            if (cFromList != null)
            {
                var cdata = cFromList.FirstOrDefault(a => a.Name == userContext.ProfileInfo.Country);
                if (cdata != null) country = cdata.Id;
            }

            var value = repository.GetPlanDetails(recharge.OldOrderId, userContext.MemberId);
            recharge.Pin = value.Pin;

            //if (recharge.PaymentType == "P")
            //RazaLogger.WriteInfoPartial(string.Format("Got Pin {0} and new orderid is:{1}", recharge.Pin, recharge.order_id));

            try
            {
                string expityDate = string.Empty;

                if (!string.IsNullOrEmpty(recharge.ExpDate))
                    expityDate = recharge.ExpDate.Replace("/", "");

                RechargeInfo rechargeInfo = new RechargeInfo
                {
                    OrderId = recharge.order_id,
                    MemberId = userContext.MemberId,
                    IpAddress =recharge.IpAddress,
                    Pin = recharge.Pin,
                    Amount = recharge.Amount,
                    CardNumber = string.IsNullOrEmpty(recharge.CardNumber) ? string.Empty : recharge.CardNumber,
                    City = string.IsNullOrEmpty(recharge.City) ? string.Empty : recharge.City,
                    State = string.IsNullOrEmpty(recharge.State) ? string.Empty : recharge.State,
                    CVV2 = string.IsNullOrEmpty(recharge.cvv) ? string.Empty : recharge.cvv,
                    Address1 = string.IsNullOrEmpty(recharge.Address) ? string.Empty : recharge.Address,
                    Address2 = "Desktop Site Recharge",
                    ZipCode = string.IsNullOrEmpty(recharge.ZipCode) ? string.Empty : recharge.ZipCode,
                    Country = string.IsNullOrEmpty(country) ? string.Empty : recharge.Country,
                    ExpiryDate = expityDate,
                    IsCcProcess = true,
                    EciFlag = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CentinelEciFlag : string.Empty,
                    CVV2Response = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CVVResponse : string.Empty,
                    PaymentTransactionId = model.Centinel_TransactionId,
                    Xid = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_XID : string.Empty,
                    Cavv = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_CAVV : string.Empty,
                    AVSResponse = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.AVSResponse : string.Empty,
                    UserType = "Old",
                    CoupanCode = string.IsNullOrEmpty(recharge.CouponCode) ? string.Empty : recharge.CouponCode,
                    ServiceFee = recharge.ServiceFee,
                    PayerId = model.CentinelAuthenticateResponse !=null ? model.CentinelAuthenticateResponse.PayerId : string.Empty
                };

                if (recharge.PaymentType == "P")
                {
                    rechargeInfo.PaymentMethod = "PayPal";
                }
                else if (recharge.PaymentType == "C")
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }

                if (recharge.autoRefill == "T")
                {
                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = Convert.ToDouble(recharge.autoRefillAmount);
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }

                var rechstatus = repository.Recharge_Pin(rechargeInfo);
                
                //Added on 01/06/15 As per Mohindar to Set Flag
                HttpContext.Current.Session["IsPaymentComplete"] = "N";


                if (rechstatus.Rechargestatus == "1")
                {
                    HttpContext.Current.Session["IsTransactionSuccess"] = true;
                    if (recharge.IsPasscodeDial == "T" && recharge.PassCodePin != null)
                    {
                        var res = repository.SetPassCode(recharge.Pin, recharge.PassCodePin);
                        //if (res)
                        //{
                        //    if (recharge.PaymentType == "P")
                        //    RazaLogger.WriteInfoPartial("Passcode is set and passcode pin is: " + recharge.PassCodePin + "");
                        //}
                    }

                    recharge.order_id = rechstatus.OrderId;

                    RechargeConfirmMail(recharge, userContext.Email);
                    
                }
                else
                {
                    model.FinalResponseMessage = string.Format("Transaction is not successfull. {0}", rechstatus.RechargeError);

                    model.FinalTransactionStatus = false;
                    model.CentinelAuthenticateResponse.Status = false;

                }
            }
            catch (Exception ex)
            {
                model.FinalResponseMessage = "Transaction is not successfull due to " + ex.Message + ex.StackTrace;
                model.CentinelAuthenticateResponse.Status = false;
            }

        }

        public void IssueNewPlan(CartAuthenticateModel model, RazaPrincipal userContext)
        {
            if ((bool)HttpContext.Current.Session["IsTransactionSuccess"])
            {
                //if(model.RechargeValues.PaymentType == "P")
                RazaLogger.WriteInfoPartial("CustID: " + userContext.MemberId + " IsTransactionSuccess is True.");
                return;
            }
            double fee = model.RechargeValues.ServiceFee;
            var repository = new DataRepository();
            string country = string.Empty;
            var cFromList = CacheManager.Instance.GetFromCountries();
            if (cFromList != null)
            {
                
                var cdata = cFromList.FirstOrDefault(a => a.Name == userContext.ProfileInfo.Country);
                if (cdata != null) country = cdata.Id;
            }


    //        RazaLogger.WriteInfoPartial(
    //string.Format(
    //    "Calling Issue new Pin with params OrderID:{0},MemberID:{1},UserType{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
    //    "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", model.RechargeValues.OrderId, model.RechargeValues.MemberId, model.RechargeValues.UserType, info.IsCcProcess ? "N" : "Y", info.CardId,
    //    info.Amount, info.ServiceFee, info.CountryFrom, info.CountryTo, info.CoupanCode, info.AniNumber, info.PaymentMethod,
    //    info.CardNumber, info.ExpiryDate, info.CVV2, info.PaymentTransactionId, info.AVSResponse, info.CVV2Response,
    //    info.Cavv, info.EciFlag, info.Xid, info.Address1, info.Address2, info.City, info.State, info.ZipCode,
    //    info.Country, info.IpAddress, info.PayerId, info.IsAutoRefill, info.AutoRefillAmount));

    //        //if (model.RechargeValues.PaymentType == "P")
    //        //{
    //            RazaLogger.WriteInfoPartial(
    //string.Format(
    //"Calling Issue new Pin with params OrderID:{0},MemberID:{1},UserType:{2},IsCCProcess:{3},CardID:{4},Amount:{5},ServiceCharge:{6},CountryFrom:{7},CountryTo:{8},CouponCode:{9},AniNumber:{10},Payment_Method:{11}", model.RechargeValues.order_id, userContext.MemberId, model.RechargeValues.UserType, "Y", model.RechargeValues.CardId,
    //model.RechargeValues.Amount, model.RechargeValues.ServiceFee, model.RechargeValues.Countryfrom, model.RechargeValues.Countryto, string.IsNullOrEmpty(model.RechargeValues.CouponCode) ? string.Empty : model.RechargeValues.CouponCode,
    //string.IsNullOrEmpty(model.RechargeValues.AniNumber) ? string.Empty : model.RechargeValues.AniNumber));
    //        //}

            try
            {
                string expityDate = string.Empty;

                if (!string.IsNullOrEmpty(model.RechargeValues.ExpDate))
                    expityDate = model.RechargeValues.ExpDate.Replace("/", "");

                var rechargeInfo = new RechargeInfo
                {
                    OrderId = model.RechargeValues.order_id,
                    MemberId = userContext.MemberId,
                    
                    //Address1 = model.RechargeValues.Address,
                    Address1 = string.IsNullOrEmpty(model.RechargeValues.Address) ? string.Empty : model.RechargeValues.Address,

                    Amount = model.RechargeValues.Amount,
                    ServiceFee = model.RechargeValues.ServiceFee,
                    
                    Country = country,
                    
                    //ZipCode = model.RechargeValues.ZipCode,
                    ZipCode = string.IsNullOrEmpty(model.RechargeValues.ZipCode) ? string.Empty : model.RechargeValues.ZipCode,

                    //State = model.RechargeValues.State,
                    State = string.IsNullOrEmpty(model.RechargeValues.State) ? string.Empty : model.RechargeValues.State,
                    
                    //City = model.RechargeValues.City,
                    City = string.IsNullOrEmpty(model.RechargeValues.City) ? string.Empty : model.RechargeValues.City,
                    
                    CardNumber = string.IsNullOrEmpty(model.RechargeValues.CardNumber) ? string.Empty : model.RechargeValues.CardNumber,
                    CVV2 = string.IsNullOrEmpty(model.RechargeValues.cvv) ? string.Empty : model.RechargeValues.cvv,


                    PaymentMethod = model.RechargeValues.PaymentType,
                    //ExpiryDate = string.IsNullOrEmpty(model.RechargeValues.ExpDate) ? string.Empty : model.RechargeValues.ExpDate,
                    ExpiryDate = expityDate,
                    
                    UserType = model.RechargeValues.UserType,
                    CardId = model.RechargeValues.CardId,
                    CountryFrom = model.RechargeValues.Countryfrom,
                    CountryTo = model.RechargeValues.Countryto,
                    CoupanCode = string.IsNullOrEmpty(model.RechargeValues.CouponCode) ? string.Empty : model.RechargeValues.CouponCode,
                    AniNumber = string.IsNullOrEmpty(model.RechargeValues.AniNumber) ? string.Empty : model.RechargeValues.AniNumber,
                    IpAddress = model.RechargeValues.IpAddress,
                    //Address2 = "",
                    Address2 = "Desktop Site NewPin",
                    IsCcProcess = true,
                    EciFlag = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CentinelEciFlag : string.Empty,
                    CVV2Response = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CVVResponse : string.Empty,
                    PaymentTransactionId = model.Centinel_TransactionId,
                    Xid = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_XID : string.Empty,
                    Cavv = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_CAVV : string.Empty,
                    AVSResponse = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.AVSResponse : string.Empty,
                    PaymentType = model.RechargeValues.PaymentType,
                    PayerId = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.PayerId : string.Empty
                };



                // find if we need to redirect to centinel or not
                if (rechargeInfo.CardNumber.StartsWith("3")) // Amex card add 50
                {
                    // EXTRA 50¢ WILL BE CHARGED FOR AMEX CREDIT CARD
                    //  rechargeInfo.Amount += 0.5;
                }
                if (rechargeInfo.PaymentType == "P")
                {
                    rechargeInfo.PaymentMethod = "PayPal";
                    //rechargeInfo.CVV2 = string.Empty;
                    //rechargeInfo.CardNumber = string.Empty;
                    //rechargeInfo.ExpiryDate = string.Empty;
                }
                else if (rechargeInfo.PaymentType == "C")
                {
                    rechargeInfo.PaymentMethod = "Credit Card";
                }

                if (rechargeInfo.CardId == 9999)
                {
                    rechargeInfo.PaymentMethod = "Free Trial";
                    rechargeInfo.CardId = 0;
                    switch (SafeConvert.ToInt32(rechargeInfo.CountryFrom))
                    {
                        case 1:
                            rechargeInfo.CardId = 161;
                            break;
                        case 2:
                            rechargeInfo.CardId = 162;
                            break;
                        case 3:
                            rechargeInfo.CardId = 102;
                            break;
                    }
                }
                if (model.RechargeValues.autoRefill == "E")
                {
                    rechargeInfo.IsAutoRefill = string.Empty;
                    rechargeInfo.AutoRefillAmount = 0;
                }
                else if (model.RechargeValues.autoRefill == "T")
                {
                    rechargeInfo.IsAutoRefill = "Y";
                    rechargeInfo.AutoRefillAmount = Convert.ToDouble(model.RechargeValues.autoRefillAmount);
                }
                else
                {
                    rechargeInfo.IsAutoRefill = "N";
                    rechargeInfo.AutoRefillAmount = 0;
                }
                if (model.RechargeValues.PinlessNumbers != null && model.RechargeValues.PinlessNumbers.Any())
                {
                    RazaLogger.WriteInfo("add all pinless numbers");
                    int count = 1;
                    foreach (var item in model.RechargeValues.PinlessNumbers)
                    {
                        if (item.PinlessNumber != null)
                        {
                            if (count == 1)
                            {
                                rechargeInfo.AniNumber = item.PinlessNumber;
                                count++;
                            }
                            else
                            {
                                rechargeInfo.AniNumber += "*" + item.PinlessNumber;
                            }
                        }

                    }
                }


                //RazaLogger.WriteInfoPartial(
                //    string.Format(
                //        "Calling Issue new Pin with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                //        "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", rechargeInfo.OrderId, rechargeInfo.MemberId, rechargeInfo.UserType, rechargeInfo.IsCcProcess ? "N" : "Y", rechargeInfo.CardId,
                //        rechargeInfo.Amount, rechargeInfo.ServiceFee, rechargeInfo.CountryFrom, rechargeInfo.CountryTo, rechargeInfo.CoupanCode, rechargeInfo.AniNumber, rechargeInfo.PaymentMethod,
                //        rechargeInfo.CardNumber, rechargeInfo.ExpiryDate, rechargeInfo.CVV2, rechargeInfo.PaymentTransactionId, rechargeInfo.AVSResponse, rechargeInfo.CVV2Response,
                //        rechargeInfo.Cavv, rechargeInfo.EciFlag, rechargeInfo.Xid, rechargeInfo.Address1, rechargeInfo.Address2, rechargeInfo.City, rechargeInfo.State, rechargeInfo.ZipCode,
                //        rechargeInfo.Country, rechargeInfo.IpAddress, rechargeInfo.PayerId, rechargeInfo.IsAutoRefill, rechargeInfo.AutoRefillAmount));

                if (rechargeInfo.PaymentType == "P")
                {
                    RazaLogger.WriteInfoPartial(
                        string.Format(
                            "Calling Issue new Pin with params {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}," +
                            "{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}", rechargeInfo.OrderId ?? "null", rechargeInfo.MemberId ?? "null", rechargeInfo.UserType ?? "null", rechargeInfo.IsCcProcess ? "N" : "Y", rechargeInfo.CardId,
                            rechargeInfo.Amount, rechargeInfo.ServiceFee, rechargeInfo.CountryFrom, rechargeInfo.CountryTo, rechargeInfo.CoupanCode ?? "null", rechargeInfo.AniNumber ?? "null", rechargeInfo.PaymentMethod ?? "null",
                            rechargeInfo.CardNumber ?? "null", rechargeInfo.ExpiryDate ?? "null", rechargeInfo.CVV2 ?? "null", rechargeInfo.PaymentTransactionId ?? "null", rechargeInfo.AVSResponse ?? "null", rechargeInfo.CVV2Response ?? "null",
                            rechargeInfo.Cavv ?? "null", rechargeInfo.EciFlag ?? "null", rechargeInfo.Xid ?? "null", rechargeInfo.Address1 ?? "null", rechargeInfo.Address2 ?? "null", rechargeInfo.City ?? "null", rechargeInfo.State ?? "null", rechargeInfo.ZipCode ?? "null",
                            rechargeInfo.Country ?? "null", rechargeInfo.IpAddress ?? "null", rechargeInfo.PayerId ?? "null", rechargeInfo.IsAutoRefill ?? "null", rechargeInfo.AutoRefillAmount));
                }

                var response = repository.IssueNewPin(rechargeInfo);
                if (response.IssuenewpinStatus)
                {
                    HttpContext.Current.Session["IsTransactionSuccess"] = true;
                    model.RechargeValues.Pin = response.NewPin;
                    RazaLogger.WriteInfo("passcode parameter :" + model.RechargeValues.IsPasscodeDial + "  " +
                                         model.RechargeValues.PassCodePin + "");
                    if (model.RechargeValues.IsPasscodeDial == "T" && model.RechargeValues.PassCodePin != null)
                    {

                        var res = repository.SetPassCode(response.NewPin, model.RechargeValues.PassCodePin);
                        RazaLogger.WriteInfo("setpass code status is:" + res);
                        if (res)
                        {

                        }
                    }
                    CallingCardPurchaseMail(model.RechargeValues, userContext.Email);

                }
                //when purchase plan success, register all pinlessnumber one by one.

                RazaLogger.WriteInfo("creditcardhandle." + response.IssuenewpinStatus + " " + response.NewPin);
                if (response.IssuenewpinStatus)
                {
                    RazaLogger.WriteInfo("issue new plan status true and order pin is :" + response.NewPin);
                }
            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfoPartial("Issue New Pin is not successfull due to " + ex.Message + ex.StackTrace);
                //model.FinalResponseMessage = "Transaction is not successfull due to " + ex.Message + ex.StackTrace;
                model.FinalResponseMessage = "Transaction is complete. Please call our Customer Service to receive your Order Detail.";
                model.CentinelAuthenticateResponse.Status = false;
            }
        }

        public int GetUserCountryId(string country)
        {
            if (!string.IsNullOrEmpty(country))
            {
                var countrydata = CacheManager.Instance.GetFromCountries();
                var cid = countrydata.FirstOrDefault(a => a.Name == country);
                return SafeConvert.ToInt32(cid.Id);
            }
            else
            {
                return 1;
            }
        }

        public void RechargeConfirmMail(Recharge model, string email)
        {
            try
            {
                RazaLogger.WriteInfo("Sending recharge Confirmation mail.");
                string paymentmethod = string.Empty;
                if (model.PaymentType == "P")
                {
                    paymentmethod = "PayPal";
                }
                else if (model.PaymentType == "C")
                {
                    int cardlength = model.CardNumber.Length;
                    paymentmethod = model.CardType + ",...." +
                                    model.CardNumber.Substring(cardlength - 4, 4);
                }
                int acc = model.Pin.Length;
                string accountnumber = model.Pin.Substring(0, 4) + "XXXX" +
                                       model.Pin.Substring(acc - 3, 2);
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = model.ServiceFee;
                double totalcharge = model.Amount + servicecharge;
                string servername = ConfigurationManager.AppSettings["ServerName"];
                
                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId(model.Country);
                helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                
                   string redirectlink = "Account/MyAccount";
                

                string mailbody =
                    System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/email-recharge.html"));

                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--UserName-->", model.UserName);
                mailbody = mailbody.Replace(@"<!--PlanName-->", model.PlanName);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", model.order_id);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(model.Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--DateTime-->", datetime);
                mailbody = mailbody.Replace(@"<!--Currencycode-->", model.CurrencyCode);
                mailbody = mailbody.Replace(@"<!--OldOrderId-->", model.OldOrderId);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["Rechargeconfim"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("error in recharge confirmation mail: " + ex.Message);
            }

        }


        public void CallingCardPurchaseMail(Recharge model, string email)
        {
            try
            {
                RazaLogger.WriteInfo("parameter in calling card purchase mail: " + email + "," + model.CardNumber + "," +
                                     model.Pin +
                                     ","
                                     + model.ServiceFee + "," + model.Amount + "," + model.PlanName + "," +
                                     model.order_id + "," + model.TransactionType);

                string paymentmethod = string.Empty;
                if (model.PaymentType == "P")
                {
                    paymentmethod = "PayPal";
                }
                else if (model.PaymentType == "C")
                {
                    int cardlength = model.CardNumber.Length;
                    paymentmethod = model.CardType + ",...." +
                                    model.CardNumber.Substring(cardlength - 4, 4);
                }
                string accountnumber = string.Empty;
                int acc = model.Pin.Length;
                if (!string.IsNullOrEmpty(model.Pin))
                {
                    accountnumber = model.Pin.Substring(0, 3) + "XXXXX" +
                                           model.Pin.Substring(acc - 2, 2);
                }
                string datetime = Convert.ToString(DateTime.Now);
                double servicecharge = model.ServiceFee;
                double totalcharge = model.Amount + servicecharge;
                string servername = ConfigurationManager.AppSettings["ServerName"];
                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId(model.Country);
                helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                string redirectlink = "Account/MyAccount";

                string bannername = "newplan_details.jpg";

                //if (model.TransactionType == "PurchaseNewPlan") // existing customer new plan
                //{
                //    bannername = "newplan_details.jpg";
                //}
                if (model.TransactionType == "CheckOut") //// new customer new plan
                {
                    bannername = "activation-confirmed-banner.jpg";
                }

                string mailbody =
                    System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/calling-card-purchase-details.html"));

                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--UserName-->", model.UserName);
                mailbody = mailbody.Replace(@"<!--PlanName-->", model.PlanName);
                mailbody = mailbody.Replace(@"<!--AccessNumber-->", "Click Here");
                mailbody = mailbody.Replace(@"<!--OrderId-->", model.order_id);
                mailbody = mailbody.Replace(@"<!--AccountNumber-->", accountnumber);
                mailbody = mailbody.Replace(@"<!--Purchaseamount-->", Convert.ToString(model.Amount));
                mailbody = mailbody.Replace(@"<!--ServiceCharge-->", Convert.ToString(servicecharge));
                mailbody = mailbody.Replace(@"<!--TotalCharge-->", Convert.ToString(totalcharge));
                mailbody = mailbody.Replace(@"<!--Paymentmethod-->", paymentmethod);
                mailbody = mailbody.Replace(@"<!--Datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--EmailId-->", email);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                mailbody = mailbody.Replace(@"<!--Currencycode-->", model.CurrencyCode);
                mailbody = mailbody.Replace(@"<!--Banner-->", bannername);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    ConfigurationManager.AppSettings["CallingCardPurchase"],
                    mailbody,
                    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in Mail Send of Calling Card Purchase:" + ex.Message);
            }

        }

        public RechargeStatusModel MobileTopUpRecharge(CartAuthenticateModel model, RazaPrincipal userContext) //toprecharge for existing user...
        {
            try
            {
                if ((bool)HttpContext.Current.Session["IsTransactionSuccess"])
                {
                  return new RechargeStatusModel() {Status = true};
                }
                var repository = new DataRepository();
                var billngdata = repository.GetBillingInfo(userContext.MemberId);
                string country = string.Empty;
                var cFromList = CacheManager.Instance.GetFromCountries();
                if (cFromList != null)
                {
                    var cdata = cFromList.FirstOrDefault(a => a.Name == userContext.ProfileInfo.Country);
                    if (cdata != null) country = cdata.Id;
                }
                string destphonenumber = string.Empty;
                if (model.RechargeValues.TopUpCountryCode.Contains("-"))
                {
                    destphonenumber = model.RechargeValues.TopUpCountryCode.Replace("-", "") +
                                      model.RechargeValues.TopupMobileNumber;
                }
                else
                {
                    destphonenumber = model.RechargeValues.TopUpCountryCode + model.RechargeValues.TopupMobileNumber;
                }
                
                var topuprech = new Mobiletopup()
                {
                    NewOrderId = model.RechargeValues.order_id,
                    MemberId = userContext.MemberId,
                    OperatorCode = model.RechargeValues.TopupOperatorCode,
                    CountryId = model.RechargeValues.TopupCountry,
                    SourceAmount = model.RechargeValues.TopupSourceAmount,
                    DestinationAmt = model.RechargeValues.TopupDestAmount,
                    SmsTo = string.Empty,
                    Pin = string.Empty,
                    PurchaseAmount = model.RechargeValues.TopupSourceAmount,
                    CardNumber = string.IsNullOrEmpty(model.RechargeValues.CardNumber) ? string.Empty : model.RechargeValues.CardNumber,
                    ExpDate = string.IsNullOrEmpty(model.RechargeValues.ExpDate) ? string.Empty : model.RechargeValues.ExpDate,
                    Cvv2 = string.IsNullOrEmpty(model.RechargeValues.cvv) ? string.Empty : model.RechargeValues.cvv,
                    Address1 = model.RechargeValues.Address,
                    Address2 = string.Empty,
                    City = billngdata.City,
                    State = model.RechargeValues.State,
                    ZipCode = model.RechargeValues.ZipCode,
                    Country = country,
                    IpAddress = model.RechargeValues.IpAddress,
                    DestinationPhoneNumber = destphonenumber,
                    Operator = model.RechargeValues.TopupOperatorName,
                    CouponCode = string.IsNullOrEmpty(model.RechargeValues.CouponCode) ? string.Empty : model.RechargeValues.CouponCode,
                    Isccprocess = true,
                    EciFlag = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CentinelEciFlag : string.Empty,
                    Cvv2Response = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.CVVResponse : string.Empty,
                    PaymentTransactionId = model.Centinel_TransactionId,
                    Xid = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_XID : string.Empty,
                    Cavv = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.Centinel_CAVV : string.Empty,
                    AvsResponse = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.AVSResponse : string.Empty,
                    PayerId = string.Empty,
                };

                if (model.RechargeValues.PaymentType == "P")
                {
                    topuprech.PaymentMethod = "PayPal";
                }
                else if (model.RechargeValues.PaymentType == "C")
                {
                    topuprech.PaymentMethod = "Credit Card";
                }

                var statusmodel=new RechargeStatusModel();
                var res = repository.MobileTopupRecharge(topuprech);
                if (res.Status)
                {
                    HttpContext.Current.Session["IsTransactionSuccess"] = true;
                    RazaLogger.WriteInfo("Mobile Top Success, Sending Confirmation mail to user");
                    MobileTopUpMail(model.RechargeValues, userContext);
                }
                else
                {
                    RazaLogger.WriteInfo("Mobile topup failed");
                }
               // RazaLogger.WriteInfo(res.Status ? "seccessfully Mobiletopup recharge" : "Mobile top Recharge failed.");
                statusmodel.Status = res.Status;
                return statusmodel;
            }
            catch (Exception exception)
            {
                RazaLogger.WriteInfo("exceptopn occurred in mobiletop: "+exception.Message);
               return new RechargeStatusModel();
            }
            
        }


        public void SaveNewPendingOrder(CartAuthenticateModel model, RazaPrincipal userContext)
        {

            DataRepository repository = new DataRepository();

            RechargeInfo infomodel = new RechargeInfo()
            {
                MemberId = userContext.MemberId,
                OrderId = model.RechargeValues.order_id,
                CardId = model.RechargeValues.CardId,
                Amount = model.RechargeValues.Amount,
                CountryFrom = model.RechargeValues.Countryfrom,
                CountryTo = model.RechargeValues.Countryto,
                CoupanCode = string.IsNullOrEmpty(model.RechargeValues.CouponCode) ? string.Empty : model.RechargeValues.CouponCode,
                PaymentMethod = model.RechargeValues.PaymentMethod,
                CardNumber =string.IsNullOrEmpty(model.RechargeValues.CardNumber) ? string.Empty : model.RechargeValues.CardNumber,
                ExpiryDate =string.IsNullOrEmpty(model.RechargeValues.ExpDate) ? string.Empty : model.RechargeValues.ExpDate,
                CVV2 =string.IsNullOrEmpty(model.RechargeValues.FirstName) ? string.Empty : model.RechargeValues.cvv,
                FirstName = model.RechargeValues.FirstName,
                LastName = model.RechargeValues.LastName,
                EmailAddress = userContext.ProfileInfo.Email,
                AniNumber = userContext.ProfileInfo.PhoneNumber,
                Address1 = userContext.ProfileInfo.Address,
                Address2 = string.Empty,
                City = userContext.ProfileInfo.City,
                State = userContext.ProfileInfo.State,
                ZipCode = userContext.ProfileInfo.ZipCode,
                Country = userContext.ProfileInfo.Country,
                AuthTransactionId = string.IsNullOrEmpty(model.CcAuthTransactionId) ? string.Empty : model.CcAuthTransactionId,
                CentinelPayLoad = string.IsNullOrEmpty(model.CentinelPayload) ? string.Empty : model.CentinelPayload,
                PayResPayLoad = string.IsNullOrEmpty(model.PayResPayLoad) ? string.Empty : model.PayResPayLoad,
                CentinelTransactionId = string.IsNullOrEmpty(model.Centinel_TransactionId) ? string.Empty : model.Centinel_TransactionId,
                ProcessedBy = string.Empty,
                IpAddress = model.RechargeValues.IpAddress,
                PayerId = model.CentinelAuthenticateResponse != null ? model.CentinelAuthenticateResponse.PayerId : string.Empty
            };

            if (model.RechargeValues.PaymentType == "P") 
            {
                infomodel.PaymentMethod = "PayPal";
            }
            else if (model.RechargeValues.PaymentType == "C") 
            {
                infomodel.PaymentMethod = "Credit Card";
            }

            var response = repository.SaveNewPendingOrder(infomodel);

        }


        public void MobileTopUpMail(Recharge model, RazaPrincipal userContext)
        {
            try
            {
                RazaLogger.WriteInfo(
                    "Sending Mobiletopconfirmation Mail with parameters: " + userContext.Email + "," + model.order_id +
                    "," + model.TopUpCountryCode + model.TopupMobileNumber
                    + "," + model.CardNumber, "," + model.CurrencyCode);


                string paymentmethod = model.PaymentMethod;
                string creditcard = string.Empty;

                if (model.PaymentType == "P")
                {
                    paymentmethod = "PayPal";
                }
                else if (model.PaymentType == "C")
                {
                    //creditcard = model.CardNumber;
                    int cardlength = model.CardNumber.Length;
                    creditcard = model.CardNumber.Substring(0, 4) + "......." + model.CardNumber.Substring(cardlength - 4, 4);
                    paymentmethod = model.CardType + ",...." +
                                    model.CardNumber.Substring(cardlength - 4, 4);
                }

                //string creditcard = model.CardNumber;
                //string paymentmethod = model.PaymentMethod;

                string orderId = model.order_id;
                string mobileTopupNumber = model.TopUpCountryCode + model.TopupMobileNumber;

                string topupamount = SafeConvert.ToString(model.TopupSourceAmount);
                string taxamount = "0%";
                string email = userContext.Email;
                string datetime = DateTime.Now.ToString();
                string currencycode = model.CurrencyCode;

                
                string helplinenumber = string.Empty;
                var usercountryid = GetUserCountryId(userContext.ProfileInfo.Country);
                
                if (userContext != null && userContext.ProfileInfo != null)
                {
                    helplinenumber = Helpers.GetHelpLineNumber(usercountryid);
                }
                else
                {
                    helplinenumber = (string)HttpContext.Current.Session["HelpNumber"];
                }
                string redirectlink = string.Empty;
                if (userContext != null && userContext.UserType.ToLower() == "old")
                {
                    redirectlink = "Account/MyAccount";
                }



                string mailbody =
                                 System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"/Email-Temp/mobile-topup.html"));
                string servername = ConfigurationManager.AppSettings["ServerName"];
                mailbody = mailbody.Replace(@"<!--ServerName-->", servername);
                mailbody = mailbody.Replace(@"<!--datetime-->", datetime);
                mailbody = mailbody.Replace(@"<!--OrderNumber-->", orderId);
                mailbody = mailbody.Replace(@"<!--MobileTopupNumber-->", mobileTopupNumber);
                mailbody = mailbody.Replace(@"<!--Creditcard-->", creditcard);
                mailbody = mailbody.Replace(@"<!--TopupAmount-->", topupamount);
                mailbody = mailbody.Replace(@"<!--PaymentMethod-->", "Credit Card");
                mailbody = mailbody.Replace(@"<!--Amountwithtax-->", topupamount);
                mailbody = mailbody.Replace(@"<!--Taxamount-->", taxamount);
                mailbody = mailbody.Replace(@"<!--CurrencyCode-->", currencycode);
                mailbody = mailbody.Replace(@"<!--HelpNumber-->", helplinenumber);
                mailbody = mailbody.Replace(@"<!--Redirectlink-->", redirectlink);
                
                mailbody = mailbody.Replace(@"<!--EmailId-->", email);

                Helpers.SendEmail(
                    ConfigurationManager.AppSettings["senderemailaddress"],
                    ConfigurationManager.AppSettings["sendername"],
                    email,
                    "Your Mobile Topup is successful.",
                    mailbody,
                    true);

                Helpers.SendEmail(
    ConfigurationManager.AppSettings["senderemailaddress"],
    ConfigurationManager.AppSettings["sendername"],
    "sabal@raza.com",
    "Your Mobile Topup is successful.",
    mailbody,
    true);


            }
            catch (Exception ex)
            {
                RazaLogger.WriteInfo("Error in sending Mobile topup mail: " + ex.Message);
            }

        }


    }
}