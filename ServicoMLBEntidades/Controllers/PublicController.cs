using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Public;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/public")]
[AllowAnonymous]
public class PublicController : ControllerBase
{
    private readonly PublicService _publicService;

    public PublicController(PublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet("status")]
    public async Task<ActionResult<PublicStatusResponse>> GetStatus(CancellationToken ct)
    {
        var resposta = await _publicService.GetStatusAsync(ct);
        return Ok(resposta);
    }

    [HttpGet("etapas")]
    public async Task<ActionResult<List<PublicEtapaResponse>>> GetEtapas(CancellationToken ct)
    {
        var resposta = await _publicService.GetEtapasAsync(ct);
        return Ok(resposta);
    }

    [HttpGet("medicoes")]
    public async Task<ActionResult<List<PublicMedicaoResponse>>> GetMedicoes(CancellationToken ct)
    {
        var resposta = await _publicService.GetMedicoesAprovadasAsync(ct);
        return Ok(resposta);
    }

    [HttpGet("mutiroes")]
    public async Task<ActionResult<List<PublicMutiraoResponse>>> GetMutiroes(CancellationToken ct)
    {
        var resposta = await _publicService.GetProximosMutiroesAsync(ct);
        return Ok(resposta);
    }
}
