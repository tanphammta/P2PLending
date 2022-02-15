using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class AddUpdateLinkedPaymentRequest
    {
        public int? LinkedPaymentId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceAccountId { get; set; }
        public string ServiceAccountName { get; set; }
        public string Phone { get; set; }
    }
}
