using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class MedicaoRepository : IMedicaoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MedicaoRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Medicao medicao, CancellationToken ct = default)
    {
        await _dbContext.Medicoes.AddAsync(medicao, ct);
    }

    public Task<List<Medicao>> ListarAsync(Guid? etapaObraId, CancellationToken ct = default)
    {
        var query = _dbContext.Medicoes.Include(m => m.EtapaObra).AsQueryable();

        if (etapaObraId.HasValue)
        {
            query = query.Where(m => m.EtapaObraId == etapaObraId.Value);
        }

        return query.OrderByDescending(m => m.Data).ToListAsync(ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
