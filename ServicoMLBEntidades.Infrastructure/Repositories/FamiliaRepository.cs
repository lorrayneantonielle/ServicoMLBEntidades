using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class FamiliaRepository : IFamiliaRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamiliaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Familia familia, CancellationToken ct = default)
    {
        await _dbContext.Familias.AddAsync(familia, ct);
    }

    public Task<Familia?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        return _dbContext.Familias
            .Include(f => f.Membros)
            .Include(f => f.Documentos)
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public Task<Familia?> ObterPorMembroIdAsync(Guid membroId, CancellationToken ct = default)
    {
        return _dbContext.Familias
            .Include(f => f.Membros)
            .Include(f => f.Documentos)
            .FirstOrDefaultAsync(f => f.Membros.Any(m => m.Id == membroId), ct);
    }

    public async Task<(IReadOnlyList<Familia> Itens, int Total)> ListarAsync(
        FamiliaStatus? status,
        string? nome,
        int? numeroMembros,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _dbContext.Familias
            .Include(f => f.Membros)
            .Include(f => f.Documentos)
            .Where(f => !f.Excluida)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(f => f.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(f => f.Membros.Any(m => EF.Functions.ILike(m.Nome, $"%{nome}%")));
        }

        if (numeroMembros.HasValue)
        {
            query = query.Where(f => f.Membros.Count == numeroMembros.Value);
        }

        var total = await query.CountAsync(ct);
        var itens = await query
            .OrderBy(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (itens, total);
    }

    public Task<bool> ExisteCpfEmFamiliaAtivaAsync(string cpf, Guid? membroIdIgnorar, CancellationToken ct = default)
    {
        return _dbContext.Membros.AnyAsync(
            m => m.Cpf == cpf && !m.FamiliaExcluida && (membroIdIgnorar == null || m.Id != membroIdIgnorar),
            ct);
    }

    public async Task AdicionarStatusHistoricoAsync(FamiliaStatusHistorico historico, CancellationToken ct = default)
    {
        await _dbContext.FamiliaStatusHistoricos.AddAsync(historico, ct);
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
