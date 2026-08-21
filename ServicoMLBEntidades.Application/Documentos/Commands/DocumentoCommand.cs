using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Documentos.Commands;

public class DocumentoCommand
{
    public Guid FamiliaId { get; set; }
    public DocumentoTipo Tipo { get; set; }
    public string NomeArquivo { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public Stream Conteudo { get; set; } = Stream.Null;
}
