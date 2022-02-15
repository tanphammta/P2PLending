using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class AccountRegistrationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
