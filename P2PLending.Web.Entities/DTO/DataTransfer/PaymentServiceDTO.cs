using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class PaymentServiceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string IconPath { get; set; }
        public long IconLastModifiedDate { get; set; }
    }
}
