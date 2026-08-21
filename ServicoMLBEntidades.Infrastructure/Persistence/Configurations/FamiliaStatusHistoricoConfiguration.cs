using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class FamiliaStatusHistoricoConfiguration : IEntityTypeConfiguration<FamiliaStatusHistorico>
{
    public void Configure(EntityTypeBuilder<FamiliaStatusHistorico> builder)
    {
        builder.ToTable("familia_status_historico");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FamiliaId).HasColumnName("familia_id").IsRequired();
        builder.Property(x => x.StatusAnterior).HasColumnName("status_anterior").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.StatusNovo).HasColumnName("status_novo").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.Motivo).HasColumnName("motivo");
        builder.Property(x => x.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(x => x.DataTransicao).HasColumnName("data_transicao").IsRequired();

        builder.HasIndex(x => x.FamiliaId);
    }
}
