using Microsoft.EntityFrameworkCore;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Infrastructure.Persistence;

namespace ServicoMLBEntidades.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(RefreshToken refreshToken, CancellationToken ct = default)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, ct);
    }

    public Task<RefreshToken?> ObterPorTokenHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
    }

    public async Task RevogarTodosAtivosPorUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var agora = DateTimeOffset.UtcNow;
        var tokensAtivos = await _dbContext.RefreshTokens
            .Where(x => x.UsuarioId == usuarioId && x.RevokedAt == null && x.ExpiresAt > agora)
            .ToListAsync(ct);

        foreach (var token in tokensAtivos)
        {
            token.RevokedAt = agora;
        }
    }

    public Task SalvarAlteracoesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
