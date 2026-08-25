using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IEtapaObraRepository
{
    Task AdicionarAsync(EtapaObra etapa, CancellationToken ct = default);

    Task<EtapaObra?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

    Task<List<EtapaObra>> ListarAsync(CancellationToken ct = default);

    Task<bool> ExisteOrdemAsync(int ordem, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
