using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Entities;

namespace UserApi.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<EmailQueue> EmailQueues => Set<EmailQueue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<EmailQueue>(entity =>
        {
            entity.ToTable("EmailQueue");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.To)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.Subject)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.Body)
                  .IsRequired();

            entity.Property(e => e.IsSent)
                  .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}