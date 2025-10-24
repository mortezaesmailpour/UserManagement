namespace UserApi.Service;

public interface IEmailService
{
    Task SendWelcomeEmail(string toEmail, string name);
}