using MvcApplication1.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using CardinalCommerce;

namespace Raza.Test
{


    /// <summary>
    ///This is a test class for CreditCartHandlerTest and is intended
    ///to contain all CreditCartHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CreditCartHandlerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SubmitTransaction
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Code\\Raza\\MvcApplication1", "/")]
        [UrlToTest("http://localhost:50668/")]
        public void SubmitTransactionTest()
        {
            CreditCartHandler target = new CreditCartHandler(); // TODO: Initialize to an appropriate value
            string cardNumber = string.Empty; // TODO: Initialize to an appropriate value
            string expDate = string.Empty; // TODO: Initialize to an appropriate value
            string cvv = string.Empty; // TODO: Initialize to an appropriate value
            double amount = 0F; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            // string actual;
            //// actual = target.SubmitTransaction(cardNumber, expDate, cvv, amount);
            // Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void TestCentinelRequest()
        {
            var ccRequest = new CentinelRequest();

            ccRequest.add("Version", "1.7");
            ccRequest.add("MsgType", "cmpi_lookup");
            ccRequest.add("MerchantId", "40158");
            ccRequest.add("ProcessorId", "202");
            ccRequest.add("TransactionPwd", "razatest");
            ccRequest.add("TransactionType", "C");
            //ccRequest.add("CardNumber", recharge.CardNumber);
            //ccRequest.add("CardExpMonth", recharge.ExpMonth);
            //ccRequest.add("CardExpyear", "2013");//recharge.ExpYear);
            //ccRequest.add("OrderNumber", "1233444");
            //ccRequest.add("OrderDescription", "This is where you put the order description");
            //ccRequest.add("Amount", recharge.Amount.ToString());
            //ccRequest.add("CurrencyCode", "840");
            //ccRequest.add("UserAgent", Request.ServerVariables["HTTP_USER_AGENT"]);

            //ccRequest.add("BrowserHeader", Request.ServerVariables["HTTP_ACCEPT"]);
            //ccRequest.add("Installment", "");
            //ccRequest.add("IPAddress", Request.ServerVariables["REMOTE_ADDR"]);
            //ccRequest.add("EMail", UserContext.Email);


            //ccRequest.add("BillingFirstName", UserContext.FirstName);
            //ccRequest.add("BillingLastName", UserContext.LastName);
            //ccRequest.add("BillingAddress1", UserContext.ProfileInfo.Address);
            //ccRequest.add("BillingAddress2", "");
            //ccRequest.add("BillingCity", UserContext.ProfileInfo.City);
            //ccRequest.add("BillingState", UserContext.ProfileInfo.State);
            //ccRequest.add("BillingCountryCode", UserContext.ProfileInfo.Country);
            //ccRequest.add("BillingPostalCode", UserContext.ProfileInfo.ZipCode);
            //ccRequest.add("ShippingFirstName", UserContext.FirstName);
            //ccRequest.add("ShippingLastName", UserContext.LastName);
            //ccRequest.add("ShippingAddress1", UserContext.ProfileInfo.Address);

            ccRequest.add("CardNumber", "4000000000000002");
            ccRequest.add("CardExpMonth", "01");
            ccRequest.add("CardExpyear", "2013");//recharge.ExpYear);
            ccRequest.add("OrderNumber", "1234512345");
            ccRequest.add("OrderDescription", "This is where you put the order description");
            ccRequest.add("Amount", "1");
            ccRequest.add("CurrencyCode", "840");
            ccRequest.add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");

            ccRequest.add("BrowserHeader", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            ccRequest.add("Installment", "");
            ccRequest.add("IPAddress", "::1");
            ccRequest.add("EMail", "testuser@cardinalcommerce.com");
            
            ccRequest.add("BillingFirstName", "Joe");
            ccRequest.add("BillingLastName", "Shopper");
            ccRequest.add("BillingAddress1", "12345 Main Street");
            ccRequest.add("BillingAddress2", "");
            ccRequest.add("BillingCity", "Mentor");
            ccRequest.add("BillingState", "OH");
            ccRequest.add("BillingCountryCode", "US");
            ccRequest.add("BillingPostalCode", "44094");

            ccRequest.add("ShippingFirstName", "Joe");
            ccRequest.add("ShippingLastName", "Shopper");
            ccRequest.add("ShippingAddress1", "12345 Main Street");
            ccRequest.add("ShippingAddress2", "");
            ccRequest.add("ShippingCity", "Mentor");
            ccRequest.add("ShippingState", "OH");
            ccRequest.add("ShippingCountryCode", "US");
            ccRequest.add("ShippingPostalCode", "44094");

            var ccResponse = ccRequest.sendHTTP("https://centineltest.cardinalcommerce.com/maps/txns.asp", 10000);

            var errorNo = ccResponse.getValue("ErrorNo");
            var errorDesc = ccResponse.getValue("ErrorDesc");
            var enrolled = ccResponse.getValue("Enrolled");
            var payload = ccResponse.getValue("Payload");
            var acsurl = ccResponse.getValue("ACSUrl");
            var transactionId = ccResponse.getValue("TransactionId");

            var message = ccResponse.getUnparsedResponse();

        }
    }
}
