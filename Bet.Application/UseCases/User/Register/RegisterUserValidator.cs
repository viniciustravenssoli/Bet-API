using Bet.Communication.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Bet.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(c => c.UserName).NotEmpty().WithMessage("EmptyUserName");
        RuleFor(c => c.Email).NotEmpty().WithMessage("EmptyUserEmail");
        RuleFor(c => c.Phone).NotEmpty().WithMessage("EmptyUserPhone");
        RuleFor(c => c.Password).SetValidator(new PasswordValidator());
        When(c => !string.IsNullOrEmpty(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage("InvalidUserEmail");
        });

        When(c => !string.IsNullOrEmpty(c.Phone), () =>
        {
            RuleFor(c => c.Phone).Custom((telefone, context) =>
            {
                string patternPhone = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                var isMatch = Regex.IsMatch(telefone, patternPhone);

                if (!isMatch)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(telefone), "InvalidPhoneNumber"));
                }
            });
        });

    }
}
