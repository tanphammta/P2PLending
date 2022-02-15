using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.Relative;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class BorrowerRelativePersonRepository: Repository<RelativePerson>, IBorrowerRelativePersonRepository
    {
        public BorrowerRelativePersonRepository(P2PLendingDbContext context) : base(context)
        {

        }
    }
}
