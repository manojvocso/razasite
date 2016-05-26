using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
    public class OneTouchSetups
    {
        public string Destination { get; set; }
        public string OneTouch_Name { get; set; }
        public string OneTouch_Number { get; set; }
        public  string PlanName { get; set; }
    }

    public class OneTouchSet
    {
        public List<OneTouchSetups> oneTouchList = new List<OneTouchSetups>();
       
        public static OneTouchSet Create(string data)
        {
            if (data.Split('|')[0].Split('=')[1] == "1")
            {
                var res = OneTouchSetUp(data);
                return res;
            }
            else
            {
                var res = new OneTouchSet();
                if (data.Split('|').Length > 1)
                {
                    string[] allrows = data.Split('|')[1].Split('~');
                    foreach (string eachrow in allrows)
                    {
                        res.oneTouchList.Add(new OneTouchSetups()
                        {
                          
                        });
                    }
                }
                return res;
               
              
            }
        }

        private static OneTouchSet OneTouchSetUp(string data)
        {
            var res = new OneTouchSet();

            if (data.Split('|').Length > 1)
            {
                string[] allrows = data.Split('|')[1].Split('~');
                foreach (string eachrow in allrows)
                {
                    res.oneTouchList.Add(new OneTouchSetups()
                    {
                        OneTouch_Number = eachrow.Split(',')[0],
                        Destination = eachrow.Split(',')[1],
                        OneTouch_Name = eachrow.Split(',')[2]
                    });
                }
            }
            return res;
        }
    }
}