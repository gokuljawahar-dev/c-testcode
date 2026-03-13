namespace LXP.Common.Utils;

using System.Net;
using System.Net.Mail;

public class EmailGenerator
{
    public static void Sendpassword(string password, string Email)
    {
        var fromMail = "sanjairock85@gmail.com";
        var senderPass = "vmrc sKxx ihyK jscu";
        var message = new MailMessage { From = new MailAddress(fromMail) };
        message.To.Add(Email);
        message.Subject = $"Confidential! New Password for Your Accounts";
        message.Body =
            $"Dear Learner \r\n\r\nThis is a notification from the management. Your new password for  accounts has been generated. Please take a moment to update your password.\r\n\r\nNew Password:{password}\r\n\r\nRemember to change your password promptly for security reasons.\r\n\r\nThank you, Management Team";
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromMail, senderPass),
            EnableSsl = true
        };

        smtpClient.Send(message);
    }
}
