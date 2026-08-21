using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Infrastructure.Identity;

namespace ServicoMLBEntidades.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Familia> Familias => Set<Familia>();
    public DbSet<Membro> Membros => Set<Membro>();
    public DbSet<Documento> Documentos => Set<Documento>();
    public DbSet<FamiliaStatusHistorico> FamiliaStatusHistoricos => Set<FamiliaStatusHistorico>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
