using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.Extension
{
    public static class DateTimeExtension
    {
        public static long ToUnixSeconds(this DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static long? ToUnixSeconds(this DateTime? dateTime)
        {
            if(dateTime == null)
            {
                return null;
            }
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static DateTime ToDateTime(this double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public static DateTime ToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}
