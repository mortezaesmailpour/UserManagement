using UserApi.Domain.Entities;

namespace UserApi.Application.Interfaces;

public interface IQueuedEmailRepository
{
    Task AddAsync(EmailQueue email, CancellationToken ct = default);
    Task<List<EmailQueue>> GetUnsentAsync(int take = 10, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
