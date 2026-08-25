namespace ServicoMLBEntidades.Application.Public;

public class PublicStatusResponse
{
    public string NomeEmpreendimento { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PercentualConclusaoGeral { get; set; }
}
