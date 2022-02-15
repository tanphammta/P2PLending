using P2PLending.Web.Entities.Entities.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Interface
{
    public interface IFacebookService
    {
        Task<FacebookUserResource> GetUserFromFacebookAsync(string facebookToken);
    }
}
