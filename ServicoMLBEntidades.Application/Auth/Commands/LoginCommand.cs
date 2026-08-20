namespace ServicoMLBEntidades.Application.Auth.Commands;

public class LoginCommand
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
