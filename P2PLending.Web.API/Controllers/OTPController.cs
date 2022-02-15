

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;

namespace P2PLending.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController: ControllerBase
    {
        public IOTPService _otpService; 
        public OTPController(IOTPService otpService)
        {
            _otpService = otpService;
        }

        /// <summary>
        /// Send OTP, create new or update verification session by Phone number and Device ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("send-otp")]
        public IActionResult SendOTP([FromBody] PhoneVerificationRequest request)
        {
            var result = _otpService.SendOTP(request);
            return Ok(result);
        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public IActionResult Verify([FromBody] VerifyOTPRequest request)
        {
            var result = _otpService.VerifyOTP(request);
            return Ok(result);
        }
    }
}
