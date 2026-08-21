using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class Documento
{
    public Guid Id { get; set; }
    public Guid FamiliaId { get; set; }
    public DocumentoTipo Tipo { get; set; }
    public DocumentoStatus Status { get; set; } = DocumentoStatus.Pendente;
    public string? ArquivoPath { get; set; }
    public string? ArquivoMimeType { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Familia? Familia { get; set; }
}
