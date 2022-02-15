using P2PLending.Web.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.Entities.OperatorDepartment
{
    public class Position: BaseEntity
    {
        /// <summary>
        /// <para>1 = manager</para>
        /// <para>2 = team leader</para>
        /// <para>3 = staff</para>
        /// </summary>
        public int rank { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
