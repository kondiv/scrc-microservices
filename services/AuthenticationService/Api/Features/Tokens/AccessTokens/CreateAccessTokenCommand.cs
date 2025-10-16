using Api.Dto;
using Domain.Entities;
using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Tokens.AccessTokens;

public sealed record CreateAccessTokenCommand(UserDto User) : IRequest<Result<string>>;