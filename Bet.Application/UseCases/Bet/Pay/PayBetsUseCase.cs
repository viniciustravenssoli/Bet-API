using Bet.Application.Services.Email;
using Bet.Domain.Repository.Bet;
using Bet.Infra.Context;
using System.Security.Cryptography;

namespace Bet.Application.UseCases.Bet.Pay;
public class PayBetsUseCase : IPayBetsUseCase
{
    private readonly IBetUpdateOnlyRepository _betUpdateOnlyRepository;
    private readonly IEmailService _emailService;
    private readonly BetContext _betContext;

    public PayBetsUseCase(IBetUpdateOnlyRepository betUpdateOnlyRepository, BetContext betContext, IEmailService emailService)
    {
        _betUpdateOnlyRepository = betUpdateOnlyRepository;
        _betContext = betContext;
        _emailService = emailService;
    }

    public async Task Execute()
    {
        var unpaidBetsDictionary = await _betUpdateOnlyRepository.GetNotPaidBetsAsDictionary();

        foreach (var bet in unpaidBetsDictionary.Values)
        {
            await Task.WhenAll(bet.UserBets.Select(async userBet =>
            {
                if (bet.Winner == userBet.ChosenTeam)
                {
                    userBet.User.Balance += userBet.BetAmount * userBet.Odd;
                    var earnerd = (userBet.BetAmount * userBet.Odd).ToString();
                    await _emailService.ConfirmationBetWinner("corpo", earnerd, userBet.User.Email) ;
                }
            }));

            bet.Paid = true;
        }

        await _betContext.SaveChangesAsync();
    }
}
