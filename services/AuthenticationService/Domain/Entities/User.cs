namespace Domain.Entities;

public class User
{
    public Guid Id { get; private init; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Login { get; private set; }
    public string HashPassword { get; private set; }
    public int RoleId { get; private set; }
    public virtual Role Role { get; private set; } = null!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    public User(string fullName, string email, string login, string hashPassword, int roleId)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Login = login;
        HashPassword = hashPassword;
        RoleId = roleId;
    }
}