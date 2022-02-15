using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using System.Threading.Tasks;

namespace P2PLending.Web.API.Controllers
{
    [Authorize]
    [Route("api/firebase")]
    public class FirebaseController: ControllerBase
    {
        private IFirebaseService _firebaseService;
        public FirebaseController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody]FirebaseMessageRequest request)
        {
            var result = await _firebaseService.SendMessage(request);

            return Ok(result);
        }
    }
}
