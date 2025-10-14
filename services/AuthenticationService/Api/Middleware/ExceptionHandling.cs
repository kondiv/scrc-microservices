using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Api.Middleware;

public class ExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandling> _logger;

    public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation error: {Message}", ex.Message);
            await HandleValidationException(context, ex);
        }
    }

    private static async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .Select(failure => new
            {
                failure.PropertyName,
                failure.ErrorMessage,
            });

        var response = new
        {
            Type = "ValidationError",
            Message = "One or more validation errors occurred.",
            Errors = errors
        };
        
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        await context.Response.WriteAsync(json);
    }
}