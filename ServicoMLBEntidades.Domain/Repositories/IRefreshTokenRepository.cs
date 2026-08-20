using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task AdicionarAsync(RefreshToken refreshToken, CancellationToken ct = default);

    Task<RefreshToken?> ObterPorTokenHashAsync(string tokenHash, CancellationToken ct = default);

    Task RevogarTodosAtivosPorUsuarioAsync(Guid usuarioId, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
