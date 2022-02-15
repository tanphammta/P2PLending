using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class PaymentService: BaseEntity
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
    }
}
