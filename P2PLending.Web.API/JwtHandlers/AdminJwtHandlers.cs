using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using P2PLending.Web.Entities.Token;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P2PLending.Web.API.JwtHandlers
{
    public class AdminJwtHandler
    {
        private readonly IConfiguration _configuration;

        public AdminJwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResource CreateAccessToken(int userId, string email, string role)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["Tokens:AccessExpireMinutes"]));
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

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["Tokens:RefreshExpireMinutes"]));
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
