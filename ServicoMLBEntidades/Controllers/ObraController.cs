using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Obra;
using ServicoMLBEntidades.Application.Obra.Commands;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/obra")]
[Authorize]
public class ObraController : ControllerBase
{
    private readonly EtapaObraService _etapaObraService;
    private readonly MedicaoService _medicaoService;
    private readonly OcorrenciaService _ocorrenciaService;
    private readonly IValidator<EtapaObraCommand> _etapaValidator;
    private readonly IValidator<EtapaPercentualUpdateCommand> _percentualValidator;
    private readonly IValidator<MedicaoCommand> _medicaoValidator;
    private readonly IValidator<OcorrenciaCommand> _ocorrenciaValidator;

    public ObraController(
        EtapaObraService etapaObraService,
        MedicaoService medicaoService,
        OcorrenciaService ocorrenciaService,
        IValidator<EtapaObraCommand> etapaValidator,
        IValidator<EtapaPercentualUpdateCommand> percentualValidator,
        IValidator<MedicaoCommand> medicaoValidator,
        IValidator<OcorrenciaCommand> ocorrenciaValidator)
    {
        _etapaObraService = etapaObraService;
        _medicaoService = medicaoService;
        _ocorrenciaService = ocorrenciaService;
        _etapaValidator = etapaValidator;
        _percentualValidator = percentualValidator;
        _medicaoValidator = medicaoValidator;
        _ocorrenciaValidator = ocorrenciaValidator;
    }

    private Guid UsuarioIdAtual => Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

    [HttpGet("etapas")]
    public async Task<ActionResult<List<EtapaObraResponse>>> ListEtapas(CancellationToken ct)
    {
        var etapas = await _etapaObraService.ListEtapasAsync(ct);
        return Ok(etapas);
    }

    [HttpPost("etapas")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<EtapaObraResponse>> CreateEtapa(EtapaObraCommand command, CancellationToken ct)
    {
        await _etapaValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _etapaObraService.CreateEtapaAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpPatch("etapas/{id:guid}")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<EtapaObraResponse>> UpdateEtapaPercentual(Guid id, EtapaPercentualUpdateCommand command, CancellationToken ct)
    {
        await _percentualValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _etapaObraService.UpdatePercentualAsync(id, command, ct);
        return Ok(resposta);
    }

    [HttpGet("medicoes")]
    public async Task<ActionResult<List<MedicaoResponse>>> ListMedicoes([FromQuery] Guid? etapaObraId, CancellationToken ct)
    {
        var medicoes = await _medicaoService.ListMedicoesAsync(etapaObraId, ct);
        return Ok(medicoes);
    }

    [HttpPost("medicoes")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<MedicaoResponse>> CreateMedicao(MedicaoCommand command, CancellationToken ct)
    {
        await _medicaoValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _medicaoService.CreateMedicaoAsync(command, UsuarioIdAtual, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }

    [HttpGet("ocorrencias")]
    public async Task<ActionResult<List<OcorrenciaResponse>>> ListOcorrencias([FromQuery] Guid? etapaObraId, CancellationToken ct)
    {
        var ocorrencias = await _ocorrenciaService.ListOcorrenciasAsync(etapaObraId, ct);
        return Ok(ocorrencias);
    }

    [HttpPost("ocorrencias")]
    [Authorize(Roles = "AdminGeral,TecnicoObra")]
    public async Task<ActionResult<OcorrenciaResponse>> CreateOcorrencia(OcorrenciaCommand command, CancellationToken ct)
    {
        await _ocorrenciaValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _ocorrenciaService.CreateOcorrenciaAsync(command, UsuarioIdAtual, ct);
        return StatusCode(StatusCodes.Status201Created, resposta);
    }
}
