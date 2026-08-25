using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class PresencaRepository : IPresencaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PresencaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Presenca presenca, CancellationToken ct = default)
    {
        await _dbContext.Presencas.AddAsync(presenca, ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
