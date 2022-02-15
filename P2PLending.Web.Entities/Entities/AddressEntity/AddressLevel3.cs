using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AddressEntity
{
    public class AddressLevel3
    {
        public string level3_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string level2_id { get; set; }
        public AddressLevel2 address_level_2 { get; set; }
    }
}
