using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace P2PLending.Web.Helper.Helpers
{
    public class AccountHelper
    {
        //public static AuthorizationTokensResource CreateAccessTokens(AdminAccount account, string deviceId,
        // string deviceName, JwtHandler jwtHandler)
        //{
        //    var accessToken = jwtHandler.CreateAccessToken(account.id, account.email, string.Empty);
        //    var refreshToken = jwtHandler.CreateRefreshToken(account.id);

        //    return new AuthorizationTokensResource { AccessToken = accessToken, RefreshToken = refreshToken };
        //}

        public static string CreateEmailConfirmToken()
        {
            byte[] data = new byte[64];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }
            return Regex.Replace(Convert.ToBase64String(data), "[/]", ".");
        }

        public static byte[] CreatePasswordSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
