using P2PLending.Web.Entities.Entities.Loans;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface ILoanApplicationAdditionalInfoRepository: IRepository<LoanApplicationAdditionalInfo>
    {
        List<LoanApplicationAdditionalInfo> ListAdditionalInfoByApplicationId(int loanApplicationId);
    }
}
