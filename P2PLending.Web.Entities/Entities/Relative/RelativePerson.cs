using P2PLending.Web.Entities.Entities.Base;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Relative
{
    public class RelativePerson: BaseEntity
    {
        public int borrower_profile_id { get; set; }
        public BorrowerProfile borrower_profile { get; set; }
        public int type_id { get; set; }
        public RelativePersonType relative_person_type { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public int no { get; set; }
    }
}
