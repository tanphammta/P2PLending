using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Base;
using P2PLending.Web.Entities.Entities.Borrower;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanApplication: BaseEntity
    {
        public int account_id { get; set; }
        public int loan_product_id { get; set; }
        public int borrower_profile_id { get; set; }
        public int loan_amount { get; set; }
        public int loan_duration { get; set; }
        public float interest_rate { get; set; }
        public int fees { get; set; }
        public int borrower_linked_payment_service_id { get; set; }
        public string status { get; set; }
    }
}
