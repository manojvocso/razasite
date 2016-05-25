using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
   public class GetForwardNumber
    {
        public List<GetForwardList> GetNumberList = new List<GetForwardList>();
        public static GetForwardNumber Create(string data)
        {
            var getNumber = new GetForwardNumber();
            if (data.Split('|')[0].Split('=')[1] == "1")
            {
               
                if (data.Split('|').Length > 0)
                {
                    string[] allrows = data.Split('|')[1].Split('~');
                    foreach (var eachrow in allrows)
                    {
                        getNumber.GetNumberList.Add(new GetForwardList()
                        {
                            Sno = Convert.ToInt16(eachrow.Split(',')[0]),
                            FollowMeNumber = eachrow.Split(',')[1],
                            Name = eachrow.Split(',')[2],
                            DestNum = eachrow.Split(',')[3],
                            ActiveDate = eachrow.Split(',')[4],
                            ExpDate = eachrow.Split(',')[5],
                            Status = eachrow.Split(',')[6]
                           });

                    }
                }
                return getNumber;
            }

            else
            {
                return getNumber;
            }

        }
    }

    public class GetForwardList
    {
        public string Status { get; set; }
        public int Sno { get; set; }
        public  string FollowMeNumber { get; set; }
        public  string DestNum { get; set; }
        public  string ActiveDate { get; set; }
        public  string ExpDate { get; set; }
        public  string Name { get; set; }



    }
}
