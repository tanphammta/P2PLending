using P2PLending.Web.Entities.Entities.Base;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class LoanManagementParameterConfig: BaseEntity
    {
        public string name { get; set; }
        public float value { get; set; }
        /// <summary>
        /// 'day' or 'month'
        /// </summary>
        public string unit { get; set; }
        public string description { get; set; }
    }
}
