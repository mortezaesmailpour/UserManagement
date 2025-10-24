using UserApi.Application.DTOs;

namespace UserApi.Application.Interfaces;

public interface IUserService
{
    Task<ReadUserDto> CreateUserAsync(CreateUserDto dto);
    Task<ReadUserDto?> GetUserByIdAsync(string id);
}