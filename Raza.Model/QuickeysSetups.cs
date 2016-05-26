using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class QuickeysSetups
    {
        public QuickeysSetups()
        {
            Setups = new List<EachQuickeysSetup>();
        }
        public List<EachQuickeysSetup> Setups { get; set; }

        public static QuickeysSetups Create(string data)
        {
            var setup = new QuickeysSetups();

            if (data.Split('|').Length > 1)
            {
                string[] allrows = data.Split('|')[1].Split('~');
                foreach (string eachrow in allrows)
                {
                    setup.Setups.Add(new EachQuickeysSetup()
                    {

                        AccessNumber = "1234321",
                        CustomerSerialNumber = "4321234",
                        OriginalPriceCurrencyCode = "USD",
                        OriginalPrice = "10.23",
                        PinNumber = "12345",
                        PlanName = "PN",
                        QuickeysSetup = "abcd"
                    });
                }

            }
            return setup;
        }
    }

    public class EachQuickeysSetup
    {
        public string PlanName { get; set; }
        public string OriginalPriceCurrencyCode { get; set; }
        public string OriginalPrice { get; set; }
        public string AccessNumber { get; set; }
        public string CustomerSerialNumber { get; set; }
        public string PinNumber { get; set; }
        public string QuickeysSetup { get; set; }
    }


    public class QuickeySetups
    {
        public string Destination { get; set; }
        public string SpeedDialNumber { get; set; }
        public string NickName { get; set; }

    }
    public class QuickeySetup
    {
        public List<QuickeySetups> quickeyList = new List<QuickeySetups>();
        public static QuickeySetup Create(string data)
        {
             var setup = new QuickeySetup();

            if (data.Split('~').Length > 1)
            {
                string[] allrows = data.Split('~')[1].Split('`');
                foreach (string eachrow in allrows)
                {
                    setup.quickeyList.Add(new QuickeySetups()
                    {
                        Destination=eachrow.Split('|')[0],
                        SpeedDialNumber=eachrow.Split('|')[1],
                        NickName=eachrow.Split('|')[2]
                    });
                }
            }
            return setup;
        }
    }
}

