using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public int id { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
    }
}
