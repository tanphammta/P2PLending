using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.OTP
{
    public class SMSOTP: BaseEntity
    {
        public int phone_verification_id { get; set; }
        public PhoneVerification phone_verification { get; set; }
        public string phone { get; set; }
        public string value { get; set; }
        public string status { get; set; }
        public DateTime expired_date { get; set; }
    }
}
