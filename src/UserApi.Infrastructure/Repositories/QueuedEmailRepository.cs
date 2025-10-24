using Microsoft.EntityFrameworkCore;
using UserApi.Application.Interfaces;
using UserApi.Domain.Entities;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repositories;

public class QueuedEmailRepository(AppDbContext db) : IQueuedEmailRepository
{
    public async Task AddAsync(EmailQueue email, CancellationToken ct = default)
    {
        await db.EmailQueues.AddAsync(email, ct);
        await db.SaveChangesAsync(ct);
    }

    public Task<List<EmailQueue>> GetAllAsync(CancellationToken ct = default)
    {
        return db.EmailQueues.ToListAsync(ct);
    }

    public async Task<List<EmailQueue>> GetUnsentAsync(int take = 10, CancellationToken ct = default)
    {
        return await db.EmailQueues
            .Where(e => !e.IsSent)
            .OrderBy(e => e.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
