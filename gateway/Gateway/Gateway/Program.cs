using Shared.Auth.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TechnicalSpecialist", policy =>
        policy.RequireRole("TECHNICALSPECIALIST", "ADMIN"));
});

builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();