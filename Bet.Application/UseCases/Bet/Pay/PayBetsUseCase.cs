using Bet.Application.Services.Email;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.User;
using Bet.Infra;

namespace Bet.Application.UseCases.Bet.Pay;
public class PayBetsUseCase : IPayBetsUseCase
{
    private readonly IBetUpdateOnlyRepository _betUpdateOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public PayBetsUseCase(IBetUpdateOnlyRepository betUpdateOnlyRepository, IEmailService emailService, IUserUpdateOnlyRepository userUpdateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _betUpdateOnlyRepository = betUpdateOnlyRepository;
        _emailService = emailService;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
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
                    await _userUpdateOnlyRepository.BulkUpdateBalanceAsync(userBet.UserId, userBet.BetAmount * userBet.Odd);
                    var earnerd = (userBet.BetAmount * userBet.Odd).ToString();
                    await _emailService.ConfirmationBetWinner("body", earnerd, userBet.User.Email) ;
                }
            }));

            bet.Paid = true;
            _betUpdateOnlyRepository.Update(bet);

        }

        await _unitOfWork.Commit();
    }
}
