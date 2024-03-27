
using Bet.Domain.Repository.Bet;
using Bet.Infra.Context;

namespace Bet.Application.UseCases.Bet.Pay;
public class PayBestUseCase : IPayBetsUseCase
{
    private readonly IBetUpdateOnlyRepository _betUpdateOnlyRepository;
    private readonly BetContext _betContext;

    public PayBestUseCase(IBetUpdateOnlyRepository betUpdateOnlyRepository, BetContext betContext)
    {
        _betUpdateOnlyRepository = betUpdateOnlyRepository;
        _betContext = betContext;
    }

    public async Task Execute()
    {
        var unpaidBets = await _betUpdateOnlyRepository.GetNotPaidBets();

        foreach (var bet in unpaidBets)
        {
            foreach (var userBet in bet.UserBets)
            {
                var user = userBet.User;
                if (bet.Winner == userBet.ChosenTeam) // Implemente seu método para verificar se a aposta foi vencida
                {
                    // Pague ao usuário se a aposta foi vencida
                    user.Balance += userBet.BetAmount * userBet.Odd;
                }
                bet.Paid = true; // Marque a aposta como paga
            }
            
        }

        await _betContext.SaveChangesAsync();
    }
}
