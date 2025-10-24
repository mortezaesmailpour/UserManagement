using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using UserApi.Application.Interfaces;

namespace UserApi.Application.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendWelcomeEmail(string toEmail, string name)
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
            Subject = "Welcome!",
            Body = $"Dear {name},\nWelcome to our site!!!",
            IsBodyHtml = false
        };
        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}
