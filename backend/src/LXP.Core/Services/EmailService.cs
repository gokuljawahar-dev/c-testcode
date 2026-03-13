namespace LXP.Core.Services;

using System.Net;
using System.Net.Mail;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.Extensions.Options;

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            using (var mail = new MailMessage(this._emailSettings.SenderEmail, recipientEmail))
            {
                mail.Subject = subject;
                mail.Body = body;

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(
                        this._emailSettings.SenderEmail,
                        this._emailSettings.SenderPassword
                    );
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mail);
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
