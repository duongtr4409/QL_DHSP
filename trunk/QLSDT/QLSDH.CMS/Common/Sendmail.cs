using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for Sendmail
/// </summary>
public class Sendmail
{
    public Sendmail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Với gmail thì có thể dùng các port sau : 25, 465 và 587
    //  Với yahoo thì có thể dùng các port sau : 25, 465 và 587
    //  Với domain thông thường thì port là : 25
    /// </summary>
    /// <param name="mailFrom"></param>
    /// <param name="mailpass"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="mailTo"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <param name="enableSsl"></param>
    /// <returns></returns>
    public static string SendMailFull(string mailFrom, string mailpass, string host, string port, string mailTo, string subject, string content, bool enableSsl)
    {
        try
        {
            var msg = new MailMessage
            {
                IsBodyHtml = true,
                Body = content,
                From = new MailAddress(mailFrom, mailFrom)
            };
            msg.To.Add(new MailAddress(mailTo));
            msg.Subject = subject;

            var client = new SmtpClient(host, int.Parse(port))
            {
                Credentials =
                    new NetworkCredential(mailFrom, mailpass),
                EnableSsl = enableSsl
            };

            client.Send(msg);

            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}