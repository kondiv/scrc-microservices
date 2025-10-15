using Api.Features.Tokens.RefreshTokens.Create;
using Common.Interfaces;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace Api.Features.Tokens.RefreshTokens.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<string>>
{
    private readonly AuthDbContext _context;
    private readonly ITokenHasher _tokenHasher;
    private readonly IMediator _mediator;

    public RefreshTokenCommandHandler(AuthDbContext context, ITokenHasher tokenHasher, IMediator mediator)
    {
        _context = context;
        _tokenHasher = tokenHasher;
        _mediator = mediator;
    }

    public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = _tokenHasher.HashToken(request.Token);

        var refreshToken = await _context
            .RefreshTokens
            .FirstOrDefaultAsync(rt => rt.HashToken == hashedToken, cancellationToken);

        if (refreshToken is null or { IsExpired: true } or { RevokedAt: not null })
        {
            return Result<string>.Failure(new AuthError("Unauthorized"));
        }
        
        refreshToken.Revoke();
        
        await _context.SaveChangesAsync(cancellationToken);

        var newRefreshToken = await _mediator.Send(new CreateRefreshTokenCommand(refreshToken.UserId),
            cancellationToken);

        return newRefreshToken;
    }
}