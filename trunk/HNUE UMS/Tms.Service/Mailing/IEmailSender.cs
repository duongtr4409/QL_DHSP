using System.Collections.Generic;
using Ums.Core.Entities.Mailing;

namespace Ums.Services.Mailing
{
    public interface IEmailSender
    {
        void SendAsync(string subject, string body, IList<EmailAddress> to);
    }
}