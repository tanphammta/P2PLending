using AutoMapper;
using Microsoft.EntityFrameworkCore;
using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Entities.AddressEntity;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class AddressRepository: Repository<Address>, IAddressRepository
    {
        private IMapper _mapper;
        public AddressRepository(P2PLendingDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public AddressDTO GetAddress(int id)
        {
            var address = (from adr in DbSet
                          join level1 in _context.AddressLevel1s on adr.level1_id equals level1.level1_id
                          join level2 in _context.AddressLevel2s on adr.level2_id equals level2.level2_id
                          join level3 in _context.AddressLevel3s on adr.level3_id equals level3.level3_id
                          where adr.id == id
                          select new AddressDTO()
                          {
                              Level1Id = adr.level1_id,
                              Level1 = level1.name,
                              Level2Id = adr.level2_id,
                              Level2 = level2.name,
                              Level3Id = adr.level3_id,
                              Level3 = level3.name,
                              Detail = adr.address_detail
                          }).FirstOrDefault();
            return address;
        }
    }
}
