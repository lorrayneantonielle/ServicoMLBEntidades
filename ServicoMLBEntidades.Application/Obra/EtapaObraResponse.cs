namespace ServicoMLBEntidades.Application.Obra;

public class EtapaObraResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public decimal PercentualConclusao { get; set; }
}
