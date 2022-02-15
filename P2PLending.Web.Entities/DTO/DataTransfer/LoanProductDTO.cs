using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Entities.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class LoanProductDTO
    {
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumAmount { get; set; }
        public int MaximumAmount { get; set; }
        public string AmountUnit { get; set; }
        public int MinimumDuration { get; set; }
        public int MaximumDuration { get; set; }
        public string DurationUnit { get; set; }
        public string Icon { get; set; }
        public string IconPath { get; set; }
        public long IconLastModifiedDate { get; set; }
        public List<LoanTypeAdditionalInfoDTO> AdditionalInfos { get; set; }
    }

    public class LoanTypeAdditionalInfoDTO
    {
        public int Id { get; set; }
        public int LoanTypeId { get; set; }
        public string InfoName { get; set; }
        public string InfoFormat { get; set; }
    }
}
