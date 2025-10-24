using UserApi.Application.DTOs;
using UserApi.Domain.Entities;

namespace UserApi.Application.Mappers;

public static class UserMapper
{
    public static ReadUserDto ToDto(this User user)
    {
        return new ReadUserDto(user.Id, user.Name, user.Email);
    }
}