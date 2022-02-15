using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{

    public class LoanApplicationValidateAttributeDTO
    {
        public int Id { get; set; }
        public int LoanApplicationId { get; set; }
        public string AttributeName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int ReferenceId { get; set; }
        public bool IsValidate { get; set; }
        public string Reason { get; set; }
    }
}
