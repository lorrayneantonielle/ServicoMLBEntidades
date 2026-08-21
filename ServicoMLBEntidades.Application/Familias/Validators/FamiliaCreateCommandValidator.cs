using FluentValidation;
using ServicoMLBEntidades.Application.Familias.Commands;
using ServicoMLBEntidades.Application.Membros.Validators;

namespace ServicoMLBEntidades.Application.Familias.Validators;

public class FamiliaCreateCommandValidator : AbstractValidator<FamiliaCreateCommand>
{
    public FamiliaCreateCommandValidator()
    {
        RuleFor(x => x.RendaFamiliar).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SituacaoVulnerabilidade).NotEmpty();
        RuleForEach(x => x.Membros).SetValidator(new MembroDadosValidator());
    }
}
