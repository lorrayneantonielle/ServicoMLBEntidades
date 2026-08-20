using Microsoft.Extensions.Configuration;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Infrastructure.Storage;

public class LocalDocumentoStorageService : IDocumentoStorageService
{
    private readonly string _basePath;

    public LocalDocumentoStorageService(IConfiguration configuration)
    {
        var configuredPath = configuration["Storage:DocumentosPath"]
            ?? throw new InvalidOperationException("Configuração 'Storage:DocumentosPath' não encontrada.");

        _basePath = Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(AppContext.BaseDirectory, configuredPath);

        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SalvarAsync(Stream conteudo, string nomeArquivoOriginal, string mimeType, CancellationToken ct = default)
    {
        var extensao = Path.GetExtension(nomeArquivoOriginal);
        var nomeArmazenado = $"{Guid.NewGuid()}{extensao}";
        var caminhoCompleto = Path.Combine(_basePath, nomeArmazenado);

        await using var arquivo = File.Create(caminhoCompleto);
        await conteudo.CopyToAsync(arquivo, ct);

        return nomeArmazenado;
    }

    public async Task<(Stream Conteudo, string MimeType)> ObterAsync(string arquivoPath, CancellationToken ct = default)
    {
        var caminhoCompleto = ResolverCaminho(arquivoPath);
        if (!File.Exists(caminhoCompleto))
        {
            throw new FileNotFoundException("Documento não encontrado no armazenamento.", arquivoPath);
        }

        var bytes = await File.ReadAllBytesAsync(caminhoCompleto, ct);
        var mimeType = "application/octet-stream";
        return (new MemoryStream(bytes), mimeType);
    }

    public void Remover(string arquivoPath)
    {
        var caminhoCompleto = ResolverCaminho(arquivoPath);
        if (File.Exists(caminhoCompleto))
        {
            File.Delete(caminhoCompleto);
        }
    }

    private string ResolverCaminho(string arquivoPath)
    {
        return Path.Combine(_basePath, arquivoPath);
    }
}
