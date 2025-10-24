using UserApi.Domain.Entities;

namespace UserApi.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
}