using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Repositories;

public interface IFamiliaRepository
{
    Task AdicionarAsync(Familia familia, CancellationToken ct = default);

    Task<Familia?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

    Task<Familia?> ObterPorMembroIdAsync(Guid membroId, CancellationToken ct = default);

    Task<(IReadOnlyList<Familia> Itens, int Total)> ListarAsync(
        FamiliaStatus? status,
        string? nome,
        int? numeroMembros,
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<bool> ExisteCpfEmFamiliaAtivaAsync(string cpf, Guid? membroIdIgnorar, CancellationToken ct = default);

    Task AdicionarStatusHistoricoAsync(FamiliaStatusHistorico historico, CancellationToken ct = default);

    Task SalvarAlteracoesAsync(CancellationToken ct = default);
}
