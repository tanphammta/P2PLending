using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class BorrowerProfileAddressRepository: Repository<Address>, IBorrowerProfileAddressRepository
    {
        public BorrowerProfileAddressRepository(P2PLendingDbContext context) : base(context)
        {

        }
    }
}
