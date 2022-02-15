using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Helper.Helpers;
using P2PLending.Web.Helper.JwtHandlers;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace P2PLending.Web.Business.Implement
{
    public class OperationAccountService: IOperationAccountService
    {
        private IOperationAccountRepository _operationAccountRepository;
        private IUnitOfWork _unitOfWork;
        private IJwtHandler _jwtHandler;
        private IMapper _mapper;
        private IEmailRepository _emailRepository;
        public OperationAccountService(
            IOperationAccountRepository operationAccountRepository,
            IUnitOfWork unitOfWork,
            IJwtHandler jwtHandler,
            IMapper mapper,
            IEmailRepository emailRepository)
        {
            _operationAccountRepository = operationAccountRepository;
            _unitOfWork = unitOfWork;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _emailRepository = emailRepository;
        }
        public AccountOperation Authenticate(string username, string password)
        {
            try
            {
                var account = _operationAccountRepository.Queryable().FirstOrDefault(a => a.username == username && !a.is_wait_password_create);
                if (account == null)
                    throw new Exception("Record not found");

                var saltBytes = Convert.FromBase64String(account.password_salt);
                string hashed = AccountHelper.HashPassword(password, saltBytes);
                if (hashed == account.password)
                {
                    return account;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public AccountOperation GetByEmail(string email)
        {
            var account = _operationAccountRepository.Queryable().FirstOrDefault(acc => acc.email == email);

            return account;
        }

        public bool CreateResetPasswordRequest(string email, string baseURL)
        {
            try
            {
                var account = _operationAccountRepository.Queryable().FirstOrDefault(acc => acc.email == email);
               
                if (account != null)
                {
                    var key = CryptoHelper.GenerateCrypto();
                    var token = _jwtHandler.CreateResetPasswordToken(email, key);

                    var resetPasswordLink = @$"{baseURL}/api/admin/account/password/verify-reset?token={token.Token}";

                    account.password_reset_token = key;
                    account.password_reset_expire = DateTime.UtcNow.AddMinutes(15);
                    account.password_reset_token_consumed = false;
                    _operationAccountRepository.Update(account);
                    _unitOfWork.SaveChanges();
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public BaseResult VerifyPasswordReset(string email, string code, string newPassword)
        {
            var account = _operationAccountRepository.Queryable().FirstOrDefault(acc => acc.email == email);

            if(account == null)
            {
                throw new Exception("Account not existed");
            }

            if (account.password_reset_token_consumed ?? false)
            {
                throw new Exception("Token expired or not correct");
            }

            if(DateTime.UtcNow > account.password_reset_expire)
            {
                throw new Exception("Token expired or not correct");
            }

            if(account.password_reset_token != code)
            {
                throw new Exception("Token expired or not correct");
            }

            var saltBytes = Convert.FromBase64String(account.password_salt);
            string hashed = AccountHelper.HashPassword(newPassword, saltBytes);
            if (hashed == account.password)
            {
                //new password == account.password
                throw new Exception("New password need to different with current password");
            }

            var salt = AccountHelper.CreatePasswordSalt();

            account.password_salt = Convert.ToBase64String(salt);
            account.password = AccountHelper.HashPassword(newPassword, salt);
            account.password_hash_algorithm = "HMACSHA1";
            account.registration_time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            if (account.is_wait_password_create)
            {
                account.is_wait_password_create = false;
            }
            _operationAccountRepository.Update(account);
            _unitOfWork.SaveChanges();
            return new BaseResult()
            {
                IsSuccess = true,
                Message = "Token verified successfully"
            };
        }

        public BaseResult CreateAccountRequest(APCreateAccountRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var existedAccount = _operationAccountRepository.Queryable().FirstOrDefault(acc => acc.username == request.Username || acc.email == request.Email);
                if (existedAccount != null)
                {
                    throw new Exception("Username or Email registered!");
                }
                var account = _mapper.Map<AccountOperation>(request);
                var key = CryptoHelper.GenerateCrypto();
                var token = _jwtHandler.CreateResetPasswordToken(account.email, key);

                var resetPasswordLink = @$"{request.BaseURL}/api/admin/account/password/verify-create-account?token={token.Token}";

                _emailRepository.SendEmail(request.Email, "Confirm Account Create", resetPasswordLink);

                account.password_reset_token = key;
                account.password_reset_expire = DateTime.UtcNow.AddMinutes(60);
                account.password_reset_token_consumed = false;
                account.is_wait_password_create = true;

                _operationAccountRepository.Add(account);
                
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return new BaseResult()
                {
                    IsSuccess = true,
                    Message = "Create Account Successfully"
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
