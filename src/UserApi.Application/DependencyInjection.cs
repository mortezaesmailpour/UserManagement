using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Application.Services;
using UserApi.Application.Validators;

namespace UserApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
