using Bet.Communication.Request;
using FluentValidation;
using System.Drawing;

namespace Bet.Application.UseCases.User.JoinBet;
public class JoinBetValidator : AbstractValidator<RequestJoinBet>
{
    public JoinBetValidator()
    {
        RuleFor(x => x.BetAmount).NotNull().WithMessage("Valor não pode ser nulo")
            .GreaterThan(1.99).WithMessage("Valor da aposta deve ser no minimo 2 reais")
            .LessThan(2000).WithMessage("Valor da aposta não pode ultrapassar 2.000");
        RuleFor(x => x.ChoseTeamId).NotNull().WithMessage("Voce deve escolher o ganhador para entrar na aposta");

    }
}
