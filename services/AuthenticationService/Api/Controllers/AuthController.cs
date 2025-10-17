using Api.Features.Tokens.AccessTokens;
using Api.Features.Tokens.RefreshTokens.Create;
using Api.Features.Tokens.RefreshTokens.Refresh;
using Api.Features.Users.LoginUser;
using Api.Features.Users.RegisterUser;
using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultPattern.Errors;

namespace Api.Controllers;

[ApiController]
public sealed class AuthController : ControllerBase
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

        var tokens = await Task.WhenAll(
            _mediator.Send(new CreateAccessTokenCommand(loginResult.Value), cancellationToken),
            _mediator.Send(new CreateRefreshTokenCommand(loginResult.Value.Id), cancellationToken));

        var accessTokenResult = tokens[0];
        var refreshTokenResult = tokens[1];

        if (accessTokenResult.IsFailure || refreshTokenResult.IsFailure)
        {
            return Unauthorized(accessTokenResult.Error.Message);
        }
        
        WriteTokenToCookie(HttpContext, "access_token", accessTokenResult.Value, DateTimeOffset.UtcNow.AddMinutes(2));
        WriteTokenToCookie(HttpContext, "refresh_token", refreshTokenResult.Value,
            DateTimeOffset.UtcNow.AddDays(30));

        return Ok(loginResult.Value);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken);

        if (refreshToken is null)
        {
            return Unauthorized();
        }

        var newRefreshToken = await _mediator.Send(new RefreshTokenCommand(refreshToken),
            cancellationToken);

        if (newRefreshToken.Succeeded)
        {
            WriteTokenToCookie(HttpContext, "refresh_token", newRefreshToken.Value,
                DateTimeOffset.UtcNow.AddDays(30));
            
            return NoContent();
        }

        return newRefreshToken.Error.ErrorCode switch
        {
            ErrorCode.DbUpdateConcurrency => new BadRequestObjectResult(newRefreshToken.Error.Message),
            _ => Unauthorized()
        };
    }

    private void WriteTokenToCookie(HttpContext context, string key, string value, DateTimeOffset expiresAt)
    {
        context.Response.Cookies.Append(key, value, new CookieOptions()
        {
            Secure = true,
            HttpOnly = true,
            Expires = expiresAt
        });
    }
}