using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasIndex(u => new { u.Email, u.Login }).IsUnique();
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(u => u.FullName).HasMaxLength(256).IsRequired().HasColumnName("full_name");
        builder.Property(u => u.Email).HasMaxLength(128).IsRequired().HasColumnName("email");
        builder.Property(u => u.Login).HasMaxLength(128).IsRequired().HasColumnName("login");
        builder.Property(u => u.HashPassword).HasMaxLength(512).IsRequired().HasColumnName("hash_password");
        builder.Property(u => u.RoleId).HasColumnName("role_id");
        
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(r => r.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}