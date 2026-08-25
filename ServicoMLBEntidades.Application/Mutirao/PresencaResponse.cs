namespace ServicoMLBEntidades.Application.Mutirao;

public class PresencaResponse
{
    public Guid Id { get; set; }
    public Guid MutiraoEscalaId { get; set; }
    public Guid FamiliaId { get; set; }
    public DateTimeOffset DataRegistro { get; set; }
    public int PontuacaoConcedida { get; set; }
}
