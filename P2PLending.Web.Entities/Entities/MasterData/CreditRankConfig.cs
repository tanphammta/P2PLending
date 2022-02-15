using P2PLending.Web.Entities.Entities.Base;

namespace P2PLending.Web.Entities.Entities.MasterData
{
    public class CreditRankConfig : BaseEntity
    {
        public string rank { get; set; }
        public int? min_score { get; set; }
        public int? max_score { get; set; }
        public float? interest_rate { get; set; }
        public string unit { get; set; }
    }
}
