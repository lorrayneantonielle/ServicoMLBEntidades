using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Familias;
using ServicoMLBEntidades.Application.Familias.Commands;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/familias")]
[Authorize]
public class FamiliasController : ControllerBase
{
    private readonly FamiliaService _familiaService;
    private readonly FamiliaStatusService _familiaStatusService;
    private readonly IValidator<FamiliaCreateCommand> _createValidator;
    private readonly IValidator<FamiliaUpdateCommand> _updateValidator;
    private readonly IValidator<FamiliaStatusUpdateCommand> _statusValidator;

    public FamiliasController(
        FamiliaService familiaService,
        FamiliaStatusService familiaStatusService,
        IValidator<FamiliaCreateCommand> createValidator,
        IValidator<FamiliaUpdateCommand> updateValidator,
        IValidator<FamiliaStatusUpdateCommand> statusValidator)
    {
        _familiaService = familiaService;
        _familiaStatusService = familiaStatusService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _statusValidator = statusValidator;
    }

    [HttpGet]
    public async Task<ActionResult> ListFamilias(
        [FromQuery] FamiliaStatus? status,
        [FromQuery] string? nome,
        [FromQuery] int? numeroMembros,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var (itens, total) = await _familiaService.ListFamiliasAsync(status, nome, numeroMembros, page, pageSize, ct);
        return Ok(new { items = itens, total });
    }

    [HttpPost]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<ActionResult<FamiliaResponse>> CreateFamilia(FamiliaCreateCommand command, CancellationToken ct)
    {
        await _createValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _familiaService.CreateFamiliaAsync(command, ct);
        return CreatedAtAction(nameof(GetFamilia), new { id = resposta.Id }, resposta);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FamiliaResponse>> GetFamilia(Guid id, CancellationToken ct)
    {
        var resposta = await _familiaService.GetFamiliaAsync(id, ct);
        return Ok(resposta);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<ActionResult<FamiliaResponse>> UpdateFamilia(Guid id, FamiliaUpdateCommand command, CancellationToken ct)
    {
        await _updateValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _familiaService.UpdateFamiliaAsync(id, command, ct);
        return Ok(resposta);
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<ActionResult<FamiliaResponse>> UpdateFamiliaStatus(Guid id, FamiliaStatusUpdateCommand command, CancellationToken ct)
    {
        await _statusValidator.ValidateAndThrowAsync(command, ct);
        var usuarioId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var resposta = await _familiaStatusService.UpdateStatusAsync(id, usuarioId, command, ct);
        return Ok(resposta);
    }
}
