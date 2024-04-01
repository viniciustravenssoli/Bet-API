using Bet.Communication.Request;

namespace Bet.Application.UseCases.Bet.DefineWinner;
public interface IDefineWinner
{
    Task Execute(RequestDefineWinner request, long id);
}
