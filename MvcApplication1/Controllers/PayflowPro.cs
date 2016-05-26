using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Raza.Common;

namespace MvcApplication1.Controllers
{
    public class AuthCC_VeriSign
    {
        public string RESULT = "NULL";
        public string PNREF = "NULL";
        public string RESPMSG = "NULL";
        public string AUTHCODE = "NULL";
        public string AVSADDR = "NULL";
        public string AVSZIP = "NULL";
        public string IAVS = "NULL";
        public string CVV2MATCH = "NULL";
        public string DUPLICATE = "NULL";
        

        public void AuthCC(string RequestId, string Card_Number, string Exp_Date, string CVV2, double Amount, string CurrencyCode, 
            string FName, string LName, string City, string State, string ZipCode, string Country, string Address, string EmailAddress, 
            string Home_Phone, string IP_Address, string Cavv, string EciFlag, string Xid, string Comment1, string Comment2)
        {
            //Guid.NewGuid().ToString(), info.RechargeValues.CardNumber, info.RechargeValues.ExpDate, 
            //    info.RechargeValues.cvv, info.RechargeValues.Amount, info.RechargeValues.CurrencyCode, info.RechargeValues.FirstName,
            //    info.RechargeValues.LastName, info.RechargeValues.City, info.RechargeValues.State, info.RechargeValues.ZipCode,
            //    info.RechargeValues.Country, usercontext.Email, phonenumber, info.RechargeValues.IpAddress,
            //    cavv, eciflag, Xid, comment, comment2
            #region TO AUTHORIZE CREDIT CARD TRHRU VERISIGN
            string PWD = ConfigurationManager.AppSettings["PWD"], USER = ConfigurationManager.AppSettings["USER"], VENDOR = ConfigurationManager.AppSettings["VENDOR"], PARTNER = ConfigurationManager.AppSettings["PARTNER"];

            PayPal.Payments.DataObjects.UserInfo User = new PayPal.Payments.DataObjects.UserInfo(USER, VENDOR, PARTNER, PWD);
            PayPal.Payments.DataObjects.PayflowConnectionData Connection = new PayPal.Payments.DataObjects.PayflowConnectionData("payflowpro.paypal.com");
            PayPal.Payments.DataObjects.Invoice Inv = new PayPal.Payments.DataObjects.Invoice();
            PayPal.Payments.DataObjects.Currency Amt = new PayPal.Payments.DataObjects.Currency(new decimal(Amount));

            Inv.Amt = Amt;
            //Inv.PoNum = "PO12345";
            Inv.InvNum = RequestId;
            Inv.Comment1 = Comment1;
            Inv.Comment2 = Comment2;

            // Set the Billing Address details.
            PayPal.Payments.DataObjects.BillTo Bill = new PayPal.Payments.DataObjects.BillTo();
            Bill.BillToFirstName = FName;
            Bill.BillToLastName = LName;
            Bill.BillToCity = City;
            Bill.BillToState = State;
            Bill.BillToStreet = Address;
            Bill.BillToZip = ZipCode;
            Bill.BillToPhone = Home_Phone;
            Bill.BillToEmail = EmailAddress;
            Bill.BillToCountry = Country;
            Inv.BillTo = Bill;

            //PayPal.Payments.DataObjects.CustomerInfo CustInfo=new PayPal.Payments.DataObjects.CustomerInfo();
            //CustInfo.CustIP = IP_Address;

            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card Number and Expiration Date of the Credit Card.
            PayPal.Payments.DataObjects.CreditCard CC = new PayPal.Payments.DataObjects.CreditCard(Card_Number, Exp_Date);
            CC.Cvv2 = CVV2;

            // Create a new Tender - Card Tender data object.
            PayPal.Payments.DataObjects.CardTender Card = new PayPal.Payments.DataObjects.CardTender(CC);

            // Create a new Auth Transaction.
            PayPal.Payments.Transactions.AuthorizationTransaction Trans = new PayPal.Payments.Transactions.AuthorizationTransaction(User, Connection, Inv, Card, RequestId);
            if (Cavv.Length > 0)
            {
                PayPal.Payments.DataObjects.BuyerAuthStatus AuthStatus = new PayPal.Payments.DataObjects.BuyerAuthStatus();
                AuthStatus.CAVV = Cavv;
                AuthStatus.ECI = EciFlag;
                AuthStatus.XID = Xid;
                Trans.BuyerAuthStatus = AuthStatus;
            }

            // Submit the Transaction
            PayPal.Payments.DataObjects.Response Resp = Trans.SubmitTransaction();


            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                PayPal.Payments.DataObjects.TransactionResponse TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null)
                {
                    RESULT = TrxnResponse.Result.ToString();
                    PNREF = TrxnResponse.Pnref;
                    RESPMSG = TrxnResponse.RespMsg;
                    AUTHCODE = TrxnResponse.AuthCode;
                    AVSADDR = TrxnResponse.AVSAddr;
                    AVSZIP = TrxnResponse.AVSZip;
                    IAVS = TrxnResponse.IAVS;
                    CVV2MATCH = TrxnResponse.CVV2Match;
                    // If value is true, then the Request ID has not been changed and the original response
                    // of the original transction is returned. 
                    DUPLICATE = TrxnResponse.Duplicate;
                }
            }
            #endregion
        }
    }

    public class SaleCC_VeriSign
    {
        public string RESULT = "NULL";
        public string PNREF = "NULL";
        public string RESPMSG = "NULL";
        public string AUTHCODE = "NULL";
        public string AVSADDR = "NULL";
        public string AVSZIP = "NULL";
        public string IAVS = "NULL";
        public string CVV2MATCH = "NULL";
        public string DUPLICATE = "NULL";

        public void SaleCC(string RequestId, string Card_Number, string Exp_Date, string CVV2, double Amount, string CurrencyCode, string FName, string LName, string City, string State, string ZipCode, string Country, string Address, string EmailAddress, string Home_Phone, string IP_Address, string Cavv, string EciFlag, string Xid, string Comment1, string Comment2)
        {
            #region SALE TRANSACTION THRU VERISIGN

            string PWD = ConfigurationManager.AppSettings["PWD"], USER = ConfigurationManager.AppSettings["USER"], VENDOR = ConfigurationManager.AppSettings["VENDOR"], PARTNER = ConfigurationManager.AppSettings["PARTNER"];

            PayPal.Payments.DataObjects.UserInfo User = new PayPal.Payments.DataObjects.UserInfo(USER, VENDOR, PARTNER, PWD);
            PayPal.Payments.DataObjects.PayflowConnectionData Connection = new PayPal.Payments.DataObjects.PayflowConnectionData("payflowpro.paypal.com");
            PayPal.Payments.DataObjects.Invoice Inv = new PayPal.Payments.DataObjects.Invoice();
            PayPal.Payments.DataObjects.Currency Amt = new PayPal.Payments.DataObjects.Currency(new decimal(Amount), CurrencyCode);

            Inv.Amt = Amt;
            //Inv.PoNum = "PO12345";
            Inv.InvNum = RequestId;
            Inv.Comment1 = Comment1;
            Inv.Comment2 = Comment2;

            // Set the Billing Address details.
            PayPal.Payments.DataObjects.BillTo Bill = new PayPal.Payments.DataObjects.BillTo();
            Bill.BillToFirstName = FName;
            Bill.BillToLastName = LName;
            Bill.BillToCity = City;
            Bill.BillToState = State;
            Bill.BillToCountry = Country;


            //            if(System.Text.RegularExpressions.Regex.IsMatch(ZipCode,"^[a-zA-Z]"))
            //            {
            //                Bill.Street = Address;
            //                Bill.Zip = ZipCode;
            //            }
            if (Address.Length > 0 && ZipCode.Length > 0)
            {
                Bill.BillToStreet = Address;
                Bill.BillToZip = ZipCode;
            }

            Bill.BillToPhone = Home_Phone;
            Bill.BillToEmail = EmailAddress;
            Inv.BillTo = Bill;

            //PayPal.Payments.DataObjects.CustomerInfo CustInfo=new PayPal.Payments.DataObjects.CustomerInfo();
            //CustInfo.CustIP = IP_Address;

            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card Number and Expiration Date of the Credit Card.
            PayPal.Payments.DataObjects.CreditCard CC = new PayPal.Payments.DataObjects.CreditCard(Card_Number, Exp_Date);
            if (CVV2.Length > 0)
                CC.Cvv2 = CVV2;

            // Create a new Tender - Card Tender data object.
            PayPal.Payments.DataObjects.CardTender Card = new PayPal.Payments.DataObjects.CardTender(CC);

            // Create a new Sale Transaction.
            PayPal.Payments.Transactions.SaleTransaction Trans = new PayPal.Payments.Transactions.SaleTransaction(User, Connection, Inv, Card, RequestId);

            // Submit the Transaction
            PayPal.Payments.DataObjects.Response Resp = Trans.SubmitTransaction();


            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                PayPal.Payments.DataObjects.TransactionResponse TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null)
                {
                    RESULT = TrxnResponse.Result.ToString();
                    PNREF = TrxnResponse.Pnref;
                    RESPMSG = TrxnResponse.RespMsg;
                    AUTHCODE = TrxnResponse.AuthCode;
                    AVSADDR = TrxnResponse.AVSAddr;
                    AVSZIP = TrxnResponse.AVSZip;
                    IAVS = TrxnResponse.IAVS;
                    CVV2MATCH = TrxnResponse.CVV2Match;
                    // If value is true, then the Request ID has not been changed and the original response
                    // of the original transction is returned.

                    //                    string DupMsg; 
                    //                    if (TrxnResponse.Duplicate == "1") 
                    //                    { 
                    //                        DupMsg = "Duplicate Transaction"; 
                    //                    } 
                    //                    else 
                    //                    { 
                    //                        DupMsg = "Not a Duplicate Transaction"; 
                    //                    } 

                    DUPLICATE = TrxnResponse.Duplicate;
                }
            }
            #endregion
        }
    }

    public class CaptureCC_VeriSign
    {
        public string RESULT = "NULL";
        public string PNREF = "NULL";
        public string RESPMSG = "NULL";
        public string AUTHCODE = "NULL";
        public string AVSADDR = "NULL";
        public string AVSZIP = "NULL";
        public string IAVS = "NULL";
        public string CVV2MATCH = "NULL";
        public string DUPLICATE = "NULL";

        public void CaptureCC(string RequestId,string OrigId, string Card_Number, string Exp_Date, string CVV2, double Amount, string CurrencyCode, string FName, string LName, string City, string State, string ZipCode, string Country, string Address, string EmailAddress, string Home_Phone, string IP_Address, string Cavv, string EciFlag, string Xid, string Comment1, string Comment2)
        {
            #region SALE TRANSACTION THRU VERISIGN

            string PWD = ConfigurationManager.AppSettings["PWD"], USER = ConfigurationManager.AppSettings["USER"], VENDOR = ConfigurationManager.AppSettings["VENDOR"], PARTNER = ConfigurationManager.AppSettings["PARTNER"];

            PayPal.Payments.DataObjects.UserInfo User = new PayPal.Payments.DataObjects.UserInfo(USER, VENDOR, PARTNER, PWD);
            PayPal.Payments.DataObjects.PayflowConnectionData Connection = new PayPal.Payments.DataObjects.PayflowConnectionData("payflowpro.paypal.com");
            PayPal.Payments.DataObjects.Invoice Inv = new PayPal.Payments.DataObjects.Invoice();
            PayPal.Payments.DataObjects.Currency Amt = new PayPal.Payments.DataObjects.Currency(new decimal(Amount), CurrencyCode);

            

            Inv.Amt = Amt;
            //Inv.PoNum = "PO12345";
            Inv.InvNum = RequestId;
            Inv.Comment1 = Comment1;
            Inv.Comment2 = Comment2;

            // Set the Billing Address details.
            PayPal.Payments.DataObjects.BillTo Bill = new PayPal.Payments.DataObjects.BillTo();
            Bill.BillToFirstName = FName;
            Bill.BillToLastName = LName;
            Bill.BillToCity = City;
            Bill.BillToState = State;
            Bill.BillToCountry = Country;


            //            if(System.Text.RegularExpressions.Regex.IsMatch(ZipCode,"^[a-zA-Z]"))
            //            {
            //                Bill.Street = Address;
            //                Bill.Zip = ZipCode;
            //            }
            if (Address.Length > 0 && ZipCode.Length > 0)
            {
                Bill.BillToStreet = Address;
                Bill.BillToZip = ZipCode;
            }

            Bill.BillToPhone = Home_Phone;
            Bill.BillToEmail = EmailAddress;
            Inv.BillTo = Bill;

            //PayPal.Payments.DataObjects.CustomerInfo CustInfo=new PayPal.Payments.DataObjects.CustomerInfo();
            //CustInfo.CustIP = IP_Address;

            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card Number and Expiration Date of the Credit Card.
            PayPal.Payments.DataObjects.CreditCard CC = new PayPal.Payments.DataObjects.CreditCard(Card_Number, Exp_Date);
            if (CVV2.Length > 0)
                CC.Cvv2 = CVV2;

            // Create a new Tender - Card Tender data object.
            PayPal.Payments.DataObjects.CardTender Card = new PayPal.Payments.DataObjects.CardTender(CC);

            // Create a new Sale Transaction.
            PayPal.Payments.Transactions.CaptureTransaction Trans =
                new PayPal.Payments.Transactions.CaptureTransaction(OrigId, User, Connection, Inv, RequestId);

            // Submit the Transaction
            PayPal.Payments.DataObjects.Response Resp = Trans.SubmitTransaction();


            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                PayPal.Payments.DataObjects.TransactionResponse TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null)
                {
                    RESULT = TrxnResponse.Result.ToString();
                    PNREF = TrxnResponse.Pnref;
                    RESPMSG = TrxnResponse.RespMsg;
                    AUTHCODE = TrxnResponse.AuthCode;
                    AVSADDR = TrxnResponse.AVSAddr;
                    AVSZIP = TrxnResponse.AVSZip;
                    IAVS = TrxnResponse.IAVS;
                    CVV2MATCH = TrxnResponse.CVV2Match;
                    // If value is true, then the Request ID has not been changed and the original response
                    // of the original transction is returned.

                    //                    string DupMsg; 
                    //                    if (TrxnResponse.Duplicate == "1") 
                    //                    { 
                    //                        DupMsg = "Duplicate Transaction"; 
                    //                    } 
                    //                    else 
                    //                    { 
                    //                        DupMsg = "Not a Duplicate Transaction"; 
                    //                    } 

                    DUPLICATE = TrxnResponse.Duplicate;
                }
            }
            #endregion
        }
    }

    public class VoidCC_VeriSign
    {
        public string RESULT = "NULL";
        public string PNREF = "NULL";
        public string RESPMSG = "NULL";
        public string AUTHCODE = "NULL";
        public string AVSADDR = "NULL";
        public string AVSZIP = "NULL";
        public string IAVS = "NULL";
        public string CVV2MATCH = "NULL";
        public string DUPLICATE = "NULL";

        public void VoidCC(string RequestId, string OrigId, string Card_Number, string Exp_Date, string CVV2, double Amount, string CurrencyCode, string FName, string LName, string City, string State, string ZipCode, string Country, string Address, string EmailAddress, string Home_Phone, string IP_Address, string Cavv, string EciFlag, string Xid, string Comment1, string Comment2)
        {
            #region SALE TRANSACTION THRU VERISIGN

            string PWD = ConfigurationManager.AppSettings["PWD"], USER = ConfigurationManager.AppSettings["USER"], VENDOR = ConfigurationManager.AppSettings["VENDOR"], PARTNER = ConfigurationManager.AppSettings["PARTNER"];

            PayPal.Payments.DataObjects.UserInfo User = new PayPal.Payments.DataObjects.UserInfo(USER, VENDOR, PARTNER, PWD);
            PayPal.Payments.DataObjects.PayflowConnectionData Connection = new PayPal.Payments.DataObjects.PayflowConnectionData("payflowpro.paypal.com");
            PayPal.Payments.DataObjects.Invoice Inv = new PayPal.Payments.DataObjects.Invoice();
            PayPal.Payments.DataObjects.Currency Amt = new PayPal.Payments.DataObjects.Currency(new decimal(Amount), CurrencyCode);

            Inv.Amt = Amt;
            //Inv.PoNum = "PO12345";
            Inv.InvNum = RequestId;
            Inv.Comment1 = Comment1;
            Inv.Comment2 = Comment2;

            // Set the Billing Address details.
            PayPal.Payments.DataObjects.BillTo Bill = new PayPal.Payments.DataObjects.BillTo();
            Bill.BillToFirstName = FName;
            Bill.BillToLastName = LName;
            Bill.BillToCity = City;
            Bill.BillToState = State;
            Bill.BillToCountry = Country;


            //            if(System.Text.RegularExpressions.Regex.IsMatch(ZipCode,"^[a-zA-Z]"))
            //            {
            //                Bill.Street = Address;
            //                Bill.Zip = ZipCode;
            //            }
            if (Address.Length > 0 && ZipCode.Length > 0)
            {
                Bill.BillToStreet = Address;
                Bill.BillToZip = ZipCode;
            }

            Bill.BillToPhone = Home_Phone;
            Bill.BillToEmail = EmailAddress;
            Inv.BillTo = Bill;

            //PayPal.Payments.DataObjects.CustomerInfo CustInfo=new PayPal.Payments.DataObjects.CustomerInfo();
            //CustInfo.CustIP = IP_Address;

            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card Number and Expiration Date of the Credit Card.
            PayPal.Payments.DataObjects.CreditCard CC = new PayPal.Payments.DataObjects.CreditCard(Card_Number, Exp_Date);
            if (CVV2.Length > 0)
                CC.Cvv2 = CVV2;

            // Create a new Tender - Card Tender data object.
            PayPal.Payments.DataObjects.CardTender Card = new PayPal.Payments.DataObjects.CardTender(CC);

            // Create a new Sale Transaction.
            PayPal.Payments.Transactions.VoidTransaction Trans = new PayPal.Payments.Transactions.VoidTransaction(OrigId, User, Connection, Inv, RequestId);

            // Submit the Transaction
            PayPal.Payments.DataObjects.Response Resp = Trans.SubmitTransaction();


            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                PayPal.Payments.DataObjects.TransactionResponse TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null)
                {
                    RESULT = TrxnResponse.Result.ToString();
                    PNREF = TrxnResponse.Pnref;
                    RESPMSG = TrxnResponse.RespMsg;
                    AUTHCODE = TrxnResponse.AuthCode;
                    AVSADDR = TrxnResponse.AVSAddr;
                    AVSZIP = TrxnResponse.AVSZip;
                    IAVS = TrxnResponse.IAVS;
                    CVV2MATCH = TrxnResponse.CVV2Match;
                    // If value is true, then the Request ID has not been changed and the original response
                    // of the original transction is returned.

                    //                    string DupMsg; 
                    //                    if (TrxnResponse.Duplicate == "1") 
                    //                    { 
                    //                        DupMsg = "Duplicate Transaction"; 
                    //                    } 
                    //                    else 
                    //                    { 
                    //                        DupMsg = "Not a Duplicate Transaction"; 
                    //                    } 

                    DUPLICATE = TrxnResponse.Duplicate;
                }
            }
            #endregion
        }
    }
}