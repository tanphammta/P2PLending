using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.Constants
{
    public static class Status
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
    }

    public static class OTPStatus
    {
        public const string Created = "Created";
        public const string Sent = "Sent";
        public const string FailedToSend = "FailedToSend";
        public const string Verified = "Verified";
    }

    public static class VerificationType
    {
        public const string SignIn = "SignIn";
        public const string Registration = "Registration";
    }

    public static class AdditionalInfoFormat
    {
        public const string Media = "Media";
        public const string Text = "Text";
    }

    public static class UserRole
    {
        public const string Borrower = "Borrower";
        public const string Investor = "Investor";
        public const string Admin = "Admin";
        public const string Sale = "Sale";
        public const string Appraisal = "Appraisal";
        public const string Collector = "Collector";
        public const string Accountant = "Accountant";
    }

    public static class OtherMessage
    {
        public const string OMSG01 = "Tài khoản không tồn tại";
    }

    public static class BorrowerAppMessage
    {
        public const string MSG02  = "Vui lòng nhập số điện thoại để tiếp tục.";
        public const string MSG03  = "Số điện thoại không hợp lệ. Vui lòng thử lại!";
        public const string MSG04  = "Số điện thoại đã tồn tại. Vui lòng sử dụng số điện thoại khác!";
        public const string MSG05  = "Sai mã OTP. Quý khách vui lòng nhập lại mã OTP. Nếu quý khách nhập sai mã OTP quá 3 lần thì hệ thống sẽ xóa toàn bộ thông tin đã điền và kết thúc quá trình đăng ký tài khoản.";
        public const string MSG06  = "Quý khách đã nhập sai mã OTP 3 lần.";
        public const string MSG07  = "Sai mật khẩu tạm thời OTP. Quý khách vui lòng nhập lại mật khẩu tạm thời OTP. Nếu quý khách nhập sai mật khẩu tạm thời OTP quá 3 lần thì hệ thống sẽ xóa toàn bộ thông tin đã điền và kết thúc quá trình đăng ký tài khoản.";
        public const string MSG08  = "Quý khách đã nhập sai mật khẩu tạm thời OTP 3 lần. Thông tin của quý khách sẽ bị xóa, vui lòng điền lại thông tin để đăng tài khoản.";
        public const string MSG09  = "Mật khẩu nhập vào không hợp lệ. Mật khẩu chỉ gồm 6 số, không bao gồm chữ cái và các ký tự khác.";
        public const string MSG10  = "Mật khẩu xác nhận không hợp lệ. Mật khẩu chỉ gồm 6 số, không bao gồm chữ cái và các ký tự khác.";
        public const string MSG11  = "Mật khẩu và mật khẩu xác nhận không giống nhau. Vui lòng thử lại!";
        public const string MSG12  = "Vui lòng nhập lại số điện thoại theo đầu số mới của nhà mạng.";
        public const string MSG22  = "Bạn có muốn xóa đơn vay hiện tại không?";
        public const string MSG23  = "Bạn có muốn hủy yêu cầu vay hiện tại không?";
        public const string MSG24  = "Vui lòng nhập họ và tên của bạn để tiếp tục.";
        public const string MSG25  = "Họ và tên không hợp lệ. Vui lòng thử lại!";
        public const string MSG26  = "Vui lòng nhập số chứng minh nhân dân của bạn để tiếp tục.";
        public const string MSG27  = "Số chứng minh nhân dân không hợp lệ. Vui lòng thử lại!";
        public const string MSG28  = "Vui lòng nhập số điện thoại người thân để tiếp tục.";
        public const string MSG29  = "Số điện thoại người thân không hợp lệ. Vui lòng thử lại!";
        public const string MSG30  = "Vui lòng nhập tên cơ quan để tiếp tục.";
        public const string MSG31  = "Vui lòng nhập mức thu nhập hàng tháng để tiếp tục.";
        public const string MSG32  = "Vui lòng tải lên ảnh bạn đang cầm CMND để tiếp tục.";
        public const string MSG33  = "Vui lòng tải lên ảnh mặt trước CMND của bạn để tiếp tục.";
        public const string MSG34  = "Vui lòng tải lên ảnh mặt sau CMND của bạn để tiếp tục.";
        public const string MSG35  = "Vui lòng tải lên ảnh bảng lương của bạn để tiếp tục.";
        public const string MSG36  = "Thông tin bổ sung không hợp lệ. Vui lòng thử lại!";
        public const string MSG37  = "Vui lòng nhập địa chỉ thường trú của bạn để tiếp tục.";
        public const string MSG38  = "Vui lòng nhập địa chỉ tạm trú của bạn để tiếp tục.";
        public const string MSG39  = "Vui lòng nhập địa chỉ công ty của bạn để tiếp tục.";
        public const string MSG40  = "Vui lòng nhập họ và tên chủ tài khoản ví để tiếp tục.";
        public const string MSG41  = "Họ và tên chủ tài khoản ví không hợp lệ. Vui lòng thử lại!";
        public const string MSG42  = "Vui lòng nhập số tài khoản ví của bạn.";
        public const string MSG43  = "Số tài khoản ví không hợp lệ. Vui lòng thử lại!";
        public const string MSG44  = "Vui lòng nhập số tài khoản ví xác nhận để tiếp tục.";
        public const string MSG45  = "Số tài khoản ví xác nhận không hợp lệ. Vui lòng thử lại!";
        public const string MSG46  = "Số tài khoản ví và số tài khoản ví xác nhận không giống nhau. Vui lòng thử lại!";
        public const string MSG47  = "Sai mã OTP. Quý khách vui lòng nhập lại mã OTP. ";
        public const string MSG48  = "Quý khách đã nhập sai mã OTP 3 lần. Thông tin đã điền của quý khách sẽ bị xóa, vui lòng điền lại thông tin để tạo khoản vay.";
        public const string MSG48A = "Vui lòng nhập lại số điện thoại theo đầu số mới của nhà mạng.";
        public const string MSG49  = "Đã có lỗi trong quá trình xác nhận. Vui lòng thử lại!";
        public const string MSG50  = "Đơn vay của bạn đã sẵn sàng trên sàn giao dịch. Chúng tôi sẽ cập nhật thông tin khi khoản vay được huy động. Vui lòng giữ liên lạc để cập nhật trạng thái của khoản vay.";
        public const string MSG51  = @"Chúc mừng bạn đã ký hợp đồng thành công. Tiền sẽ được giải ngân về tài khoản của bạn khi hợp đồng được duyệt.
Xin cảm ơn!
";
        public const string MSG52  = "Đã có lỗi trong quá trình ký hợp đồng. Vui lòng thử lại!";
        public const string MSG53  = "Vui lòng đồng ý với điều khoản hợp đồng để tiếp tục ký hợp đồng.";
        public const string MSG1   = "Bạn đang sử dụng phiên bản cũ. Vui lòng cập nhật phiên bản mới nhất để sử dụng.";
        public const string MSG54  = "Password không chính xác.";
        public const string MSG55  = "Số điện thoại giới thiệu bị trùng số điện thoại đăng nhập.";
        public const string MSG56  = "Vui lòng nhập số điện thoại người giới thiệu";
    }

    public static class AdminPortalMessage
    {
        //public const string MSG1 = "All required fields are required";
        //public const string MSG2 = "User name cannot contain special characters: !~@#$%^&*-=`|\\(){}[]:;\"'<>,?/";
        public const string MSG01 = "Vui lòng nhập username và password";
        public const string MSG02 = "Username hoặc password không chính xác.";
        public const string MSG04 = "Tài khoản của bạn đã bị đóng bởi ban quản trị. Vui lòng liên hệ ban quản trị để được hỗ trợ";
        public const string MSG05 = "Mật khẩu mới và mật khẩu nhập lại không trùng. Vui lòng thực hiện lại.";
        public const string MSG06 = "Không tìm thấy kết quả thỏa mãn điều kiện tìm kiếm";
        public const string MSG07 = "Chưa chọn thông tin cần cập nhật";
        public const string MSG08 = "Đơn này đã được review, bạn không thể review lại đơn";
        public const string MSG09 = "Đơn này đã được thẩm định, bạn không thể thẩm định lại đơn";
        public const string MSG10 = "Bạn không thể giải ngân đơn vay này. Vui lòng kiểm tra lại.";
        public const string MSG11 = "Vui lòng kiểm tra các thông tin này";
        public const string MSG12 = "Vui lòng chọn ít nhất 1 lý do từ chối";
        public const string MSG13 = "Vui lòng nhập thông tin bổ sung khi chọn Lý do khác";
        public const string MSG14 = "Vui lòng chọn ít nhất 1 checkbox";
        public const string MSG15 = "Bạn không có quyền thực hiện hành động này";
        public const string MSG4105 = "Số tiền thanh toán không đủ để tất toán, vui lòng chọn hình thức thanh toán khác.";
        public const string MSG4106 = "Vui lòng nhập HUYGIAODICH để xác nhận hủy giao dịch thanh toán nợ";
        public const string MSG4107 = "Bạn không thể hủy giao dịch này.";
        public const string MSG4108 = "Không được phép chọn ngày thanh toán sớm hơn ngày thanh toán của giao dịch gần nhất. Vui lòng chọn lại ngày";
        public const string MSG4203 = "Bạn chỉ có thể tạo giao dịch thanh toán nợ trong vòng 5 ngày gần nhất.";

    }

    public static class SessionKey
    {
        public const string CurrentUser = "CURRENT_USER";
    }

    public static class FirebaseConfig
    {
        public const string ClickAction = "FLUTTER_NOTIFICATION_CLICK";
        public const string ChannelId = "p2p-borrower-high-channel";
    }

    public static class PaymentType
    {
        public const string Bank = "Bank";
        public const string VirtualWallet = "VirtualWallet";
    }

    public static class LoanSubmitType
    {
        /// <summary>
        /// Đơn vay được người vay gửi đi lần đầu
        /// Tên: Đơn mới
        /// </summary>
        public const string New = "New";
        /// <summary>
        /// Các đơn vay sau lần vay đầu
        /// </summary>
        public const string Reloan = "Reloan";
    }

    public static class LoanStatus
    {
        /// <summary>
        /// Đơn vay chưa được gửi đi
        /// </summary>
        public const string Draft = "Draft";
        /// <summary>
        /// Đơn vay người vay gửi lên
        /// </summary>
        public const string Submitted = "Submitted";
        /// <summary>
        /// Đơn vay người vay gửi lại sau khi cập nhật thông tin
        /// </summary>
        public const string Resubmit = "Resubmit";

        /// <summary>
        /// Đơn vay được submit lên sale leader
        /// </summary>
        public const string SaleSubmit = "SaleSubmit";
        /// <summary>
        /// Đơn vay Sale Leader trả lại Sale
        /// </summary>
        public const string SaleLeaderReturn = "SaleLeaderReturn";
        /// <summary>
        /// Sale submit lại đơn bị Sale Leader trả về
        /// </summary>
        public const string SaleResubmit = "SaleResubmit";

        /// <summary>
        /// Đơn vay Sale Leader submit lên thẩm định viên
        /// </summary>
        public const string SaleLeaderSubmit = "SaleLeaderSubmit";
        /// <summary>
        /// Đơn vay thẩm định viên trả lại Sale
        /// </summary>
        public const string AppraisalReturn = "AppraisalReturn";
        /// <summary>
        /// Đơn vay thẩm định viên duyệt và submit lên Risk Leader
        /// </summary>
        public const string AppraisalSubmit = "AppraisalSubmit";
        /// <summary>
        /// Đơn vay Risk Leader trả về người vay
        /// </summary>
        public const string RiskPending = "RiskPending";
        /// <summary>
        /// Đơn vay risk leader trả lại cho thẩm định viên
        /// </summary>
        public const string RiskLeaderReturn = "RiskLeaderReturn";
        /// <summary>
        /// Đơn vay chờ xác nhận lãi suất
        /// </summary>
        public const string InterestRateConfirm = "InterestRateConfirm";
        /// <summary>
        /// Đơn vay trên sàn, đang huy động
        /// </summary>
        public const string OnMarket = "OnMarket";
        /// <summary>
        /// Khoản vay đã huy động đủ, chờ giải ngân
        /// </summary>
        public const string FundRaised = "FundRaised";
        /// <summary>
        /// Khoản vay đã quá hạn huy động
        /// </summary>
        public const string FundOvertime = "FundOvertime";
        /// <summary>
        /// Khoản vay đang chờ giải ngân
        /// </summary>
        public const string DisburmentPending = "DisburmentPending";
        /// <summary>
        /// Khoản vay đã được ký hợp đồng và giải ngân, đang trong thời gian trả nợ
        /// </summary>  
        public const string OnLoan = "OnLoan";
        /// <summary>
        /// Đơn vay được người vay cập nhật khi chưa xét duyêt
        /// </summary>
        public const string Updated = "Updated";
        /// <summary>
        /// Đơn vay yêu cầu bổ sung thông tin
        /// </summary>
        public const string UpdateRequired = "UpdateRequired";
        /// <summary>
        /// Đơn vay yêu cầu bổ sung thông tin
        /// </summary>
        public const string MoreInfoRequired = "MoreInfoRequired";
        /// <summary>
        /// Đơn vay bị từ chối
        /// </summary>
        public const string Reject = "Reject";
        /// <summary>
        /// Đơn vay bị hủy
        /// </summary>
        public const string Cancel = "Cancel";
    }

    public static class TableName
    {
        public const string AccountOperation = "account_operation";
        public const string AccountMobile = "account_mobile";
        public const string PhoneVerifications = "phone_verifications";
        public const string SMSOTPS = "sms_otps";
        public const string BorrowerProfiles = "borrower_profiles";
        public const string LoanApplication = "loan_applications";
        public const string LoanApplicationAdditionalInfos = "loan_application_additional_infos";
        public const string BorrowerRelativePersons = "borrower_relative_persons";
        public const string RelativePersonTypes = "relative_person_types";
        public const string MaritalStatuses = "marital_statuses";
        public const string Occupations = "occupations";
        public const string LoanProducts = "loan_products";
        public const string LoanProductAdditionalInfos = "loan_product_additional_info";
        public const string AddressLevel1 = "address_level_1";
        public const string AddressLevel2 = "address_level_2";
        public const string AddressLevel3 = "address_level_3";
        public const string Addresses = "addresses";
        public const string MobileLinkedPayments = "mobile_linked_payments";
        public const string LoanApplicationValidateAttributes = "loan_application_validate_attributes";
        public const string Departments = "departments";
        public const string Positions = "positions";
        public const string CreditRankConfigs = "credit_rank_configs";
        public const string DPDCollectFeeConfigs = "dpd_collect_fee_configs";
        public const string FeesParameterConfigs = "fees_parameter_configs";
        public const string LoanManagementParameterConfigs = "loan_management_parameter_configs";
        public const string LoanPeriodManagementFeeConfigs = "loan_period_management_fee_configs";
        public const string PaymentServices = "payment_services";
        public const string LoanValidateAttributeConfigs = "loan_validate_attribute_configs";
    }
}
