using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.Borrower;
using P2PLending.Web.Entities.Entities.Loans;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class LoanApplicationRequest
    {
        public int Id { get; set; }
        public int LoanProductId { get; set; }
        public int LoanAmount { get; set; }
        public int LoanDuration { get; set; }
        public int LinkedPaymentId { get; set; }
        public float ExpectedInterestRate { get; set; }
        public int ServiceFees { get; set; }
        public LARRelativePerson RelativePerson1 { get; set; }
        public LARRelativePerson RelativePerson2 { get; set; }
        public int ExpectedTotalPayment { get; set; }   
        public LARBorrowerProfile Profile { get;set; }
        public LARAddress ResidentAddress { get; set; }
        public LARAddress CurrentAddress { get; set; }
        public LAROccupation Occupation { get; set; }
        public string OwnedType { get; set; }
        public int MaritalStatusId { get; set; }
        public string IdCardWithUserImage { get; set; }
        public string IdCardFrontImage { get; set; }
        public string IdCardBackImage { get; set; }
        public List<LARAdditionalInfo> AdditionalInfos { get; set; }
    }

    public class LARRelativePerson
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class LARBorrowerProfile
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public long DateOfBirth { get; set; }
        public string IdCardNumber { get; set; }
        public string IdCardIssuedBy { get; set; }
        public long IdCardIssuedDate { get; set; }
    }

    public class LARAddress
    {
        public string Level1_Id { get; set; }
        public string Level2_Id { get; set; }
        public string Level3_Id { get; set; }
        public string Detail { get; set; }
    }

    public class LAROccupation
    {
        public string OccupationCode { get; set; }
        public string OccupationName { get; set; }
        public string OccupationPosition { get; set; }
        public LARAddress OccupationAddress { get; set; }
        public string WorkplaceName { get; set; }
        public int MonthlyIncome { get; set; }
        public LARAddress CurrentWorkAddress { get; set; }
        public string WorkplacePhone { get; set; }
    }

    public class LARAdditionalInfo
    {
        public int LoanProductAdditionalInfoId { get; set; }
        public string value { get; set; }
    }
}
