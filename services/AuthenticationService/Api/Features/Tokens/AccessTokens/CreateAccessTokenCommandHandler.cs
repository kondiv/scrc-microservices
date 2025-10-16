using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Auth.Configuration;
using Shared.ResultPattern;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Api.Features.Tokens.AccessTokens;

internal sealed class CreateAccessTokenCommandHandler : IRequestHandler<CreateAccessTokenCommand, Result<string>>
{
    private readonly JwtOptions _jwtOptions;

    public CreateAccessTokenCommandHandler(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<string>> Handle(CreateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.User.Id.ToString()),
            new Claim(ClaimTypes.Email, request.User.Email),
            new Claim(ClaimTypes.Role, request.User.Role)
        };
        
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(
            issuer:  _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
            signingCredentials: signingCredentials);
        
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return await Task.FromResult(Result<string>.Success(token));
    }
}