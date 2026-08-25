using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IOcorrenciaRepository
{
    Task AdicionarAsync(Ocorrencia ocorrencia, CancellationToken ct = default);

    Task<List<Ocorrencia>> ListarAsync(Guid? etapaObraId, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
