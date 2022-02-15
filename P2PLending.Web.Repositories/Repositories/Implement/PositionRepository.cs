using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.OperatorDepartment;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class PositionRepository: Repository<Position>, IPositionRepository
    {
        public PositionRepository(P2PLendingDbContext context): base(context)
        {

        }
    }
}
