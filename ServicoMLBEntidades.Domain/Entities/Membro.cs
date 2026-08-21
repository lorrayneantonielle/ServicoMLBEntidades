namespace ServicoMLBEntidades.Domain.Entities;

public class Membro
{
    public Guid Id { get; set; }
    public Guid FamiliaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Vinculo { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;

    /// <summary>Espelha Familia.Excluida — necessário para o índice único parcial de CPF (FR-033).</summary>
    public bool FamiliaExcluida { get; set; }

    public Familia? Familia { get; set; }
}
