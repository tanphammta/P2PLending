using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Business.Interface
{
    public interface IOperationAccountService
    {
        AccountOperation Authenticate(string username, string password);
        bool CreateResetPasswordRequest(string email, string baseURL);
        BaseResult VerifyPasswordReset(string email, string code, string newPassword);
        BaseResult CreateAccountRequest(APCreateAccountRequest request);
    }
}
