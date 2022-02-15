using P2PLending.Web.Entities.DTO.DataTransfer;
using P2PLending.Web.Entities.Entities.AddressEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface IAddressRepository: IRepository<Address>
    {
        AddressDTO GetAddress(int id);
    }
}
