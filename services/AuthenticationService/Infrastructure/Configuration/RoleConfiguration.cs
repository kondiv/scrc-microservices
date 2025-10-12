using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");
        
        builder.HasIndex(r => r.NormalizedName).IsUnique();
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(r => r.Name).HasMaxLength(64).IsRequired().HasColumnName("name");
        builder.Property(r => r.NormalizedName).HasMaxLength(64).IsRequired().HasColumnName("normalized_name");
        
        builder.HasData([
            new Role("Admin"){ Id = 1 },
            new Role("Scientist"){ Id = 2 },
            new Role("Technical Specialist"){ Id = 3 },
        ]);
    }
}