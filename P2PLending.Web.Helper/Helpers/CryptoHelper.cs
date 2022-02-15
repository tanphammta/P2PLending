using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace P2PLending.Web.Helper.Helpers
{
    public static class CryptoHelper
    {
        /// <summary>
        /// Generate random 256 bit crypto key
        /// </summary>
        /// <returns></returns>
        public static string GenerateCrypto()
        {
            string valid = Guid.NewGuid().ToString();
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[32];
                rng.GetBytes(buffer);
                res.Append(Convert.ToBase64String(buffer));
            }

            return res.ToString();
        }
    }
}
