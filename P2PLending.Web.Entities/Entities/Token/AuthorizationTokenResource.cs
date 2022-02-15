using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Token
{
    public class AuthorizationTokensResource
    {
        public TokenResource AccessToken { get; set; }
        public TokenResource RefreshToken { get; set; }
        public string FCMToken { get; set; }
    }
}
