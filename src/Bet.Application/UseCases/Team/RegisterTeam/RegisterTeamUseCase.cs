using Bet.Communication.Request;
using Bet.Domain.Repository.Team;
using Bet.Infra;

namespace Bet.Application.UseCases.Team.RegisterTeam;
public class RegisterTeamUseCase : IRegisterTeamUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITeamRepository _temRepository;

    public RegisterTeamUseCase(IUnitOfWork unitOfWork, ITeamRepository temRepository)
    {
        _unitOfWork = unitOfWork;
        _temRepository = temRepository;
    }

    public async Task Execute(RequestRegisterTeam request)
    {
        var team = new Domain.Entities.Team
        {
            Name = request.TeamName
        };

        await _temRepository.Add(team);
        await _unitOfWork.Commit();
    }
}
