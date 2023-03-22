using System.Collections.Generic;
using System.Linq;
using Ums.Core.Entities.Shared;

namespace Ums.Core.Entities.Mailing
{
    public class Receiver
    {
        public Receiver()
        {
        }

        public Receiver(List<EmailAddress> emails)
        {
            Emails = emails;
        }

        public Receiver(params string[] emails)
        {
            Emails = emails.ToMarketingEmailAddress();
        }
        public List<EmailAddress> Emails { get; set; } = new List<EmailAddress>();
        public List<StringPair> Objects { get; set; } = new List<StringPair>();
        public void AddObject(string key, string value)
        {
            Objects.Add(new StringPair(key, value));
        }
        public string[] To => Emails.Select(i => i.Email).ToArray();
    }

    public class EmailAddress
    {
        public EmailAddress()
        {
            
        }
        public EmailAddress(string email)
        {
            Email = email;
        }
        public string Email { get; set; }
        public List<StringPair> Objects { get; set; } = new List<StringPair>();
        public void AddObject(string key, string value)
        {
            Objects.Add(new StringPair(key, value));
        }
    }

    public static class Helper
    {
        public static Receiver ToMarketingEmail(this string[] emails)
        {
            return new Receiver
            {
                Emails = emails.Select(i => new EmailAddress
                {
                    Email = i
                }).ToList()
            };
        }

        public static List<EmailAddress> ToMarketingEmailAddress(this string[] emails)
        {
            return emails.Select(i => new EmailAddress { Email = i }).ToList();
        }
    }
}
