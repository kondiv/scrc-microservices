using EquipmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentService.Infrastructure;

public class EquipmentServiceDbContext : DbContext
{
    public DbSet<Equipment> Equipments => Set<Equipment>();
    
    public EquipmentServiceDbContext(DbContextOptions<EquipmentServiceDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}