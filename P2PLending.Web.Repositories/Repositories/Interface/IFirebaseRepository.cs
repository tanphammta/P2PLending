using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface IFirebaseRepository
    {
        Task<string> GetFirebaseToken(string deviceId);
        Task<string> SendMessage(string token, string title, string body, Dictionary<string, string> datas);
        Task<BatchResponse> SendMessages(List<string> tokens, string title, string body, Dictionary<string, string> datas);
        Task<string> SendTopicMessage(string topic, string title, string body, Dictionary<string, string> datas);
    }
}
