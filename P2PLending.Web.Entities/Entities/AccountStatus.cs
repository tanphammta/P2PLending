using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities
{
    [Table("account_status")]
    public class AccountStatus: BaseEntity
    {
        public string status_code { get; set; }
        public string status_name { get; set; }
    }
}
