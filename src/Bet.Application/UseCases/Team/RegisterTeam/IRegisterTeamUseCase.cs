using Bet.Communication.Request;
using Bet.Communication.Response;

namespace Bet.Application.UseCases.Team.RegisterTeam;
public interface IRegisterTeamUseCase
{
    Task Execute(RequestRegisterTeam request);
}

