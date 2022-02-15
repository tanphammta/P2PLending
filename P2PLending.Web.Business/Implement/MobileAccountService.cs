using Microsoft.Extensions.Options;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.DTO.Setting;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.Facebook;
using P2PLending.Web.Entities.Token;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Helper.Helpers;
using P2PLending.Web.Helper.JwtHandlers;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Implement
{
    public class MobileAccountService: IMobileAccountService
    {
        private IMobileAccountRepository _mobileAccountRepository;
        private IPhoneVerificationRepository _phoneVerificationRepository;
        private IFirebaseRepository _firebaseRepository;
        private IUnitOfWork _unitOfWork;
        private IJwtHandler _jwtHandler;
        private SupportSetting _supportSetting;
       
        public MobileAccountService(
            IMobileAccountRepository mobileAccountRepository,
            IPhoneVerificationRepository phoneVerificationRepository,
            IFirebaseRepository firebaseRepository,
            IAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            IJwtHandler jwtHandler,
            IOptions<SupportSetting> supportOption
            )
        {
            _firebaseRepository = firebaseRepository;
            _mobileAccountRepository = mobileAccountRepository;
            _phoneVerificationRepository = phoneVerificationRepository;
            _unitOfWork = unitOfWork;
            _jwtHandler = jwtHandler;
            _supportSetting = supportOption.Value;
        }

        public PhoneNumberCheckResult CheckPhoneNumber(string phone, string deviceId)
        {
            var checkPhoneNumberFormatResult = PhoneHelper.IsCorrectPhoneNumberFormat(phone);
            if (!checkPhoneNumberFormatResult.IsSuccess)
            {
                return new PhoneNumberCheckResult()
                {
                    IsCorrectPhoneNumberFormat = checkPhoneNumberFormatResult.IsSuccess,
                    IsBorrowerExist = false,
                    IsInvestorExist = false,
                    IsOTPLock = false,
                    LockExpired = null,
                    IsSuccess = false,
                    Message = checkPhoneNumberFormatResult.Message,
                    MessageCode = checkPhoneNumberFormatResult.MessageCode
                };
            }
            var isBorrowerExist = _mobileAccountRepository.Queryable().Any(account => account.phone == phone && account.role == UserRole.Borrower);
            var isInvestorExist = _mobileAccountRepository.Queryable().Any(account => account.phone == phone && account.role == UserRole.Investor);
            bool isLock = false;
            long? lockExpired = 0;
            var phoneVerification = _phoneVerificationRepository.Queryable().FirstOrDefault(v => v.phone == phone && v.verify_device_id == deviceId);
            if(phoneVerification != null)
            {
                isLock = phoneVerification.is_lock;
                lockExpired = phoneVerification.lock_expired_date.ToUnixSeconds();
            }

            return new PhoneNumberCheckResult()
            {
                IsBorrowerExist = isBorrowerExist,
                IsInvestorExist = isInvestorExist,
                IsCorrectPhoneNumberFormat = checkPhoneNumberFormatResult.IsSuccess,
                IsOTPLock = isLock,
                LockExpired = lockExpired,
                IsSuccess = true
            };
        }

        public BaseResult BorrowerAccountRegisting(MobileAccountRegistrationRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var verification = _phoneVerificationRepository.Queryable().FirstOrDefault(
                    ver => ver.phone == request.Phone 
                    && ver.verify_device_id == request.DeviceId);
                if (verification == null 
                    || DateTime.UtcNow > verification.verification_expired
                    || request.VerifiedCode != verification.verified_code)
                {
                    throw new Exception("Verification failed");
                }

                if(_mobileAccountRepository.Queryable().Any(acc => acc.phone == request.Phone && acc.role == UserRole.Borrower))
                {
                    return new BaseResult()
                    {
                        IsSuccess = false,
                        Message = BorrowerAppMessage.MSG04,
                        MessageCode = nameof(BorrowerAppMessage.MSG04)
                    };
                }

                var mobileAccount = new AccountMobile()
                {
                    phone = request.Phone,
                    role = UserRole.Borrower,
                    create_date = DateTime.Now,
                    password = request.Password,
                    is_active = true
                };
                var salt = AccountHelper.CreatePasswordSalt();

                mobileAccount.password_salt = Convert.ToBase64String(salt);
                mobileAccount.password = AccountHelper.HashPassword(mobileAccount.password, salt);
                mobileAccount.password_hash_algorithm = "HMACSHA1";
                mobileAccount.registration_time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                mobileAccount.update_date = DateTime.Now;

                _mobileAccountRepository.Add(mobileAccount);
                _unitOfWork.SaveChanges();

                verification.verification_expired = null;
                verification.verified_code = null;
                verification.retry_times = 0;
                verification.update_date = DateTime.Now;

                _phoneVerificationRepository.Update(verification);
                _unitOfWork.SaveChanges();

                _unitOfWork.Commit();
                return new BaseResult()
                {
                    IsSuccess = true
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<MobileAuthResult> SignIn(MobileLoginRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var result = new MobileAuthResult();
                
                // authenticate
                var account = _mobileAccountRepository.Queryable().FirstOrDefault(acc => acc.phone == request.Phone && acc.is_active && acc.role == UserRole.Borrower);
                if (account == null)
                {
                    throw new MessageException(BorrowerAppMessage.MSG02, nameof(BorrowerAppMessage.MSG02));
                }

                var saltBytes = Convert.FromBase64String(account.password_salt);
                string hashed = AccountHelper.HashPassword(request.Password, saltBytes);

                if (hashed != account.password)
                {
                    throw new MessageException(BorrowerAppMessage.MSG54, nameof(BorrowerAppMessage.MSG54));
                }

                result.PreviousDeviceId = account.current_device_id;
                result.PreviousFCMToken = account.fcm_token;
                result.CurrentDeviceId = request.DeviceId;

                var accessToken = _jwtHandler.CreateAccessToken(account);
                var refreshToken = _jwtHandler.CreateRefreshToken(account.id);

                var authorizationTokens = new AuthorizationTokensResource
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    FCMToken = request.FCMToken
                };

                account.current_device_id = request.DeviceId;
                account.current_device_name = request.DeviceName;
                account.fcm_token = request.FCMToken;
                account.update_date = DateTime.Now;
                result.CurrentFCMToken = request.FCMToken;
                _mobileAccountRepository.Update(account);
                _unitOfWork.SaveChanges();
                
                var userInfo = new MobileUserDTO() 
                { 
                    AccountId = account.id,
                    Phone = account.phone,
                    Role = account.role,
                    Name = account.name
                };

                if(!string.IsNullOrEmpty(result.PreviousDeviceId) && result.CurrentDeviceId != result.PreviousDeviceId)
                {
                    result.IsOtherDeviceLoggedIn = true;
                    var messageResult = _firebaseRepository.SendTopicMessage(
                        result.PreviousDeviceId,
                        $"Tài khoản {request.Phone} đăng nhập trên thiết bị khác",
                        @$"Tài khoản {request.Phone} đăng nhập trên thiết bị {request.DeviceName} vào lúc {DateTime.Now.ToString("hh:mm dd/MM/yyyy")}. 
Nếu không phải là bạn, vui lòng liên hệ với chúng tôi theo SĐT {_supportSetting.Phone} hoặc email {_supportSetting.Email} để được hỗ trợ",
                        new Dictionary<string, string>()
                        {
                            {"PreviousDeviceId",result.PreviousDeviceId},
                            {"LoginDeviceId",result.CurrentDeviceId},
                            {"LoginTime", DateTime.Now.ToUnixSeconds().ToString()}
                        }
                    );
                }

                result.User = userInfo;
                result.IsSuccess = true;
                result.TokensResource = authorizationTokens;

                _unitOfWork.Commit();
                return result;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public MobileUserDTO GetById(int id)
        {
            var account = _mobileAccountRepository.Get(id);
            if(account != null)
            {
                var userInfo = new MobileUserDTO()
                {
                    AccountId = account.id,
                    Phone = account.phone,
                    Role = account.role,
                    Name = account.name
                };

                return userInfo;
            }

            return null;
        }

        public BaseResult AddReferer(int userId, string refererPhone)
        {
            var account = _mobileAccountRepository.Get(userId);

            if(account == null)
            {
                return new BaseResult()
                {
                    IsSuccess = false,
                    Message = OtherMessage.OMSG01,
                    MessageCode = nameof(OtherMessage.OMSG01)
                };
            }
            if(account.phone == refererPhone)
            {
                return new BaseResult()
                {
                    IsSuccess = false,
                    Message = BorrowerAppMessage.MSG55,
                    MessageCode = nameof(BorrowerAppMessage.MSG55)
                };
            }
            var referralUser = _mobileAccountRepository.Queryable().FirstOrDefault(acc => acc.phone == refererPhone && acc.role == UserRole.Borrower);
            
            account.referrer_account_id = referralUser?.id;
            account.referrer_phone = refererPhone;
            account.update_date = DateTime.Now;

            _mobileAccountRepository.Update(account);
            _unitOfWork.SaveChanges();

            return new BaseResult()
            {
                IsSuccess = true
            };
        }

        public BaseResult ResetPassword(MobileResetPasswordRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var verification = _phoneVerificationRepository.Queryable().FirstOrDefault(
                    ver => ver.phone == request.Phone
                    && ver.verify_device_id == request.DeviceId);
                if (verification == null
                    || DateTime.UtcNow > verification.verification_expired
                    || request.VerifiedCode != verification.verified_code)
                {
                    throw new Exception("Verification failed");
                }
                var mobileAccount = _mobileAccountRepository.Queryable().FirstOrDefault(acc => acc.phone == request.Phone && acc.role == UserRole.Borrower);
                if (mobileAccount == null)
                {
                    return new BaseResult()
                    {
                        IsSuccess = false,
                        Message = OtherMessage.OMSG01,
                        MessageCode = nameof(OtherMessage.OMSG01)
                    };
                }

                var salt = AccountHelper.CreatePasswordSalt();

                mobileAccount.password_salt = Convert.ToBase64String(salt);
                mobileAccount.password = AccountHelper.HashPassword(request.NewPassword, salt);
                mobileAccount.password_hash_algorithm = "HMACSHA1";
                mobileAccount.update_date = DateTime.Now;

                _mobileAccountRepository.Update(mobileAccount);
                _unitOfWork.SaveChanges();

                verification.verification_expired = null;
                verification.verified_code = null;
                verification.retry_times = 0;
                verification.update_date = DateTime.Now;

                _phoneVerificationRepository.Update(verification);
                _unitOfWork.SaveChanges();

                _unitOfWork.Commit();
                return new BaseResult()
                {
                    IsSuccess = true
                };
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public BaseResult SignOut(int userId)
        {
            var account = _mobileAccountRepository.Get(userId);
            if(account == null)
            {
                return new BaseResult()
                {
                    IsSuccess = false,
                    Message = OtherMessage.OMSG01,
                    MessageCode = nameof(OtherMessage.OMSG01)
                };
            }

            account.current_device_id = null;
            account.current_device_name = null;
            account.fcm_token = null;
            account.update_date = DateTime.Now;

            _mobileAccountRepository.Update(account);
            _unitOfWork.SaveChanges();

            return new BaseResult()
            {
                IsSuccess = true
            };
        }

        public BaseResult UpdateFacebookUserData(FacebookUserResource facebookData, MobileUserDTO currentUser)
        {
            var mobileAccount = _mobileAccountRepository.Get(currentUser.AccountId);
            mobileAccount.name = facebookData.Name;
            mobileAccount.avatar = facebookData.Picture;
            mobileAccount.email = facebookData.Email;
            mobileAccount.facebook_id = facebookData.Id;
            _mobileAccountRepository.Update(mobileAccount);
            _unitOfWork.SaveChanges();
            return new BaseResult()
            {
                IsSuccess = true
            };
        }
    }
}
