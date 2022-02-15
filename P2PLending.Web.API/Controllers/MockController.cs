using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Helper.Constants;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace P2PLending.Web.API.Controllers
{
    [Authorize]
    [Route("api/mock")]
    public class MockController : ControllerBase
    {
        private ILoanApplicationService _loanApplicationService;
        public MockController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        [HttpGet("get-loan-statuses")]
        public IActionResult GetLoanStatuses()
        {
            var x = typeof(LoanStatus).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Select(p => p.Name);
            return Ok(x);
        }

        [HttpPost("update-loan-status")]
        public IActionResult UpdateLoanApplicationStatus([FromBody] UpdateLoanStatusRequest request)
        {
            var result = _loanApplicationService.UpdateLoanApplicationStatus(request.Id, request.Status);
            return Ok(result);
        }

        [HttpGet("get-loan-validate-attributes")]
        public IActionResult GetLoanAttributes(int id)
        {
            var result = _loanApplicationService.GetLoanApplicationValidateAttributeDTOs(id);
            return Ok(result);
        }

        [HttpPost("update-loan-validate-attributes")]
        public IActionResult UpdateLoanApplicationValidate([FromBody] UpdateLoanValidateRequest request)
        {
            var result = _loanApplicationService.UpdateLoanValidateAttributes(request);
            return Ok(result);
        }


    }
}
