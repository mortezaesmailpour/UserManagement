using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Application.Services;
using UserApi.Application.Validators;
using UserApi.Domain.Entities;
using UserApi.Infrastructure.Data;
using UserApi.Infrastructure.Repositories;
using UserApi.Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailSenderService, MockEmailSenderService>();
builder.Services.AddScoped<IQueuedEmailRepository, QueuedEmailRepository>();
builder.Services.AddHostedService<EmailBackgroundWorker>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
