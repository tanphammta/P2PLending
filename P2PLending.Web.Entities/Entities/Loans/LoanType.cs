using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Loans
{
    public class LoanType: BaseEntity
    {
        public string type_code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int minimum_amount { get; set; }
        public int maximum_amount { get; set; }
        public string amount_unit { get; set; }
        public int minimum_duration { get; set; }
        public int maximum_duration { get; set; }
        public string duration_unit { get; set; }
    }
}
