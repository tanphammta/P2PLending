using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanTypeAdditionalInfo: BaseEntity
    {
        public int loan_type_id { get; set; }
        public LoanType loan_type { get; set; }
        public string info_name { get; set; }
        public string info_format { get; set; }
    }
}
