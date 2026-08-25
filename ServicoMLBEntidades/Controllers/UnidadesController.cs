using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Unidades;
using ServicoMLBEntidades.Application.Unidades.Commands;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/unidades")]
[Authorize]
public class UnidadesController : ControllerBase
{
    private readonly UnidadeService _unidadeService;
    private readonly IValidator<UnidadeCommand> _unidadeValidator;
    private readonly IValidator<UnidadeAtribuicaoCommand> _atribuicaoValidator;

    public UnidadesController(
        UnidadeService unidadeService,
        IValidator<UnidadeCommand> unidadeValidator,
        IValidator<UnidadeAtribuicaoCommand> atribuicaoValidator)
    {
        _unidadeService = unidadeService;
        _unidadeValidator = unidadeValidator;
        _atribuicaoValidator = atribuicaoValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<UnidadeResponse>>> ListUnidades([FromQuery] UnidadeStatus? status, CancellationToken ct)
    {
        var unidades = await _unidadeService.ListUnidadesAsync(status, ct);
        return Ok(unidades);
    }

    [HttpPost]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<UnidadeResponse>> CreateUnidade(UnidadeCommand command, CancellationToken ct)
    {
        await _unidadeValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _unidadeService.CreateUnidadeAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpPut("{id:guid}/atribuicao")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<UnidadeResponse>> AtribuirUnidade(Guid id, UnidadeAtribuicaoCommand command, CancellationToken ct)
    {
        await _atribuicaoValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _unidadeService.AtribuirUnidadeAsync(id, command, ct);
        return Ok(resposta);
    }
}
