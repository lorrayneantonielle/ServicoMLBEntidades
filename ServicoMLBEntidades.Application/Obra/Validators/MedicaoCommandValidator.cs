using FluentValidation;
using ServicoMLBEntidades.Application.Obra.Commands;

namespace ServicoMLBEntidades.Application.Obra.Validators;

public class MedicaoCommandValidator : AbstractValidator<MedicaoCommand>
{
    public MedicaoCommandValidator()
    {
        RuleFor(x => x.EtapaObraId).NotEmpty();
        RuleFor(x => x.Data).Must(d => d != default).WithMessage("Data da medição é obrigatória.");
        RuleFor(x => x.StatusAprovacao).IsInEnum();
        RuleFor(x => x.RecursosLiberados).GreaterThanOrEqualTo(0).When(x => x.RecursosLiberados.HasValue);
    }
}
