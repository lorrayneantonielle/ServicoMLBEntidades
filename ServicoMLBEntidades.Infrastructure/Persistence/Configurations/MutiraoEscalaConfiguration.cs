using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class MutiraoEscalaConfiguration : IEntityTypeConfiguration<MutiraoEscala>
{
    public void Configure(EntityTypeBuilder<MutiraoEscala> builder)
    {
        builder.ToTable("mutirao_escalas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Data).HasColumnName("data").IsRequired();
        builder.Property(x => x.Turno).HasColumnName("turno").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.VagasTotais).HasColumnName("vagas_totais").IsRequired();
        builder.Property(x => x.PontuacaoPorPresenca).HasColumnName("pontuacao_por_presenca").IsRequired();

        builder.HasMany(x => x.Presencas)
            .WithOne(x => x.MutiraoEscala)
            .HasForeignKey(x => x.MutiraoEscalaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
