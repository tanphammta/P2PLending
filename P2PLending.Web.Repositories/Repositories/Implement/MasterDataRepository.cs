using Microsoft.EntityFrameworkCore;
using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Entities.Entities.MasterData;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class MasterDataRepository: IMasterDataRepository
    {
        private readonly P2PDBContextFactory _dbContextFactory;
        public MasterDataRepository(P2PDBContextFactory dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<AddressLevel1>> ListAddressLevel1()
        {
            using(var _context = _dbContextFactory.Create())
            {
                return await _context.AddressLevel1s.ToListAsync();
            }
        }

        public async Task<List<AddressLevel2>> ListAddressLevel2()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.AddressLevel2s.ToListAsync();
            }
        }

        public async Task<List<AddressLevel3>> ListAddressLevel3()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.AddressLevel3s.ToListAsync();
            }
        }

        public async Task<List<MaritalStatus>> ListMaritalStatuses()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.MaritalStatuses.ToListAsync();
            }
        }

        public async Task<List<Occupation>> ListOccupations()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.Occupations.ToListAsync();
            }
        }

        public async Task<List<RelativePersonType>> ListRelativePersonTypes()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.RelativePersonTypes.ToListAsync();
            }
        }

        public List<PaymentService> ListPaymentServices()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return _context.PaymentServices.ToList();
            }
        }

        public List<PaymentService> ListPaymentServices(List<int> serviceIds)
        {
            using (var _context = _dbContextFactory.Create())
            {
                return _context.PaymentServices.Where(p => serviceIds.Contains(p.id)).ToList();
            }
        }

        public PaymentService GetPaymentService(int serviceId)
        {
            using (var _context = _dbContextFactory.Create())
            {
                return _context.PaymentServices.FirstOrDefault(p => p.id == serviceId);
            }
        }

        public async Task<List<CreditRankConfig>> ListCreditRankConfigs()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return await _context.CreditRankConfigs.ToListAsync();
            }
        }

        public List<LoanValidateAttributeConfig> ListValidateAttributes()
        {
            using (var _context = _dbContextFactory.Create())
            {
                return _context.LoanValidateAttributes.ToList();
            }
        }
    }
}
