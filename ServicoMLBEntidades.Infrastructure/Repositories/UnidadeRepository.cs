using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class UnidadeRepository : IUnidadeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UnidadeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(UnidadeHabitacional unidade, CancellationToken ct = default)
    {
        await _dbContext.UnidadesHabitacionais.AddAsync(unidade, ct);
    }

    public Task<UnidadeHabitacional?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return _dbContext.UnidadesHabitacionais.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public Task<UnidadeHabitacional?> ObterPorFamiliaIdAsync(Guid familiaId, CancellationToken ct = default)
    {
        return _dbContext.UnidadesHabitacionais.FirstOrDefaultAsync(u => u.FamiliaId == familiaId, ct);
    }

    public Task<List<UnidadeHabitacional>> ListarAsync(UnidadeStatus? status, CancellationToken ct = default)
    {
        var query = _dbContext.UnidadesHabitacionais.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(u => u.Status == status.Value);
        }

        return query.OrderBy(u => u.Identificador).ToListAsync(ct);
    }

    public Task<bool> ExisteIdentificadorAsync(string identificador, CancellationToken ct = default)
    {
        return _dbContext.UnidadesHabitacionais.AnyAsync(u => u.Identificador == identificador, ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
