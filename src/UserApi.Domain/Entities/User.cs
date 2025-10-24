using Microsoft.AspNetCore.Identity;

namespace UserApi.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}