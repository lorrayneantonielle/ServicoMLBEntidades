using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Unidades;

public class UnidadeResponse
{
    public Guid Id { get; set; }
    public string Identificador { get; set; } = string.Empty;
    public decimal Metragem { get; set; }
    public string LocalizacaoTerreno { get; set; } = string.Empty;
    public UnidadeStatus Status { get; set; }
    public Guid? FamiliaId { get; set; }
}
