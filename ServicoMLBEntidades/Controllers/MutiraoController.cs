using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Mutirao;
using ServicoMLBEntidades.Application.Mutirao.Commands;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/mutirao")]
[Authorize]
public class MutiraoController : ControllerBase
{
    private readonly MutiraoService _mutiraoService;
    private readonly PresencaService _presencaService;
    private readonly IValidator<MutiraoEscalaCommand> _escalaValidator;
    private readonly IValidator<PresencaCommand> _presencaValidator;

    public MutiraoController(
        MutiraoService mutiraoService,
        PresencaService presencaService,
        IValidator<MutiraoEscalaCommand> escalaValidator,
        IValidator<PresencaCommand> presencaValidator)
    {
        _mutiraoService = mutiraoService;
        _presencaService = presencaService;
        _escalaValidator = escalaValidator;
        _presencaValidator = presencaValidator;
    }

    [HttpGet("escalas")]
    public async Task<ActionResult<List<MutiraoEscalaResponse>>> ListEscalas(CancellationToken ct)
    {
        var escalas = await _mutiraoService.ListEscalasAsync(ct);
        return Ok(escalas);
    }

    [HttpPost("escalas")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<MutiraoEscalaResponse>> CreateEscala(MutiraoEscalaCommand command, CancellationToken ct)
    {
        await _escalaValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _mutiraoService.CreateEscalaAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpPost("presencas")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<PresencaResponse>> RegistrarPresenca(PresencaCommand command, CancellationToken ct)
    {
        await _presencaValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _presencaService.RegistrarPresencaAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpGet("pontuacao")]
    public async Task<ActionResult<List<PontuacaoFamiliaResponse>>> GetPontuacaoPorFamilia(
        [FromQuery] Guid? familiaId, [FromQuery] bool? baixaParticipacao, CancellationToken ct)
    {
        var resposta = await _mutiraoService.GetPontuacaoPorFamiliaAsync(familiaId, baixaParticipacao, ct);
        return Ok(resposta);
    }
}
