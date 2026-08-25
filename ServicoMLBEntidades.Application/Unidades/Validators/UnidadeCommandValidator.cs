using FluentValidation;
using ServicoMLBEntidades.Application.Unidades.Commands;

namespace ServicoMLBEntidades.Application.Unidades.Validators;

public class UnidadeCommandValidator : AbstractValidator<UnidadeCommand>
{
    public UnidadeCommandValidator()
    {
        RuleFor(x => x.Identificador).NotEmpty();
        RuleFor(x => x.Metragem).GreaterThan(0);
        RuleFor(x => x.LocalizacaoTerreno).NotEmpty();
    }
}
