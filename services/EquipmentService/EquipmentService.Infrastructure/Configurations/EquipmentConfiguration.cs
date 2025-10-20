using EquipmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EquipmentService.Infrastructure.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("equipment");
        
        builder.HasIndex(eq => eq.SerialNumber).IsUnique();
        builder.HasIndex(eq => new { eq.DeliveredAt, eq.Id }).IsDescending();
        
        builder.HasKey(eq => eq.Id);

        builder.Property(eq => eq.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(eq => eq.Name).HasMaxLength(128).HasColumnName("name");
        builder.Property(eq => eq.SerialNumber).HasMaxLength(256).HasColumnName("serial_number");
        builder.Property(eq => eq.Category)
            .HasConversion<string>()
            .HasMaxLength(64)
            .HasColumnName("category");
        builder.Property(eq => eq.Status)
            .HasConversion<string>()
            .HasMaxLength(64)
            .HasColumnName("status");
        builder.Property(eq => eq.Voltage)
            .HasConversion<string>()
            .HasMaxLength(64)
            .HasColumnName("voltage");
        builder.Property(eq => eq.DeliveredAt).HasColumnName("delivered_at");
        builder.Property(eq => eq.InUseSince).HasColumnName("in_use_since");
        builder.Property(eq => eq.LastUpdatedAt).HasColumnName("last_updated_at");
        builder.Property(eq => eq.WarrantyExpiresAt).HasColumnName("warranty_expires_at");
    }
}