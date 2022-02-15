using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.Entities
{
    [Serializable]
    public class MessageException: Exception
    {
        public string MessageCode;
        public MessageException(string message, string messageCode): base(message)
        {
            MessageCode = messageCode;
        }
    }
}
