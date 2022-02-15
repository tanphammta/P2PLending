using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface IMasterDataRepository
    {
        Task<List<AddressLevel1>> ListAddressLevel1();
        Task<List<AddressLevel2>> ListAddressLevel2();
        Task<List<AddressLevel3>> ListAddressLevel3();
        Task<List<MaritalStatus>> ListMaritalStatuses();
        Task<List<Occupation>> ListOccupations();
        Task<List<RelativePersonType>> ListRelativePersonTypes();
        List<PaymentService> ListPaymentServices();
        List<PaymentService> ListPaymentServices(List<int> serviceIds);
        PaymentService GetPaymentService(int serviceId);
        Task<List<CreditRankConfig>> ListCreditRankConfigs();
        List<LoanValidateAttributeConfig> ListValidateAttributes();
    }
}
