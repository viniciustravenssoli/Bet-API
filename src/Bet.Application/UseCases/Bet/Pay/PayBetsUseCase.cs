﻿using Bet.Application.BaseExceptions;
using Bet.Application.Services.Email;
using Bet.Domain.Entities;
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
        var unpaidBetsDictionary = await _betUpdateOnlyRepository.GetNotPaidBetsWithAWinnerAsDictionary();

        foreach (var bet in unpaidBetsDictionary.Values)
        {
            await Task.WhenAll(bet.UserBets.Select(async userBet =>
            {
                ProcessUserBetPayment(userBet, bet);
            }));

            bet.Paid = true;
            _betUpdateOnlyRepository.Update(bet);
        }
        await _unitOfWork.Commit();
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
