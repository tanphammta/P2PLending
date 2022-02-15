using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class CreditRankConfigDTO
    {
        public int Id { get; set; }
        public string Rank { get; set; }
        public int InterestRate { get; set; }
        public string Unit { get; set; }
    }
}
