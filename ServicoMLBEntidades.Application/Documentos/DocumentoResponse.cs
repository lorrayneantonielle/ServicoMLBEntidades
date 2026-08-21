using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Documentos;

public class DocumentoResponse
{
    public Guid Id { get; set; }
    public Guid FamiliaId { get; set; }
    public DocumentoTipo Tipo { get; set; }
    public DocumentoStatus Status { get; set; }
    public string? ArquivoPath { get; set; }
}
