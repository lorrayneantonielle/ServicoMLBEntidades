using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Mutirao.Commands;

public class MutiraoEscalaCommand
{
    public DateOnly Data { get; set; }
    public Turno Turno { get; set; }
    public int VagasTotais { get; set; }
    public int PontuacaoPorPresenca { get; set; }
}
