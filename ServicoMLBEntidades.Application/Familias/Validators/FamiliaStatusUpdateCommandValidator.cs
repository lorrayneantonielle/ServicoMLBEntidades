using FluentValidation;
using ServicoMLBEntidades.Application.Familias.Commands;

namespace ServicoMLBEntidades.Application.Familias.Validators;

public class FamiliaStatusUpdateCommandValidator : AbstractValidator<FamiliaStatusUpdateCommand>
{
    public FamiliaStatusUpdateCommandValidator()
    {
        RuleFor(x => x.NovoStatus).IsInEnum();
    }
}
