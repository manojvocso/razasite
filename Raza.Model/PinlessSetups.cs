using System;
using System.Collections.Generic;

namespace Raza.Model
{
    public class PinlessSetups
    {
        public PinlessSetups()
        {
            Setups = new List<PinlessSetup>();
        }
        public List<PinlessSetup> Setups { get; set; }

        public static PinlessSetups Create(string data)
        {
            var setup = new PinlessSetups();

            if (data.Split('|').Length > 1)
            {
                string[] allrows = data.Split('|')[1].Split('~');
                foreach (string eachrow in allrows)
                {
                    setup.Setups.Add(new PinlessSetup()
                    {
                        SerialNo = eachrow.Split(',')[0],
                        PlanName = eachrow.Split(',')[1],
                        OrigPrice = eachrow.Split(',')[2],
                        CardType = eachrow.Split(',')[3],
                        CustomerSrNoPin = eachrow.Split(',')[4],
                        Pin = eachrow.Split(',')[5],
                        OrderId = eachrow.Split(',')[6]
                    });
                }

            }
            return setup;
        }

       
    }

    public class PinlessSetup
    {
        public string SerialNo { get; set; }

        public string PlanName { get; set; }

        public string OrigPrice { get; set; }

        public string CardType { get; set; }

        public string CustomerSrNoPin { get; set; }

        public string Pin { get; set; }

        public string OrderId { get; set; }

    }

    public class PinLessSetupNumbers
    {
        public string PinlessNumber { get; set; }
        public string CountryCode { get; set; }
        public string OrderId { get; set; }
        public string UnmaskPinlessNumber { get; set; }
    }


    public class PinLessNumbers
    {
        public List<PinLessSetupNumbers> PinlessNumberList = new List<PinLessSetupNumbers>();
        public static PinLessNumbers Create(string data)
        {
            var pinlessnumbers = new PinLessNumbers();
            if (data.Split('|')[0].Split('=')[1] == "1")
            {

                if (data.Split('|').Length > 1)
                {
                    string[] allrows = data.Split('|')[1].Split('~');
                    foreach (string eachrow in allrows)
                    {
                        pinlessnumbers.PinlessNumberList.Add(new PinLessSetupNumbers()
                        {
                            CountryCode = eachrow.Split(',')[0],
                            PinlessNumber = Convert.ToInt64(eachrow.Split(',')[1]).ToString("(000)-000-0000"),
                           //PinlessNumber =  String.Format("{0:(###)-###-####}", eachrow.Split(',')[1]),
                            UnmaskPinlessNumber=eachrow.Split(',')[1]
                            //mine file.
                        });

                    }
                }
                return pinlessnumbers;
            }

            else
            {
                return pinlessnumbers;
            }

        }
    }
}


