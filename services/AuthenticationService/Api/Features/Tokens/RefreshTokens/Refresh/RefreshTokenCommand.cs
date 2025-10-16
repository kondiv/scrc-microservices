using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Tokens.RefreshTokens.Refresh;

public sealed record RefreshTokenCommand(string Token) : IRequest<Result<string>>;