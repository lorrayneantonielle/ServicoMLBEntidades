using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class PresencaConfiguration : IEntityTypeConfiguration<Presenca>
{
    public void Configure(EntityTypeBuilder<Presenca> builder)
    {
        builder.ToTable("presencas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.MutiraoEscalaId).HasColumnName("mutirao_escala_id").IsRequired();
        builder.Property(x => x.FamiliaId).HasColumnName("familia_id").IsRequired();
        builder.Property(x => x.DataRegistro).HasColumnName("data_registro").IsRequired();
        builder.Property(x => x.PontuacaoConcedida).HasColumnName("pontuacao_concedida").IsRequired();

        builder.HasIndex(x => new { x.MutiraoEscalaId, x.FamiliaId }).IsUnique();

        builder.HasOne(x => x.Familia)
            .WithMany()
            .HasForeignKey(x => x.FamiliaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
