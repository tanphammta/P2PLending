using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.OTP
{
    [Table("phone_signin_locks")]
    public class PhoneSigninLock: BaseEntity
    {
        public string phone { get; set; }
        [DefaultValue(false)]
        public bool signin_lock { get; set; }
        public DateTime signin_lock_expired { get; set; }
    }
}
