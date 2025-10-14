using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Tokens.RefreshTokens.Create;

public record CreateRefreshTokenCommand(Guid UserId) : IRequest<Result<string>>; 