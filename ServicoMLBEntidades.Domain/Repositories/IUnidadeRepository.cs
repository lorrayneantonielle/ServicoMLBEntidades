using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IUnidadeRepository
{
    Task AdicionarAsync(UnidadeHabitacional unidade, CancellationToken ct = default);

    Task<UnidadeHabitacional?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

    Task<UnidadeHabitacional?> ObterPorFamiliaIdAsync(Guid familiaId, CancellationToken ct = default);

    Task<List<UnidadeHabitacional>> ListarAsync(UnidadeStatus? status, CancellationToken ct = default);

    Task<bool> ExisteIdentificadorAsync(string identificador, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
