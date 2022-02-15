using P2PLending.Web.Helper.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.Helpers
{
    public static class LoanApplicationHelper
    {
        public static readonly Dictionary<string, string> StatusMappings = new Dictionary<string, string>
        {
            { LoanStatus.Submitted, "Đang đợi duyệt" },
            { LoanStatus.UpdateRequired, "Cần cập nhật thông tin" },
            { LoanStatus.MoreInfoRequired, "Cần bổ sung thông tin" },
            { LoanStatus.Reject, "Bị từ chối" },
            { LoanStatus.Cancel, "Đã xóa" },
            { LoanStatus.InterestRateConfirm , "Đợi xác nhận lãi suất" },
            { LoanStatus.OnMarket , "Đang huy động vốn " },
            { LoanStatus.FundRaised, "Đã huy động đủ" },
            { LoanStatus.FundOvertime , "Hết thời gian huy động " },
            { LoanStatus.DisburmentPending, "Đang chờ nhận tiền" },
            { LoanStatus.OnLoan, "Đang vay tiền" }
        };
    }
}
