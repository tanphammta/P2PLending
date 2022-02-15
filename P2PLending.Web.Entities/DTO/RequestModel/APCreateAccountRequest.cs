using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class APCreateAccountRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public string Avatar { get; set; }
        public int? ManagerId { get; set; }
        public int PositionId { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string BaseURL { get; set; }
    }
}
