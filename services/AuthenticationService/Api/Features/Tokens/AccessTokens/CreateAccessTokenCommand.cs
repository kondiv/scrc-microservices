using Api.Dto;
using Domain.Entities;
using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Tokens.AccessTokens;

public record CreateAccessTokenCommand(UserDto User) : IRequest<Result<string>>;