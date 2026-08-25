using ServicoMLBEntidades.Domain.Entities;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IPresencaRepository
{
    Task AdicionarAsync(Presenca presenca, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
