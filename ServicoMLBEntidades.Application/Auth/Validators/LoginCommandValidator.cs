using FluentValidation;
using ServicoMLBEntidades.Application.Auth.Commands;

namespace ServicoMLBEntidades.Application.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty();
    }
}
