namespace ServicoMLBEntidades.Domain.Entities;

public class Ocorrencia
{
    public Guid Id { get; set; }
    public Guid EtapaObraId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public Guid RegistradoPorUsuarioId { get; set; }

    public EtapaObra? EtapaObra { get; set; }
}
