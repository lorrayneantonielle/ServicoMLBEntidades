using ServicoMLBEntidades.Domain.Models;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IPontuacaoQueryService
{
    Task<List<PontuacaoFamiliaResultado>> ObterPontuacaoPorFamiliaAsync(Guid? familiaId, CancellationToken ct = default);
}
