using Bet.Communication.Request;
using FluentValidation;

namespace Bet.Application.UseCases.Bet.DefineWinner;
public class DefineWinnerValidator : AbstractValidator<RequestDefineWinner>
{
    public DefineWinnerValidator()
    {
        RuleFor(x => x.Winner).IsInEnum().NotEmpty().NotNull().WithMessage("Valor Invalido");
    }
}
