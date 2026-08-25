using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Obra;

public class MedicaoResponse
{
    public Guid Id { get; set; }
    public Guid EtapaObraId { get; set; }
    public DateOnly Data { get; set; }
    public StatusAprovacao StatusAprovacao { get; set; }
    public decimal? RecursosLiberados { get; set; }
    public string? Observacao { get; set; }

    /// <summary>Aprovada para uma etapa cujo percentual de conclusão ainda não reflete 100% (edge case spec.md).</summary>
    public bool Divergente { get; set; }
}
