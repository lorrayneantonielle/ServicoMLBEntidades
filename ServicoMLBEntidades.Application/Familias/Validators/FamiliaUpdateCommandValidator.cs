using FluentValidation;
using ServicoMLBEntidades.Application.Familias.Commands;

namespace ServicoMLBEntidades.Application.Familias.Validators;

public class FamiliaUpdateCommandValidator : AbstractValidator<FamiliaUpdateCommand>
{
    public FamiliaUpdateCommandValidator()
    {
        RuleFor(x => x.RendaFamiliar).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SituacaoVulnerabilidade).NotEmpty();
    }
}
