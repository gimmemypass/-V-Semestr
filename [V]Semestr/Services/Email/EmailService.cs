using _V_Semestr.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        private readonly SmtpClient _client;

        public EmailService(IOptions<SmtpSettings> option)
        {
            _settings = option.Value;
            _client = new SmtpClient(_settings.Server)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            };
        }
        public Task SendEmail(string email, string subject, string body)
        {
            var message = new MailMessage(
                "TTMG - your guid to GameDev",
                email,
                subject,
                body);
            return _client.SendMailAsync(message); 
        }
    }
}
