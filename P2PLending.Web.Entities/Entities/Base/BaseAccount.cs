using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P2PLending.Web.Entities.Entities.Base
{
    public class BaseAccount: BaseEntity
    {
        [Required]
        public string password { get; set; }
        [Required]
        public string password_salt { get; set; }
        public string password_hash_algorithm { get; set; }
        public long registration_time { get; set; }
        [MaxLength(30)]
        public string role { get; set; }
        [Required]
        public bool is_active { get; set; } = false;
    }
}
