using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Membros;
using ServicoMLBEntidades.Application.Membros.Commands;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/membros")]
[Authorize]
public class MembrosController : ControllerBase
{
    private readonly MembroService _membroService;
    private readonly IValidator<MembroCommand> _validator;

    public MembrosController(MembroService membroService, IValidator<MembroCommand> validator)
    {
        _membroService = membroService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<List<MembroResponse>>> ListMembrosPorFamilia([FromQuery] Guid familiaId, CancellationToken ct)
    {
        var membros = await _membroService.ListMembrosPorFamiliaAsync(familiaId, ct);
        return Ok(membros);
    }

    [HttpPost]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<ActionResult<MembroResponse>> CreateMembro(MembroCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);
        var resposta = await _membroService.CreateMembroAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<ActionResult<MembroResponse>> UpdateMembro(Guid id, MembroCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);
        var resposta = await _membroService.UpdateMembroAsync(id, command, ct);
        return Ok(resposta);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "AdminGeral,AssistenteSocial")]
    public async Task<IActionResult> DeleteMembro(Guid id, CancellationToken ct)
    {
        await _membroService.DeleteMembroAsync(id, ct);
        return NoContent();
    }
}
