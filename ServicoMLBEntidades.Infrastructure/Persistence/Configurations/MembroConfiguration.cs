using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Infrastructure.Persistence.Configurations;

public class MembroConfiguration : IEntityTypeConfiguration<Membro>
{
    public void Configure(EntityTypeBuilder<Membro> builder)
    {
        builder.ToTable("membros");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FamiliaId).HasColumnName("familia_id").IsRequired();
        builder.Property(x => x.Nome).HasColumnName("nome").IsRequired();
        builder.Property(x => x.DataNascimento).HasColumnName("data_nascimento").IsRequired();
        builder.Property(x => x.Vinculo).HasColumnName("vinculo").IsRequired();
        builder.Property(x => x.Cpf).HasColumnName("cpf").HasMaxLength(11).IsRequired();
        builder.Property(x => x.FamiliaExcluida).HasColumnName("familia_excluida").IsRequired();

        builder.HasIndex(x => x.FamiliaId);
        builder.HasIndex(x => x.Cpf).IsUnique().HasFilter("familia_excluida = false");
    }
}
