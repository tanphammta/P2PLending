using AutoMapper;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Entities.Entities.AccountLinkedPayment;
using P2PLending.Web.Repositories.Repositories.Interface;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Business.Implement
{
    public class LinkedPaymentService: ILinkedPaymentService
    {
        private IMasterDataRepository _masterDataRepository;
        private IMapper _mapper;
        private IMobileLinkedPaymentRepository _mobileLinkedPaymentRepository;
        private IUnitOfWork _unitOfWork;
        public LinkedPaymentService(
            IMasterDataRepository masterDataRepository,
            IMapper mapper,
            IMobileLinkedPaymentRepository mobileLinkedPaymentRepository,
            IUnitOfWork unitOfWork)
        {
            _masterDataRepository = masterDataRepository;
            _mapper = mapper;
            _mobileLinkedPaymentRepository = mobileLinkedPaymentRepository;
            _unitOfWork = unitOfWork;
        }

        public List<PaymentServiceDTO> GetPaymentServices()
        {
            var paymentServices = _masterDataRepository.ListPaymentServices();

            var paymentServiceDto = _mapper.Map<List<PaymentServiceDTO>>(paymentServices);

            return paymentServiceDto;
        }

        public List<MobileLinkedPaymentDTO> GetLinkedPaymentService(int accountId)
        {
            var linkedPayments = _mobileLinkedPaymentRepository.Queryable().Where(p => p.account_id == accountId).ToList();

            var serviceIds = linkedPayments.Select(p => p.service_id).Distinct().ToList();

            var services = _masterDataRepository.ListPaymentServices(serviceIds);

            var linkedPaymentDto = new List<MobileLinkedPaymentDTO>();
            foreach(var payment in linkedPayments)
            {
                var paymentDto = _mapper.Map<MobileLinkedPaymentDTO>(payment);
                var service = services.FirstOrDefault(s => s.id == paymentDto.ServiceId);
                if(service != null)
                {
                    paymentDto.ServiceName = service.name;
                    paymentDto.Icon = service.icon;
                    paymentDto.Type = service.type;
                }

                linkedPaymentDto.Add(paymentDto);
            }

            return linkedPaymentDto;
        }

        public MobileLinkedPaymentDTO AddUpdateLinkedPayment(int accountId, AddUpdateLinkedPaymentRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var linkedPayment = new MobileLinkedPayment();
                if (request.LinkedPaymentId == null)
                {
                    //add
                    linkedPayment = new MobileLinkedPayment()
                    {
                        account_id = accountId,
                        create_date = DateTime.Now,
                        service_account_id = request.ServiceAccountId,
                        service_account_name = request.ServiceAccountName,
                        service_id = request.ServiceId,
                        phone = request.Phone,
                    };

                    _mobileLinkedPaymentRepository.Add(linkedPayment);
                }
                else
                {
                    //update
                    linkedPayment = _mobileLinkedPaymentRepository.Get(request.LinkedPaymentId.Value);
                    if (linkedPayment != null)
                    {
                        linkedPayment.update_date = DateTime.Now;
                        linkedPayment.service_account_id = request.ServiceAccountId;
                        linkedPayment.service_account_name = request.ServiceAccountName;
                        linkedPayment.service_id = request.ServiceId;
                        linkedPayment.phone = request.Phone;

                        _mobileLinkedPaymentRepository.Update(linkedPayment);
                    }
                    else
                    {
                        throw new NullReferenceException($"Linked Payment id {request.LinkedPaymentId} does not exist");
                    }
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();

                var result = _mapper.Map<MobileLinkedPaymentDTO>(linkedPayment);
                var service = _masterDataRepository.GetPaymentService(linkedPayment.service_id);
                if (service != null)
                {
                    result.ServiceName = service.name;
                    result.Icon = service.icon;
                    result.Type = service.type;
                }
                return result;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public BaseResult RemoveLinkedPayment(int linkedPaymentId)
        {
            var linkedPayment = _mobileLinkedPaymentRepository.Get(linkedPaymentId);
            if (linkedPayment != null)
            {
                _mobileLinkedPaymentRepository.Delete(linkedPayment);
                _unitOfWork.SaveChanges();
                return new BaseResult()
                {
                    IsSuccess = true
                };
            }
            return new BaseResult()
            {
                IsSuccess = false
            };
        }
    }
}
