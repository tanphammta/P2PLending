using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Account
{
    public class AccountOperation: BaseAccount
    {
        public string username { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string phone { get; set; }
        public string full_name { get; set; }
        public int manager_id { get; set; }
        public int department_id { get; set; }
        public int position_id { get; set; }
        public string password_reset_token { get; set; }
        public DateTime? password_reset_expire { get; set; }
        public bool? password_reset_token_consumed { get; set; }
        public bool is_wait_password_create { get; set; }
    }
}
