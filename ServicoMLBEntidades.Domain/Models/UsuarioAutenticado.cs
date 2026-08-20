namespace ServicoMLBEntidades.Domain.Models;

public class UsuarioAutenticado
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string NomeCompleto { get; init; } = string.Empty;
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
}
