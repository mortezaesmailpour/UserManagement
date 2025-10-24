using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserApi.Application.Interfaces;

namespace UserApi.Infrastructure.Workers;

public class EmailBackgroundWorker(IServiceScopeFactory scopeFactory, ILogger<EmailBackgroundWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Email background worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var queuedEmailRepo = scope.ServiceProvider.GetRequiredService<IQueuedEmailRepository>();
            var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

            var pendingEmails = await queuedEmailRepo.GetUnsentAsync(10, stoppingToken); 

            foreach (var email in pendingEmails)
            {
                try
                {
                    await emailSenderService.SendAsync(email.To, email.Subject, email.Body);

                    email.IsSent = true;
                    email.SentAt = DateTime.UtcNow;
                    await queuedEmailRepo.SaveChangesAsync(stoppingToken);
                    logger.LogInformation("Email sent to {To}", email.To);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error sending email to {To}", email.To);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private Task SendEmailAsync(string to, string subject, string body)
    {
        // For demo — replace with real SMTP client logic
        Console.WriteLine($"📧 Sending email to {to}: {subject}");
        return Task.CompletedTask;
    }
}
