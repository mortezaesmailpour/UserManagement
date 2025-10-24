namespace UserApi.Application.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmail(string toEmail, string name);
}