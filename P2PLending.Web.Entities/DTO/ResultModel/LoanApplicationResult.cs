using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class LoanApplicationResult: BaseResult
    {
        public LoanApplicationDTO Data { get; set; }
    }
}
