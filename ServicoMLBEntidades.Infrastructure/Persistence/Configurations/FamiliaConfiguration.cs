using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class FamiliaConfiguration : IEntityTypeConfiguration<Familia>
{
    public void Configure(EntityTypeBuilder<Familia> builder)
    {
        builder.ToTable("familias");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.RendaFamiliar).HasColumnName("renda_familiar").HasColumnType("decimal(12,2)").IsRequired();
        builder.Property(x => x.SituacaoVulnerabilidade).HasColumnName("situacao_vulnerabilidade").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.PontuacaoAcumulada).HasColumnName("pontuacao_acumulada").IsRequired();
        builder.Property(x => x.Excluida).HasColumnName("excluida").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.HasIndex(x => x.Status);

        builder.HasMany(x => x.Membros)
            .WithOne(x => x.Familia)
            .HasForeignKey(x => x.FamiliaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Documentos)
            .WithOne(x => x.Familia)
            .HasForeignKey(x => x.FamiliaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
