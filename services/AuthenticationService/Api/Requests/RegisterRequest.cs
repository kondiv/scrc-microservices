using Domain.ValueObjects;

namespace Api.Requests;

public sealed record RegisterRequest(FullName FullName, string Email, string Login, string PlainPassword, string Role);