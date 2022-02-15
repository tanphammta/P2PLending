using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Loans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Interface
{
    public interface ILoanApplicationService
    {
        List<LoanProductDTO> GetLoanProducts();
        Task<LoanApplicationMasterDataDTO> GetMasterDatas(MobileUserDTO account);
        LoanApplicationResult CreateLoanApplication(LoanApplicationRequest request, MobileUserDTO user);
        LoanApplicationResult UpdateLoanApplication(LoanApplicationRequest request, MobileUserDTO user);
        List<LoanApplicationDTO> GetLoanApplicationByBorrower(int borrowerId);
        Task<LoanApplicationDetail> GetLoanApplicationDetailById(int id);
        BaseResult UpdateLoanApplicationStatus(int id, string status);
        BaseResult UpdateLoanValidateAttributes(UpdateLoanValidateRequest request);
        List<LoanApplicationValidateAttributeDTO> GetLoanApplicationValidateAttributeDTOs(int id);
    }
}
