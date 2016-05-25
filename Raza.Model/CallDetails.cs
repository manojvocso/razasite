using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    using System.Globalization;

    public class CallDetails
    {
        public CallDetails()
        {
            AllCalls = new List<EachCall>();
        }

        public List<DateTime> date { get; set; }
        public List<EachCall> AllCalls { get; set; }

        public static CallDetails Create(string data)
        {
            var calldetail = new CallDetails();

            if (data.Split('|').Length > 1)
            {
                string[] allrows = data.Split('|')[1].Split('`');

                foreach (string eachrow in allrows)
                {
                    try
                    {
                        List<DateTime> dates = new List<DateTime>();
                        string format = "M/d/yyyy h:mm:ss tt";
                        var date = DateTime.ParseExact(eachrow.Split(',')[0], format, CultureInfo.InvariantCulture);
                        dates.Add(date);
                    }
                    catch
                    {
                        
                    }

                }

                foreach (string eachrow in allrows)
                {
                    if (eachrow.Split(',')[5] == "0" && eachrow.Split(',')[4] == "00:00:00")
                    {
                        continue;
                    }
                    calldetail.AllCalls.Add(new EachCall()
                     {
                         CallDate = DateTime.Parse(eachrow.Split(',')[0], CultureInfo.InvariantCulture),
                         SourceNumber = eachrow.Split(',')[1],
                         DestinationNumber = eachrow.Split(',')[2].Replace("#", string.Empty),
                         DestinationCity = eachrow.Split(',')[3],
                         CallDuration = eachrow.Split(',')[4],
                         CallAmount = eachrow.Split(',')[5]
                     });
                }
            }

            return calldetail;
        }
    }

    public class EachCall
    {
        
        public DateTime CallDate { get; set; }
        public string SourceNumber { get; set; }
        public string DestinationNumber { get; set; }
        public string DestinationCity { get; set; }
        public string CallDuration { get; set; }
        public string CallAmount { get; set; }
        public DateTime date { get; set; }

        //public string PinNumber { get; set; }
        //public string CardName { get; set; }

        
        
    }
}
