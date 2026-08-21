namespace ServicoMLBEntidades.Application.Membros.Commands;

public class MembroCommand
{
    public Guid FamiliaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Vinculo { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}
