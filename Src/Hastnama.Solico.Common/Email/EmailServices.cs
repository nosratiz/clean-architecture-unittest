using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Common.Email
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSetting _emailSetting;

        public EmailServices(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = new EmailSetting
            {
                Host = emailSetting.Value.Host,
                UserName = emailSetting.Value.UserName,
                Password = emailSetting.Value.Password,
                Port = emailSetting.Value.Port,
                From = emailSetting.Value.From
            };
        }

        public Task SendMessage(string emailTo, string subject, string body)
        {
            MailMessage message = new MailMessage(_emailSetting.From, emailTo, subject, body)
            {
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.Default
            };

            NetworkCredential credential = new NetworkCredential(_emailSetting.UserName, _emailSetting.Password);

            SmtpClient smtp = new SmtpClient(_emailSetting.Host, _emailSetting.Port)
            {
                Credentials = credential,
                EnableSsl = true
            };

            try
            {
                var result = smtp.SendMailAsync(message);
            }
            catch (Exception)
            {
                //
            }
            return Task.CompletedTask;
        }
    }
}