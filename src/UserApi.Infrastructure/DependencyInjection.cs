using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure.Data;
using UserApi.Infrastructure.Repositories;
using UserApi.Infrastructure.Workers;

namespace UserApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IEmailSenderService, MockEmailSenderService>();
        services.AddScoped<IQueuedEmailRepository, QueuedEmailRepository>();

        services.AddHostedService<EmailBackgroundWorker>();

        return services;
    }
    public static IHost AddInfrastructure(this IHost application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }
        return application;
    }
}
