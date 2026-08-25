namespace ServicoMLBEntidades.Domain.Entities;

public class EtapaObra
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public decimal PercentualConclusao { get; set; }

    public List<Medicao> Medicoes { get; set; } = [];
    public List<Ocorrencia> Ocorrencias { get; set; } = [];
}
