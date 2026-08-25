using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class MutiraoEscalaRepository : IMutiraoEscalaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MutiraoEscalaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(MutiraoEscala escala, CancellationToken ct = default)
    {
        await _dbContext.MutiraoEscalas.AddAsync(escala, ct);
    }

    public Task<MutiraoEscala?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return _dbContext.MutiraoEscalas
            .Include(e => e.Presencas)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task<List<MutiraoEscala>> ListarAsync(CancellationToken ct = default)
    {
        return _dbContext.MutiraoEscalas
            .Include(e => e.Presencas)
            .OrderBy(e => e.Data)
            .ToListAsync(ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
