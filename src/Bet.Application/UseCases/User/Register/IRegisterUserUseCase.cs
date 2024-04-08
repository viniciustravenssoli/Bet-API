using Bet.Communication.Request;
using Bet.Communication.Response;

namespace Bet.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUser> Execute(RequestRegisterUser request);
}
