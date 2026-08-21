using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Documentos;
using ServicoMLBEntidades.Application.Familias.Commands;
using ServicoMLBEntidades.Application.Membros;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Familias;

public class FamiliaService
{
    private readonly IFamiliaRepository _familiaRepository;

    public FamiliaService(IFamiliaRepository familiaRepository)
    {
        _familiaRepository = familiaRepository;
    }

    public async Task<FamiliaResponse> CreateFamiliaAsync(FamiliaCreateCommand command, CancellationToken ct = default)
    {
        foreach (var membro in command.Membros)
        {
            if (await _familiaRepository.ExisteCpfEmFamiliaAtivaAsync(membro.Cpf, null, ct))
            {
                throw new ConflictException($"CPF {membro.Cpf} já está vinculado a outra família ativa.");
            }
        }

        var agora = DateTimeOffset.UtcNow;
        var familia = new Familia
        {
            Id = Guid.NewGuid(),
            RendaFamiliar = command.RendaFamiliar,
            SituacaoVulnerabilidade = command.SituacaoVulnerabilidade,
            Status = FamiliaStatus.PreCadastro,
            CreatedAt = agora,
            UpdatedAt = agora,
        };

        foreach (var membroCommand in command.Membros)
        {
            familia.Membros.Add(new Membro
            {
                Id = Guid.NewGuid(),
                FamiliaId = familia.Id,
                Nome = membroCommand.Nome,
                DataNascimento = membroCommand.DataNascimento,
                Vinculo = membroCommand.Vinculo,
                Cpf = membroCommand.Cpf,
            });
        }

        await _familiaRepository.AdicionarAsync(familia, ct);
        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(familia);
    }

    public async Task<FamiliaResponse> UpdateFamiliaAsync(Guid id, FamiliaUpdateCommand command, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        familia.RendaFamiliar = command.RendaFamiliar;
        familia.SituacaoVulnerabilidade = command.SituacaoVulnerabilidade;
        familia.UpdatedAt = DateTimeOffset.UtcNow;

        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(familia);
    }

    public async Task<FamiliaResponse> GetFamiliaAsync(Guid id, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        return MapToResponse(familia);
    }

    public async Task<(List<FamiliaResponse> Itens, int Total)> ListFamiliasAsync(
        FamiliaStatus? status,
        string? nome,
        int? numeroMembros,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var (itens, total) = await _familiaRepository.ListarAsync(status, nome, numeroMembros, page, pageSize, ct);
        return (itens.Select(MapToResponse).ToList(), total);
    }

    internal static FamiliaResponse MapToResponse(Familia familia)
    {
        return new FamiliaResponse
        {
            Id = familia.Id,
            RendaFamiliar = familia.RendaFamiliar,
            SituacaoVulnerabilidade = familia.SituacaoVulnerabilidade,
            Status = familia.Status,
            PontuacaoAcumulada = familia.PontuacaoAcumulada,
            Membros = familia.Membros.Select(MembroService.MapToResponse).ToList(),
            Documentos = familia.Documentos.Select(DocumentoService.MapToResponse).ToList(),
            UnidadeHabitacionalId = null,
        };
    }
}
