using Bet.Communication.Request;
using Bet.Communication.Response;

namespace Bet.Application.UseCases.Bet.Register;
public interface IRegisterBetUseCase
{
    Task<ResponseRegisterBet> Execute(RequestRegisterBet request);
}
