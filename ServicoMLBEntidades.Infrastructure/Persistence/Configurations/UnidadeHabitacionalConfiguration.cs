using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class UnidadeHabitacionalConfiguration : IEntityTypeConfiguration<UnidadeHabitacional>
{
    public void Configure(EntityTypeBuilder<UnidadeHabitacional> builder)
    {
        builder.ToTable("unidades_habitacionais");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Identificador).HasColumnName("identificador").IsRequired();
        builder.Property(x => x.Metragem).HasColumnName("metragem").HasColumnType("decimal(8,2)").IsRequired();
        builder.Property(x => x.LocalizacaoTerreno).HasColumnName("localizacao_terreno").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.FamiliaId).HasColumnName("familia_id");

        builder.HasIndex(x => x.Identificador).IsUnique();

        builder.HasOne(x => x.Familia)
            .WithOne(f => f.UnidadeHabitacional)
            .HasForeignKey<UnidadeHabitacional>(x => x.FamiliaId);
    }
}
