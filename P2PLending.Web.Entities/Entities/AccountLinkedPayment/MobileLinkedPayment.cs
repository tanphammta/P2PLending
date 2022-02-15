using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AccountLinkedPayment
{
    public class MobileLinkedPayment: BaseEntity
    {
        public int account_id { get; set; }
        public int service_id { get; set; }
        public string service_account_id { get; set; }
        public string service_account_name { get; set; }
        public string phone { get; set; }
    }
}
