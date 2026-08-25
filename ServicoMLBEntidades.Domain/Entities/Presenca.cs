namespace ServicoMLBEntidades.Domain.Entities;

public class Presenca
{
    public Guid Id { get; set; }
    public Guid MutiraoEscalaId { get; set; }
    public Guid FamiliaId { get; set; }
    public DateTimeOffset DataRegistro { get; set; }
    public int PontuacaoConcedida { get; set; }

    public MutiraoEscala? MutiraoEscala { get; set; }
    public Familia? Familia { get; set; }
}
