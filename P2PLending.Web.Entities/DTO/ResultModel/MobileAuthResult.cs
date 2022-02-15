using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class MobileAuthResult: BaseResult
    {
        public MobileUserDTO User { get; set; }
        public AuthorizationTokensResource TokensResource { get; set; }
        public bool IsOtherDeviceLoggedIn { get; set; }
        public string PreviousDeviceId { get; set; }
        public string CurrentDeviceId { get; set; }
        public string PreviousFCMToken { get; set; }
        public string CurrentFCMToken { get; set; }
    }
}
