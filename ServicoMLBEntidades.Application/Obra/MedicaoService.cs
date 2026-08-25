using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Obra.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Obra;

public class MedicaoService
{
    private readonly IMedicaoRepository _medicaoRepository;
    private readonly IEtapaObraRepository _etapaObraRepository;

    public MedicaoService(IMedicaoRepository medicaoRepository, IEtapaObraRepository etapaObraRepository)
    {
        _medicaoRepository = medicaoRepository;
        _etapaObraRepository = etapaObraRepository;
    }

    public async Task<MedicaoResponse> CreateMedicaoAsync(MedicaoCommand command, Guid usuarioId, CancellationToken ct = default)
    {
        var etapa = await _etapaObraRepository.ObterPorIdAsync(command.EtapaObraId, ct)
            ?? throw new NotFoundException("Etapa da obra não encontrada.");

        var medicao = new Medicao
        {
            Id = Guid.NewGuid(),
            EtapaObraId = etapa.Id,
            Data = command.Data,
            StatusAprovacao = command.StatusAprovacao,
            RecursosLiberados = command.RecursosLiberados,
            Observacao = command.Observacao,
            RegistradoPorUsuarioId = usuarioId,
            EtapaObra = etapa,
        };

        await _medicaoRepository.AdicionarAsync(medicao, ct);
        await _medicaoRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(medicao);
    }

    public async Task<List<MedicaoResponse>> ListMedicoesAsync(Guid? etapaObraId, CancellationToken ct = default)
    {
        var medicoes = await _medicaoRepository.ListarAsync(etapaObraId, ct);
        return medicoes.Select(MapToResponse).ToList();
    }

    internal static MedicaoResponse MapToResponse(Medicao medicao) => new()
    {
        Id = medicao.Id,
        EtapaObraId = medicao.EtapaObraId,
        Data = medicao.Data,
        StatusAprovacao = medicao.StatusAprovacao,
        RecursosLiberados = medicao.RecursosLiberados,
        Observacao = medicao.Observacao,
        Divergente = medicao.StatusAprovacao == StatusAprovacao.Aprovada
            && medicao.EtapaObra is not null
            && medicao.EtapaObra.PercentualConclusao < 100,
    };
}
