namespace ServicoMLBEntidades.Domain.Services;

public interface IDocumentoStorageService
{
    Task<string> SalvarAsync(Stream conteudo, string nomeArquivoOriginal, string mimeType, CancellationToken ct = default);

    Task<(Stream Conteudo, string MimeType)> ObterAsync(string arquivoPath, CancellationToken ct = default);

    void Remover(string arquivoPath);
}
