using Api.Dto;
using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Users.LoginUser;

public sealed record LoginUserCommand(string Login, string Password) : IRequest<Result<UserDto>>;