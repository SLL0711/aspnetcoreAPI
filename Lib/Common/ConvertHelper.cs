using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Common
{
    public class ConvertHelper
    {
        public static long GetTimestamp()
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            long millisecondes = Convert.ToInt64((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);

            return millisecondes;
        }

        public static int ToInt(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ToDecimal(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0;
            }
        }

        public static string Tostring(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            return obj.ToString();
        }
    }
}
