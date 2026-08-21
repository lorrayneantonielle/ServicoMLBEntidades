using ServicoMLBEntidades.Application.Membros.Commands;

namespace ServicoMLBEntidades.Application.Familias.Commands;

public class FamiliaCreateCommand
{
    public decimal RendaFamiliar { get; set; }
    public string SituacaoVulnerabilidade { get; set; } = string.Empty;
    public List<MembroCommand> Membros { get; set; } = [];
}
