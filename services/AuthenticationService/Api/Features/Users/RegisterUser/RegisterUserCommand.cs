using Domain.ValueObjects;
using MediatR;
using Shared.ResultPattern;

namespace Api.Features.Users.RegisterUser;

public sealed record RegisterUserCommand(FullName FullName, string Email, string Login, string PlainPassword, string Role)
    : IRequest<Result<Guid>>;