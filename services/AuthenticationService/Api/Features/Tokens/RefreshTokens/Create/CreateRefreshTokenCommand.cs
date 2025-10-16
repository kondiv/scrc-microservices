using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Tokens.RefreshTokens.Create;

public sealed record CreateRefreshTokenCommand(Guid UserId) : IRequest<Result<string>>; 