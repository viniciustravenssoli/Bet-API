using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;

namespace Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
public class GetAllOpenWithOdd : IGetAllOpenWithOdd
{
    private readonly IBetReadOnlyRepository _betReadOnlyRepository;

    public GetAllOpenWithOdd(IBetReadOnlyRepository betReadOnlyRepository)
    {
        _betReadOnlyRepository = betReadOnlyRepository;
    }

    public async Task<ResponseBetInfo> Execute(int page, int pageSize)
    {
        var unpaidBets = await _betReadOnlyRepository.GetUnpaidBetsWithUserBets(page, pageSize);

        var betInfos = unpaidBets.Select(bet =>
        {
            var userBets = bet.UserBets.ToList();
            var (totalAmount, amountOnTeamA, amountOnTeamB) = CalculateAmounts(userBets);

            var userBetOnA = CreateUserBet(Team.TeamA, totalAmount, amountOnTeamA, amountOnTeamB);
            var userBetOnB = CreateUserBet(Team.TeamB, totalAmount, amountOnTeamA, amountOnTeamB);

            return new BetInfo
            {
                BetId = bet.Id,
                TeamAOdd = userBetOnA.Odd,
                TeamBOdd = userBetOnB.Odd,
            };
        }).ToList();

        var response = new ResponseBetInfo
        {
            betInfos = betInfos,
        };

        return response;
    }

    private UserBet CreateUserBet(Team chosenTeam, double totalAmount, double amountOnTeamA, double amountOnTeamB)
    {
        var userBet = new UserBet(chosenTeam);

        userBet.CalculateOdd(totalAmount, amountOnTeamA, amountOnTeamB);
        return userBet;
    }

    private (double totalAmount, double amountOnTeamA, double amountOnTeamB) CalculateAmounts(List<UserBet> userBets)
    {
        var totalAmount = userBets.Sum(ub => ub.BetAmount);
        var amountOnTeamA = userBets.Where(ub => ub.ChosenTeam == Team.TeamA).Sum(ub => ub.BetAmount);
        var amountOnTeamB = userBets.Where(ub => ub.ChosenTeam == Team.TeamB).Sum(ub => ub.BetAmount);
        return (totalAmount, amountOnTeamA, amountOnTeamB);
    }
}
