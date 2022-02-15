using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Helper.Extension;
using P2PLending.Web.Helper.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace P2PLending.Web.API.Controllers.BorrowerApp
{
    [Authorize]
    [Route("api/borrower/[controller]")]
    [ApiController]
    public class LoanController: BorrowerBaseController
    {
        private ILoanApplicationService _loanApplicationService;
        private IWebHostEnvironment _webHostEnvironment;
        public LoanController(
            IMobileAccountService mobileAccountService,
            ILoanApplicationService loanApplicationService,
            IWebHostEnvironment webHostEnvironment) : base(mobileAccountService)
        {
            _loanApplicationService = loanApplicationService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Get Loan Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("loan-products")]
        public IActionResult GetLoanProducts()
        {
            string baseUrl = HTTPContextHelper.GetBaseURL(HttpContext);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var datas = _loanApplicationService.GetLoanProducts();
            foreach(var data in datas)
            {
                data.IconPath = $"{baseUrl}/icons/{data.Icon}";
                string path = Path.GetFullPath($"Images/Icons/{data.Icon}", contentRootPath);
                if (System.IO.File.Exists(path))
                {
                    var lastWriteLite = System.IO.File.GetLastWriteTime(path);
                    data.IconLastModifiedDate = lastWriteLite.ToUnixSeconds();
                }
            }

            return Ok(datas);
        }

        /// <summary>
        /// Get master datas
        /// </summary>
        /// <returns></returns>
        [HttpGet("master-data")]
        public async Task<IActionResult> GetMasterData()
        {
            if(CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }

            var datas = await _loanApplicationService.GetMasterDatas(CurrentUser);

            return Ok(datas);
        }

        [Authorize]
        [HttpPost("create-loan-application")]
        public IActionResult CreateAndSubmitLoanApplication([FromBody] LoanApplicationRequest request)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            var result = _loanApplicationService.CreateLoanApplication(request, CurrentUser);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("update-loan-application")]
        public IActionResult UpdateAndSubmitLoanApplication([FromBody] LoanApplicationRequest request)
        {
            if (CurrentUser == null)
            {
                throw new Exception("Cannot get user session");
            }
            var result = _loanApplicationService.UpdateLoanApplication(request, CurrentUser);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-loan-applications-by-borrower")]
        public IActionResult GetLoanApplicationByBorrower(int id)
        {
            string baseUrl = HTTPContextHelper.GetBaseURL(HttpContext);
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            
            var result = _loanApplicationService.GetLoanApplicationByBorrower(id);
            foreach (var data in result)
            {
                data.IconPath = $"{baseUrl}/icons/{data.Icon}";
                string path = Path.GetFullPath($"Images/Icons/{data.Icon}", contentRootPath);
                if (System.IO.File.Exists(path))
                {
                    var lastWriteLite = System.IO.File.GetLastWriteTime(path);
                    data.IconLastModifiedDate = lastWriteLite.ToUnixSeconds();
                }
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-loan-application-detail")]
        public async Task<IActionResult> GetLoanApplicationDetailById(int id)
        {
           
            var result = await _loanApplicationService.GetLoanApplicationDetailById(id);

            return Ok(result);
        }
    }
}
