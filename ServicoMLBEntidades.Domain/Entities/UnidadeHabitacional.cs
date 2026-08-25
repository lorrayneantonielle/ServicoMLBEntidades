using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class UnidadeHabitacional
{
    public Guid Id { get; set; }
    public string Identificador { get; set; } = string.Empty;
    public decimal Metragem { get; set; }
    public string LocalizacaoTerreno { get; set; } = string.Empty;
    public UnidadeStatus Status { get; set; } = UnidadeStatus.Livre;
    public Guid? FamiliaId { get; set; }

    public Familia? Familia { get; set; }
}
