using P2PLending.Web.Entities.Entities.Base;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class DPDCollectFeeConfig : BaseEntity
    {
        public int? from_days { get; set; }
        public int? to_days { get; set; }
        public string paid_target { get; set; }
        public string beneficial_target { get; set; }
        public string description { get; set; }
        public float value { get; set; }
        public string unit { get; set; }
    }
}
