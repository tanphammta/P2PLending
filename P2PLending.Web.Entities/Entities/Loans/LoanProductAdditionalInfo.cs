using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanProductAdditionalInfo: BaseEntity
    {
        public int loan_product_id { get; set; }
        public LoanProduct loan_type { get; set; }
        public string info_name { get; set; }
        public string info_format { get; set; }
    }
}
