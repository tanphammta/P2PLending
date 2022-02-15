using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanApplicationAttributeValidate: BaseEntity
    {
        public int loan_application_id { get; set; }
        public LoanApplication loan_application { get; set; }
        public string page_name { get; set; }
        public string attribute_name { get; set; }
        public bool is_validate { get; set; }
        public string reason { get; set; }
    }
}
