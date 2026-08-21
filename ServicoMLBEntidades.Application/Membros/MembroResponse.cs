namespace ServicoMLBEntidades.Application.Membros;

public class MembroResponse
{
    public Guid Id { get; set; }
    public Guid FamiliaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Vinculo { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}
