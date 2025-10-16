using System.Security.Cryptography;
using System.Text;
using Common.Interfaces;

namespace Infrastructure.Security;

public class HmacTokenHasher : ITokenHasher
{
    private readonly byte[] _keyBytes;

    public HmacTokenHasher(string secret)
    {
        _keyBytes = Encoding.UTF8.GetBytes(secret);
    }

    public string HashToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        using var hmac = new HMACSHA256(_keyBytes);
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = hmac.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}