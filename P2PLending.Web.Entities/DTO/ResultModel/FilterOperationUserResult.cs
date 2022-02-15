using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.ResultModel
{
    public class FilterOperationUserResult: Result<List<OperationAccountInfo>>
    {

    }

    public class OperationAccountInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public bool IsActive { get; set; }
    }
}
