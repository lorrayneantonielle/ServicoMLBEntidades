using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IMedicaoRepository
{
    Task AdicionarAsync(Medicao medicao, CancellationToken ct = default);

    Task<List<Medicao>> ListarAsync(Guid? etapaObraId, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
