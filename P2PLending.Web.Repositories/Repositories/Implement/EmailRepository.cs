using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using P2PLending.Web.Entities.DTO.Setting;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class EmailRepository: IEmailRepository
    {
        private EmailSettings _emailSetting;
        private SmtpClient smtpClient;
        IHostingEnvironment _env;
        public EmailRepository(IOptions<EmailSettings> emailSetting,
            IHostingEnvironment env)
        {
            _emailSetting = emailSetting.Value;
            smtpClient = new SmtpClient(_emailSetting.Host)
            {
                Port = _emailSetting.Port,
                UseDefaultCredentials = _emailSetting.UseDefaultCredentials,
                Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password),
                EnableSsl = _emailSetting.EnableSsl,
            };
            _env = env;
        }

        public bool SendEmail(string to, string subject, string content)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_emailSetting.Username),
                Subject = subject,
                Body = content,
                IsBodyHtml = true,
            };

            if (_env.IsDevelopment())
            {
                _emailSetting.Recipents.ForEach(mail =>
                {
                    email.To.Add(new MailAddress(mail));
                });
            }
            else
            {
                email.To.Add(new MailAddress(to));
            }

            smtpClient.Send(email);

            return true;
        }
    }
}
