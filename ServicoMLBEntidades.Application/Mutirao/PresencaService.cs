using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Mutirao.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Mutirao;

public class PresencaService
{
    private readonly IMutiraoEscalaRepository _mutiraoEscalaRepository;
    private readonly IFamiliaRepository _familiaRepository;
    private readonly IPresencaRepository _presencaRepository;

    public PresencaService(
        IMutiraoEscalaRepository mutiraoEscalaRepository,
        IFamiliaRepository familiaRepository,
        IPresencaRepository presencaRepository)
    {
        _mutiraoEscalaRepository = mutiraoEscalaRepository;
        _familiaRepository = familiaRepository;
        _presencaRepository = presencaRepository;
    }

    public async Task<PresencaResponse> RegistrarPresencaAsync(PresencaCommand command, CancellationToken ct = default)
    {
        var escala = await _mutiraoEscalaRepository.ObterPorIdAsync(command.MutiraoEscalaId, ct)
            ?? throw new NotFoundException("Escala de mutirão não encontrada.");

        var familia = await _familiaRepository.ObterPorIdAsync(command.FamiliaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        if (escala.Presencas.Any(p => p.FamiliaId == familia.Id))
        {
            throw new ConflictException("A família já possui presença registrada nesta escala.");
        }

        if (escala.Presencas.Count >= escala.VagasTotais)
        {
            throw new ConflictException("Vagas esgotadas para esta escala de mutirão.");
        }

        var presenca = new Presenca
        {
            Id = Guid.NewGuid(),
            MutiraoEscalaId = escala.Id,
            FamiliaId = familia.Id,
            DataRegistro = DateTimeOffset.UtcNow,
            PontuacaoConcedida = escala.PontuacaoPorPresenca,
        };

        familia.PontuacaoAcumulada += escala.PontuacaoPorPresenca;
        familia.UpdatedAt = DateTimeOffset.UtcNow;

        await _presencaRepository.AdicionarAsync(presenca, ct);
        await _presencaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(presenca);
    }

    internal static PresencaResponse MapToResponse(Presenca presenca) => new()
    {
        Id = presenca.Id,
        MutiraoEscalaId = presenca.MutiraoEscalaId,
        FamiliaId = presenca.FamiliaId,
        DataRegistro = presenca.DataRegistro,
        PontuacaoConcedida = presenca.PontuacaoConcedida,
    };
}
