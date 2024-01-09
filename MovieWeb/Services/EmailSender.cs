using MovieWeb.IServices;
using System.Net.Mail;
using System.Net;

namespace MovieWeb.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("thnhphan90@gmail.com", "rtha lqgd bpet amlo")
            };
            MailMessage mail = new MailMessage(from: "thnhphan90@gmail.com", to: email, subject, message);
            mail.IsBodyHtml = true;
            return client.SendMailAsync(mail);
        }
    }
}
