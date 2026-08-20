using Microsoft.AspNetCore.Identity;

namespace ServicoMLBEntidades.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string NomeCompleto { get; set; } = string.Empty;
}
