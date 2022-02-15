using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Token
{
    public class TokenResource
    {
        public string Token { get; set; }
        public long Expiry { get; set; }
    }
}
