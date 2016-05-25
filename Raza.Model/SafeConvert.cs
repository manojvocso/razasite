// -----------------------------------------------------------------------
// <copyright file="SafeConvert.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Raza.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SafeConvert
    {
        public static int ToInt32(string value)
        {
            int val;

            if (int.TryParse(value, out val))
            {
                return val;
            }

            return 0;
        }

        public static decimal ToDecimal(string value)
        {
            decimal val;

            if (decimal.TryParse(value, out val))
            {
                return val;
            }

            return 0;
        }

        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;

            return Convert.ToString(value);
        }

        public static int ToInt32(object value)
        {
            if (value == null || value == DBNull.Value) return 0;

            return Convert.ToInt32(value);
        }
    }
}
