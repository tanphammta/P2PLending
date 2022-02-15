using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Interface
{
    public interface IMobileAccountService
    {
        PhoneNumberCheckResult CheckPhoneNumber(string phone, string deviceId);
        BaseResult BorrowerAccountRegisting(MobileAccountRegistrationRequest request);
        Task<MobileAuthResult> SignIn(MobileLoginRequest request);
        MobileUserDTO GetById(int id);
        BaseResult AddReferer(int userId, string refererPhone);
        BaseResult ResetPassword(MobileResetPasswordRequest request);
        BaseResult SignOut(int userId);
        BaseResult UpdateFacebookUserData(FacebookUserResource facebookData, MobileUserDTO currentUser);
    }
}
