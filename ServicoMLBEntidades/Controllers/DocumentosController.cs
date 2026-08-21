using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Documentos;
using ServicoMLBEntidades.Application.Documentos.Commands;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/documentos")]
[Authorize]
public class DocumentosController : ControllerBase
{
    private readonly DocumentoService _documentoService;
    private readonly IValidator<DocumentoCommand> _validator;

    public DocumentosController(DocumentoService documentoService, IValidator<DocumentoCommand> validator)
    {
        _documentoService = documentoService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DocumentoResponse>>> ListDocumentosPorFamilia([FromQuery] Guid familiaId, CancellationToken ct)
    {
        var documentos = await _documentoService.ListDocumentosPorFamiliaAsync(familiaId, ct);
        return Ok(documentos);
    }

    [HttpPost]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    [RequestSizeLimit(10_485_760)]
    public async Task<ActionResult<DocumentoResponse>> UploadDocumento(
        [FromForm] Guid familiaId,
        [FromForm] DocumentoTipo tipo,
        IFormFile arquivo,
        CancellationToken ct)
    {
        await using var stream = arquivo.OpenReadStream();

        var command = new DocumentoCommand
        {
            FamiliaId = familiaId,
            Tipo = tipo,
            NomeArquivo = arquivo.FileName,
            MimeType = arquivo.ContentType,
            TamanhoBytes = arquivo.Length,
            Conteudo = stream,
        };

        await _validator.ValidateAndThrowAsync(command, ct);
        var resposta = await _documentoService.UploadDocumentoAsync(command, ct);

        return StatusCode(StatusCodes.Status201Created, resposta);
    }
}
