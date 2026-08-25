using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class MedicaoConfiguration : IEntityTypeConfiguration<Medicao>
{
    public void Configure(EntityTypeBuilder<Medicao> builder)
    {
        builder.ToTable("medicoes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.EtapaObraId).HasColumnName("etapa_obra_id").IsRequired();
        builder.Property(x => x.Data).HasColumnName("data").IsRequired();
        builder.Property(x => x.StatusAprovacao).HasColumnName("status_aprovacao").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.RecursosLiberados).HasColumnName("recursos_liberados").HasColumnType("decimal(14,2)");
        builder.Property(x => x.Observacao).HasColumnName("observacao");
        builder.Property(x => x.RegistradoPorUsuarioId).HasColumnName("registrado_por_usuario_id").IsRequired();

        builder.HasIndex(x => x.EtapaObraId);
    }
}
