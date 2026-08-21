using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class FamiliaStatusHistorico
{
    public Guid Id { get; set; }
    public Guid FamiliaId { get; set; }
    public FamiliaStatus StatusAnterior { get; set; }
    public FamiliaStatus StatusNovo { get; set; }
    public string? Motivo { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTimeOffset DataTransicao { get; set; }

    public Familia? Familia { get; set; }
}
