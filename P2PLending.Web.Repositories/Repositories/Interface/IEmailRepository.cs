using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface IEmailRepository
    {
        bool SendEmail(string to, string subject, string content);
    }
}
