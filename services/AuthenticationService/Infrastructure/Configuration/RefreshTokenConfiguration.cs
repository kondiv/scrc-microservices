using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token");
        
        builder.HasIndex(r => r.HashToken).IsUnique();

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id).ValueGeneratedNever().HasColumnName("id");
        builder.Property(r => r.HashToken).HasMaxLength(512).IsRequired().HasColumnName("hash_token");
        builder.Property(r => r.CreatedAt).ValueGeneratedNever().IsRequired().HasColumnName("created_at");
        builder.Property(r => r.ExpiresAt).ValueGeneratedNever().IsRequired().HasColumnName("expires_at");
        builder.Property(r => r.RevokedAt).ValueGeneratedNever().HasColumnName("revoked_at");
        builder.Property(r => r.UserId).HasColumnName("user_id");
    }
}