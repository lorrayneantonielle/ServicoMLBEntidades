using FluentValidation;
using ServicoMLBEntidades.Application.Obra.Commands;

namespace ServicoMLBEntidades.Application.Obra.Validators;

public class EtapaObraCommandValidator : AbstractValidator<EtapaObraCommand>
{
    public EtapaObraCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Ordem).GreaterThan(0);
    }
}
