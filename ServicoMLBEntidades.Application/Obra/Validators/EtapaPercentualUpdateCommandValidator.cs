using FluentValidation;
using ServicoMLBEntidades.Application.Obra.Commands;

namespace ServicoMLBEntidades.Application.Obra.Validators;

public class EtapaPercentualUpdateCommandValidator : AbstractValidator<EtapaPercentualUpdateCommand>
{
    public EtapaPercentualUpdateCommandValidator()
    {
        RuleFor(x => x.PercentualConclusao).InclusiveBetween(0, 100);
    }
}
