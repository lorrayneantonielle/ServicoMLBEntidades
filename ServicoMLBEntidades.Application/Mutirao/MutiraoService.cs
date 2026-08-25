using ServicoMLBEntidades.Application.Mutirao.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Mutirao;

public class MutiraoService
{
    private readonly IMutiraoEscalaRepository _mutiraoEscalaRepository;
    private readonly IPontuacaoQueryService _pontuacaoQueryService;

    public MutiraoService(IMutiraoEscalaRepository mutiraoEscalaRepository, IPontuacaoQueryService pontuacaoQueryService)
    {
        _mutiraoEscalaRepository = mutiraoEscalaRepository;
        _pontuacaoQueryService = pontuacaoQueryService;
    }

    public async Task<MutiraoEscalaResponse> CreateEscalaAsync(MutiraoEscalaCommand command, CancellationToken ct = default)
    {
        var escala = new MutiraoEscala
        {
            Id = Guid.NewGuid(),
            Data = command.Data,
            Turno = command.Turno,
            VagasTotais = command.VagasTotais,
            PontuacaoPorPresenca = command.PontuacaoPorPresenca,
        };

        await _mutiraoEscalaRepository.AdicionarAsync(escala, ct);
        await _mutiraoEscalaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(escala);
    }

    public async Task<List<MutiraoEscalaResponse>> ListEscalasAsync(CancellationToken ct = default)
    {
        var escalas = await _mutiraoEscalaRepository.ListarAsync(ct);
        return escalas.Select(MapToResponse).ToList();
    }

    public async Task<List<PontuacaoFamiliaResponse>> GetPontuacaoPorFamiliaAsync(
        Guid? familiaId, bool? baixaParticipacao, CancellationToken ct = default)
    {
        var resultados = await _pontuacaoQueryService.ObterPontuacaoPorFamiliaAsync(familiaId, ct);

        return resultados
            .Where(r => baixaParticipacao == null || r.BaixaParticipacao == baixaParticipacao)
            .Select(PontuacaoFamiliaResponse.FromResultado)
            .ToList();
    }

    internal static MutiraoEscalaResponse MapToResponse(MutiraoEscala escala) => new()
    {
        Id = escala.Id,
        Data = escala.Data,
        Turno = escala.Turno,
        VagasTotais = escala.VagasTotais,
        PontuacaoPorPresenca = escala.PontuacaoPorPresenca,
        VagasDisponiveis = escala.VagasTotais - escala.Presencas.Count,
    };
}
