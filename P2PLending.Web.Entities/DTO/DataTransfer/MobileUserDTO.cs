using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class MobileUserDTO
    {
        public int AccountId { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int CreditPoints { get; set; }
    }
}
