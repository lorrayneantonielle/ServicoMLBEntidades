using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class EtapaObraRepository : IEtapaObraRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EtapaObraRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(EtapaObra etapa, CancellationToken ct = default)
    {
        await _dbContext.EtapasObra.AddAsync(etapa, ct);
    }

    public Task<EtapaObra?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return _dbContext.EtapasObra.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task<List<EtapaObra>> ListarAsync(CancellationToken ct = default)
    {
        return _dbContext.EtapasObra.OrderBy(e => e.Ordem).ToListAsync(ct);
    }

    public Task<bool> ExisteOrdemAsync(int ordem, CancellationToken ct = default)
    {
        return _dbContext.EtapasObra.AnyAsync(e => e.Ordem == ordem, ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
