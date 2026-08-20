using ServicoMLBEntidades.Domain.Models;

namespace ServicoMLBEntidades.Domain.Services;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) GerarAccessToken(UsuarioAutenticado usuario);

    (string Token, DateTimeOffset ExpiresAt) GerarRefreshToken();

    string CalcularHash(string refreshToken);
}
