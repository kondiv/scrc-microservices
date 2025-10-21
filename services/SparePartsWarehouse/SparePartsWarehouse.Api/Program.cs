using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SparePartsWarehouseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(options => 
    options.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();