using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Token
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public int AccessExpireMinutes { get; set; }
        public int RefreshExpireMinutes { get; set; }
    }
}
