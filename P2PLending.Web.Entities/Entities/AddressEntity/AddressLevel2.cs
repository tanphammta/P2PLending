using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AddressEntity
{
    public class AddressLevel2
    {
        public string level2_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string level1_id { get; set; }
        public AddressLevel1 address_level_1 { get; set; }
        public List<AddressLevel3> level3s { get; set; }
    }
}
