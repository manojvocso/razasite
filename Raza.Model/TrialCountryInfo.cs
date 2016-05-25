// -----------------------------------------------------------------------
// <copyright file="TrialCountryInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;

namespace Raza.Model
{
    public class TrialCountryInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Minutes { get; set; }

        public string Desc { get; set;   }
    }

    public class TrialCountry
    {
        public static List<TrialCountryInfo> Create(string data)
        {
            string res = data.Split('|')[1];
            string[] allrows = res.Split('~');

            var list = new List<TrialCountryInfo>();
            for (int i = 0; i < allrows.Length && allrows[i].Length > 0; i++)
            {
                list.Add(new TrialCountryInfo
                {
                    Id = allrows[i].Split(',')[0],
                    Name = allrows[i].Split(',')[1],
                    Minutes = allrows[i].Split(',')[2],
                    Desc = string.Format("{0} - {1} MIN.", allrows[i].Split(',')[1], allrows[i].Split(',')[2])
                });
            }

            return list;
        }
    }
}
