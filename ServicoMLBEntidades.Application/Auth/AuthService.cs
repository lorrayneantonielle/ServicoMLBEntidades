using ServicoMLBEntidades.Application.Auth.Commands;
using ServicoMLBEntidades.Application.Common.Exceptions;
using ServicoMLBEntidades.Domain.Entities;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Application.Auth;

public class AuthService
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<TokenResponse> LoginAsync(LoginCommand command, CancellationToken ct = default)
    {
        var usuario = await _identityService.ValidarCredenciaisAsync(command.Email, command.Senha, ct)
            ?? throw new UnauthorizedException("Email ou senha inválidos.");

        return await EmitirTokensAsync(usuario.Id, ct);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var hash = _jwtTokenService.CalcularHash(command.RefreshToken);
        var tokenArmazenado = await _refreshTokenRepository.ObterPorTokenHashAsync(hash, ct)
            ?? throw new UnauthorizedException("Refresh token inválido.");

        if (!tokenArmazenado.IsAtivo)
        {
            throw new UnauthorizedException("Refresh token expirado ou revogado.");
        }

        tokenArmazenado.RevokedAt = DateTimeOffset.UtcNow;

        var resposta = await EmitirTokensAsync(tokenArmazenado.UsuarioId, ct);

        await _refreshTokenRepository.SalvarAlteracoesAsync(ct);

        return resposta;
    }

    public async Task LogoutAsync(Guid usuarioId, CancellationToken ct = default)
    {
        await _refreshTokenRepository.RevogarTodosAtivosPorUsuarioAsync(usuarioId, ct);
        await _refreshTokenRepository.SalvarAlteracoesAsync(ct);
    }

    private async Task<TokenResponse> EmitirTokensAsync(Guid usuarioId, CancellationToken ct)
    {
        var usuario = await _identityService.ObterPorIdAsync(usuarioId, ct)
            ?? throw new UnauthorizedException("Usuário não encontrado.");

        var (accessToken, accessTokenExpiresAt) = _jwtTokenService.GerarAccessToken(usuario);
        var (refreshToken, refreshTokenExpiresAt) = _jwtTokenService.GerarRefreshToken();

        await _refreshTokenRepository.AdicionarAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuario.Id,
            TokenHash = _jwtTokenService.CalcularHash(refreshToken),
            ExpiresAt = refreshTokenExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow,
        }, ct);

        await _refreshTokenRepository.SalvarAlteracoesAsync(ct);

        return new TokenResponse
        {
            AccessToken = accessToken,
            ExpiresAt = accessTokenExpiresAt,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        };
    }
}
