using FluentValidation;
using ServicoMLBEntidades.Application.Unidades.Commands;

namespace ServicoMLBEntidades.Application.Unidades.Validators;

public class UnidadeAtribuicaoCommandValidator : AbstractValidator<UnidadeAtribuicaoCommand>
{
    public UnidadeAtribuicaoCommandValidator()
    {
        RuleFor(x => x.FamiliaId).NotEqual(Guid.Empty);
    }
}
