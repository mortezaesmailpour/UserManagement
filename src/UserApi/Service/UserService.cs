namespace UserApi.Service;

public class UserService(IUserRepository userRepository, IEmailService emailService, ILogger<UserService> logger) : IUserService
{
    public async Task<ReadUserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existing = await userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("Email already exists.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password
        };
        var createdUser = await userRepository.AddAsync(user);
        try
        {
            await emailService.SendWelcomeEmail(createdUser.Email, createdUser.Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send welcome email to {Email}", createdUser.Email);
        }
        return new ReadUserDto(createdUser.Id, createdUser.Name, createdUser.Email);
    }

    public async Task<ReadUserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("User not found.");
        return new ReadUserDto(user.Id, user.Name, user.Email);
    }
}
