using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class BorrowerProfileRepository: Repository<BorrowerProfile>, IBorrowerProfileRepository
    {
        public BorrowerProfileRepository(P2PLendingDbContext context) : base(context)
        {

        }
    }
}
