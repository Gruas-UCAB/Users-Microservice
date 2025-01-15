using System.Net;
using System.Net.Mail;
using UsersMicroservice.core.Application;

namespace UsersMicroservice.core.Infrastructure
{
    public class EmailSenderService(IConfiguration configuration) : IEmailSender<EmailContent>
    {
        private readonly IConfiguration _configuration = configuration;
        public async Task SendEmail(string to, EmailContent data)
        {
            var smtpHost = _configuration["SmtpSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["SmtpSettings:SmtpPort"]);
            var name = Environment.GetEnvironmentVariable("EMAIL_SENDER_NAME");
            var mail = Environment.GetEnvironmentVariable("EMAIL_SENDER_ADDRESS");
            var password = Environment.GetEnvironmentVariable("EMAIL_SENDER_PASSWORD");

            var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(mail, password),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail, name),
                Subject = data.Subject,
                Body = data.Body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);
            await client.SendMailAsync(mailMessage);
        }
    }
}
