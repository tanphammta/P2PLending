using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class LoanApplicationValidateAttributeRepository: Repository<LoanApplicationValidateAttribute>, ILoanApplicationValidateAttributeRepository
    {
        public LoanApplicationValidateAttributeRepository(P2PLendingDbContext context) : base(context)
        {

        }

        public List<LoanApplicationValidateAttribute> ListInvalidateAttributeByLoanApplication(int loanApplicationId)
        {
            var result = DbSet.Where(p => p.loan_application_id == loanApplicationId && !p.is_validate).ToList();
            return result;
        }

        public List<LoanApplicationValidateAttribute> ListAllAttributeByLoanApplication(int loanApplicationId)
        {
            var result = DbSet.Where(p => p.loan_application_id == loanApplicationId).ToList();
            return result;
        }
    }
}
