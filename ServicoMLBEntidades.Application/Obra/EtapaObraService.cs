using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Obra.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Obra;

public class EtapaObraService
{
    private readonly IEtapaObraRepository _etapaObraRepository;

    public EtapaObraService(IEtapaObraRepository etapaObraRepository)
    {
        _etapaObraRepository = etapaObraRepository;
    }

    public async Task<EtapaObraResponse> CreateEtapaAsync(EtapaObraCommand command, CancellationToken ct = default)
    {
        if (await _etapaObraRepository.ExisteOrdemAsync(command.Ordem, ct))
        {
            throw new ConflictException($"Já existe uma etapa cadastrada com a ordem {command.Ordem}.");
        }

        var etapa = new EtapaObra
        {
            Id = Guid.NewGuid(),
            Nome = command.Nome,
            Ordem = command.Ordem,
            PercentualConclusao = 0,
        };

        await _etapaObraRepository.AdicionarAsync(etapa, ct);
        await _etapaObraRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(etapa);
    }

    public async Task<EtapaObraResponse> UpdatePercentualAsync(Guid id, EtapaPercentualUpdateCommand command, CancellationToken ct = default)
    {
        var etapa = await _etapaObraRepository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException("Etapa da obra não encontrada.");

        etapa.PercentualConclusao = command.PercentualConclusao;

        await _etapaObraRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(etapa);
    }

    public async Task<List<EtapaObraResponse>> ListEtapasAsync(CancellationToken ct = default)
    {
        var etapas = await _etapaObraRepository.ListarAsync(ct);
        return etapas.Select(MapToResponse).ToList();
    }

    internal static EtapaObraResponse MapToResponse(EtapaObra etapa) => new()
    {
        Id = etapa.Id,
        Nome = etapa.Nome,
        Ordem = etapa.Ordem,
        PercentualConclusao = etapa.PercentualConclusao,
    };
}
