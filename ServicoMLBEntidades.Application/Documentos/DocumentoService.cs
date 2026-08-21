using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Application.Documentos.Commands;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Application.Documentos;

public class DocumentoService
{
    private readonly IFamiliaRepository _familiaRepository;
    private readonly IDocumentoStorageService _storageService;

    public DocumentoService(IFamiliaRepository familiaRepository, IDocumentoStorageService storageService)
    {
        _familiaRepository = familiaRepository;
        _storageService = storageService;
    }

    public async Task<List<DocumentoResponse>> ListDocumentosPorFamiliaAsync(Guid familiaId, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(familiaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        return familia.Documentos.Select(MapToResponse).ToList();
    }

    public async Task<DocumentoResponse> UploadDocumentoAsync(DocumentoCommand command, CancellationToken ct = default)
    {
        var familia = await _familiaRepository.ObterPorIdAsync(command.FamiliaId, ct)
            ?? throw new NotFoundException("Família não encontrada.");

        var arquivoPath = await _storageService.SalvarAsync(command.Conteudo, command.NomeArquivo, command.MimeType, ct);

        var documento = familia.Documentos.FirstOrDefault(d => d.Tipo == command.Tipo);
        if (documento is null)
        {
            documento = new Documento
            {
                Id = Guid.NewGuid(),
                FamiliaId = familia.Id,
                Tipo = command.Tipo,
            };
            familia.Documentos.Add(documento);
        }

        documento.ArquivoPath = arquivoPath;
        documento.ArquivoMimeType = command.MimeType;
        documento.Status = DocumentoStatus.Validado;
        documento.UpdatedAt = DateTimeOffset.UtcNow;

        await _familiaRepository.SalvarAlteracoesAsync(ct);

        return MapToResponse(documento);
    }

    internal static DocumentoResponse MapToResponse(Documento documento) => new()
    {
        Id = documento.Id,
        FamiliaId = documento.FamiliaId,
        Tipo = documento.Tipo,
        Status = documento.Status,
        ArquivoPath = documento.ArquivoPath,
    };
}
