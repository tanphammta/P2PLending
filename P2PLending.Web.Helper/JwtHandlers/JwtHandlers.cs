
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Token;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P2PLending.Web.Helper.JwtHandlers
{
    public class JwtHandler: IJwtHandler
    {
        private readonly TokenSettings _tokenSettings;

        public JwtHandler(IOptions<TokenSettings> options)
        {
            _tokenSettings = options.Value;
        }

        public TokenResource CreateAccessToken(AccountOperation account)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.id.ToString()),
                new Claim(ClaimTypes.Email, account.email),
                new Claim(ClaimTypes.Role, account.role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(_tokenSettings.AccessExpireMinutes);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, ((DateTimeOffset)expiry).ToUnixTimeSeconds());
        }

        public TokenResource CreateAccessToken(AccountMobile user)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.phone),
                new Claim(ClaimTypes.Role, user.role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var key = System.Text.Encoding.ASCII.GetBytes(_tokenSettings.Key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(_tokenSettings.AccessExpireMinutes);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, ((DateTimeOffset)expiry).ToUnixTimeSeconds());
        }

        public TokenResource CreateRefreshToken(int userId)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(_tokenSettings.RefreshExpireMinutes);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, ((DateTimeOffset)expiry).ToUnixTimeSeconds());
        }

        public TokenResource CreateResetPasswordToken(string email, string key)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("key", key),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(15);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, ((DateTimeOffset)expiry).ToUnixTimeSeconds());
        }

        private JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims, DateTime now, DateTime expiry, SigningCredentials credentials)
            => new JwtSecurityToken(claims: claims, notBefore: now, expires: expiry, signingCredentials: credentials);

        private static TokenResource CreateTokenResource(string token, long expiry)
            => new TokenResource { Token = token, Expiry = expiry };
    }
}
