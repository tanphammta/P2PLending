using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;

namespace P2PLending.Web.Business.Interface
{
    public interface IOTPService
    {
        public OTPResult SendOTP(PhoneVerificationRequest request);

        public OTPResult VerifyOTP(VerifyOTPRequest request);
    }
}
