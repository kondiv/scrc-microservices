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

        builder.HasIndex(x => x.Model).IsUnique();
        builder.HasIndex(x => new { x.LastUpdatedAt, x.Id }).IsDescending();
        
        builder.Property(x => x.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(x => x.Model).HasMaxLength(128).HasColumnName("model");
        builder.Property(x => x.Amount).IsConcurrencyToken().HasColumnName("amount");
        builder.Property(x => x.CompatibleEquipmentModels).HasMaxLength(2048).HasColumnName("compatible_equipment_models");
        builder.Property(x => x.LastUpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("now()")
            .HasColumnName("last_updated_at");
        builder.Property(x => x.FirstTimeDeliveredAt).HasColumnName("first_time_delivered_at");
    }
}