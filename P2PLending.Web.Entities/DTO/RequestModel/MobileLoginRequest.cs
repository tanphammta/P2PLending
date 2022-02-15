using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class MobileLoginRequest
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string FCMToken { get; set; }
    }
}
