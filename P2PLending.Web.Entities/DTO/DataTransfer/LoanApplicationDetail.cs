using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.DataTransfer
{
    public class LoanApplicationDetail
    {
        public int Id { get; set; }
        public int LoanProductId { get; set; }
        public string LoanProduct { get; set; }
        
        public float InterestRate { get; set; }
        public int Fees { get; set; }
        public LADLinkedPayment LinkedPayment { get; set; }
        public LADOccupation Occupation { get; set; }
        public LADRelativePerson RelativePerson1 { get; set; }
        public LADRelativePerson RelativePerson2 { get; set; }
        public LADBorrowerProfile Profile { get; set; }
        public AddressDTO ResidentAddress { get; set;}
        public AddressDTO CurrentAddress { get; set; }
        public List<LADAdditionalInfo> AdditionalInfos { get; set; }
        public string OwnedType { get; set; }
        public int MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; }
        public string IdCardWithUserImage { get; set; }
        public string IdCardFrontImage { get; set; }
        public string IdCardBackImage { get; set; }
        public List<LoanApplicationValidateAttributeDTO> InvalidateDatas { get; set; }
    }

    public class LADLinkedPayment
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
    }

    public class LADRelativePerson
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    } 

    public class LADBorrowerProfile
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public long DateOfBirth { get; set; }
        public string IdCardNumber { get; set; }
        public string IdCardIssuedBy { get; set; }
        public long IdCardIssuedDate { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
    }

    public class LADOccupation
    {
        public string OccupationCode { get; set; }
        public string OccupationName { get; set; }
        public string OccupationPosition { get; set; }
        public AddressDTO OccupationAddress { get; set; }
        public string WorkplaceName { get; set; }
        public int MonthlyIncome { get; set; }
        public string WorkplacePhone { get; set; }
    }

    public class LADAdditionalInfo
    {
        public int Id { get; set; }
        public int LoanProductAdditionalInfoId { get; set; }
        public string InfoName { get; set; }
        public string InfoFormat { get; set; }
        public string Value { get; set; }
        public byte[] BinaryValue { get; set; }
    }
}
