using Bet.Application.Services.LoggedUser;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Repository.Bet;
using Bet.Infra;

namespace Bet.Application.UseCases.Bet.Register;
public class RegisterBetUseCase : IRegisterBetUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IBetWriteOnlyRepository _betRepository;

    public RegisterBetUseCase(IUnitOfWork unitOfWork, ILoggedUser loggedUser, IBetWriteOnlyRepository betRepository)
    {
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _betRepository = betRepository;
    }

    public async Task<ResponseRegisterBet> Execute(RequestRegisterBet request)
    {

        var bet = new Domain.Entities.Bet
        {
            TeamA = Domain.Entities.Team.TeamA,
            TeamB = Domain.Entities.Team.TeamB,
            Winner = null,
            ExpiryTime = request.ExpiryTime,
            Paid = false,
        };

        await _betRepository.Add(bet);
        await _unitOfWork.Commit();

        var response = new ResponseRegisterBet
        {
           ExpiryTime = bet.ExpiryTime,
        };

        return response;

    }
}
