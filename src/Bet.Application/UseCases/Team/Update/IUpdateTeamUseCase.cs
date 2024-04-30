using Bet.Communication.Request;

namespace Bet.Application.UseCases.Team.Update;
public interface IUpdateTeamUseCase
{
    Task Execute(RequestUpdateTeam request, long teamId);
}
