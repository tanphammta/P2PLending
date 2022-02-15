using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class LoanTypeAdditionalInfoRepository: Repository<LoanProductAdditionalInfo>, ILoanTypeAdditionalInfoRepository
    {
        public LoanTypeAdditionalInfoRepository(P2PLendingDbContext context) : base(context)
        {

        }

        public List<LoanProductAdditionalInfo> ListAdditionalInfoByProductId(int productId)
        {
            var result = DbSet.Where(p => p.loan_product_id == productId).ToList();

            return result;
        }
    }
}
