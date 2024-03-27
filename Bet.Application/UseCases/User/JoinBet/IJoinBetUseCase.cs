using Bet.Communication.Request;
using Bet.Communication.Response;

namespace Bet.Application.UseCases.User.JoinBet;
public interface IJoinBetUseCase
{
    Task<ResponseJoinBet> Execute(RequestJoinBet request);
}
