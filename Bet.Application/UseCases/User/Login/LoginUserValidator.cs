using Bet.Communication.Request;
using FluentValidation;

namespace Bet.Application.UseCases.User.Login;
public class LoginUserValidator : AbstractValidator<RequestLogin>
{
    public LoginUserValidator()
    {
        RuleFor(l => l.Email).NotEmpty().WithMessage("EmptyEmail");
        RuleFor(l => l.Password).NotEmpty().WithMessage("EmptyPassword");
    }
}
