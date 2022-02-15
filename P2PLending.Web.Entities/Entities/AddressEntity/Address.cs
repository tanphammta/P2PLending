using P2PLending.Web.Entities.Entities.Base;
using P2PLending.Web.Entities.Entities.Borrower;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AddressEntity
{
    public class Address : BaseEntity
    {
        public string level1_id { get; set; }
        public string level2_id { get; set; }
        public string level3_id { get; set; }
        public string address_detail { get; set; }
    }
}
