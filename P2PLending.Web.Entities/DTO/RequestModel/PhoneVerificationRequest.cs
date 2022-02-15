using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class PhoneVerificationRequest
    {
        public string Phone { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string VerificationType { get; set; }
    }
}
