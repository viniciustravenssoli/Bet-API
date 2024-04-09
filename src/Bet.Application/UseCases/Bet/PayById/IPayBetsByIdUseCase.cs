namespace Bet.Application.UseCases.Bet.PayById;
public interface IPayBetsByIdUseCase
{
    Task Execute(long id);
}
