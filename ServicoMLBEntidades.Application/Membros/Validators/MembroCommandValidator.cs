using FluentValidation;
using ServicoMLBEntidades.Application.Membros.Commands;

namespace ServicoMLBEntidades.Application.Membros.Validators;

/// <summary>Regras de Nome/DataNascimento/Vinculo/Cpf, sem FamiliaId — reaproveitado na
/// validação de membros aninhados em FamiliaCreateCommand, onde a família ainda não existe.</summary>
internal class MembroDadosValidator : AbstractValidator<MembroCommand>
{
    public MembroDadosValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.DataNascimento).Must(d => d != default).WithMessage("Data de nascimento é obrigatória.");
        RuleFor(x => x.Vinculo).NotEmpty();
        RuleFor(x => x.Cpf).NotEmpty().Matches("^[0-9]{11}$").WithMessage("CPF deve conter 11 dígitos numéricos.");
    }
}

public class MembroCommandValidator : AbstractValidator<MembroCommand>
{
    public MembroCommandValidator()
    {
        RuleFor(x => x.FamiliaId).NotEmpty();
        Include(new MembroDadosValidator());
    }
}
