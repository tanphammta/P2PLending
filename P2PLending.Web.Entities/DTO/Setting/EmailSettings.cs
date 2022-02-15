using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Entities.DTO.Setting
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Recipents { get; set; }
    }
}
