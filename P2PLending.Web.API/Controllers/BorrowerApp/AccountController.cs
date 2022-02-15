using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLending.Web.Entities.DTO.RequestModel;
using Microsoft.AspNetCore.Http;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Helper.JwtHandlers;
using System.Threading.Tasks;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Entities.DTO.DataTransfer;
using System;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Helper.Helpers;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace P2PLending.Web.API.Controllers.BorrowerApp
{
    [Route("api/borrower/[controller]")]
    [ApiController]
    public class AccountController : BorrowerBaseController
    {
        private readonly IJwtHandler _jwtHandler;
        private ILinkedPaymentService _linkedPaymentService;
        private IWebHostEnvironment _webHostEnvironment;
        private IFacebookService _facebookService;
        public AccountController(
            IMobileAccountService mobileAccountService, 
            IJwtHandler jwtHandler,
            ILinkedPaymentService linkedPaymentService,
            IWebHostEnvironment webHostEnvironment,
            IFacebookService facebookService): base(mobileAccountService)
        {
            _jwtHandler = jwtHandler;
            _linkedPaymentService = linkedPaymentService;
            _webHostEnvironment = webHostEnvironment;
            _facebookService = facebookService;
    }

        /// <summary>
        /// Check phone number format, exist account, lock otp
        /// </summary>
        /// <param name="phone">Phone Number</param>
        /// <returns>IsBorrowerExist, IsInvestorExist</returns>
        [AllowAnonymous]
        [HttpPost("check-phone_number")]
        public IActionResult CheckPhoneNumber([FromBody] PhoneCheckExistRequest request)
        {
            var result = _mobileAccountService.CheckPhoneNumber(request.Phone, request.DeviceId);
            return Ok(result);
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <returns>Return success/failed message</returns>
        [AllowAnonymous]
        [HttpPost("registing")]
        public IActionResult Registing([FromBody] MobileAccountRegistrationRequest request)
        {
            var result = _mobileAccountService.BorrowerAccountRegisting(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] MobileResetPasswordRequest request)
        {
            var result = _mobileAccountService.ResetPassword(request);
            return Ok(result);
        }

        /// <summary>
        /// Return Auth token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] MobileLoginRequest request)
        {
            var result = await _mobileAccountService.SignIn(request);
            HttpContext.Session.Set(SessionKey.CurrentUser, result.User);

            //return basic user info and authentication token
            return Ok(new
            {
                AuthenticationToken = result.TokensResource,
                User = result.User
            });
        }
        /// <summary>
        /// Adding account's referrer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Borrower)]
        [HttpPost("add-referer")]
        public IActionResult AddReferer(AddReferrerRequest request)
        {
            if(CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            if (string.IsNullOrEmpty(request.RefererPhone))
            {
                // empty referer phone
                return BadRequest(
                    new BaseResult() { 
                        IsSuccess = false,
                        Message = BorrowerAppMessage.MSG56,
                        MessageCode = nameof(BorrowerAppMessage.MSG56)
                    }
                );
            }
            var result = _mobileAccountService.AddReferer(CurrentUser.AccountId, request.RefererPhone);

            return Ok(result);
        }

        /// <summary>
        /// Remove logged in device, remove session
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Borrower)]
        [HttpGet("signout")]
        public IActionResult SignOut()
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            HttpContext.Session.Remove(SessionKey.CurrentUser);
            
            return Ok();
        }

        [Authorize]
        [HttpGet("get-linked-payments")]
        public IActionResult AddLinkedPayment(int accountId)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }

            string baseUrl = HTTPContextHelper.GetBaseURL(HttpContext);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var linkedPayments = _linkedPaymentService.GetLinkedPaymentService(CurrentUser.AccountId);

            foreach (var data in linkedPayments)
            {
                data.IconPath = $"{baseUrl}/icons/{data.Icon}";
                string path = Path.GetFullPath($"Images/Icons/{data.Icon}", contentRootPath);
                if (System.IO.File.Exists(path))
                {
                    var lastWriteLite = System.IO.File.GetLastWriteTime(path);
                    data.IconLastModifiedDate = lastWriteLite.ToUnixSeconds();
                }
            }

            return Ok(linkedPayments);
        }

        [Authorize(Roles = UserRole.Borrower)]
        [HttpPost("add-update-linked-payment")]
        public IActionResult AddUpdateLinkedPayment([FromBody] AddUpdateLinkedPaymentRequest request)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            string baseUrl = HTTPContextHelper.GetBaseURL(HttpContext);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var result = _linkedPaymentService.AddUpdateLinkedPayment(CurrentUser.AccountId, request);

            if(result != null)
            {
                result.IconPath = $"{baseUrl}/icons/{result.Icon}";
                string path = Path.GetFullPath($"Images/Icons/{result.Icon}", contentRootPath);
                if (System.IO.File.Exists(path))
                {
                    var lastWriteLite = System.IO.File.GetLastWriteTime(path);
                    result.IconLastModifiedDate = lastWriteLite.ToUnixSeconds();
                }
            }
            
            return Ok(result);
        }

        [Authorize(Roles = UserRole.Borrower)]
        [HttpDelete("remove-linked-payment")]
        public IActionResult RemoveLinkedPayment(int linkedPaymentId)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            var result = _linkedPaymentService.RemoveLinkedPayment(linkedPaymentId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-payment-services")]
        public IActionResult GetPaymentServices()
        {
            string baseUrl = HTTPContextHelper.GetBaseURL(HttpContext);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var datas = _linkedPaymentService.GetPaymentServices();
            foreach (var data in datas)
            {
                data.IconPath = $"{baseUrl}/icons/{data.Icon}";
                string path = Path.GetFullPath($"Images/Icons/{data.Icon}", contentRootPath);
                if (System.IO.File.Exists(path))
                {
                    var lastWriteLite = System.IO.File.GetLastWriteTime(path);
                    data.IconLastModifiedDate = lastWriteLite.ToUnixSeconds();
                }
            }

            return Ok(datas);
        }

        [Authorize]
        [HttpPost("login-facebook")]
        public async Task<IActionResult> LoginFacebook([FromBody] FacebookLoginRequest request)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            var facebookUser = await _facebookService.GetUserFromFacebookAsync(request.Token);
            var result = _mobileAccountService.UpdateFacebookUserData(facebookUser, CurrentUser);
            return Ok(facebookUser);
        }
    }
}
