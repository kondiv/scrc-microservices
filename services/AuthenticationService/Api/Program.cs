using System.Reflection;
using Api.Behaviors;
using Api.Extensions;
using Api.Middleware;
using Common.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Auth.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbConnection")));

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddServices();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandling>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();