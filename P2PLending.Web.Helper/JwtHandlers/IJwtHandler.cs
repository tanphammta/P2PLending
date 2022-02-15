using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.JwtHandlers
{
    public interface IJwtHandler
    {
        TokenResource CreateAccessToken(AccountOperation account);
        TokenResource CreateAccessToken(AccountMobile user);
        TokenResource CreateRefreshToken(int userId);
        TokenResource CreateResetPasswordToken(string email, string key);
    }
}
