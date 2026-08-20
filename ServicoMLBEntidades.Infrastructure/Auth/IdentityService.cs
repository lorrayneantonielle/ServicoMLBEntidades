using Microsoft.AspNetCore.Identity;
using ServicoMLBEntidades.Domain.Models;
using ServicoMLBEntidades.Domain.Services;
using ServicoMLBEntidades.Infrastructure.Identity;

namespace ServicoMLBEntidades.Infrastructure.Auth;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UsuarioAutenticado?> ValidarCredenciaisAsync(string email, string senha, CancellationToken ct = default)
    {
        var usuario = await _userManager.FindByEmailAsync(email);
        if (usuario is null)
        {
            return null;
        }

        var senhaValida = await _userManager.CheckPasswordAsync(usuario, senha);
        if (!senhaValida)
        {
            return null;
        }

        return await MontarUsuarioAutenticadoAsync(usuario);
    }

    public async Task<UsuarioAutenticado?> ObterPorIdAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var usuario = await _userManager.FindByIdAsync(usuarioId.ToString());
        return usuario is null ? null : await MontarUsuarioAutenticadoAsync(usuario);
    }

    private async Task<UsuarioAutenticado> MontarUsuarioAutenticadoAsync(ApplicationUser usuario)
    {
        var roles = await _userManager.GetRolesAsync(usuario);

        return new UsuarioAutenticado
        {
            Id = usuario.Id,
            Email = usuario.Email ?? string.Empty,
            NomeCompleto = usuario.NomeCompleto,
            Roles = roles.ToList(),
        };
    }
}
