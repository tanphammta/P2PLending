using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.OTP;
using P2PLending.Web.Helper.Constants;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Helper.Helpers;
using P2PLending.Web.Repositories.Repositories.Implement;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Business.Implement
{
    public class OTPService : IOTPService
    {
        public IPhoneVerificationRepository _phoneVerificationRepository;
        public ISMSOTPRepository _smsOTPRepository;
        public IUnitOfWork _unitOfWork;
        public OTPService(
            IPhoneVerificationRepository phoneVerificationRepository, 
            ISMSOTPRepository smsOTPRepository,
            IUnitOfWork unitOfWork)
        {
            _phoneVerificationRepository = phoneVerificationRepository;
            _smsOTPRepository = smsOTPRepository;
            _unitOfWork = unitOfWork;
        }

        public OTPResult SendOTP(PhoneVerificationRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var result = new OTPResult();
                var phoneVerification = _phoneVerificationRepository.Queryable().FirstOrDefault(ver => ver.phone == request.Phone && ver.verify_device_id == request.DeviceId);
                //if phone is not verified before, create new record
                if (phoneVerification == null)
                {
                    phoneVerification = new PhoneVerification()
                    {
                        phone = request.Phone,
                        verification_type = request.VerificationType,
                        verify_device_id = request.DeviceId,
                        verify_device_name = request.DeviceName,
                        create_date = DateTime.Now,
                        is_lock = false,
                        lock_expired_date = null,
                        verification_expired = DateTime.UtcNow.AddDays(1),
                        retry_times = 0,
                    };

                    phoneVerification = _phoneVerificationRepository.Add(phoneVerification);
                    _unitOfWork.SaveChanges();

                    var otp = OTPHelper.GenerateOTP();
                    var smsOTP = new SMSOTP()
                    {
                        phone_verification_id = phoneVerification.id,
                        value = otp,
                        status = OTPStatus.Created,
                        create_date = DateTime.Now,
                        phone = request.Phone,
                        expired_date = DateTime.Now.AddSeconds(120),
                    };

                    smsOTP = _smsOTPRepository.Add(smsOTP);
                    _unitOfWork.SaveChanges();
                    phoneVerification.update_date = DateTime.Now;
                    phoneVerification.current_sms_otp_id = smsOTP.id;
                    _phoneVerificationRepository.Update(phoneVerification);
                    _unitOfWork.SaveChanges();
                    // result
                    result = new OTPResult()
                    {
                        Phone = phoneVerification.phone,
                        DeviceId = phoneVerification.verify_device_id,
                        OTP = smsOTP.value,
                        OTPExpiredDate = smsOTP.expired_date.ToUnixSeconds(),
                        IsSuccess = true,
                        RetryTimes = phoneVerification.retry_times,
                        IsLock = phoneVerification.is_lock,
                        LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds()
                    };
                } 
                //if exists phone verification
                else
                {
                    // if this verifiation session closed, reset data for new session
                    if(DateTime.UtcNow > phoneVerification.verification_expired)
                    {
                        phoneVerification = new PhoneVerification()
                        {
                            phone = request.Phone,
                            verification_type = request.VerificationType,
                            verify_device_id = request.DeviceId,
                            verify_device_name = request.DeviceName,
                            create_date = DateTime.Now,
                            is_lock = false,
                            lock_expired_date = null,
                            verification_expired = DateTime.UtcNow.AddDays(1),
                            retry_times = 0,
                        };
                        phoneVerification = _phoneVerificationRepository.Update(phoneVerification);
                        _unitOfWork.SaveChanges();
                    }
                    // if phone verification is lock, return lock
                    if (phoneVerification.is_lock)
                    {
                        if (DateTime.UtcNow > phoneVerification.lock_expired_date)
                        {
                            phoneVerification.is_lock = false;
                            phoneVerification.lock_expired_date = null;
                        }
                        else
                        {
                            result = new OTPResult()
                            {
                                Phone = phoneVerification.phone,
                                DeviceId = phoneVerification.verify_device_id,
                                OTP = null,
                                IsSuccess = false,
                                Message = BorrowerAppMessage.MSG06,
                                MessageCode = nameof(BorrowerAppMessage.MSG06),
                                RetryTimes = phoneVerification.retry_times,
                                IsLock = phoneVerification.is_lock,
                                LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds(),
                            };
                            return result;
                        }
                    }

                    if (phoneVerification.retry_times <= 2)
                    {

                        // if retry <= 2 times, create and send new OTP
                        var otp = OTPHelper.GenerateOTP();
                        var smsOTP = new SMSOTP()
                        {
                            phone_verification_id = phoneVerification.id,
                            value = otp,
                            status = OTPStatus.Created,
                            create_date = DateTime.Now,
                            phone = phoneVerification.phone,
                            expired_date = DateTime.UtcNow.AddSeconds(120)
                        };
                        smsOTP = _smsOTPRepository.Add(smsOTP);
                        _unitOfWork.SaveChanges();
                        phoneVerification.current_sms_otp_id = smsOTP.id;
                        phoneVerification.update_date = DateTime.Now;
                        _phoneVerificationRepository.Update(phoneVerification);
                        _unitOfWork.SaveChanges();
                        result = new OTPResult()
                        {
                            Phone = phoneVerification.phone,
                            DeviceId = phoneVerification.verify_device_id,
                            OTP = smsOTP.value,
                            OTPExpiredDate = ((DateTimeOffset)smsOTP.expired_date).ToUnixTimeSeconds(),
                            IsSuccess = true,
                            RetryTimes = phoneVerification.retry_times,
                            IsLock = phoneVerification.is_lock,
                            LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds()
                        };
                    }
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return result;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public OTPResult VerifyOTP(VerifyOTPRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var phoneVerification = _phoneVerificationRepository.Queryable().FirstOrDefault(ver => ver.phone == request.Phone && ver.verify_device_id == request.DeviceId);
                if (phoneVerification == null)
                {
                    return new OTPResult()
                    {
                        IsSuccess = false
                    };
                }
                if (DateTime.UtcNow > phoneVerification.verification_expired
                    || phoneVerification.is_lock)
                {
                    return new OTPResult()
                    {
                        Phone = phoneVerification.phone,
                        DeviceId = phoneVerification.verify_device_id,
                        IsSuccess = false,
                        RetryTimes = phoneVerification.retry_times,
                        IsLock = phoneVerification.is_lock,
                        LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds()
                    };
                }
                var smsOTP = _smsOTPRepository.Get(phoneVerification.current_sms_otp_id);

                if (smsOTP != null && DateTime.UtcNow <= smsOTP.expired_date && request.Value == smsOTP.value)
                {
                    phoneVerification.verified = true;
                    phoneVerification.verified_code = Guid.NewGuid().ToString();
                    phoneVerification.update_date = DateTime.Now;
                    phoneVerification.verified_date = DateTime.Now;
                    phoneVerification.verification_expired = DateTime.Now.AddHours(2);

                    _phoneVerificationRepository.Update(phoneVerification);

                    smsOTP.status = OTPStatus.Verified;
                    smsOTP.update_date = DateTime.Now;
                    _smsOTPRepository.Update(smsOTP);

                    _unitOfWork.SaveChanges();
                    _unitOfWork.Commit();
                    return new OTPResult()
                    {
                        Phone = phoneVerification.phone,
                        DeviceId = phoneVerification.verify_device_id,
                        IsSuccess = true,
                        RetryTimes = phoneVerification.retry_times,
                        IsLock = phoneVerification.is_lock,
                        LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds(),
                        VerifiedCode = phoneVerification.verified_code,
                        VerificationExpired = phoneVerification.verification_expired.ToUnixSeconds(),
                    };
                }
                else
                {
                    //if user input wrong otp, increase retry count, lock verification if count >= 3
                    string message = String.Empty;
                    string messageCode = String.Empty;

                    phoneVerification.retry_times++;
                    if (phoneVerification.retry_times >= 3)
                    {
                        if (request.VerificationType == VerificationType.SignIn)
                        {
                            message = BorrowerAppMessage.MSG06;
                            messageCode = nameof(BorrowerAppMessage.MSG06);
                        }
                        else if (request.VerificationType == VerificationType.Registration)
                        {
                            message = BorrowerAppMessage.MSG08;
                            messageCode = nameof(BorrowerAppMessage.MSG08);
                        }
                        phoneVerification.is_lock = true;
                        phoneVerification.lock_expired_date = DateTime.UtcNow.AddMinutes(15);
                    }
                    else
                    {
                        if (request.VerificationType == VerificationType.SignIn)
                        {
                            message = BorrowerAppMessage.MSG05;
                            messageCode = nameof(BorrowerAppMessage.MSG05);
                        }
                        else if (request.VerificationType == VerificationType.Registration)
                        {
                            message = BorrowerAppMessage.MSG07;
                            messageCode = nameof(BorrowerAppMessage.MSG07);
                        }
                    }

                    _unitOfWork.SaveChanges();
                    _unitOfWork.Commit();
                    return new OTPResult()
                    {
                        Phone = phoneVerification.phone,
                        DeviceId = phoneVerification.verify_device_id,
                        RetryTimes = phoneVerification.retry_times,
                        IsSuccess = false,
                        Message = message,
                        MessageCode = messageCode,
                        IsLock = phoneVerification.is_lock,
                        LockExpired = phoneVerification.lock_expired_date.ToUnixSeconds()
                    };
                }
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
