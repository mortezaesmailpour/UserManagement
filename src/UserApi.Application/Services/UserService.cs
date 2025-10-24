using Microsoft.AspNetCore.Identity;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Application.Mappers;
using UserApi.Domain.Entities;
using UserApi.Domain.Exceptions;

namespace UserApi.Application.Services;

public class UserService(UserManager<User> userManager, IQueuedEmailRepository queuedEmailRepository) : IUserService
{
    public async Task<ReadUserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existing = await userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new UserAlreadyExistsException($"A user with email {dto.Email} already exists.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Email,
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await queuedEmailRepository.AddAsync(CreateWelcomeEmail(user.Email, user.Name));

        return user.ToDto();
    }

    private static EmailQueue CreateWelcomeEmail(string email, string name) => new()
    {
        To = email,
        Subject = "Welcome!",
        Body = $"Dear {name},\nWelcome to our site!!!",
        IsSent = false
    };

    public async Task<ReadUserDto?> GetUserByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id) ?? throw new UserNoTFoundException($"User with ID {id} not found.");
        return user.ToDto();
    }
}
