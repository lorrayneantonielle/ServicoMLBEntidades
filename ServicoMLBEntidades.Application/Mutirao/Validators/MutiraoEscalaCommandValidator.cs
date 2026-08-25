using FluentValidation;
using ServicoMLBEntidades.Application.Mutirao.Commands;

namespace ServicoMLBEntidades.Application.Mutirao.Validators;

public class MutiraoEscalaCommandValidator : AbstractValidator<MutiraoEscalaCommand>
{
    public MutiraoEscalaCommandValidator()
    {
        RuleFor(x => x.Data).NotEqual(default(DateOnly));
        RuleFor(x => x.Turno).IsInEnum();
        RuleFor(x => x.VagasTotais).GreaterThan(0);
        RuleFor(x => x.PontuacaoPorPresenca).GreaterThan(0);
    }
}
