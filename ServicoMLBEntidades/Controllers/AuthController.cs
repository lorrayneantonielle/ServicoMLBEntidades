using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Auth;
using ServicoMLBEntidades.Application.Auth.Commands;
using FluentValidation;

namespace ServicoMLBEntidades.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IValidator<LoginCommand> _loginValidator;
    private readonly IValidator<RefreshTokenCommand> _refreshTokenValidator;

    public AuthController(
        AuthService authService,
        IValidator<LoginCommand> loginValidator,
        IValidator<RefreshTokenCommand> refreshTokenValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
        _refreshTokenValidator = refreshTokenValidator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Login(LoginCommand command, CancellationToken ct)
    {
        await _loginValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _authService.LoginAsync(command, ct);
        return Ok(resposta);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenCommand command, CancellationToken ct)
    {
        await _refreshTokenValidator.ValidateAndThrowAsync(command, ct);
        var resposta = await _authService.RefreshTokenAsync(command, ct);
        return Ok(resposta);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        await _authService.LogoutAsync(usuarioId, ct);
        return NoContent();
    }
}
