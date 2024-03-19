using FluentValidation;

namespace Bet.Application.UseCases.User;
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(p => p).NotEmpty().WithMessage("Passoword Vazio");
        When(p => !string.IsNullOrEmpty(p), () =>
        {
            RuleFor(p => p.Length).GreaterThanOrEqualTo(6).WithMessage("Senha Precisa conter 6 ou mais caracteres");
        });
    }
}
 