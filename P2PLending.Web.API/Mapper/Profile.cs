using AutoMapper;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.Entities.Account;
using P2PLending.Web.Entities.Entities.AccountLinkedPayment;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.Loans;
using P2PLending.Web.Entities.Entities.MasterData;

namespace P2PLending.Web.API.Mapper
{
    public class MySQLToDTOProfile : Profile
    {
        public MySQLToDTOProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<LoanProductAdditionalInfo, LoanTypeAdditionalInfoDTO>();
            CreateMap<LoanProduct, LoanProductDTO>();
            CreateMap<PaymentService, PaymentServiceDTO>();
            CreateMap<MobileLinkedPayment, MobileLinkedPaymentDTO>();
            CreateMap<CreditRankConfig, CreditRankConfigDTO>();
            CreateMap<Address, AddressDTO>();
            CreateMap<LoanApplicationValidateAttribute, LoanApplicationValidateAttributeDTO>();
        }
    }

    public class DTOToMySQLProfile : Profile
    {
        public DTOToMySQLProfile()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();

            CreateMap<APCreateAccountRequest, AccountOperation>();
        }
    }
}
