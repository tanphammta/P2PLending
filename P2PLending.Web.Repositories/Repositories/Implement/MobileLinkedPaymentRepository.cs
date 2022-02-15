using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.AccountLinkedPayment;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class MobileLinkedPaymentRepository: Repository<MobileLinkedPayment>, IMobileLinkedPaymentRepository
    {
        public MobileLinkedPaymentRepository(P2PLendingDbContext context) : base(context)
        {

        }
    }
}
