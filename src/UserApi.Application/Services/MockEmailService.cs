using UserApi.Application.Interfaces;

namespace UserApi.Application.Services;

public class MockEmailService : IEmailService
{
    public Task SendWelcomeEmail(string to, string name)
    {
        Console.WriteLine($"Mock : Sent Email to {to}: 'Dear {name}, Welcome to our site!!!'");
        Thread.Sleep(500);
        return Task.CompletedTask;
    }
}