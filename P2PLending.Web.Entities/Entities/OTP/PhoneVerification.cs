using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.OTP
{
    public class PhoneVerification: BaseEntity
    {
        public string phone { get; set; }
        public string verification_type { get; set; }
        public bool verified { get; set; }
        public string verify_device_id { get; set; }
        public string verify_device_name { get; set; }
        public DateTime? verified_date { get; set; }
        public int retry_times { get; set; }
        public bool is_lock { get; set; }
        public DateTime? lock_expired_date { get; set; }
        public int current_sms_otp_id { get; set; }
        public DateTime? verification_expired { get; set; }
        public List<SMSOTP> smsOTPs { get; set; }
        public string verified_code { get; set; }
    }
}
