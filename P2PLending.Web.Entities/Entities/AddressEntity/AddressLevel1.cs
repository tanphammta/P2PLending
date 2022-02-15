using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.AddressEntity
{
    public class AddressLevel1
    {
        public string level1_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<AddressLevel2> level2s { get; set; }
    }
}
