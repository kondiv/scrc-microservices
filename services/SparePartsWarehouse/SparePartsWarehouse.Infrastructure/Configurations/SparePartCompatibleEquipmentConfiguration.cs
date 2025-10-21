using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Infrastructure.Configurations;

public sealed class SparePartCompatibleEquipmentConfiguration : IEntityTypeConfiguration<SparePartCompatibleEquipment>
{
    public void Configure(EntityTypeBuilder<SparePartCompatibleEquipment> builder)
    {
        builder.ToTable("spare_part_compatible_equipments");
        
        builder.HasKey(x => new { x.SparePartId, x.CompatibleEquipmentId });
        builder.Property(x => x.SparePartId).HasColumnName("spare_part_id");
        builder.Property(x => x.CompatibleEquipmentId).HasColumnName("compatible_equipment_id");
    }
}