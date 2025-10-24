using UserApi.Application.Interfaces;

namespace UserApi.Application.Services;

public class MockEmailSenderService : IEmailSenderService
{
    public Task SendAsync(string to, string subject, string body)
    {
        Console.WriteLine($"Mock : Sent Email to {to} {subject}: {body}");
        Thread.Sleep(500);
        return Task.CompletedTask;
    }
}