using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class LoanValidateAttributeConfig: BaseEntity
    {
        public string attribute_name { get; set; }
        public string display_name { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
    }
}
