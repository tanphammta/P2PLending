using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class UpdateLoanStatusRequest
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}
