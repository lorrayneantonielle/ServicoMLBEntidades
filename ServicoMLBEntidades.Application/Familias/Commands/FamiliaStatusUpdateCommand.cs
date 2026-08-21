using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Familias.Commands;

public class FamiliaStatusUpdateCommand
{
    public FamiliaStatus NovoStatus { get; set; }
    public string? Motivo { get; set; }
}
