using P2PLending.Web.Entities.DTO.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class CreateLoanApplicationResult: BaseResult
    {
        public int LoanApplicationId { get; set; }
    }
}
