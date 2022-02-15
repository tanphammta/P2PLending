using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class LoanApplicationRepository: Repository<LoanApplication>, ILoanApplicationRepository
    {
        public LoanApplicationRepository(P2PLendingDbContext context) : base(context)
        {

        }
    }
}
