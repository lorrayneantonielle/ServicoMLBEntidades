using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Domain.Entities;

public class Familia
{
    public Guid Id { get; set; }
    public decimal RendaFamiliar { get; set; }
    public string SituacaoVulnerabilidade { get; set; } = string.Empty;
    public FamiliaStatus Status { get; set; } = FamiliaStatus.PreCadastro;
    public int PontuacaoAcumulada { get; set; }
    public bool Excluida { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public List<Membro> Membros { get; set; } = [];
    public List<Documento> Documentos { get; set; } = [];
}
