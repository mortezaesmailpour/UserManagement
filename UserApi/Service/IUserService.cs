namespace UserApi.Service;

public interface IUserService
{
    Task<ReadUserDto> CreateUserAsync(CreateUserDto dto);
    Task<ReadUserDto?> GetUserByIdAsync(Guid id);
}