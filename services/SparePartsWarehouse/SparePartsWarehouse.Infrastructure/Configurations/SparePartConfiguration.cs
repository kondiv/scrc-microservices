using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Infrastructure.Configurations;

public sealed class SparePartConfiguration : IEntityTypeConfiguration<SparePart> 
{
    public void Configure(EntityTypeBuilder<SparePart> builder)
    {
        builder.ToTable("spare_parts");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(x => x.Model).HasMaxLength(128).HasColumnName("model");
        builder.HasMany(x => x.SparePartCompatibleEquipments)
            .WithOne(x => x.SparePart)
            .HasForeignKey(x => x.SparePartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}