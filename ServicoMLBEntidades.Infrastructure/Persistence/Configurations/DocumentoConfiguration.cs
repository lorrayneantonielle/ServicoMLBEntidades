using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("documentos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FamiliaId).HasColumnName("familia_id").IsRequired();
        builder.Property(x => x.Tipo).HasColumnName("tipo").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.ArquivoPath).HasColumnName("arquivo_path");
        builder.Property(x => x.ArquivoMimeType).HasColumnName("arquivo_mime_type");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.HasIndex(x => x.FamiliaId);
        builder.HasIndex(x => new { x.FamiliaId, x.Tipo }).IsUnique();
    }
}
