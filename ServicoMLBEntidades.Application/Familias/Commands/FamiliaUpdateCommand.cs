namespace ServicoMLBEntidades.Application.Familias.Commands;

public class FamiliaUpdateCommand
{
    public decimal RendaFamiliar { get; set; }
    public string SituacaoVulnerabilidade { get; set; } = string.Empty;
}
