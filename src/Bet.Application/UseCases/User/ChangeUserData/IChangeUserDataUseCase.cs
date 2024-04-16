using Bet.Communication.Request;

namespace Bet.Application.UseCases.User.ChangeUserData;
public interface IChangeUserDataUseCase
{
    Task Execute(RequestChangeUserData request);
}
