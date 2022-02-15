using FirebaseAdmin.Messaging;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Interface
{
    public interface IFirebaseService
    {
        Task<string> SendMessage(FirebaseMessageRequest request);
    }
}
