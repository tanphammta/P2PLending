using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    [Table("counties")]
    public class County: BaseEntity
    {
        public int city_id { get; set; }
        public string name { get; set; }
    }
}
