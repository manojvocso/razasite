using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Raza.Model.Helpers
{
    public static class MessageFactory
    {
        private static readonly Dictionary<string, string> MessageList = new Dictionary<string, string>();
        public static string ToDisplayMessage(this string message)
        {
            if (MessageList.ContainsKey(message))
                return MessageList[message];

            return message;
        }

        public static void InitilizeFactory(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (MessageList.ContainsKey(dr[0].ToString()))
                    {
                        MessageList.Add(dr[0].ToString(), dr[1].ToString());
                    }
                    else
                    {
                        MessageList[dr[0].ToString()] = dr[1].ToString();
                    }
                }
            }
        }

    }
}
