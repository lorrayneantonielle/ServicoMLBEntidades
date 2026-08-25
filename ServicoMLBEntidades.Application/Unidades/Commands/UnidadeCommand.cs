namespace ServicoMLBEntidades.Application.Unidades.Commands;

public class UnidadeCommand
{
    public string Identificador { get; set; } = string.Empty;
    public decimal Metragem { get; set; }
    public string LocalizacaoTerreno { get; set; } = string.Empty;
}
