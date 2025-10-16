using System.Security.Cryptography;
using Common.Interfaces;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Auth.Configuration;
using Shared.ResultPattern;

namespace Api.Features.Tokens.RefreshTokens.Create;

internal sealed class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Result<string>>
{
    private readonly AuthDbContext _context;
    private readonly ITokenHasher _hasher;
    private readonly JwtOptions _jwtOptions;

    public CreateRefreshTokenCommandHandler(AuthDbContext context, ITokenHasher hasher, IOptions<JwtOptions> jwtOptions)
    {
        _context = context;
        _hasher = hasher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<string>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        
        var token = Convert.ToBase64String(randomBytes);
        var hashToken = _hasher.HashToken(token);

        var expiresAt = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        await _context.RefreshTokens.AddAsync(new RefreshToken(
            hashToken,
            expiresAt)
        {
            UserId = request.UserId
        }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(token);
    }
}