using FluentValidation;
using System.Text.RegularExpressions;

namespace Bet.Application.UseCases.User;
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(p => p).NotEmpty().WithMessage("Password Vazio");
        RuleFor(p => p).Must(BeValidPassword).WithMessage("A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial");
        When(p => !string.IsNullOrEmpty(p), () =>
        {
            RuleFor(p => p.Length).GreaterThanOrEqualTo(6).WithMessage("Senha Precisa conter 6 ou mais caracteres");
        });
    }

    private bool BeValidPassword(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$";
        return Regex.IsMatch(password, pattern);
    }
}
 