using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace UserInterface.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public SmtpEmailSender(IOptions<SmtpOptions> optionsAccessor,
                           ILogger<SmtpEmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public SmtpOptions Options { get; }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(Options.SenderName, Options.SenderEmail));
            mail.Subject = subject;
            mail.To.Add(new MailboxAddress(toEmail, toEmail));
            mail.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Options.Url, Options.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(Options.SenderEmail, Options.Password);
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
        }
    }


    public class SmtpOptions
    {
        public static string SECTION_NAME = "SMTP";

        public string SenderEmail { get; set; } = string.Empty;
        public string? SenderName { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}

