using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class EtapaObraConfiguration : IEntityTypeConfiguration<EtapaObra>
{
    public void Configure(EntityTypeBuilder<EtapaObra> builder)
    {
        builder.ToTable("etapas_obra");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Nome).HasColumnName("nome").IsRequired();
        builder.Property(x => x.Ordem).HasColumnName("ordem").IsRequired();
        builder.Property(x => x.PercentualConclusao).HasColumnName("percentual_conclusao").HasColumnType("decimal(5,2)").IsRequired();

        builder.HasIndex(x => x.Ordem).IsUnique();

        builder.HasMany(x => x.Medicoes)
            .WithOne(x => x.EtapaObra)
            .HasForeignKey(x => x.EtapaObraId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Ocorrencias)
            .WithOne(x => x.EtapaObra)
            .HasForeignKey(x => x.EtapaObraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
