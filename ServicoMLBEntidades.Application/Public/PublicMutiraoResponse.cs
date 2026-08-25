using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Public;

public class PublicMutiraoResponse
{
    public DateOnly Data { get; set; }
    public Turno Turno { get; set; }
    public int VagasDisponiveis { get; set; }
}
