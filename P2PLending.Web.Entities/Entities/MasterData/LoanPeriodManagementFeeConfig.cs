using P2PLending.Web.Entities.Entities.Base;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class LoanPeriodManagementFeeConfig : BaseEntity
    {
        public int period { get; set; }
        public string name { get; set; }
        public string paid_target { get; set; }
        public string beneficial_target { get; set; }
        public string description { get; set; }
        public float value { get; set; }
        public string unit { get; set; }
    }
}
