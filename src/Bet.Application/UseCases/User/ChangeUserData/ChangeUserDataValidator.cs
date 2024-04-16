using Bet.Communication.Request;
using FluentValidation;

namespace Bet.Application.UseCases.User.ChangeUserData;
public class ChangeUserDataValidator : AbstractValidator<RequestChangeUserData>
{
    public ChangeUserDataValidator()
    {
        When(c => !string.IsNullOrEmpty(c.NewEmail), () =>
        {
            RuleFor(c => c.NewEmail).EmailAddress().WithMessage("InvalidUserEmail");
        });

        When(c => !string.IsNullOrEmpty(c.NewPhone), () =>
        {
            RuleFor(c => c.NewPhone).Matches("[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}").WithMessage("InvalidPhoneNumber");
        });

        When(c => !string.IsNullOrEmpty(c.NewName), () =>
        {
            RuleFor(c => c.NewName).NotEmpty().WithMessage("EmptyUserName");
        });
    }
}
