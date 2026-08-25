using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Unidades.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Unidades;

public class UnidadeService
{
    private static readonly FamiliaStatus[] StatusElegiveisParaAtribuicao =
    [
        FamiliaStatus.Aprovada,
        FamiliaStatus.UnidadeAtribuida,
        FamiliaStatus.EmConstrucao,
        FamiliaStatus.Finalizada,
    ];

    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IFamiliaRepository _familiaRepository;

    public UnidadeService(IUnidadeRepository unidadeRepository, IFamiliaRepository familiaRepository)
    {
        _unidadeRepository = unidadeRepository;
        _familiaRepository = familiaRepository;
    }

    public async Task<UnidadeResponse> CreateUnidadeAsync(UnidadeCommand command, CancellationToken ct = default)
    {
        if (await _unidadeRepository.ExisteIdentificadorAsync(command.Identificador, ct))
        {
            throw new ConflictException($"Já existe uma unidade cadastrada com o identificador {command.Identificador}.");
        }

        var unidade = new UnidadeHabitacional
        {
            Id = Guid.NewGuid(),
            Identificador = command.Identificador,
            Metragem = command.Metragem,
            LocalizacaoTerreno = command.LocalizacaoTerreno,
            Status = UnidadeStatus.Livre,
        };

        await _unidadeRepository.AdicionarAsync(unidade, ct);
        await _unidadeRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(unidade);
    }

    public async Task<List<UnidadeResponse>> ListUnidadesAsync(UnidadeStatus? status, CancellationToken ct = default)
    {
        var unidades = await _unidadeRepository.ListarAsync(status, ct);
        return unidades.Select(MapToResponse).ToList();
    }

    public async Task<UnidadeResponse> AtribuirUnidadeAsync(Guid unidadeId, UnidadeAtribuicaoCommand command, CancellationToken ct = default)
    {
        var unidade = await _unidadeRepository.ObterPorIdAsync(unidadeId, ct)
            ?? throw new NotFoundException("Unidade habitacional não encontrada.");

        if (unidade.Status != UnidadeStatus.Livre)
        {
            throw new ConflictException("Unidade já está reservada ou ocupada por outra família.");
        }

        var familia = await _familiaRepository.ObterPorIdAsync(command.FamiliaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        if (!StatusElegiveisParaAtribuicao.Contains(familia.Status))
        {
            throw new ConflictException("A família precisa estar com status Aprovada ou posterior para receber uma unidade.");
        }

        unidade.Status = UnidadeStatus.Reservada;
        unidade.FamiliaId = familia.Id;

        if (familia.Status == FamiliaStatus.Aprovada)
        {
            familia.Status = FamiliaStatus.UnidadeAtribuida;
            familia.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await _unidadeRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(unidade);
    }

    internal static UnidadeResponse MapToResponse(UnidadeHabitacional unidade) => new()
    {
        Id = unidade.Id,
        Identificador = unidade.Identificador,
        Metragem = unidade.Metragem,
        LocalizacaoTerreno = unidade.LocalizacaoTerreno,
        Status = unidade.Status,
        FamiliaId = unidade.FamiliaId,
    };
}
