using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Entities;

namespace UserApi.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureUserEntity(builder);
    }

    private void ConfigureUserEntity(ModelBuilder builder)
    {
        var entity = builder.Entity<User>();

        entity.ToTable("Users");

        entity.Property(u => u.Name)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(u => u.Email)
              .IsRequired()
              .HasMaxLength(255);

        entity.HasIndex(u => u.Email)
              .IsUnique();

        entity.Property(u => u.CreatedAt)
              .HasDefaultValueSql("NOW()")
              .ValueGeneratedOnAdd();

        entity.Property(u => u.UpdatedAt)
              .HasDefaultValueSql("NOW()")
              .ValueGeneratedOnAddOrUpdate();

        entity.Property(u => u.IsDeleted)
              .HasDefaultValue(false);

        entity.HasQueryFilter(u => !u.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;

            entry.Entity.UpdatedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}