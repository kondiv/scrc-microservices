namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private init; }
    public string HashToken { get; private init; }
    public DateTimeOffset CreatedAt { get; private init; }
    public DateTimeOffset ExpiresAt { get; private init; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public Guid UserId { get; init; }
    public virtual User User { get; private set; } = null!;
    public bool IsExpired => ExpiresAt < DateTimeOffset.UtcNow;

    public RefreshToken(string hashToken, DateTimeOffset expiresAt)
    {
        Id = Guid.NewGuid();
        HashToken = hashToken;
        CreatedAt = DateTimeOffset.UtcNow;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        if (RevokedAt is not null)
        {
            return;
        }

        RevokedAt = DateTimeOffset.UtcNow;
    }
}