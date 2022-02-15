using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AccountLinkedPayment
{
    [Table("borrower_linked_payment_services")]
    public class BorrowerLinkedPaymentService: BaseEntity
    {
        [ForeignKey("account_mobile")]
        public int account_id { get; set; }
        public string service_code { get; set; }
        public string service_name { get; set; }
        public string service_account_id { get; set; }
        public string service_owner_name { get; set; }
    }
}
