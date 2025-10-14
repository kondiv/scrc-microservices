using Api.Features.Tokens.AccessTokens;
using Api.Features.Tokens.RefreshTokens.Create;
using Api.Features.Users.LoginUser;
using Api.Features.Users.RegisterUser;
using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultPattern.Errors;

namespace Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Guid>> Register(RegisterRequest registerRequest,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(
            registerRequest.FullName,
            registerRequest.Email,
            registerRequest.Login,
            registerRequest.PlainPassword,
            registerRequest.Role);
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            ErrorCode.AlreadyExists => Conflict(result.Error),
            _ => BadRequest(result.Error.Message),
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(loginRequest.Login, loginRequest.Password);

        var loginResult = await _mediator.Send(command, cancellationToken);

        if (loginResult.IsFailure)
        {
            return Unauthorized(loginResult.Error.Message);
        }
        
        var accessTokenResult = await _mediator.Send(new CreateAccessTokenCommand(loginResult.Value), cancellationToken);
        var refreshTokenResult = await _mediator.Send(new CreateRefreshTokenCommand(loginResult.Value.Id), cancellationToken);

        if (accessTokenResult.IsFailure || refreshTokenResult.IsFailure)
        {
            return Unauthorized(accessTokenResult.Error.Message);
        }
        
        HttpContext.Response.Cookies.Append("access_token", accessTokenResult.Value, new CookieOptions()
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(2),
            HttpOnly = true,
            Secure = true
        });
        
        HttpContext.Response.Cookies.Append("refresh_token", refreshTokenResult.Value, new CookieOptions()
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            Secure = true
        });

        return Ok(loginResult.Value);
    }
}