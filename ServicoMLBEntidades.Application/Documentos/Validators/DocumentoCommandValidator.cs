using FluentValidation;
using ServicoMLBEntidades.Application.Documentos.Commands;

namespace ServicoMLBEntidades.Application.Documentos.Validators;

public class DocumentoCommandValidator : AbstractValidator<DocumentoCommand>
{
    private static readonly string[] MimeTypesPermitidos = ["application/pdf", "image/jpeg", "image/png"];
    private const long TamanhoMaximoBytes = 10 * 1024 * 1024;

    public DocumentoCommandValidator()
    {
        RuleFor(x => x.FamiliaId).NotEmpty();
        RuleFor(x => x.Tipo).IsInEnum();

        RuleFor(x => x.TamanhoBytes)
            .GreaterThan(0).WithMessage("Arquivo não pode estar vazio.")
            .LessThanOrEqualTo(TamanhoMaximoBytes).WithMessage("Arquivo excede o tamanho máximo de 10MB.");

        RuleFor(x => x.MimeType)
            .Must(mime => MimeTypesPermitidos.Contains(mime))
            .WithMessage("Tipo de arquivo não permitido. Permitidos: PDF, JPEG, PNG.");
    }
}
