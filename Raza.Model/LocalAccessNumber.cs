// -----------------------------------------------------------------------
// <copyright file="LocalAccessNumber.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Raza.Model
{
    public class LocalAccessNumber
    {
        public string SNo { get; set; }

        public string AreaCode { get; set; }

        public string NextThree { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Ani { get; set; }

        public string NonAni { get; set; }

        public string Type { get; set; }

        public string CountryId { get; set; }

        public string PhoneNo { get; set; }

        public string PinLocalNumber { get; set; }

        public string PinLessLocalNumber { get; set; }

        public string AccessNumber { get; set; }

        //public string PinLocalNumber
        //{
        //    get { return string.Format("{0}-{1}-{2}", AreaCode, NextThree, Ani); }
        //}

        //public string PinLessLocalNumber
        //{
        //    get { return string.Format("{0}-{1}-{2}", AreaCode, NextThree, NonAni); }
        //}

    }

    

    public class CreateLocalAccessNumberModel
    {
        //  status=1|IL,847-241-2104,847-241-2105,Algonquin~IL,847-416-5008,847-416-5009,Algonquin~
        //         IL,847-464-8001,847-464-8002,Algonquin
        public static List<LocalAccessNumber> Create(string data)
        {
            var numberlist = new List<LocalAccessNumber>();

            string[] allrows = data.Split('|')[1].Split('~');
            int i = 1;
            foreach (string eachrow in allrows)
            {
                //string plan = eachrow.Split(',')[1];
                numberlist.Add(new LocalAccessNumber()
                {
                    SNo = SafeConvert.ToString(i),
                    AccessNumber   = eachrow.Split(',')[0],
                    State = eachrow.Split(',')[1],
                     City= eachrow.Split(',')[2]                   
                    
                });
                i++;
            }
            
            return numberlist;
        }

    }
}


