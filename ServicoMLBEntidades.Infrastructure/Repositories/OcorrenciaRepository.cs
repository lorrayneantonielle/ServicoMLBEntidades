using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class OcorrenciaRepository : IOcorrenciaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OcorrenciaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Ocorrencia ocorrencia, CancellationToken ct = default)
    {
        await _dbContext.Ocorrencias.AddAsync(ocorrencia, ct);
    }

    public Task<List<Ocorrencia>> ListarAsync(Guid? etapaObraId, CancellationToken ct = default)
    {
        var query = _dbContext.Ocorrencias.AsQueryable();

        if (etapaObraId.HasValue)
        {
            query = query.Where(o => o.EtapaObraId == etapaObraId.Value);
        }

        return query.OrderByDescending(o => o.Data).ToListAsync(ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
