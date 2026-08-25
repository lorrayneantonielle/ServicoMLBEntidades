namespace ServicoMLBEntidades.Application.Obra;

public class OcorrenciaResponse
{
    public Guid Id { get; set; }
    public Guid EtapaObraId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
}
