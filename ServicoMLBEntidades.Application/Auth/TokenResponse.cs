namespace ServicoMLBEntidades.Application.Auth;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}
