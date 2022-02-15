using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2PLending.Web.API.Middlewares
{
    [Serializable]
    public class CustomException: Exception
    {
        public string MessageCode;
        public CustomException(string message, string messageCode): base(message)
        {
            MessageCode = messageCode;
        }
    }
}
