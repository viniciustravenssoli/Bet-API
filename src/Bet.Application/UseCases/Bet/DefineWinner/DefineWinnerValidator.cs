using Bet.Communication.Request;
using FluentValidation;

namespace Bet.Application.UseCases.Bet.DefineWinner;
public class DefineWinnerValidator : AbstractValidator<RequestDefineWinner>
{
    public DefineWinnerValidator()
    {
        RuleFor(x => x.TeamWinnerId).NotNull().NotEmpty().WithMessage("Valor Invalido");
    }
}
