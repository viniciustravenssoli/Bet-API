namespace Bet.Application.Services.Email;
public interface IEmailService
{
    Task ConfirmationBetWinner(string body, string earnedValue, string emailto);
}
