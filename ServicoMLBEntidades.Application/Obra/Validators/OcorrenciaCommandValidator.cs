using FluentValidation;
using ServicoMLBEntidades.Application.Obra.Commands;

namespace ServicoMLBEntidades.Application.Obra.Validators;

public class OcorrenciaCommandValidator : AbstractValidator<OcorrenciaCommand>
{
    public OcorrenciaCommandValidator()
    {
        RuleFor(x => x.EtapaObraId).NotEmpty();
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Data).Must(d => d != default).WithMessage("Data da ocorrência é obrigatória.");
    }
}
