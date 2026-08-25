using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IMutiraoEscalaRepository
{
    Task AdicionarAsync(MutiraoEscala escala, CancellationToken ct = default);

    Task<MutiraoEscala?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

    Task<List<MutiraoEscala>> ListarAsync(CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
