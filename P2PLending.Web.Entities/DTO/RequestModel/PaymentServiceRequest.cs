using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class PaymentServiceRequest
    {
        public int? Id { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string AccountNumber { get; set; }
        public string OwnerName { get; set; }
    }
}
