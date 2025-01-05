using System.Net;
using System.Net.Mail;
using UsersMicroservice.core.Application;

namespace UsersMicroservice.core.Infrastructure
{
    public class EmailSenderService : IEmailSender<EmailContent>
    {
        public Task SendEmail(string to, EmailContent data)
        {
            var mail = Environment.GetEnvironmentVariable("EMAIL_SENDER_ADDRESS");
            var password = Environment.GetEnvironmentVariable("EMAIL_SENDER_PASSWORD");

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            return client.SendMailAsync(new MailMessage(from: mail, to, data.subject, data.body));
        }
    }
}
