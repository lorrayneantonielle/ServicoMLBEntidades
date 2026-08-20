using FluentValidation;
using ServicoMLBEntidades.Application.Auth.Commands;

namespace ServicoMLBEntidades.Application.Auth.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
