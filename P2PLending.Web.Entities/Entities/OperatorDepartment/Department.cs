using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Entities.OperatorDepartment
{
    public class Department: BaseEntity
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
