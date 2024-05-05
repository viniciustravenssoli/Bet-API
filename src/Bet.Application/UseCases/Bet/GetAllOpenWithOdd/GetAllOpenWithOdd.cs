using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.Team;

namespace Bet.Application.UseCases.Bet.GetAllOpenWithOdd
{
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

            var betInfoTasks = unpaidBets.Select(async bet =>
            {
                var userBets = bet.UserBets.ToList();
                var (totalAmount, amountOnTeamA, amountOnTeamB) = CalculateAmounts(userBets);

                var teamA = await _teamRepository.GetByIdAsync(bet.HomeId);
                var teamB = await _teamRepository.GetByIdAsync(bet.VisitorId);

                var userBetOnA = CreateUserBet(teamA, totalAmount, amountOnTeamA, amountOnTeamB, bet);
                var userBetOnB = CreateUserBet(teamB, totalAmount, amountOnTeamA, amountOnTeamB, bet);

                return new BetInfo
                {
                    BetId = bet.Id,
                    TeamAOdd = userBetOnA.Odd,
                    TeamBOdd = userBetOnB.Odd,
                    HomeTeamName = teamA.Name,
                    VisitorTeamName = teamB.Name,
                };
            });

            var betInfos = await Task.WhenAll(betInfoTasks);

            return new ResponseBetInfo
            {
                betInfos = betInfos.ToList(),
            };
        }

        private UserBet CreateUserBet(Domain.Entities.Team chosenTeam, double totalAmount, double amountOnTeamA, double amountOnTeamB, Domain.Entities.Bet bet)
        {
            var userBet = new UserBet(chosenTeam, bet);
            userBet.CalculateOddOnRequest(totalAmount, amountOnTeamA, amountOnTeamB);
            return userBet;
        }

        private (double totalAmount, double amountOnTeamA, double amountOnTeamB) CalculateAmounts(List<UserBet> userBets)
        {
            var totalAmount = userBets.Sum(ub => ub.BetAmount);
            var amountOnTeamA = userBets.Where(ub => ub.ChosenTeamId == ub.Bet.HomeId).Sum(ub => ub.BetAmount);
            var amountOnTeamB = userBets.Where(ub => ub.ChosenTeamId == ub.Bet.VisitorId).Sum(ub => ub.BetAmount);
            return (totalAmount, amountOnTeamA, amountOnTeamB);
        }
    }
}
