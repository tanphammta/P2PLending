using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class MobileLinkedPaymentDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceAccountName { get; set; }
        public string ServiceAccountId { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string IconPath { get; set; }
        public long IconLastModifiedDate { get; set; }
    }
}
