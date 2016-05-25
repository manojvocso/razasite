using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class MobileTopupOperator
    {
        public string CountryId { get; set; }
        public string OperatorName { get; set; }
        public string OperatorCode { get; set; }
        public double SourceAmount { get; set; }
        public string SourceAmountwithSign { get; set; }
        public double DestinationAmount { get; set; }
        public string Currency { get; set; }
        public string CarierImage { get; set; }

    }


   public class TopUpOperator
    {
       public List<MobileTopupOperator> OperatorList { get; set; }

       public TopUpOperator()
        {
            OperatorList = new List<MobileTopupOperator>();
        }
       
       public static TopUpOperator Create(string res)
       {
           var topUpOperator = new TopUpOperator();
           if (res.Split('|')[0].Split('=')[1] == "1")
           {
               string data = res.Split('|')[1];
               string[] allrows = data.Split('~');
               if (allrows.Length > 1)
               {
                   for (int i = 0; i < allrows.Length && allrows[i].Length > 0; i++)
                   {
                        // string a= "130,Aircel,1155,5.0000,200.0000,INR";
                       // country id, operatorname, opretor code, source amount, destamount, 
                      topUpOperator.OperatorList.Add(new MobileTopupOperator
                       {
                           CountryId = allrows[i].Split(',')[0],
                           OperatorName = allrows[i].Split(',')[1],
                           OperatorCode = allrows[i].Split(',')[2],
                           SourceAmount = Convert.ToDouble(allrows[i].Split(',')[3]),
                           DestinationAmount= Convert.ToDouble(allrows[i].Split(',')[4]),
                           Currency = allrows[i].Split(',')[5]
                       });
                   }
               }
           }
           return topUpOperator;
       } 
    }
}
