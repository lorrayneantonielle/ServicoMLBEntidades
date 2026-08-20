using ServicoMLBEntidades.Domain.Models;

namespace ServicoMLBEntidades.Domain.Services;

public interface IIdentityService
{
    Task<UsuarioAutenticado?> ValidarCredenciaisAsync(string email, string senha, CancellationToken ct = default);

    Task<UsuarioAutenticado?> ObterPorIdAsync(Guid usuarioId, CancellationToken ct = default);
}
