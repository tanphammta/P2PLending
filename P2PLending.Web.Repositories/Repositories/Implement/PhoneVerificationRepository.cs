using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.OTP;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class PhoneVerificationRepository: Repository<PhoneVerification>, IPhoneVerificationRepository
    {
        public PhoneVerificationRepository(P2PLendingDbContext context): base(context)
        {

        }
    }
}
