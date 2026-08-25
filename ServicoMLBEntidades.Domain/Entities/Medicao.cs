using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class Medicao
{
    public Guid Id { get; set; }
    public Guid EtapaObraId { get; set; }
    public DateOnly Data { get; set; }
    public StatusAprovacao StatusAprovacao { get; set; } = StatusAprovacao.Pendente;
    public decimal? RecursosLiberados { get; set; }
    public string? Observacao { get; set; }
    public Guid RegistradoPorUsuarioId { get; set; }

    public EtapaObra? EtapaObra { get; set; }
}
