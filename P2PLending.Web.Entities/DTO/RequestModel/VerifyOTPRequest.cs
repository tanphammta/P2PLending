using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class VerifyOTPRequest
    {
        public string Phone { get; set; }
        public string DeviceId { get; set; }
        public string Value { get; set; }
        public string VerificationType { get; set; }
    }
}
