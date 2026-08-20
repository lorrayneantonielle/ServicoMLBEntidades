using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServicoMLBEntidades.Domain.Auth;
using ServicoMLBEntidades.Domain.Models;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly double _accessTokenExpirationHours;
    private readonly double _refreshTokenExpirationDays;

    public JwtTokenService(IConfiguration configuration)
    {
        _secret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Configuração 'Jwt:Secret' não encontrada.");
        _issuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Configuração 'Jwt:Issuer' não encontrada.");
        _accessTokenExpirationHours = configuration.GetValue<double?>("Jwt:AccessTokenExpirationHours") ?? 8;
        _refreshTokenExpirationDays = configuration.GetValue<double?>("Jwt:RefreshTokenExpirationDays") ?? 7;
    }

    public (string Token, DateTimeOffset ExpiresAt) GerarAccessToken(UsuarioAutenticado usuario)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddHours(_accessTokenExpirationHours);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new("nomeCompleto", usuario.NomeCompleto),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(usuario.Roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _issuer,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public (string Token, DateTimeOffset ExpiresAt) GerarRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes);
        var expiresAt = DateTimeOffset.UtcNow.AddDays(_refreshTokenExpirationDays);
        return (token, expiresAt);
    }

    public string CalcularHash(string refreshToken)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
