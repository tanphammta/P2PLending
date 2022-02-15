using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    [Table("loan_application_types")]
    public class LoanType: BaseEntity
    {
        public string name { get; set; }
        public string description { get; set; }
        public int minimum_amount { get; set; }
        public int maximum_amount { get; set; }
        public int minimum_duration { get; set; }
        public int maximum_duration { get; set; }
    }
}
