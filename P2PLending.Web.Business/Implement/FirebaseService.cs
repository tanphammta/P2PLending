using FirebaseAdmin.Messaging;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.DTO.RequestModel;
using P2PLending.Web.Entities.DTO.ResultModel;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Implement
{
    public class FirebaseService: IFirebaseService
    {
        private IFirebaseRepository _firebaseRepository;
        public FirebaseService(IFirebaseRepository firebaseRepository)
        {
            _firebaseRepository = firebaseRepository;
        }

        public async Task<string> SendMessage(FirebaseMessageRequest request)
        {
            try
            {
                var datas = new Dictionary<string, string>();
                if (request.Datas != null && request.Datas.Any())
                {
                    request.Datas.ForEach(data =>
                    {
                        if (!string.IsNullOrWhiteSpace(data.Key))
                        {
                            datas.Add(data.Key, data.Value);
                        }
                    });
                }
                datas.Add("Type", request.Type);
                if (request.Token != null)
                {
                    return await _firebaseRepository.SendMessage(request.Token, request.Title, request.Body, datas);
                }
                else if (request.Topic != null)
                {
                    return await _firebaseRepository.SendTopicMessage(request.Topic, request.Title, request.Body, datas);
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        //public Task<string> SendMessage(string token, string title, string body, Dictionary<string, string> datas)
        //{
        //    return _firebaseRepository.SendMessage(token, title, body, datas);
        //}

        //public Task<BatchResponse> SendMessages(List<string> tokens, string title, string body, Dictionary<string, string> datas)
        //{
        //    return _firebaseRepository.SendMessages(tokens, title, body, datas);
        //}

        //public Task<string> SendTopicMessage(string topic, string title, string body, Dictionary<string, string> datas)
        //{
        //    return _firebaseRepository.SendTopicMessage(topic, title, body, datas);
        //}
    }
}
