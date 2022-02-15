using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanApplicationAdditionalInfo: BaseEntity
    {
        public int loan_application_id { get; set; }
        public LoanApplication loan_application { get; set; }
        public int loan_product_additional_info_id { get; set; }
        public LoanProductAdditionalInfo loan_product_additional_info { get; set; }
        public string name { get; set; }
        public string format { get; set; }
        public string value { get; set; }
        public byte[] binary_value { get; set; }
    }
}
