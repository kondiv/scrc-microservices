using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Infrastructure.Configurations;

internal sealed class SupplyConfiguration : IEntityTypeConfiguration<Supply>
{
    public void Configure(EntityTypeBuilder<Supply> builder)
    {
        builder.ToTable("supply");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(s => s.ArrivedAt).HasColumnName("arrived_at");
        builder.Property(s => s.ProcessedAt).HasColumnName("processed_at");
        builder
            .OwnsMany(s => s.SupplyItems, si =>
            {
                si.ToTable("supply_item");

                si.WithOwner().HasForeignKey("supply_id");
                
                si.HasKey("Id");
                si.Property(x => x.Id).HasColumnName("id");
                si.Property(x => x.Amount).HasColumnName("amount");
                si.Property(x => x.Model).HasMaxLength(256).HasColumnName("model");
                si.Property(x => x.CompatibleEquipmentModels)
                    .HasMaxLength(2048)
                    .HasColumnName("compatible_equipment_models");
            });
    }
}