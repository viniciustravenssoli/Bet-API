using Bet.Communication.Request;
using Bet.Communication.Response;

namespace Bet.Application.UseCases.User.Login;
public interface ILoginUseCase
{
    Task<ResponseLogin> Execute(RequestLogin request);
}
