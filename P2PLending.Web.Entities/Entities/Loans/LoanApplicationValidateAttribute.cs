using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanApplicationValidateAttribute: BaseEntity
    {
        public int loan_application_id { get; set; }
        public string attribute_name { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
        public int reference_id { get; set; }
        public bool is_validate { get; set; }
        public string reason { get; set; }
    }
}
