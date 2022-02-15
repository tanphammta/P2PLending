using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class OTPResult: BaseResult
    {
        public string Phone { get; set; }
        public string DeviceId { get; set; }
        public string OTP { get; set; }
        public long? OTPExpiredDate { get; set; }
        public int RetryTimes { get; set; }
        public string VerifiedCode { get; set; }
        public long? VerificationExpired { get; set; }
        public bool IsLock { get; set; } = false;
        public long? LockExpired { get; set; }
    }
}
