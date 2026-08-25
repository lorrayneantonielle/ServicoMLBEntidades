using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Obra.Commands;

public class MedicaoCommand
{
    public Guid EtapaObraId { get; set; }
    public DateOnly Data { get; set; }
    public StatusAprovacao StatusAprovacao { get; set; }
    public decimal? RecursosLiberados { get; set; }
    public string? Observacao { get; set; }
}
