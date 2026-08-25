using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class MutiraoEscala
{
    public Guid Id { get; set; }
    public DateOnly Data { get; set; }
    public Turno Turno { get; set; }
    public int VagasTotais { get; set; }
    public int PontuacaoPorPresenca { get; set; }

    public List<Presenca> Presencas { get; set; } = [];
}
