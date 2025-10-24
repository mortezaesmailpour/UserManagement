using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using UserApi.Application.Interfaces;

namespace UserApi.Application.Services;

public class EmailSenderService(IConfiguration config) : IEmailSenderService
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var emailSettings = config.GetSection("EmailSettings");

        using var client = new SmtpClient(emailSettings["SmtpServer"]!, int.Parse(emailSettings["Port"]!))
        {
            Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSettings["SenderEmail"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}
