using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class LoanApplicationDTO
    {
        public int Id { get; set; }
        public string LoanProduct { get; set; }
        public int LoanDuration { get; set; }
        public string DurationUnit { get; set; }
        public string Status { get; set; }
        public int LoanAmount { get; set; }
        public int RaisedAmount { get; set; }
        public int DebtAmount { get; set; }
        public string StatusDisplay { get; set; }
        public float InterestRate { get; set; }
        public int ServiceFees { get; set; }
        public string Phone { get; set; }
        public long CreateDate { get; set; }
        public string Icon { get; set; }
        public string IconPath { get; set; }
        public long IconLastModifiedDate { get; set; }
    }
}