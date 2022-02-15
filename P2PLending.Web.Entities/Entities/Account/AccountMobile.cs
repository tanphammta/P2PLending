using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Account
{
    public class AccountMobile: BaseAccount
    {
        public string phone { get; set; }
        public string current_device_id { get; set; }
        public string current_device_name { get; set; }
        public string fcm_token { get; set; }
        public string referrer_phone { get; set; }
        public int? referrer_account_id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string email { get; set; }
        public string facebook_id { get; set; }
    }
}
