using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Mutirao;

public class MutiraoEscalaResponse
{
    public Guid Id { get; set; }
    public DateOnly Data { get; set; }
    public Turno Turno { get; set; }
    public int VagasTotais { get; set; }
    public int PontuacaoPorPresenca { get; set; }
    public int VagasDisponiveis { get; set; }
}
