using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class UpdateLoanValidateRequest
    {
        public int Id { get; set; }
        public List<InvalidateAttribute> InvalidateAttributes { get; set; }
    }

    public class InvalidateAttribute
    {
        public string AttributeName { get; set; }
        public string Reason { get; set; }
    }
}
