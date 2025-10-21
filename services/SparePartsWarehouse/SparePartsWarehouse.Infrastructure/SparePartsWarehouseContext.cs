using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Infrastructure;

public sealed class SparePartsWarehouseContext : DbContext
{
    public DbSet<SparePart> SpareParts => Set<SparePart>();

    public SparePartsWarehouseContext(DbContextOptions<SparePartsWarehouseContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}