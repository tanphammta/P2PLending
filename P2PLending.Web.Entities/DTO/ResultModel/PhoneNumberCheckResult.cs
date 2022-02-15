using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class PhoneNumberCheckResult: BaseResult
    {
        public bool IsBorrowerExist { get; set; }
        public bool IsInvestorExist { get; set; }
        public bool IsCorrectPhoneNumberFormat { get; set; }
        public bool IsOTPLock { get; set; }
        public long? LockExpired { get; set; }
    }
}
