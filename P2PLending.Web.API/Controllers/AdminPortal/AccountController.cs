using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.Token;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Helper.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using P2PLending.Web.Helper.JwtHandlers;

namespace P2PLending.Web.API.Controllers.AdminPortal
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IOperationAccountService _operationAccountService;
        private readonly IJwtHandler _jwtHandler;

        public AccountController(
            IConfiguration configuration,
            IOperationAccountService operationAccountService,
            IJwtHandler jwtHandler)
        {
            _configuration = configuration;
            _operationAccountService = operationAccountService;
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Return Auth token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]APLoginRequest request)
        {
            var result = _operationAccountService.Authenticate(request.Username, request.Password);

            var accessToken = _jwtHandler.CreateAccessToken(result);
            var refreshToken = _jwtHandler.CreateRefreshToken(result.id);

            var authorizationTokens = new AuthorizationTokensResource { AccessToken = accessToken, RefreshToken = refreshToken };

            //return basic user info and authentication token
            return Ok(new
            {
                AuthenticationToken = authorizationTokens
            });
        }

        /// <summary>
        /// Request reset password, send email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPasswordRequest([FromBody] APResetPasswordRequest request)
        {
            string baseUrl = $@"{HttpContext.Request.Host}://{HttpContext.Request.Scheme}";
            _operationAccountService.CreateResetPasswordRequest(request.Email, request.BaseURL);
            return Ok();
        }

        /// <summary>
        /// Verify reset password token, update new password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password-verify")]
        public IActionResult VerifyResetPassword(ResetPasswordRequest request)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Token);

            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
            var code = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "key").Value;

            var result = _operationAccountService.VerifyPasswordReset(email, code, request.NewPassword);

            return Ok();
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("create")]
        public IActionResult CreateAccount([FromBody] APCreateAccountRequest request)
        {
            _operationAccountService.CreateAccountRequest(request);

            return Ok();
        }

        /// <summary>
        /// Filter list users
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("filter")]
        public IActionResult FilterOperationUser()
        {
            return Ok();
        }
    }
}
