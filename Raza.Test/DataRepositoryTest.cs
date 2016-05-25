using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web;

using Raza.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Raza.Model;
using System.Collections.Generic;



namespace Raza.Test
{
    using Raza.Test.mobilereference;
    //using MvcApplication1.Models;
    //using MvcApplication1.Controllers;

    /// <summary>
    ///This is a test class for DataRepositoryTest and is intended
    ///to contain all DataRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataRepositoryTest
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
        ///A test for FreeTrial_Country_List
        ///</summary>
        [TestMethod()]
        public void FreeTrial_Country_ListTest()
        {
            DataRepository target = new DataRepository(); // TODO: Initialize to an appropriate value
            string Email = "neeraj.kaushik@live.in"; // TODO: Initialize to an appropriate value
            string Password = "sam007"; // TODO: Initia


            //var res= target.FreeTrial_Country_List();

            Assert.Inconclusive("A method that does not eturn a value cannot be verified.");
        }

        [TestMethod()]
        public void testLogin()
        {

            Raza_AuthHeader header = new Raza_AuthHeader() { AuthUsername = "mobileapp", AuthPassword = "app123" };

            string Email = "habibrjalali@gmail.com";
            string Password = "4rt61dr";
            string phone = "708-430-0065";
            string deviceid = "82FE9059-2C8D-4614-B97B-1A51DC5ABA95";
            mobilereference.Mobile_ServicesSoapClient client = new Mobile_ServicesSoapClient();
            var response = client.Login(ref header, Email, Password, phone, deviceid);
        }

        

      

        [TestMethod()]
        public void GetLowestRateforall()
        {
            var target = new DataRepository(); // TODO: Initialize to an appropriate value

            int countryfrom = 1;
            int countryto = 0;

            var res = target.GetLowestRates(1, 0);

            Assert.IsNotNull(res);

        }


        [TestMethod()]
        public void GetRate()
        {
            var target = new DataRepository(); // TODO: Initialize to an appropriate value
            
            var res = target.GetRates(new Rates()
            {
                CountryFrom = 1,
                CountryTo = 347,
                CardName = String.Empty
            });

            Assert.IsNotNull(res);

        }


        [TestMethod()]
        public void GetCountrywithLowestRate()
        {
            var target = new DataRepository(); // TODO: Initialize to an appropriate value
            var res = target.GetCountryWithLowestRates();
            

            Assert.IsNotNull(res);
        }

        [TestMethod()]
        public void GetExpMonths()
        {

            


            
        }

        public void TestSignup()
        {
            var target = new DataRepository(); // TODO: Initialize to an appropriate value
            
        }


    }
}
