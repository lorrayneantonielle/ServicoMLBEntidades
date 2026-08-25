using FluentValidation;
using ServicoMLBEntidades.Application.Mutirao.Commands;

namespace ServicoMLBEntidades.Application.Mutirao.Validators;

public class PresencaCommandValidator : AbstractValidator<PresencaCommand>
{
    public PresencaCommandValidator()
    {
        RuleFor(x => x.MutiraoEscalaId).NotEqual(Guid.Empty);
        RuleFor(x => x.FamiliaId).NotEqual(Guid.Empty);
    }
}
