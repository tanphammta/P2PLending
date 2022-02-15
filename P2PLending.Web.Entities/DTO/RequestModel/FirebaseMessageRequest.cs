using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.RequestModel
{
    public class FirebaseMessageRequest
    {
        public string Topic { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public List<FirebaseMessageData> Datas { get; set; }
    }

    public class FirebaseMessageData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
