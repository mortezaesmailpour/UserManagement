using Microsoft.AspNetCore.Identity;

namespace UserApi.Domain.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = null!;
}