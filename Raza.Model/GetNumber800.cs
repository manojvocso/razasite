using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
  public class GetNumber800
    {
        public List<GetList800> GetNumberList = new List<GetList800>();
        public static GetNumber800 Create(string data)
        {
            var getNumber = new GetNumber800();
            if (data.Split('|')[0].Split('=')[1] == "1")
            {

                if (data.Split('|').Length > 1)
                {
                    string[] allrows = data.Split('|')[1].Split(',');
                    foreach (string eachrow in allrows)
                    {
                        getNumber.GetNumberList.Add(new GetList800()
                        {
                            GetNumber = eachrow.Split(',')[0]
                           
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

    public class GetList800
    {
        
        public string GetNumber { get; set; }



    }
}
