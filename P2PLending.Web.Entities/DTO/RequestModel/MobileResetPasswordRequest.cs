using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class MobileResetPasswordRequest
    {
        public string Phone { get; set; }
        public string DeviceId { get; set; }
        public string VerifiedCode { get; set; }
        public string NewPassword { get; set; }
    }
}
