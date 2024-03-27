using Bet.Application.BaseExceptions;
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.User;
using Bet.Domain.Repository.UserBet;
using Bet.Infra;

namespace Bet.Application.UseCases.User.JoinBet;
public class JoinBetUseCase : IJoinBetUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserBetWriteOnlyRepository _userBetRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBetReadOnlyRepository _betReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public JoinBetUseCase(ILoggedUser loggedUser, IUserBetWriteOnlyRepository userBetRepository, IUnitOfWork unitOfWork, IBetReadOnlyRepository betReadOnlyRepository, IUserUpdateOnlyRepository userUpdateOnlyRepository)
    {
        _loggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
        _userBetRepository = userBetRepository ?? throw new ArgumentNullException(nameof(userBetRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _betReadOnlyRepository = betReadOnlyRepository ?? throw new ArgumentNullException(nameof(betReadOnlyRepository));
        _userUpdateOnlyRepository = userUpdateOnlyRepository ?? throw new ArgumentNullException(nameof(userUpdateOnlyRepository));
    }

    public async Task<ResponseJoinBet> Execute(RequestJoinBet request)
    {
        var user = await _loggedUser.GetUser() ?? throw new BetException("Usuário não encontrado");

        if (request.BetAmount <= 0)
        {
            throw new BetException("O valor da aposta deve ser maior que zero.");
        }

        var bet = await _betReadOnlyRepository.GetById(request.BetId) ?? throw new BetException("Aposta não encontrada");

        // Verifica se o usuário tem saldo suficiente
        if (user.Balance < request.BetAmount)
        {
            throw new BetException("Saldo insuficiente para realizar a aposta.");
        }

        var userBets = await _userBetRepository.GetUserBetsByBetIdAsync(request.BetId);

        double totalAmount = userBets.Sum(ub => ub.BetAmount);
        double amountOnTeamA = userBets.Where(ub => ub.ChosenTeam == Team.TeamA).Sum(ub => ub.BetAmount);
        double amountOnTeamB = userBets.Where(ub => ub.ChosenTeam == Team.TeamB).Sum(ub => ub.BetAmount);

        var userBet = new UserBet
        {
            UserId = user.Id,
            BetId = request.BetId,
            BetAmount = request.BetAmount,
            ChosenTeam = request.Chose,
        };

        userBet.CalculateOdd(totalAmount, amountOnTeamA, amountOnTeamB);

        // Atualiza o saldo do usuário
        user.Balance -= userBet.BetAmount;

        // Adiciona a aposta do usuário
        await _userBetRepository.Add(userBet);

        // Atualiza o saldo do usuário no repositório
        await _userUpdateOnlyRepository.UpdateBalance(user.Id, user.Balance);

        // Commit das alterações no banco de dados
        await _unitOfWork.Commit();

        var response = new ResponseJoinBet
        {
            Value = userBet.BetAmount
        };

        return response;
    }
}
