using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Familias.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;

namespace ServicoMLBEntidades.Application.Familias;

public class FamiliaStatusService
{
    private static readonly FamiliaStatus[] Sequencia =
    [
        FamiliaStatus.PreCadastro,
        FamiliaStatus.EmAnalise,
        FamiliaStatus.Aprovada,
        FamiliaStatus.UnidadeAtribuida,
        FamiliaStatus.EmConstrucao,
        FamiliaStatus.Finalizada,
    ];

    private static readonly DocumentoTipo[] TiposObrigatorios =
    [
        DocumentoTipo.RG,
        DocumentoTipo.CPF,
        DocumentoTipo.ComprovanteRenda,
        DocumentoTipo.Certidao,
    ];

    private readonly IFamiliaRepository _familiaRepository;

    public FamiliaStatusService(IFamiliaRepository familiaRepository)
    {
        _familiaRepository = familiaRepository;
    }

    public async Task<FamiliaResponse> UpdateStatusAsync(
        Guid familiaId, Guid usuarioId, FamiliaStatusUpdateCommand command, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(familiaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        var indiceAtual = Array.IndexOf(Sequencia, familia.Status);
        var indiceNovo = Array.IndexOf(Sequencia, command.NovoStatus);

        if (indiceNovo == indiceAtual)
        {
            throw new BusinessRuleException("O novo status deve ser diferente do status atual.");
        }

        if (indiceNovo > indiceAtual)
        {
            if (indiceNovo != indiceAtual + 1)
            {
                throw new BusinessRuleException("Avanço de status deve seguir a sequência do workflow, um passo por vez.");
            }

            var documentosPendentes = TiposObrigatorios
                .Where(tipo => familia.Documentos.FirstOrDefault(d => d.Tipo == tipo)?.Status != DocumentoStatus.Validado)
                .ToList();

            if (documentosPendentes.Count > 0)
            {
                throw new BusinessRuleException(
                    $"Avanço bloqueado: documentação obrigatória pendente ({string.Join(", ", documentosPendentes)}).");
            }
        }
        else if (string.IsNullOrWhiteSpace(command.Motivo))
        {
            throw new BusinessRuleException("Motivo é obrigatório ao reverter o status da família.");
        }

        var statusAnterior = familia.Status;
        familia.Status = command.NovoStatus;
        familia.UpdatedAt = DateTimeOffset.UtcNow;

        await _familiaRepository.AdicionarStatusHistoricoAsync(new FamiliaStatusHistorico
        {
            Id = Guid.NewGuid(),
            FamiliaId = familia.Id,
            StatusAnterior = statusAnterior,
            StatusNovo = command.NovoStatus,
            Motivo = command.Motivo,
            UsuarioId = usuarioId,
            DataTransicao = DateTimeOffset.UtcNow,
        }, ct);

        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return FamiliaService.MapToResponse(familia);
    }
}
