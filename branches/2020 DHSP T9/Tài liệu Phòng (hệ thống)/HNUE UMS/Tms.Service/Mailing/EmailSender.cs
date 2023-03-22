using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Hnue.Helper;
using Ums.Core.Entities.Mailing;
using Ums.Services.System;

namespace Ums.Services.Mailing
{
    public class EmailSender : IEmailSender
    {
        private string _host;
        private string _username;
        private string _password;
        private int _port;
        private MailAddress _from;
        public string SenderName { get; set; }
        private readonly ISettingService _settingService;

        public EmailSender(ISettingService settingService)
        {
            _settingService = settingService;
        }

        private delegate void Sender(string subject, string body, IList<EmailAddress> to);

        public void SendAsync(string subject, string body, IList<EmailAddress> to)
        {
            _host = _settingService.Gets().First(i => i.Key == "SMTP_HOST").Value;
            _port = _settingService.Gets().First(i => i.Key == "SMTP_PORT").Value.ToInt();
            _username = _settingService.Gets().First(i => i.Key == "SMTP_USERNAME").Value;
            _password = _settingService.Gets().First(i => i.Key == "SMTP_PASSWORD").Value;
            var senderEmail = _settingService.Gets().First(i => i.Key == "SMTP_SENDEREMAIL").Value;
            SenderName = _settingService.Gets().First(i => i.Key == "SMTP_SENDERNAME").Value;
            _from = new MailAddress(senderEmail, SenderName, Encoding.Unicode);
            Sender s = SendEmail;
            s.BeginInvoke(subject, body, to, null, null);
        }

        public void SendEmail(string subject, string body, IList<EmailAddress> to)
        {
            var c = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };
            foreach (var i in to)
            {
                var htmlBody = i.Objects.Aggregate(body, (current, s) => current.Replace("{{" + s.Key + "}}", s.Value));
                var mail = new MailMessage
                {
                    Body = htmlBody,
                    Subject = subject,
                    IsBodyHtml = true,
                    From = _from
                };
                mail.To.Clear();
                mail.To.Add(new MailAddress(i.Email));
                c.Send(mail);
            }
        }
    }
}