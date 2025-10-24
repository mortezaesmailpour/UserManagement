namespace UserApi.Domain.Entities;

public class EmailQueue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public bool IsSent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
}

