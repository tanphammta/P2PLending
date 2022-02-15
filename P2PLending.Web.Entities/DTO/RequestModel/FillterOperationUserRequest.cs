using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class FillterOperationUserRequest
    {
        public List<string> Statuses { get; set; }
        public List<string> Departments { get; set; }
        public string Name { get; set; }
    }
}
