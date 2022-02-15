using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class LoanApplicationMasterDataDTO
    {
        public AddressList Addresses { get; set; }
        public List<MaritalStatus> MaritalStatuses { get; set; }
        public List<RelativePersonType> RelativePersonTypes { get; set; }
        public List<Occupation> Occupations { get; set; }
        public CreditRankConfigDTO UserCreditRank { get; set; }
        public List<CreditRankConfigDTO> CreditRanks { get; set; }
    }
}
