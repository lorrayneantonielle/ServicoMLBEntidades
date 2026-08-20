using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(x => x.TokenHash).HasColumnName("token_hash").IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at").IsRequired();
        builder.Property(x => x.RevokedAt).HasColumnName("revoked_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => x.UsuarioId);

        builder.Ignore(x => x.IsAtivo);
    }
}
