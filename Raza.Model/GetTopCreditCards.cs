using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Raza.Model
{
    using System.Globalization;
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
    public class GetTopCreditCards
    {

        public List<GetCard> GetCardList = new List<GetCard>();

        public static GetTopCreditCards Create(string res)
        {
            //string pre = "/images/";
            //IDictionary<int, string> flagdict = new Dictionary<int, string>();
            var getNumbers = new GetTopCreditCards();
            if (res.Split('~')[0].Split('=')[1] == "1")
            {
                string data = res.Split('~')[1];
                if (data.Length > 0)
                {
                    string[] allrows = data.Split('`');

                    foreach (string eachrow in allrows)
                    {
                            GetCard getCard = new GetCard()
                            {
                                CreditCardId = SafeConvert.ToInt32(eachrow.Split('|')[0]), //todo: getting orignel value from web service.
                                CreditCardName = eachrow.Split('|')[1],
                                CreditCardType = eachrow.Split('|')[2].ToUpper(),
                                CreditCardNumber = eachrow.Split('|')[3],
                                ExpiryMonth = eachrow.Split('|')[4],
                                ExpiryYear = eachrow.Split('|')[5],
                                CVV = eachrow.Split('|')[6],
                                CardAddedDate = eachrow.Split('|')[7],

                            };


                            getCard.CreditCardNo = getCard.CreditCardNumber.GetLast(4);
                            //getCard.MaskCardNumber = getCard.CreditCardNumber.Remove(4, 4).Insert(4, "XXXXX");


                            if (string.IsNullOrEmpty(getCard.ExpiryYear))
                            {
                                getCard.ExpiryYear = "01";
                            }

                            if (string.IsNullOrEmpty(getCard.ExpiryMonth))
                            {
                                getCard.ExpiryMonth = "01";
                            }
                            getCard.ExpiryDate = getCard.ExpiryYear + "/" + getCard.ExpiryMonth;
                            getCard.ViewExpDate = getCard.ExpiryMonth + "/" + getCard.ExpiryYear;




                            DateTime dsDateTime = DateTime.ParseExact(getCard.ExpiryDate, "yy/MM",
                                CultureInfo.InvariantCulture);

                            getCard.CardStatus = dsDateTime < DateTime.Now ? "Update" : "Active";

                            var date = DateTime.Now;
                            getCard.CurrentDate = date.ToString("yy/MM");
                            getCard.CurrentDateYear = date.ToString("yyyy/MM");

                            getNumbers.GetCardList.Add(getCard);
                        }
                    }
              
                return getNumbers;
            }

            else
            {
                return getNumbers;
            }

        }
    }

    public class GetCard
    {
        public int CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNo { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardAddedDate { get; set; }
        public string CurrentDate { get; set; }
        public string ExpiryDate { get; set; }
        public string CardStatus { get; set; }
        public string ViewExpDate { get; set; }
        public string CreditCardNumber { get; set; }
        public string MaskCardNumber { get; set; }
        public string CurrentDateYear { get; set; }
        public string CardTypeLogo { get; set; }

    }

    

    public class GetRechargeAmount
    {

        public List<GetAmount> GetAmountlist = new List<GetAmount>();

        public static GetRechargeAmount Create(string res)
        {
            
            //string pre = "/images/";
            //IDictionary<int, string> flagdict = new Dictionary<int, string>();
            var getNumbers = new GetRechargeAmount();
            if (res.Split('|')[0].Split('=')[1] == "1")
            {
                string  data=res.Split('|')[1].Split('=')[1];
                if (data.Length > 0)
                {
                    string[] allrows = data.Split(',');
                    foreach (string eachrow in allrows)
                    {
                        getNumbers.GetAmountlist.Add(new GetAmount()
                        {
                            RechAmount = Convert.ToDouble(eachrow.Split(',')[0])
                           
                        });

                    }
                }
                return getNumbers;
            }

                else
                {
                    return getNumbers;
                }

         
        }
    }
 
    public class GetAmount
    {
      
        public double RechAmount { get; set; }
    }

    public class EditCreditCardModel
    {
        public int CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardType { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string ExpiryDate { get; set; }
        public string CardStatus { get; set; }
        public string CreditCardNumber { get; set; }
        public List<ExpDateEntity> ExpMonthList { get; set; } 
        public List<ExpDateEntity> ExpYearList { get; set; } 
    }

    public class ExpDateEntity
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

}




