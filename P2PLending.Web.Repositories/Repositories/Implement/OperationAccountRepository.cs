using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.DTO.ResultModel;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class OperationAccountRepository: Repository<AccountOperation>, IOperationAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public OperationAccountRepository(IUnitOfWork unitOfWork, P2PLendingDbContext context): base(context)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
