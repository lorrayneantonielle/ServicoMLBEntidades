using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Obra.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Obra;

public class OcorrenciaService
{
    private readonly IOcorrenciaRepository _ocorrenciaRepository;
    private readonly IEtapaObraRepository _etapaObraRepository;

    public OcorrenciaService(IOcorrenciaRepository ocorrenciaRepository, IEtapaObraRepository etapaObraRepository)
    {
        _ocorrenciaRepository = ocorrenciaRepository;
        _etapaObraRepository = etapaObraRepository;
    }

    public async Task<OcorrenciaResponse> CreateOcorrenciaAsync(OcorrenciaCommand command, Guid usuarioId, CancellationToken ct = default)
    {
        var etapa = await _etapaObraRepository.ObterPorIdAsync(command.EtapaObraId, ct)
            ?? throw new NotFoundException("Etapa da obra não encontrada.");

        var ocorrencia = new Ocorrencia
        {
            Id = Guid.NewGuid(),
            EtapaObraId = etapa.Id,
            Descricao = command.Descricao,
            Data = command.Data,
            RegistradoPorUsuarioId = usuarioId,
        };

        await _ocorrenciaRepository.AdicionarAsync(ocorrencia, ct);
        await _ocorrenciaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(ocorrencia);
    }

    public async Task<List<OcorrenciaResponse>> ListOcorrenciasAsync(Guid? etapaObraId, CancellationToken ct = default)
    {
        var ocorrencias = await _ocorrenciaRepository.ListarAsync(etapaObraId, ct);
        return ocorrencias.Select(MapToResponse).ToList();
    }

    internal static OcorrenciaResponse MapToResponse(Ocorrencia ocorrencia) => new()
    {
        Id = ocorrencia.Id,
        EtapaObraId = ocorrencia.EtapaObraId,
        Descricao = ocorrencia.Descricao,
        Data = ocorrencia.Data,
    };
}
