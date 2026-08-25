namespace ServicoMLBEntidades.Domain.Models;

public class PontuacaoFamiliaResultado
{
    public Guid FamiliaId { get; set; }
    public int PontuacaoAcumulada { get; set; }
    public bool BaixaParticipacao { get; set; }
    public List<PresencaResultado> Presencas { get; set; } = [];
}

public class PresencaResultado
{
    public Guid Id { get; set; }
    public Guid MutiraoEscalaId { get; set; }
    public Guid FamiliaId { get; set; }
    public DateTimeOffset DataRegistro { get; set; }
    public int PontuacaoConcedida { get; set; }
}
