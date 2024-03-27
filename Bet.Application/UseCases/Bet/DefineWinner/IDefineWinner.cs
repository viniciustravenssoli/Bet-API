using Bet.Domain.Entities;

namespace Bet.Application.UseCases.Bet.DefineWinner;
public interface IDefineWinner
{
    Task Execute(Team winner, long id);
}
