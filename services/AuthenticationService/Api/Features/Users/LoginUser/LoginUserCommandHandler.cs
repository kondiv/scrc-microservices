using Api.Dto;
using Common.Interfaces;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace Api.Features.Users.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserDto>>
{
    private readonly AuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(AuthDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Login == request.Login)
            .Select(u => new
            {
                User = new UserDto(u.Id, u.Email, u.Role.NormalizedName),
                u.HashPassword,
            })    
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return Result<UserDto>.Failure(new AuthError("Invalid login or password"));
        }

        var passwordValid = _passwordHasher.Verify(request.Password, result.HashPassword);
        
        return passwordValid ? Result<UserDto>.Success(result.User)
            : Result<UserDto>.Failure(new AuthError("Invalid login or password"));
    }
}