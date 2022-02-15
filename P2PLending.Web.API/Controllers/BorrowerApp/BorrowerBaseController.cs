using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.DataTransfer;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using P2PLending.Web.Helper.Extension;
using Microsoft.AspNetCore.Http;
using P2PLending.Web.Helper.Constants;

namespace P2PLending.Web.API.Controllers.BorrowerApp
{
    public class BorrowerBaseController: Controller
    {
        protected MobileUserDTO CurrentUser;
        protected IMobileAccountService _mobileAccountService;
        public BorrowerBaseController(IMobileAccountService mobileAccountService)
        {
            _mobileAccountService = mobileAccountService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actName = (string)context.RouteData.Values["Action"];
            if (actName.ToLower() != "authenticate" && CurrentUser == null)
            {
                var sessionUser = HttpContext.Session.Get<MobileUserDTO>(SessionKey.CurrentUser);
                if (sessionUser != null)
                {
                    CurrentUser = sessionUser;
                }
                else
                {
                    var accessToken = context.HttpContext.Request.Headers[HeaderNames.Authorization];
                    var token = ExtractToken(accessToken);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
                        int id = 0;
                        if (Int32.TryParse(userId, out id))
                        {
                            var user = _mobileAccountService.GetById(id);
                            CurrentUser = user;
                            // save token into session
                            HttpContext.Session.Set(SessionKey.CurrentUser, user);
                        }
                    }
                }
            }
            base.OnActionExecuting(context);
        }

        private string ExtractToken(string bearer)
        {
            if (!string.IsNullOrEmpty(bearer) && bearer.Split(" ")[0] == "Bearer")
            {
                return bearer.Split(' ')[1];
            }

            return null;
        }
    }
}
