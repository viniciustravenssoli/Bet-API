
using Bet.Application.BaseExceptions;
using Bet.Application.Services.Email;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.User;
using Bet.Infra;

namespace Bet.Application.UseCases.Bet.PayById;
public class PayBetsByIdUseCase : IPayBetsByIdUseCase
{
    private readonly IBetUpdateOnlyRepository _betUpdateOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public PayBetsByIdUseCase(IBetUpdateOnlyRepository betUpdateOnlyRepository, IUserUpdateOnlyRepository userUpdateOnlyRepository, IEmailService emailService, IUnitOfWork unitOfWork)
    {
        _betUpdateOnlyRepository = betUpdateOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var betToPay = await _betUpdateOnlyRepository.GetByIdIncludeUserAndUserBets(id) ?? throw new NotFoundException("Aposta não encontrada");

        CheckForConflict(betToPay);

        betToPay.UserBets.Select(userBet => ProcessUserBetPayment(userBet, betToPay));

        betToPay.Paid = true;
        _betUpdateOnlyRepository.Update(betToPay);
        await _unitOfWork.Commit();
    }

    private void CheckForConflict(Domain.Entities.Bet bet)
    {
        if (bet.Paid)
            throw new ConflictException("Aposta já foi paga");

        if (bet.Winner == null)
            throw new ConflictException("Aposta sem ganhador definido");
    }

    private async Task ProcessUserBetPayment(UserBet userBet, Domain.Entities.Bet bet)
    {
        if (bet.Winner == userBet.ChosenTeam)
        {
            var earnings = userBet.BetAmount * userBet.Odd;
            await _userUpdateOnlyRepository.BulkUpdateBalanceAsync(userBet.UserId, earnings);
            await SendWinnerEmail(userBet.User.Email, earnings);
        }
    }

    private async Task SendWinnerEmail(string userEmail, double earnings)
    {
        var emailBody = $"Parabéns! Você ganhou {earnings} na sua aposta.";
        await _emailService.ConfirmationBetWinner(emailBody, earnings.ToString(), userEmail);
    }
}

