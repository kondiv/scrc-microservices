using Common.Interfaces;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace Api.Features.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly AuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterUserCommandHandler(AuthDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordHasher.HashPassword(request.PlainPassword);
        
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == request.Role, cancellationToken);
        if (role == null)
        {
            return Result<Guid>.Failure(new NotFoundError("Cannot find role with provided role name"));
        }
        
        var user = new User(request.FullName.ToString(), request.Email, request.Login, hashedPassword, role.Id);
        
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e) 
            when(e.InnerException is NpgsqlException {SqlState: PostgresErrorCodes.UniqueViolation})
        {
            return Result<Guid>.Failure(new AlreadyExistsError("User already exists"));
        }
        
        return Result<Guid>.Success(user.Id);
    }
}