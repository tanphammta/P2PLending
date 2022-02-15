using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Business.Interface
{
    public interface ILinkedPaymentService
    {
        List<PaymentServiceDTO> GetPaymentServices();
        List<MobileLinkedPaymentDTO> GetLinkedPaymentService(int accountId);
        MobileLinkedPaymentDTO AddUpdateLinkedPayment(int accountId, AddUpdateLinkedPaymentRequest request);
        BaseResult RemoveLinkedPayment(int linkedPaymentId);
    }
}