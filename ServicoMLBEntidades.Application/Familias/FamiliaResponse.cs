using ServicoMLBEntidades.Application.Documentos;
using ServicoMLBEntidades.Application.Membros;
using ServicoMLBEntidades.Domain.Enums;

namespace ServicoMLBEntidades.Application.Familias;

public class FamiliaResponse
{
    public Guid Id { get; set; }
    public decimal RendaFamiliar { get; set; }
    public string SituacaoVulnerabilidade { get; set; } = string.Empty;
    public FamiliaStatus Status { get; set; }
    public int PontuacaoAcumulada { get; set; }
    public List<MembroResponse> Membros { get; set; } = [];
    public List<DocumentoResponse> Documentos { get; set; } = [];
    public Guid? UnidadeHabitacionalId { get; set; }
}
