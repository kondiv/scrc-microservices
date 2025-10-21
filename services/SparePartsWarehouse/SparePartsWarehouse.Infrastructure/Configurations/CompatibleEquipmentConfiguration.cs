using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Infrastructure.Configurations;

public class CompatibleEquipmentConfiguration : IEntityTypeConfiguration<CompatibleEquipment>
{
    public void Configure(EntityTypeBuilder<CompatibleEquipment> builder)
    {
        builder.ToTable("compatible_equipments");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(x => x.Model).HasMaxLength(128).HasColumnName("model");
        builder.Property(x => x.MinVoltage).HasColumnName("min_voltage");
        builder.Property(x => x.MaxVoltage).HasColumnName("max_voltage");
        builder.HasMany(x => x.SparePartCompatibleEquipments)
            .WithOne(x => x.CompatibleEquipment)
            .HasForeignKey(x => x.CompatibleEquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}