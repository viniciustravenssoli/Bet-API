using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.Team;

namespace Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
public class GetAllOpenWithOdd : IGetAllOpenWithOdd
{
    private readonly IBetReadOnlyRepository _betReadOnlyRepository;
    private readonly ITeamRepository _teamRepository;

    public GetAllOpenWithOdd(IBetReadOnlyRepository betReadOnlyRepository, ITeamRepository teamRepository)
    {
        _betReadOnlyRepository = betReadOnlyRepository;
        _teamRepository = teamRepository;
    }

    public async Task<ResponseBetInfo> Execute(int page, int pageSize)
    {
        var unpaidBets = await _betReadOnlyRepository.GetUnpaidBetsWithUserBets(page, pageSize);

        var betInfos = unpaidBets.Select(async bet =>
        {
            var userBets = bet.UserBets.ToList();
            var (totalAmount, amountOnTeamA, amountOnTeamB) = CalculateAmounts(userBets);

            var teamA = await _teamRepository.GetByIdAsync(bet.VisitorId);
            var teamB = await _teamRepository.GetByIdAsync(bet.HomeId);

            var userBetOnA = CreateUserBet(teamA, totalAmount, amountOnTeamA, amountOnTeamB);
            var userBetOnB = CreateUserBet(teamB, totalAmount, amountOnTeamA, amountOnTeamB);

            return new BetInfo
            {
                BetId = bet.Id,
                TeamAOdd = userBetOnA.Odd,
                TeamBOdd = userBetOnB.Odd,
                HomeTeamName = teamB.Name,
                VisitorTeamName = teamA.Name,
            };
        }).ToList();

        var betInfosResults = await Task.WhenAll(betInfos);

        var response = new ResponseBetInfo
        {
            betInfos = betInfosResults.ToList(),
        };

        return response;
    }

    private UserBet CreateUserBet(Domain.Entities.Team chosenTeam, double totalAmount, double amountOnTeamA, double amountOnTeamB)
    {
        var userBet = new UserBet(chosenTeam);

        userBet.CalculateOdd(totalAmount, amountOnTeamA, amountOnTeamB);
        return userBet;
    }

    private (double totalAmount, double amountOnTeamA, double amountOnTeamB) CalculateAmounts(List<UserBet> userBets)
    {
        var totalAmount = userBets.Sum(ub => ub.BetAmount);
        var amountOnTeamA = userBets.Where(ub => ub.ChosenTeam == ub.Bet.Home).Sum(ub => ub.BetAmount);
        var amountOnTeamB = userBets.Where(ub => ub.ChosenTeam == ub.Bet.Visitor).Sum(ub => ub.BetAmount);
        return (totalAmount, amountOnTeamA, amountOnTeamB);
    }
}
