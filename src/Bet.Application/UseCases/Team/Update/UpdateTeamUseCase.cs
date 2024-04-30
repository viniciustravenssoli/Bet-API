using Bet.Application.BaseExceptions;
using Bet.Communication.Request;
using Bet.Domain.Repository.Team;
using Bet.Infra;
using Bet.Infra.Context;

namespace Bet.Application.UseCases.Team.Update;
public class UpdateTeamUseCase : IUpdateTeamUseCase
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeamUseCase(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateTeam request, long teamId)
    {
        var team = await _teamRepository.GetByIdAsync(teamId) ?? throw new NotFoundException("Time não encontrado");

        team.Name = request.NewName;

        _teamRepository.Update(team);
        await _unitOfWork.Commit();
    }
}
