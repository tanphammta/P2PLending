using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class MaritalStatus: BaseEntity
    {
        public string status { get; set; }
    }
}
