namespace ServicoMLBEntidades.Application.Obra.Commands;

public class OcorrenciaCommand
{
    public Guid EtapaObraId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
}
