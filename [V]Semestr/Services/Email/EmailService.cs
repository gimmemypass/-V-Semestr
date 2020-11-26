using _V_Semestr.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace _V_Semestr.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        private readonly SmtpClient _client;

        public EmailService(IOptions<SmtpSettings> option)
        {
            _settings = option.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _settings.Server,
                    int.Parse(_settings.Port),
                    true
                    );
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                var message = CreateMessage(email, subject, body);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
        private MimeMessage CreateMessage(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.From));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = body
            };
            return message;
        }
    }
}
