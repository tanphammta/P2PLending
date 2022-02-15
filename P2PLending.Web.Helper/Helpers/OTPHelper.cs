using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.Helpers
{
    public static class OTPHelper
    {
        public static string GenerateOTP()
        {
            Random r = new Random();
            int min = 0;
            int max = 999999;
            int random = r.Next(min, max);
            return random.ToString("D6"); // zero-padding
        }
    }
}
