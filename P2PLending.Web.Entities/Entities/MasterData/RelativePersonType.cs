using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    [Table("borrower_relative_person_types")]
    public class RelativePersonType: BaseEntity
    {
        public string type_name { get; set; }
    }
}
